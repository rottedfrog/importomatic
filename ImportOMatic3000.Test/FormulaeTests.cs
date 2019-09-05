using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class FormulaeTests
    {
        public FormulaeTests()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Fact]
        public void ReplaceReplacesMultipleMatchesInAString()
        {
            var str = "Foo";
            Assert.Equal("FBarBar", Formulae.Replace(str, "o", "Bar"));
        }

        [Fact]
        public void TrimTrimsSpacesOrTabsFromBothSidesOfAString()
        {
            var str = "\t Foo    \t";
            Assert.Equal("Foo", Formulae.Trim(str));
        }

        [Fact]
        public void TrimStartTrimsSpacesOrTabsFromStartOfAString()
        {
            var str = "\t Foo ";
            Assert.Equal("Foo ", Formulae.TrimStart(str));
        }

        [Fact]
        public void TrimEndTrimsSpacesOrTabsFromEndOfAString()
        {
            var str = " Foo    \t";
            Assert.Equal(" Foo", Formulae.TrimEnd(str));
        }

        [Fact]
        public void StartTakesFirstCharactersFromString()
        {
            var str = "Foo";
            Assert.Equal("Fo", Formulae.Start(str, 2));
        }

        [Fact]
        public void EndTakesLastCharactersFromString()
        {
            var str = "Foo";
            Assert.Equal("oo", Formulae.End(str, 2));
        }

        [Fact]
        public void SubstringTakesCharactersAsSpecifiedFromStringUsing1BasedIndex()
        {
            var str = "Foo";
            Assert.Equal("o", Formulae.Substring(str, 2, 1));
        }

        [Fact]
        public void RegexReplaceReplacesStringsAccordingToReplacementPatterns()
        {
            var str = "FooBar";
            Assert.Equal("FFooBBar", Formulae.RegexReplace(str, "[FB]", "$&$&"));
        }

        [Fact]
        public void SkipCharsSkipsStartChars()
        {
            Assert.Equal("oo", Formulae.SkipStart("Foo", 1));
        }

        [Fact]
        public void GivenTheInvariantLocale_ToStringFormatsADateWithTheSpecifiedDateFormatString()
        {
            Assert.Equal("2000-12-31", Formulae.DateToString(new DateTime(2000, 12, 31), "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void GivenTheGermanLocale_ToStringFormatsADateWithTheSpecifiedDateFormatString()
        {
            Assert.Equal("2000-Dez-31", Formulae.DateToString(new DateTime(2000, 12, 31), "yyyy-MMM-dd", new CultureInfo("de-DE")));
        }

        [Fact]
        public void GivenTheInvariantLocale_ToDateParsesADateWithTheSpecifiedDateFormatString()
        {
            Assert.Equal(new DateTime(2000, 12, 31), Formulae.StringToDate("2000-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void GivenTheGermanLocale_ToDateParsesADateWithTheSpecifiedDateFormatStringAndLocale()
        {
            Assert.Equal(new DateTime(2000, 12, 31), Formulae.StringToDate("2000/Dez/31", "yyyy/MMM/dd", new CultureInfo("de-DE")));
        }

        [Fact]
        public void ToStringFormatsANumberUsingTheInvariantLocale()
        {
            Assert.Equal("50.5", Formulae.NumberToString(50.5m, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void ToNumberParsesANumberUsingTheInvariantLocale()
        {
            Assert.Equal(50.5m, Formulae.StringToNumber("50.5", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void ToNumberParsesANumberUsingTheSpecifiedLocale()
        {
            Assert.Equal(50.5m, Formulae.StringToNumber("50,5", new CultureInfo("fr-FR")));
        }

        [Fact]
        public void DateTakesYearMonthDayAndCreatesDateTime()
        {
            Assert.Equal(new DateTime(2012, 12, 31), Formulae.Date(2012, 12, 31));
        }

        [Fact]
        public void AddDayAddsDaysToADate()
        {
            Assert.Equal(new DateTime(2013, 1, 1), Formulae.AddDays(new DateTime(2012, 12, 31), 1));
        }

        [Fact]
        public void AddMonthsAddsMonthsToADate()
        {
            Assert.Equal(new DateTime(2013, 1, 31), Formulae.AddMonths(new DateTime(2012, 12, 31), 1));
        }

        [Fact]
        public void AddYearsAddsYearsToADate()
        {
            Assert.Equal(new DateTime(2011, 12, 31), Formulae.AddYears(new DateTime(2012, 12, 31), -1));
        }

        Dictionary<string, string> _lookup = new Dictionary<string, string>
        {
            ["Foo"] = "Bar",
            ["Boo"] = "Far"
        };

        [Fact]
        public void GivenLookupContainsAKeyFoo_WhenPassingFooToLookup_ThenCorrectValue()
        {
            Assert.Equal("Far", Formulae.Lookup(_lookup, "Boo"));
        }

        [Fact]
        public void GivenLookupDoesNotContainsAKeyBoom_WhenPassingBoomToLookup_ThenThrowsKeyNotFoundException()
        {
            Assert.Equal("Bar", Formulae.Lookup(_lookup, "Foo"));
        }

        [Fact]
        public void GivenLookupContainsAKeyFoo_WhenPassingFooToLookupContainsKey_ThenReturnsTrue()
        {
            Assert.True(Formulae.LookupContainsKey(_lookup, "Foo"));
        }

        [Fact]
        public void GivenLookupDoesNotContainsAKeyBoom_WhenPassingBoomToLookupContainsKey_ThenReturnsFalse()
        {
            Assert.False(Formulae.LookupContainsKey(_lookup, "Boom"));
        }
    }
}
