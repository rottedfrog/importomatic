namespace ImportOMatic3000.Filters
{
    public class TrimEndFilter : IStringFilter
    {
        public string Apply(string value) => value.TrimEnd();
    }
}
