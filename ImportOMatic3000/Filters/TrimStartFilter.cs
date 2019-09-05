namespace ImportOMatic3000.Filters
{
    public class TrimStartFilter : IStringFilter
    {
        public string Apply(string value) => value.TrimStart();
    }
}
