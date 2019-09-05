using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExcelDataReader;

namespace ImportOMatic3000
{

    [Serializable]
    public class UnsupportedDataTypeException : Exception
    {
        public UnsupportedDataTypeException() { }
        public UnsupportedDataTypeException(string message) : base(message) { }
        public UnsupportedDataTypeException(string message, Exception inner) : base(message, inner) { }
    }

    public class ExcelRowReader : IRowReader
    {
        readonly IExcelDataReader _reader;
        public void Dispose() => _reader?.Dispose();

        public ExcelRowReader(IExcelDataReader excelDataReader, IReadOnlyList<Regex> sheetMatchExpressions)
        {
            _reader = excelDataReader;
            SheetMatchExpressions = sheetMatchExpressions;
        }

        public static Dictionary<Type, Func<IExcelDataReader, int, string>> _fieldConverters = new Dictionary<Type, Func<IExcelDataReader, int, string>>
        {
            [typeof(string)] = (reader, i) => reader.GetString(i),
            [typeof(DateTime)] = (reader, i) => reader.GetDateTime(i).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            [typeof(double)] = (reader, i) => reader.GetDouble(i).ToString(CultureInfo.InvariantCulture),
            [typeof(bool)] = (reader, i) => reader.GetBoolean(i).ToString(CultureInfo.InvariantCulture)
        };

        public IReadOnlyList<Regex> SheetMatchExpressions { get; }

        private bool IsSheetMatch(string sheetName)
        {
            if (SheetMatchExpressions == null || SheetMatchExpressions.Count == 0)
            { return true; }
            return SheetMatchExpressions.Any(rx => rx.IsMatch(sheetName));
        }

        private IEnumerable<IRow> GetRows()
        {
            int line = 0;
            string oldSheet = null;
            int rowNumber = 0;
            do
            {
                if (IsSheetMatch(_reader.Name))
                {
                    while (_reader.Read())
                    {
                        var results = new string[_reader.FieldCount];
                        for (int i = 0; i < _reader.FieldCount; ++i)
                        {
                            if (_reader.GetFieldType(i) == null)
                            { results[i] = ""; }
                            else
                            {
                                if (!_fieldConverters.TryGetValue(_reader.GetFieldType(i), out var fieldConverter))
                                { throw new UnsupportedDataTypeException($"Data Type \"{_reader.GetFieldType(i).Name}\" not supported ({_reader.Name}:{i.ToColumnRef()}{rowNumber})."); }
                                results[i] = fieldConverter(_reader, i);
                            }
                        }
                        if (_reader.Name != oldSheet)
                        {
                            oldSheet = _reader.Name;
                            rowNumber = 0;
                        }
                        yield return new SpreadsheetRow(++line, ++rowNumber, oldSheet, results);
                    }
                }
            } while (_reader.NextResult());
        }

        public IEnumerator<IRow> GetEnumerator() => GetRows().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
