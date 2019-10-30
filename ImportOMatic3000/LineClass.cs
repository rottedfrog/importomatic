using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImportOMatic3000
{
    public class LineClass
    {
        public string Name { get; set; }
        public Regex MatchingSheets { get; set; }
        public List<CellMatcher> MatchExpressions { get; set; }
        public string Section { get; set; }
        public string MatchingSection { get; set; }
        public bool OutputLine { get; set; }
        public List<ExtractedValue> ValuesToExtract { get; set; }

        public bool IsMatch(IRow row, string section) => (string.IsNullOrEmpty(MatchingSection) || MatchingSection.Equals(section, StringComparison.InvariantCultureIgnoreCase)) &&
                                                         (MatchingSheets == null || MatchingSheets.IsMatch(row.Sheet ?? "")) &&
                                                         MatchExpressions.All(expr => expr.IsMatch(row.Fields));

        public void ExtractValues(IDictionary<string, string> fieldValues, IRow row)
        {
            try
            {
                if (fieldValues == null)
                { throw new ArgumentNullException(nameof(fieldValues)); }

                if (ValuesToExtract == null)
                { return; }

                foreach (var extract in ValuesToExtract)
                {
                    fieldValues[extract.Name] = extract.Extract(row);
                }
            }
            catch (ExtractedValueException ex)
            {
                throw new LineClassException(Name, ex);
            }
        }
    }
}
