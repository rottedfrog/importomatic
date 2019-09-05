namespace ImportOMatic3000.Filters
{
    public class SubstringFilter : IStringFilter
    {
        readonly int _start, _length;

        public SubstringFilter(int start, int length)
        {
            _start = start;
            _length = length;
        }

        public string Apply(string value) => Formulae.Substring(value, _start, _length);
    }
}
