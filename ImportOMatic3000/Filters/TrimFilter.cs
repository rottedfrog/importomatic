namespace ImportOMatic3000.Filters
{
    public class TrimFilter : IStringFilter
    {
        public string Apply(string value) => value.Trim();
    }
}
