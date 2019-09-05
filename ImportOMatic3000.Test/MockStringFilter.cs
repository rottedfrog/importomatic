namespace ImportOMatic3000.Test
{
    class MockStringFilter : IStringFilter
    {
        string _suffix;
        public MockStringFilter(string suffix) => _suffix = suffix;

        public string Apply(string s) => s + _suffix;
    }
}
