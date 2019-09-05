using System.Text.RegularExpressions;

namespace ImportOMatic3000
{
    public class CellMatcher
    {
        public CellMatcher(int columnIndex, Regex expression)
        {
            Expression = expression;
            ColumnIndex = columnIndex;
        }

        public Regex Expression { get; set; }
        public int ColumnIndex { get; set; }
        public bool IsMatch(string[] row)
        {
            return ColumnIndex >= 0 && ColumnIndex < row.Length && Expression.IsMatch(row[ColumnIndex]);
        }
    }
}
