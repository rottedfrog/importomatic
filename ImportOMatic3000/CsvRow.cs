using System.Runtime.CompilerServices;

namespace ImportOMatic3000
{
    public sealed class CsvRow : IRow
    {
        public int LineNumber { get; }
        public string[] Fields { get; }

        public CsvRow(int lineNumber,  string[] fields)
        {
            LineNumber = lineNumber;
            Fields = fields;
        }

        public string Sheet => null;
        public string GetSourceField(int fieldIndex) => $"Line {LineNumber}, Column {fieldIndex.ToColumnRef()}";
        public string GetSourceRow() => $"Line {LineNumber}";
    }
}
