namespace ImportOMatic3000
{
    sealed class SpreadsheetRow : IRow
    {
        public string Sheet { get; }
        public int LineNumber { get; }
        public string[] Fields { get; }
        public int RowNumber { get; }

        public SpreadsheetRow(int lineNumber, int rowNumber, string sheet, string[] fields)
        {
            LineNumber = lineNumber;
            Sheet = sheet;
            Fields = fields;
            RowNumber = rowNumber;
        }

        public string GetSourceField(int fieldIndex)
        {
            return $"{Sheet}!{fieldIndex.ToColumnRef()}{RowNumber}";
        }

        public string GetSourceRow() => $"{Sheet}!{RowNumber}";
    }
}
