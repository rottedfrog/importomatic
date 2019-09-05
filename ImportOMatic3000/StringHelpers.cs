using System;
using System.Linq;
using System.Text;

namespace ImportOMatic3000
{
    static class ColumnRefHelpers
    {
        public static string Shorten(this string s, int maxlength)
        {
            if (s.Length > maxlength)
            { return s.Substring(0, maxlength - 3) + "..."; }
            return s;
        }

        public static int ToColumnIndex(this string s) 
        {
            s = s.ToUpperInvariant();
            if (s.Any(c => c < 'A' || c > 'Z'))
            { throw new InvalidOperationException($"Invalid Column reference {s}"); }
            return s.Aggregate(0, (r, c) => (r * 26) + (c - 'A') + 1) - 1;
        }

        public static string ToColumnRef(this int index)
        {
            if (index < 0)
            { throw new InvalidOperationException($"Cannot convert {index} to a column reference"); }

            StringBuilder sb = new StringBuilder();
            while (index > 25)
            {
                sb.Insert(0, (char)((index % 26) + 'A'));
                index /= 26;
                index--;
            }
            sb.Insert(0, (char)(index + 'A'));
            return sb.ToString();
        }
    }
}
