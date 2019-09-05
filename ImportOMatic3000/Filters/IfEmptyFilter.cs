namespace ImportOMatic3000.Filters
{
    public class IfEmptyFilter : IStringFilter
    {
        private readonly string _emptyValue;

        public IfEmptyFilter(string emptyValue) => _emptyValue = emptyValue;

        public string Apply(string value) => string.IsNullOrEmpty(value) ? _emptyValue : value;
    }
}
