using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class ColumnHelperTests
    {
        [Fact]
        public void WhenIntIs701_ReturnZZ() => Assert.Equal("ZZ", 701.ToColumnRef());

        [Fact]
        public void WhenIntIs702_ReturnAAA() => Assert.Equal("AAA", 702.ToColumnRef());

        [Fact]
        public void WhenIntIs26_ReturnAA() => Assert.Equal("AA", 26.ToColumnRef());

        [Fact]
        public void WhenIntIs0_ReturnA() => Assert.Equal("A", 0.ToColumnRef());

        [Fact]
        public void WhenRefIsA_Return0() => Assert.Equal(0, "A".ToColumnIndex());

        [Fact]
        public void WhenRefIsAA_Return26() => Assert.Equal(26, "AA".ToColumnIndex());

        [Fact]
        public void WhenRefIsZZ_Return701() => Assert.Equal(701, "ZZ".ToColumnIndex());

        [Fact]
        public void WhenRefIsAAA_Return702() => Assert.Equal(702, "AAA".ToColumnIndex());

    }
}
