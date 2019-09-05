namespace ImportOMatic3000
{
    sealed class FixedWidthRow : IRow
    {
        public int LineNumber { get; }
        public string[] Fields { get; }
        public string Sheet => null;

        public FixedWidthRow(int lineNumber, string[] fields)
        {
            LineNumber = lineNumber;
            Fields = fields;
        }

        public string GetSourceField(int fieldIndex) => GetSourceRow();
        public string GetSourceRow() => "Line " + LineNumber;
    }
}
