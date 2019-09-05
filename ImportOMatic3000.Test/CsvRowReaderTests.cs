using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class CsvRowReaderTests
    {
        [Fact]
        public void WhenDataIsSeperatedByCommas_ReadsAsSeperateFields()
        {
            using (var rowReader = new CsvRowReader("Hello,World".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader, x => Assert.Collection(x.Fields, 
                    f => Assert.Equal("Hello", f), 
                    f => Assert.Equal("World", f)));
            }
        }

        [Fact]
        public void NewLineMarksEndOfRow()
        {
            using (var rowReader = new CsvRowReader("Hello\r\nWorld".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Hello", x.Fields[0]),
                                             x => Assert.Equal("World", x.Fields[0]));
            }
        }

        [Fact]
        public void NewLineAtEndOfStreamIsIgnored()
        {
            using (var rowReader = new CsvRowReader("Hello\r\n".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Hello", x.Fields[0]));
            }
        }

        [Fact]
        public void WhenFieldIsInQuotes_StripsTheQuotes()
        {
            using (var rowReader = new CsvRowReader("\"Hello\"".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Hello", x.Fields[0]));
            }
        }

        [Fact]
        public void WhenFieldIsInQuotes_ReplacesTwoQuotesWithASingleQuote()
        {
            using (var rowReader = new CsvRowReader("\"Hello \"\"World\"\"\"".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Hello \"World\"", x.Fields[0]));
            }
        }

        [Fact]
        public void GivenFieldIsNotInQuotes_WhenItContainsAQuote_ThrowsCsvException()
        {
            using (var rowReader = new CsvRowReader("Hello \"World".ToStream(), Encoding.UTF8, ',', '"'))
            {
                var ex = Assert.Throws<CsvException>(() => rowReader.First());
                Assert.Equal(7, ex.Position);
            }
        }

        [Fact]
        public void WhenFieldIsInQuotes_AndContainsOneQuoteAloneAndNotAtEndOfField_ThrowsCsvException()
        {
            using (var rowReader = new CsvRowReader("\"Hello \"World\"".ToStream(), Encoding.UTF8, ',', '"'))
            {
                var ex = Assert.Throws<CsvException>(() => rowReader.First());
                Assert.Equal(8, ex.Position);
            }
        }

        [Fact]
        public void WhenFieldIsInQuotes_NewLineIsReadAsPartOfTheField()
        {
            using (var rowReader = new CsvRowReader("\"Hello\r\nWorld\"".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader, x => Assert.Equal("Hello\r\nWorld", x.Fields[0]));
            }
        }

        [Fact]
        public void WhenRightHandFieldIsEmpty_ReturnsEmptyField()
        {
            using (var rowReader = new CsvRowReader("Hello,".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader.First().Fields, x => Assert.Equal("Hello", x),
                                             x => Assert.Equal("", x));
            }
        }

        [Fact]
        public void WhenLeftHandFieldIsEmpty_ReturnsEmptyField()
        {
            using (var rowReader = new CsvRowReader(",World".ToStream(), Encoding.UTF8, ',', '"'))
            {
                Assert.Collection(rowReader.First().Fields, x => Assert.Equal("", x),
                                             x => Assert.Equal("World", x));
            }
        }

        [Fact]
        public void WhenPassingHashQualifierAndTildeSeparator_ThenParsesRecordCorrectly()
        {
            using (var rowReader = new CsvRowReader("Hello~World~#Field~with~tildes#".ToStream(), Encoding.UTF8, '~', '#'))
            {
                Assert.Collection(rowReader, x => Assert.Collection(x.Fields,
                    f => Assert.Equal("Hello", f),
                    f => Assert.Equal("World", f),
                    f => Assert.Equal("Field~with~tildes", f)));
            }
        }
    }
}
