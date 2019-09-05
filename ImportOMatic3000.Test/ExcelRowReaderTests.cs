using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class ExcelRowReaderTests
    {
        [Fact]
        public void GivenSingleRowSingleSheetInput_WhenSheetMatchExpressionsIsNull_ThenReturnsSingleIRow()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello,World"))
            using (var reader = new ExcelRowReader(dataReader, null))
            {
                Assert.Collection(reader, x => Assert.Collection(x.Fields, f => Assert.Equal("Hello", f),
                                                                           f => Assert.Equal("World", f)));
            }
        }

        [Fact]
        public void GivenSingleRowSingleSheetInput_ThenReturnsSingleIRow()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello,World"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Collection(x.Fields, f => Assert.Equal("Hello", f), 
                                                                           f => Assert.Equal("World", f)));
            }
        }

        [Fact]
        public void GivenSingleRowSingleSheetInput_WhenSheetDoesntMatch_ThenReturnsEmptyEnumerable()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello,World"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[] { new Regex("NotSheet1") }))
            {
                Assert.Empty(reader);
            }
        }

        [Fact]
        public void GivenSingleRowSingleSheetInput_WhenSheetMatchesExpression_ThenReturnSingleIRow()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello,World"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[] { new Regex("Sheet1") }))
            {
                Assert.Collection(reader, x => Assert.Collection(x.Fields, f => Assert.Equal("Hello", f),
                                                                           f => Assert.Equal("World", f)));
            }
        }

        [Fact]
        public void GivenSingleRowSingleSheetInput_ThenSheetReturnsSheetName()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello,World"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Equal("Sheet1", x.Sheet));
            }
        }

        [Fact]
        public void GivenTwoRowsWithDifferentSheets_ThenSheetReturnsSheetName()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello,World\nSheet2:Foo"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Equal("Sheet1", x.Sheet)
                                        , x => Assert.Equal("Sheet2", x.Sheet));
            }
        }

        [Fact]
        public void GivenSingleRowSingleSheetInput_WhenFieldIsDate_ReturnIsoDate()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:2012-12-31"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Equal("2012-12-31", x.Fields[0]));
            }
        }

        [Fact]
        public void WhenFieldIsNumber_ReturnInvariantLocaleNumber()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:2.5"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Equal("2.5", x.Fields[0]));
            }
        }

        [Fact]
        public void WhenFieldIsBoolean_ReturnTrueOrFalse()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:TRUE,FALSE"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Collection(x.Fields, 
                                                                 f => Assert.Equal("True", f), 
                                                                 f => Assert.Equal("False", f)));
            }
        }

        [Fact]
        public void WhenFieldIsUnsupportedDataType_ThrowUnsupportedDataTypeException()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:[object]"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Throws<UnsupportedDataTypeException>(() => reader.First());
            }
        }

        [Fact]
        public void WhenReadingSingleSheetMultipleRows_ThenRowNumberIncreasesMonotonically()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello\nSheet1:World"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Equal("Sheet1!1", x.GetSourceRow()),
                                          x => Assert.Equal("Sheet1!2", x.GetSourceRow()));
            }
        }

        [Fact]
        public void WhenReadingMultipleSheets_ThenRowNumberResetsToOneAtTheStartOfEachSheet()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello\nSheet2:World"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Equal("Sheet1!1", x.GetSourceRow()),
                                          x => Assert.Equal("Sheet2!1", x.GetSourceRow()));
            }
        }

        [Fact]
        public void GivenSingleSheetSingleRows_ThenGetSourceFieldReturnsCorrectExcelReference()
        {
            using (var dataReader = new MockExcelDataReader("Sheet1:Hello,World"))
            using (var reader = new ExcelRowReader(dataReader, new Regex[0]))
            {
                Assert.Collection(reader, x => Assert.Collection(x.Fields, f => Assert.Equal("Sheet1!A1", x.GetSourceField(0)),
                                                                           f => Assert.Equal("Sheet1!B1", x.GetSourceField(1))));
            }
        }
    }
}
