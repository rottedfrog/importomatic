using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportOMatic3000
{

    public class FilterParser
    {
        readonly Dictionary<string, Func<string[], IStringFilter>> _filterFactories = new Dictionary<string, Func<string[], IStringFilter>>(StringComparer.InvariantCultureIgnoreCase);
        readonly Dictionary<string, Func<string[], IInitialFieldValue>> _initialFieldValueFactories = new Dictionary<string, Func<string[], IInitialFieldValue>>(StringComparer.InvariantCultureIgnoreCase);
        public FilterParser()
        { }

        public void AddInitialValueFactory(string name, Func<string[], IInitialFieldValue> initialFieldValueFactory)
        {
            _initialFieldValueFactories.Add(name, initialFieldValueFactory);
        }

        public void AddFilterFactory(string name, Func<string[], IStringFilter> filterFactory)
        {
            _filterFactories.Add(name, filterFactory);
        }

        enum State
        {
            Begin,
            InQuote,
            InToken,
            Escaped,
        }

        private string CleanEscapes(string s)
        {
            bool escaped = false;
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!escaped && c == '\\')
                { escaped = true; }
                else
                {
                    sb.Append(c);
                    escaped = false;
                }
            }
            return sb.ToString();
        }

        private IEnumerable<(string name, string[] parameters)> ParseFilterString(string filterString)
        {
            var filters = new List<(string name, string[] parameters)>();
            var state = State.Begin;
            var tokenStart = 0;
            var tokens = new List<string>();
            for (var i = 0; i < filterString.Length; ++i)
            {
                if (state == State.Escaped)
                {
                    state = State.InQuote;

                    continue;
                }
                switch (filterString[i])
                {
                    case '\\':
                        if (state == State.InQuote)
                        { state = State.Escaped; }
                        break;
                    case ' ':
                        if (state == State.InQuote)
                        { continue; }
                        if (state == State.InToken)
                        { tokens.Add(filterString.Substring(tokenStart, i - tokenStart)); }
                        state = State.Begin;
                        break;
                    case '|':
                        if (state == State.InQuote)
                        { continue; }
                        if (state == State.InToken)
                        { tokens.Add(filterString.Substring(tokenStart, i - tokenStart)); }
                        state = State.Begin;
                        if (tokens.Count > 0)
                        {
                            yield return (tokens[0], tokens.Skip(1).ToArray());
                            tokens.Clear();
                        }
                        break;
                    case '"':
                        if (state == State.InQuote)
                        {
                            tokens.Add(CleanEscapes(filterString.Substring(tokenStart, i - tokenStart)));
                            state = State.Begin;
                        }
                        else if (state == State.Begin)
                        {
                            state = State.InQuote;
                            tokenStart = i + 1;
                        }
                        break;
                    default:
                        if (state == State.Begin)
                        {
                            state = State.InToken;
                            tokenStart = i;
                        }
                        break;
                }
            }
            if (state == State.InToken)
            { tokens.Add(filterString.Substring(tokenStart, filterString.Length - tokenStart)); }
            if (state == State.InQuote)
            { throw new InvalidOperationException($"Unterminated string starting at position {tokenStart}"); }
            yield return (tokens[0], tokens.Skip(1).ToArray());

        }

        public (IInitialFieldValue, List<IStringFilter>) Parse(string filterString)
        {
            string key = null;
            try
            {
                var filters = new List<IStringFilter>();
                var parseResult = ParseFilterString(filterString);
                var filterTokens = parseResult.First();
                IInitialFieldValue initialValue;
                if (_initialFieldValueFactories.TryGetValue(filterTokens.name, out var initialValueFactory))
                {
                    initialValue = initialValueFactory(filterTokens.parameters);
                    parseResult = parseResult.Skip(1);
                }
                else
                { initialValue = new ColumnInitialValue("A"); }
                return (initialValue, parseResult.Select(s => _filterFactories[key = s.name](s.parameters)).ToList());
            }
            catch (KeyNotFoundException ex)
            {
                if (_initialFieldValueFactories.ContainsKey(key))
                { throw new InvalidOperationException($"{key} must be specified at the start of a filter string", ex); }
                throw new InvalidOperationException($"No filter found with the name \"{key}\".", ex);
            }
            catch (Exception ex)
            {
                throw new FormatException(key + " - " + ex.Message, ex);
            }
        }
    }

}
