using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class CellMatcherTests
    {
        [Fact]
        public void IsMatchWithRowThatMatchesExpressionReturnsTrue()
        {
            var matcher = new CellMatcher(0, new Regex(".*"));
            Assert.True(matcher.IsMatch(new string[] { "Hello, World" }));
        }

        [Fact]
        public void IsMatchWithRowThatDoesNotMatchExpressionReturnsFalse()
        {
            var matcher = new CellMatcher(0, new Regex("xxx"));
            Assert.False(matcher.IsMatch(new string[] { "Hello, World" }));
        }

        [Fact]
        public void IsMatchWithNonZeroColumnIndexMatchesAgainstCorrectField()
        {
            var matcher = new CellMatcher(1, new Regex("xxx"));
            Assert.True(matcher.IsMatch(new string[] { "Hello, World", "xxx", "Hello, World" }));
        }

        [Fact]
        public void IsMatchWithColumnIndexTooHighReturnsFalse()
        {
            var matcher = new CellMatcher(500, new Regex(".*"));
            Assert.False(matcher.IsMatch(new string[] { "Hello, World" }));
        }

        [Fact]
        public void IsMatchWithColumnIndexNegativeReturnsFalse()
        {
            var matcher = new CellMatcher(-1, new Regex(".*"));
            Assert.False(matcher.IsMatch(new string[] { "Hello, World" }));
        }
    }
}
