using ImportOMatic3000.Filters;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace ImportOMatic3000.Test
{

    public class LineClassTests
    {
        LineClass _lineClassWithoutSection = new LineClass()
        {
            MatchExpressions = new List<CellMatcher>
            {
                new CellMatcher(0, new Regex("Hello")),
                new CellMatcher(1, new Regex("World")),
            }
        };

        LineClass _lineClassWithSection = new LineClass()
        {
            MatchExpressions = new List<CellMatcher>
            {
                new CellMatcher(0, new Regex("Hello")),
                new CellMatcher(1, new Regex("World")),
            },
            MatchingSection = "Foo"
        };

        LineClass _lineClassWithSheets = new LineClass()
        {
            MatchExpressions = new List<CellMatcher>
            {
                new CellMatcher(0, new Regex("Hello")),
                new CellMatcher(1, new Regex("World")),
            },
            MatchingSheets = new Regex("Sheet2")
        };
        [Fact]
        public void WhenAllExpressionsMatch_IsMatch_ReturnsTrue()
        {
            Assert.True(_lineClassWithoutSection.IsMatch(new SimpleRow("Hello", "World"), null));
        }

        [Fact]
        public void WhenAllExpressionsDoNotMatch_IsMatch_ReturnsFalse()
        {
            Assert.False(_lineClassWithoutSection.IsMatch(new SimpleRow("Goodbye", "World"), null));
        }

        [Fact]
        public void WhenAllExpressionsMatchAndSectionMatches_IsMatch_ReturnsTrue()
        {
            Assert.True(_lineClassWithSection.IsMatch(new SimpleRow("Hello", "World"), "Foo"));
        }

        [Fact]
        public void WhenAllExpressionsMatchAndSectionDoesNotMatch_IsMatch_ReturnsFalse()
        {
            Assert.False(_lineClassWithSection.IsMatch(new SimpleRow("Hello", "World"), null));
        }

        [Fact]
        public void WhenSectionMatchCaseDiffers_IsMatch_ReturnsTrue()
        {
            Assert.True(_lineClassWithSection.IsMatch(new SimpleRow("Hello", "World"), "fOo"));
        }

        LineClass _lineClassWithExtracts = new LineClass
        {
            ValuesToExtract = new List<ExtractedValue> { new ExtractedValue("Foo", new ColumnInitialValue("A")) { Filters = new List<IStringFilter> { new SubstringFilter(1, 2) } }, new ExtractedValue("Bar", new ColumnInitialValue("B")) }
        };

        [Fact]
        public void ExtractValuesAddsExtractedValuesToDictionary()
        {
            var values = new SortedDictionary<string, string>();
            _lineClassWithExtracts.ExtractValues(values, new SimpleRow("Hello", "World"));
            Assert.Collection(values, x => /*Bar*/Assert.Equal("World", x.Value),
                                      x => /*Foo*/Assert.Equal("He", x.Value));
        }

        [Fact]
        public void WhenDictionaryIsNull_ExtractValues_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _lineClassWithExtracts.ExtractValues(null, new SimpleRow("Hello", "World")));
        }

        [Fact]
        public void ExtractValuesWhenValuesToExtractIsNullDoesNotThrow()
        {
            var lt = new LineClass();
            lt.ExtractValues(new Dictionary<string, string>(), new SimpleRow("Hello", "World"));
        }

        [Fact]
        public void WhenMatchSheetIsNotNullAndDoesNotMatch_IsMatch_ReturnsFalse()
        {
            Assert.False(_lineClassWithSheets.IsMatch(new SimpleRow("Hello", "World") { Sheet = "Sheet1" }, null));
        }

        [Fact]
        public void WhenMatchSheetExpressionMatchesTheSheetAndAllExpressionsMatch_IsMatch_ReturnsTrue()
        {
            Assert.True(_lineClassWithSheets.IsMatch(new SimpleRow("Hello", "World") { Sheet = "Sheet2" }, null));
        }
    }
}
