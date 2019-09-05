using ImportOMatic3000.Filters;
using System;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class FilterParserTests
    {
        FilterParser filterParser = new FilterParser();

        public FilterParserTests()
        {
            filterParser.AddFilterFactory("Trim", s => new TrimFilter());
            filterParser.AddFilterFactory("TrimStart", s => new TrimStartFilter());
            filterParser.AddFilterFactory("TrimEnd", s => new TrimEndFilter());
            filterParser.AddFilterFactory("Substring", s => new SubstringFilter(int.Parse(s[0]), int.Parse(s[1])));
            filterParser.AddFilterFactory("Append", s => new MockStringFilter(s[0]));
            filterParser.AddInitialValueFactory("Column", s => new ColumnInitialValue(s[0]));
            filterParser.AddInitialValueFactory("Sheet", s => new SheetInitialValue());
        }

        [Fact]
        public void FiltersAreNotCaseSensitive()
        {
            var (column, result) = filterParser.Parse("triM");
            Assert.Collection(result, x => Assert.IsType<TrimFilter>(x));
        }

        [Fact]
        public void ParseTrimFilterReturnsTrimFilter()
        {
            var (column, result) = filterParser.Parse("Trim");
            Assert.Collection(result, x => Assert.IsType<TrimFilter>(x));
        }

        [Fact]
        public void WhenFilterStringContainsTwoFilters_ParseReturnsTwoFilters()
        {
            var (column, result) = filterParser.Parse("TrimStart | TrimEnd");
            Assert.Collection(result, x => Assert.IsType<TrimStartFilter>(x),
                                      x => Assert.IsType<TrimEndFilter>(x));
        }

        [Fact]
        public void WhenFilterStringContainsParameters_TheyArePassedToTheFilterFunction()
        {
            var (column, result) = filterParser.Parse("Substring 2 4");
            Assert.Equal("ooBa", result[0].Apply("FooBar"));
        }
 
        [Fact]
        public void WhenFilterStringContainsEmptyFilterExpression_TheyAreNotIncludedInResult()
        {
            var (column, result) = filterParser.Parse("TrimStart |    |   ||TrimEnd");
            Assert.Collection(result, x => Assert.IsType<TrimStartFilter>(x),
                                      x => Assert.IsType<TrimEndFilter>(x));
        }

        [Fact]
        public void WhenFilterStringIsQuoted_ItMayContainWhitespaceAndSpecialCharacters()
        {
            var (column, result) = filterParser.Parse("Append \"|, \\\"\\\\\"");
            Assert.Collection(result, x => Assert.Equal("|, \"\\", x.Apply("")));
        }

        [Fact]
        public void WhenNoColumnFilterIsSpecified_ParseReturnsZeroColumn()
        {
            var (column, result) = filterParser.Parse("Trim");
            Assert.Equal(0, ((ColumnInitialValue)column).Column);
        }

        [Fact]
        public void WhenColumnIsSpecifiedAtBeginning_FilterReturnsSpecifiedColumn()
        {
            var (column, result) = filterParser.Parse("Column E | Trim");
            Assert.Equal(4, ((ColumnInitialValue)column).Column);
            Assert.Collection(result, x => Assert.IsType<TrimFilter>(x));
        }

        [Fact]
        public void WhenColumnIsSpecifiedWithIntegerNumber_FilterReturnsIndexAsNumberLessOne()
        {
            var (column, result) = filterParser.Parse("Column 3");
            Assert.Equal(2, ((ColumnInitialValue)column).Column);
        }

        [Fact]
        public void WhenColumnIsSpecifiedAsZero_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => filterParser.Parse("Column 0"));
        }

        [Fact]
        public void WhenColumnIsSpecifiedWithNonAlphaOrNonNumeric_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => filterParser.Parse("Column $"));
            Assert.Throws<FormatException>(() => filterParser.Parse("Column A1"));
            Assert.Throws<FormatException>(() => filterParser.Parse("Column 1A"));
        }

        [Fact]
        public void WhenColumnIsSpecifiedWithLowercase_ReturnsSpecifiedColumn()
        {
            var (column, filters) = filterParser.Parse("Column z");
            Assert.Equal(25, ((ColumnInitialValue) column).Column);
        }

        [Fact]
        public void WhenAStringIsUnterminated_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => filterParser.Parse("Append \"Hello"));
        }

        [Fact]
        public void WhenColumnIsSpecified_ButNotFirst_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => filterParser.Parse("Trim | Column E"));
        }

        [Fact]
        public void WhenSheetIsSpecified_ButNotFirst_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => filterParser.Parse("Trim | Sheet"));
        }

        [Fact]
        public void WhenSheetIsSpecifiedFirst_ReturnsSheetName()
        {
            var (initialValue, filters) = filterParser.Parse("Sheet");
            var testRow = new SimpleRow("Bar") { Sheet = "Foo" };
            Assert.Equal("Foo", initialValue.GetValue(testRow));
        }
    }
}
