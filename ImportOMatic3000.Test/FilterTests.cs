using ImportOMatic3000.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class FilterTests
    {
        [Fact]
        public void SubstringFilterReturnsSubstringOfExtractedValue()
        {
            var filter = new SubstringFilter(3, 3);
            Assert.Equal("llo", filter.Apply("Hello, World"));
        }

        [Fact]
        public void TrimFilterReturnsStringWithSpacesTrimmedOnBothSides()
        {
            var filter = new TrimFilter();
            Assert.Equal("Foo", filter.Apply("   Foo        "));
        }

        [Fact]
        public void TrimStartFilterReturnsStringWithSpacesTrimmedAtStart()
        {
            var filter = new TrimStartFilter();
            Assert.Equal("Foo        ", filter.Apply("   Foo        "));
        }

        [Fact]
        public void TrimEndFilterReturnsStringWithSpacesTrimmedAtEnd()
        {
            var filter = new TrimEndFilter();
            Assert.Equal("   Foo", filter.Apply("   Foo        "));
        }

        [Fact]
        public void MatchFilterReturnsFirstSubstringMatchedByExpression()
        {
            var filter = new MatchFilter(new Regex("[a-z]+"));
            Assert.Equal("oo", filter.Apply("Foo"));
        }

        [Fact]
        public void SkipFilterSkipsTheFirstNCharacters()
        {
            var filter = new SkipFilter(1);
            Assert.Equal("oo", filter.Apply("Foo"));
        }

        [Fact]
        public void SkipFilterThrowsExceptionIfStringIsLessThanRequestedChars()
        {
            var filter = new SkipFilter(4);
            Assert.Throws<InvalidOperationException>(() => filter.Apply("Foo"));
        }

        [Fact]
        public void TakeFilterTakesTheFirstNCharacters()
        {
            var filter = new TakeFilter(1);
            Assert.Equal("F", filter.Apply("Foo"));
        }

        [Fact]
        public void TakeFilterThrowsExceptionIfStringIsLessThanRequestedChars()
        {
            var filter = new TakeFilter(4);
            Assert.Throws<InvalidOperationException>(() => filter.Apply("Foo"));
        }

        [Fact]
        public void TruncateFilterTakesNCharacters()
        {
            var filter = new TruncateFilter(1);
            Assert.Equal("F", filter.Apply("Foo"));
        }

        [Fact]
        public void TruncateFilterTakesTheWholeStringIfLessThanReguestedCharacters()
        {
            var filter = new TruncateFilter(4);
            Assert.Equal("Foo", filter.Apply("Foo"));
        }

        [Fact]
        public void ValueInitialValueTakesTheSpecifiedString()
        {
            var filter = new ValueInitialValue("Foo");
            Assert.Equal("Foo", filter.GetValue(new SimpleRow("Bar")));
        }

        [Fact]
        public void WhenParameterIsNull_IfEmptyFilterReturnsTheSpecifiedString()
        {
            var filter = new IfEmptyFilter("Foo");
            Assert.Equal("Foo", filter.Apply(null));
        }

        [Fact]
        public void WhenParameterIsEmptyString_IfEmptyFilterReturnsTheSpecifiedString()
        {
            var filter = new IfEmptyFilter("Foo");
            Assert.Equal("Foo", filter.Apply(""));
        }
    }
}
