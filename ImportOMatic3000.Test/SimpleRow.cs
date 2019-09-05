namespace ImportOMatic3000.Test
{
    public class SimpleRow : IRow
    {
        public int LineNumber => 1;

        public string[] Fields { get; }
        public string Sheet { get; set; }
        public string GetSourceField(int fieldIndex)
        {
            return "Row 1, Column " + fieldIndex;
        }

        public string GetSourceRow() => "Row 1";

        public SimpleRow(params string[] fields)
        { Fields = fields; }
    }
}
