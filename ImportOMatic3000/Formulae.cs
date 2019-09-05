using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ImportOMatic3000
{
    public static class Formulae
    {
        public static string Replace(string source, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            { throw new InvalidOperationException($"Unable to perform replace - the value to replace cannot be empty."); }
            return source.Replace(oldValue, newValue);
        }

        public static string RegexReplace(string source, string pattern, string replacement)
        {
            try
            { return Regex.Replace(source, pattern, replacement); }
            catch (ArgumentException ex)
            { throw new InvalidOperationException($"\"{pattern}\" is not a valid regular expression.", ex); }
        }

        public static string Start(string source, int count)
        {
            if (count > source.Length)
            { throw new InvalidOperationException($"Failed to get start of \"{source.Shorten(40)}\" - asked for {count} characters, but there are only {source.Length} characters available."); }
            return source.Substring(0, count);
        }

        public static string SkipStart(string source, int count)
        {
            if (count > source.Length)
            { throw new InvalidOperationException($"Unable to skip start of \"{source.Shorten(40)}\" - asked to skip {count} characters, but there are only {source.Length} characters available."); }
            return source.Substring(count);
        }

        public static string End(string source, int count)
        {
            if (count > source.Length)
            { throw new InvalidOperationException($"Failed to get end of \"{source.Shorten(40)}\" - asked for {count} characters, but there are only {source.Length} characters available."); }
            return source.Substring(source.Length - count);
        }

        public static string Substring(string source, int start, int length)
        {
            if (start <= 0 || start > source.Length)
            { throw new InvalidOperationException($"Invalid start index ({start}) for Substring of \"{source.Shorten(40)}\" - must be between one and the length of the string ({source.Length})."); }

            if (length + start > source.Length)
            { throw new InvalidOperationException($"Invalid length({length}) for Substring of \"{source.Shorten(40)}\" - the end of the requested substring ({start + length}) is past the end of the string ({source.Length})."); }
            return source.Substring(start - 1, length);
        }

        public static decimal Length(string str) => str.Length;
        public static string Trim(string source) => source.Trim();
        public static string TrimStart(string source) => source.TrimStart();
        public static string TrimEnd(string source) => source.TrimEnd();

        public static DateTime StringToDate(string value, string dateFormat, IFormatProvider formatProvider)
        {
            if (!DateTime.TryParseExact(value, dateFormat, formatProvider, DateTimeStyles.AllowWhiteSpaces, out var result))
            { throw new FormatException($"Unable to convert \"{value}\" into date using format {dateFormat}."); }
            return result;
        }

        public static decimal StringToNumber(string value, IFormatProvider numberFormat)
        {
            if (!decimal.TryParse(value, NumberStyles.Number, numberFormat, out var number))
            { throw new FormatException($"Unable to convert \"{value}\" into number."); }
            return number;
        }

        public static string NumberToString(decimal value, IFormatProvider numberFormat) => value.ToString(numberFormat);

        public static DateTime Date(int year, int month, int day) => new DateTime(year, month, day);
        public static DateTime AddDays(DateTime date, int days) => date.AddDays(days);
        public static DateTime AddMonths(DateTime date, int months) => date.AddMonths(months);
        public static DateTime AddYears(DateTime date, int years) => date.AddYears(years);

        public static string DateToString(DateTime value, string format, IFormatProvider formatProvider)
        {
            try
            { return value.ToString(format, formatProvider); }
            catch (FormatException ex)
            { throw new FormatException($"Unable to convert \"{value}\" to a string - the format \"{format}\" is not a valid date time format.", ex); }
        }

        public static bool LookupContainsKey(Dictionary<string, string> lookup, string key) => lookup.ContainsKey(key);
        public static string Lookup(Dictionary<string, string> lookup, string key)
        {
            if (lookup.TryGetValue(key, out var value))
            { return value; }
            return "";
        }
    }
}
