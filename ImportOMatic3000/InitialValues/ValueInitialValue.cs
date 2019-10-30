namespace ImportOMatic3000.Filters
{
    public class ValueInitialValue : IInitialFieldValue
    {
        private readonly string _value;

        public ValueInitialValue(string value) => _value = value;

        public string GetValue(IRow row) => _value;

        public string GetSourceField(IRow row) => $"sheet \"{row.Sheet}\"";
    }
}
