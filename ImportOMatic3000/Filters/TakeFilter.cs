namespace ImportOMatic3000.Filters
{
    public class TakeFilter : IStringFilter
    {
        private readonly int _count;
        public TakeFilter(int count) => _count = count;
        public string Apply(string value) => Formulae.Start(value, _count);
    }
}
