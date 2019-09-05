using System;
using System.Text.RegularExpressions;

namespace ImportOMatic3000.Filters
{
    public class MatchFilter : IStringFilter
    {
        readonly Regex _matchExpression;
        public MatchFilter(Regex matchExpression) => _matchExpression = matchExpression;

        public string Apply(string value)
        {
            var match = _matchExpression.Match(value);
            if (!match.Success)
            { throw new InvalidOperationException($"No match found in \"{value}\" using expression \"{_matchExpression}\"."); }
            return match.Value;
        }
    }
}
