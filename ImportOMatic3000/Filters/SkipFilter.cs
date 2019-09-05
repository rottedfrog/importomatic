namespace ImportOMatic3000.Filters
{
    public class SkipFilter : IStringFilter
    {
        private readonly int _count;
        public SkipFilter(int count) => _count = count;
        public string Apply(string value) => Formulae.SkipStart(value, _count);
    }
}
