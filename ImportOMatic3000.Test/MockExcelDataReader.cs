using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ImportOMatic3000.Test
{

    public partial class MockExcelDataReader : IExcelDataReader
    {
        private object[] CurrentRow => _sheets[_sheetIndex].Rows[_index];
        private int _index = -1;
        public object this[int i] => CurrentRow[i];

        List<(string Name, List<object[]> Rows)> _sheets = new List<(string Name, List<object[]>)>();
        private int _sheetIndex = 0;

        public MockExcelDataReader(string data)
        {
            (string Name, List<object[]> Rows) lastSheet = (null, null);
            foreach(var row in data.Split('\n').Select(StringToRowTuple).ToList())
            {
                if (lastSheet.Name != row.sheet)
                {
                    lastSheet = (row.sheet, new List<object[]>());
                    _sheets.Add(lastSheet);
                }
                lastSheet.Rows.Add(row.fields);
            }
        }

        private object StringToObject(string str)
        {
            if (DateTime.TryParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            { return date; }
            if (double.TryParse(str, out var num))
            { return num; }
            if (bool.TryParse(str, out var b))
            { return b; }
            if (str == "[object]")
            { return new object(); }
            return str;
        }

        private (string sheet, object[] fields) StringToRowTuple(string row)
        {
            var arr = row.Split(":");
            return (arr[0], arr[1].Split(',').Select(StringToObject).ToArray());
        }

        public object this[string name] => throw new NotImplementedException();

        public string Name => _sheets[_sheetIndex].Name;

        public string CodeName => Name;

        public string VisibleState => "";

        public HeaderFooter HeaderFooter => null;

        public int ResultsCount => _sheets.Count;

        public double RowHeight => 0;

        public int Depth => 1;

        public bool IsClosed => false;

        public int RecordsAffected => _sheets[_sheetIndex].Rows.Count;

        public int FieldCount => CurrentRow.Length;

        public void Close()
        { }

        public void Dispose()
        { }

        public bool GetBoolean(int i) => (bool) this[i];

        public byte GetByte(int i) => (byte) this[i];

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i) => (char) this[i];

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i) => this[i].GetType().Name;

        public DateTime GetDateTime(int i) => (DateTime) this[i];

        public decimal GetDecimal(int i) => (decimal) this[i];

        public double GetDouble(int i) => (double) this[i];

        public Type GetFieldType(int i) => this[i].GetType();

        public float GetFloat(int i) => (float) this[i];

        public Guid GetGuid(int i) => (Guid) this[i];

        public short GetInt16(int i) => (short) this[i];

        public int GetInt32(int i) => (int) this[i];

        public long GetInt64(int i) => (long) this[i];

        public string GetName(int i) => Name;

        public string GetNumberFormatString(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i) => (string) this[i];

        public object GetValue(int i) => this[i];

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i) => this[i] == null;

        public bool NextResult()
        {
            if (_sheetIndex < _sheets.Count - 1)
            {
                _sheetIndex++;
                _index = -1;
                return true;
            }
            return false;
        }

        public bool Read()
        {
            if (_index >= _sheets[_sheetIndex].Rows.Count - 1)
            { return false; }
            ++_index;
            return true;
        }

        public void Reset()
        { }
    }
}
