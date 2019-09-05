using System.Text;
using Xunit;

namespace ImportOMatic3000.Test
{

    public class FixedWidthRowReaderTests
    {
        [Fact]
        public void ReadsDataAsEnumerableOfLinesWithAllDataInFieldZero()
        {
            using (var rowReader = new FixedWidthRowReader("Hello\nWorld".ToStream(), Encoding.UTF8))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Hello", x.Fields[0]),
                                             x => Assert.Equal("World", x.Fields[0]));
            }
        }

        [Fact]
        public void ReadsData_WithOnlyOneFieldPerRow()
        {
            using (var rowReader = new FixedWidthRowReader("Hello\nWorld".ToStream(), Encoding.UTF8))
            {
                Assert.Collection(rowReader, x => Assert.Equal(1, x.Fields.Length),
                                             x => Assert.Equal(1, x.Fields.Length));
            }
        }

        [Fact]
        public void LineNumberIsOneBasedAndIncrementsOnEachRow()
        {
            using (var rowReader = new FixedWidthRowReader("Hello\nWorld".ToStream(), Encoding.UTF8))
            {
                Assert.Collection(rowReader, x => Assert.Equal(1, x.LineNumber),
                                             x => Assert.Equal(2, x.LineNumber));
            }
        }

        [Fact]
        public void GetSourceReturnsLineNumberAsStringWithPrefixLine()
        {
            using (var rowReader = new FixedWidthRowReader("Hello\nWorld".ToStream(), Encoding.UTF8))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Line 1", x.GetSourceRow()),
                                             x => Assert.Equal("Line 2", x.GetSourceRow()));
            }
        }

        [Fact]
        public void GetSourceFieldReturnsLineNumber()
        {
            using (var rowReader = new FixedWidthRowReader("Hello\nWorld".ToStream(), Encoding.UTF8))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Line 1", x.GetSourceField(0)),
                                             x => Assert.Equal("Line 2", x.GetSourceField(1)));
            }
        }

        [Fact]
        public void ReadsData_HandlesWindowsLineEndingsCorrectly()
        {
            using (var rowReader = new FixedWidthRowReader("Hello\r\nWorld".ToStream(), Encoding.UTF8))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Hello", x.Fields[0]),
                                             x => Assert.Equal("World", x.Fields[0]));
            }
        }
    }
}
