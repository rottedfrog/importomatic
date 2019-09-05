using System;

namespace ImportOMatic3000.Filters
{
    public class TruncateFilter : IStringFilter
    {
        private readonly int _count;
        public TruncateFilter(int count) => _count = count;
        public string Apply(string value) => value.Substring(0, Math.Min(value.Length, _count));
    }
}
