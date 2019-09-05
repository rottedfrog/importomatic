using ImportOMatic3000.Filters;
using ImportOMatic3000.Specs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class YamlReaderTests
    {
        [Fact]
        public void YamlSpecsReadImportObjectsAsMappings()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new NullVisitor("/Import/Foo")
            };
            using (var sr = new StringReader("Import: \n  Foo: Bar"))
            {
                var spec = reader.Parse(sr);
                Assert.NotNull(spec);
                Assert.NotNull(spec.Import);
            }
        }

        [Fact]
        public void YamlSpecsReadOutputObjectsAsMappings()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new OutputSpecVisitor(null, null),
                new NullVisitor("/Output/Foo")
            };
            using (var sr = new StringReader("Output: \n  - Foo: 1"))
            {
                var spec = reader.Parse(sr);
                Assert.NotNull(spec);
                Assert.NotNull(spec.Output);
            }
        }

        [Fact]
        public void WhenNoVisitorChecksTheNodeThrowsUnknownNodeException()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>();
            reader.Visitors = new List<IYamlNodeVisitor>();
            using (var sr = new StringReader("Output: Foo"))
            {
                var exception = Assert.Throws<UnknownNodeException>(() =>reader.Parse(sr));
                Assert.Equal("/Output", exception.YamlPath);
            }
        }

        [Fact]
        public void YamlSpecsReadFileTypeEnum()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new FileTypeVisitor(new Dictionary<string, IFileType>
                {
                    ["Fixed Width"] = new FixedWidthFileType()
                }),
            };

            using (var sr = new StringReader("Import: \n  File Type: Fixed Width"))
            {
                var result = reader.Parse(sr);
                Assert.Equal("Fixed Width", result.Import.FileType.Format);
            }
        }

        [Fact]
        public void DoubleAsteriskMatchesAllSubpathsRecursively()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new NullVisitor("**")
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n        x : Bar\n    - Bar: Foo"))
            {
                var result = reader.Parse(sr);
                //Success if it doesn't throw!
            }
        }

        [Fact]
        public void DoubleAsteriskDoesntMatchesThingsNotInTheSubpath()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new NullVisitor("/Import"),
                new NullVisitor("/Import/**")
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n      x : Bar\n    - Bar: Foo\nOutput: Boom!"))
            {
                var ex = Assert.Throws<UnknownNodeException>(() => reader.Parse(sr));
                Assert.Equal("/Output", ex.YamlPath);
            }

        }

        [Fact]
        public void YamlSpecReadsLineClassesAsOrderedList()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new NullVisitor("**")
            };

            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n        x: Bar\n    - Bar:\n        x: Foo"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Import.LineClasses, x => Assert.Equal("Foo", x.Name), x => Assert.Equal("Bar", x.Name));
            }
        }
        
        [Fact]
        public void YamlSpecReadsMatchSheetsScalarAsRegularExpression()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new MatchSheetsVisitor(),
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n        Matching Sheets: .*"))
            {
                var result = reader.Parse(sr);
                Assert.Equal(".*", result.Import.LineClasses[0].MatchingSheets.ToString());
            }
        }

        [Fact]
        public void YamlSpecsReadSheetsAsArrayOfRegularExpressions()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new FileTypeVisitor(new Dictionary<string, IFileType> { ["Excel"] = new ExcelFileType() }),
                new SheetsVisitor(),
                new SheetSequenceVisitor()
            };
            using (var sr = new StringReader("Import: \n  File Type: Excel\n  Sheets: [Sheet1, Sheet2, .*]"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(((ExcelFileType)result.Import.FileType).SheetMatchExpressions, 
                                  x => Assert.Equal("Sheet1", x.ToString()),
                                  x => Assert.Equal("Sheet2", x.ToString()),
                                  x => Assert.Equal(".*", x.ToString()));
            }
        }

        [Fact]
        public void SheetMatchExpressionsAreCreatedCaseInsensitive()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new FileTypeVisitor(new Dictionary<string, IFileType> { ["Excel"] = new ExcelFileType() }),
                new SheetsVisitor(),
                new SheetSequenceVisitor()
            };
            using (var sr = new StringReader("Import: \n  File Type: Excel\n  Sheets: [Sheet1]"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(((ExcelFileType)result.Import.FileType).SheetMatchExpressions,
                                  x => Assert.True(x.IsMatch("sheet1")));
            }
        }

        [Fact]
        public void YamlSpecsReadMatchExpressionScalarAsSingleCellMatchWithColumnIndex0()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new MatchVisitor(),
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n          Match: .*"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Import.LineClasses[0].MatchExpressions, x => Assert.Equal(0, x.ColumnIndex));
                Assert.Collection(result.Import.LineClasses[0].MatchExpressions, x => Assert.Equal(".*", x.Expression.ToString()));
            }
        }

        [Fact]
        public void YamlSpecsReadMatchExpressionMapAsListOfCellMatches()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new MatchVisitor(),
                new MatchCellsVisitor(),
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n        Match:\n          A: .*\n          C: -+"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Import.LineClasses[0].MatchExpressions, x => Assert.Equal(0, x.ColumnIndex),
                                                                               x => Assert.Equal(2, x.ColumnIndex));
                Assert.Collection(result.Import.LineClasses[0].MatchExpressions, x => Assert.Equal(".*", x.Expression.ToString()),
                                                                               x => Assert.Equal("-+", x.Expression.ToString()));
            }
        }

        [Fact]
        public void YamlSpecsReadSection()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new SectionVisitor(),
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n        Section: Monkey"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Import.LineClasses, x => Assert.Equal("Monkey", x.Section));
            }
        }

        [Fact]
        public void YamlSpecsReadMatchingSection()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new MatchingSectionVisitor(),
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n        Matching Section: Monkey"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Import.LineClasses, x => Assert.Equal("Monkey", x.MatchingSection));
            }
        }

        [Fact]
        public void YamlSpecsReadOutputRowAsBoolean()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new OutputRowVisitor(),
            };
            using (var sr = new StringReader("Import: \n  Line Classes: \n    - Foo: \n        Output Row: true"))
            {
                var result = reader.Parse(sr);
                Assert.True(result.Import.LineClasses[0].OutputLine);
            }
        }

        [Fact]
        public void YamlSpecsReadOutputFieldsInOrder()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new OutputSpecVisitor(null, null),
            };
            using (var sr = new StringReader("Output: \n  - B: 1\n  - A: 3"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Output.Fields, x => Assert.Equal("B", x.Name),
                                                        x => Assert.Equal("A", x.Name));
            }
        }

        [Fact]
        public void YamlSpecsReadOutputFieldFormulae()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new OutputSpecVisitor(null, null),
            };
            using (var sr = new StringReader("Output: \n  - A: 1 + 1"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Output.Fields, x => Assert.Equal(2m, (decimal)x.OutputFormula(null)));
            }
        }

        [Fact]
        public void WhenOutputFieldNotPresentInLineClassesOrSpecialFields_Parse_Throws_Exception()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new ExtractVisitor(),
                new ExtractableValuesVisitor(new FilterParser()),
                new OutputSpecVisitor(null, null),
            };
            using (var sr = new StringReader("Import: \n  Line Classes:\n    - Foo:\n        Extract: { Fred: Column A, Bob: Column B }\nOutput: \n  - A: Steve"))
            {
                Assert.Throws<YamlProcessingException>(() => reader.Parse(sr));
            }
        }

        [Fact]
        public void YamlSpecsReadOutputFieldFormulaeWithFieldsExtractedFromLineClassesCaseInsensitively()
        {
            var reader = new YamlSpecReader();
            var parser = new FilterParser();
            parser.AddInitialValueFactory("Column", s => new ColumnInitialValue(s[0]));
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new ExtractVisitor(),
                new ExtractableValuesVisitor(parser),
                new OutputSpecVisitor(null, null),
            };
            using (var sr = new StringReader("Import: \n  Line Classes:\n    - Foo:\n        Extract: { Fred: Column A, Bob: Column B }\nOutput: \n  - A: FreD + bob"))
            {
                var result = reader.Parse(sr);
                var parameters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
                {
                    ["Fred"] = "Foo",
                    ["Bob"] = "Bar"
                };
                Assert.Collection(result.Output.Fields, x => Assert.Equal("FooBar", (string)x.OutputFormula(parameters)));
            }
        }

        [Fact]
        public void YamlSpecsReadFieldExtractionsWithFilters()
        {
            var reader = new YamlSpecReader();
            var parser = new FilterParser();
            parser.AddFilterFactory("Trim", (s) => new TrimFilter());
            parser.AddInitialValueFactory("Column", s => new ColumnInitialValue(s[0]));
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new LineClassVisitor(),
                new IndividualLineClassVisitor(),
                new ExtractVisitor(),
                new ExtractableValuesVisitor(parser)
            };
            using (var sr = new StringReader("Import: \n  Line Classes:\n    - Foo:\n        Extract: { Fred: Column B | Trim }"))
            {
                var result = reader.Parse(sr);
                var values = result.Import.LineClasses[0].ValuesToExtract;
                Assert.Collection(values,  x => Assert.Equal("Fred", x.Name));
                Assert.Equal(1, ((ColumnInitialValue)values[0].InitialValue).Column);
                Assert.Collection(values[0].Filters, x => Assert.IsType<TrimFilter>(x));
            }
        }

        [Fact]
        public void YamlSpecsReadSeparatorAsChar()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new FileTypeVisitor(new Dictionary<string, IFileType> { ["CSV"] =  new CsvFileType() }),
                new SeparatorVisitor()
            };
            using (var sr = new StringReader("Import: \n  File Type: CSV\n  Delimited Field Separator: \",\""))
            {
                var result = reader.Parse(sr);
                Assert.Equal(',', ((CsvFileType)result.Import.FileType).Separator);
            }
        }

        [Fact]
        public void YamlSpecsReadQualifierAsChar()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new FileTypeVisitor(new Dictionary<string, IFileType> { ["CSV"] =  new CsvFileType() }),
                new QualifierVisitor()
            };
            using (var sr = new StringReader("Import: \n  File Type: CSV\n  Delimited Field Qualifier: \"\\\"\""))
            {
                var result = reader.Parse(sr);
                Assert.Equal('"', ((CsvFileType)result.Import.FileType).Qualifier);
            }
        }

        [Fact]
        public void YamlReaderWrapsVisitorExcetpionsInYamlProcessingExceptions()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new OutputSpecVisitor(null, null),
            };
            using (var sr = new StringReader("Output: \n  - A: '\"Hello\" + 1'"))
            {
                var exception = Assert.Throws<YamlProcessingException>(() => reader.Parse(sr));
            }
        }

        [Fact]
        public void GivenFileTypeIsFixed_WhenEncodingIsUTF7_SetEncodingToUTF7()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new FileTypeVisitor(new Dictionary<string, IFileType>
                {
                    ["Fixed Width"] = new FixedWidthFileType()
                }),
                new EncodingVisitor()
            };

            using (var sr = new StringReader("Import:\n  File Type: Fixed Width\n  Encoding: UTF-7"))
            {
                var result = reader.Parse(sr);
                var fileType = (FixedWidthFileType)result.Import.FileType;
                Assert.Equal("Unicode (UTF-7)", fileType.Encoding.EncodingName);
            }
        }

        [Fact]
        public void GivenFileTypeIsCsv_WhenEncodingIsUTF7_SetEncodingToUTF7()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new ImportSpecVisitor(),
                new FileTypeVisitor(new Dictionary<string, IFileType>
                {
                    ["CSV"] = new CsvFileType()
                }),
                new EncodingVisitor()
            };

            using (var sr = new StringReader("Import:\n  File Type: CSV\n  Encoding: UTF-7"))
            {
                var result = reader.Parse(sr);
                var fileType = (CsvFileType)result.Import.FileType;
                Assert.Equal("Unicode (UTF-7)", fileType.Encoding.EncodingName);
            }
        }

        [Fact]
        public void WhenSpecContainsLookups_ThenTheyAreAddedToLookupDictionary()
        {
            var reader = new YamlSpecReader();
            reader.Visitors = new List<IYamlNodeVisitor>
            {
                new LookupVisitor(),
                new LookupMapVisitor()
            };
            using (var sr = new StringReader("Lookups:\n  Foo: \n    Key1: Value1\n    Key2: Value2"))
            {
                var result = reader.Parse(sr);
                Assert.Collection(result.Lookups, 
                    map => Assert.Collection(new SortedDictionary<string, string>(map.Value),
                    kv => Assert.Equal(new KeyValuePair<string, string>("Key1", "Value1"), kv),
                    kv => Assert.Equal(new KeyValuePair<string, string>("Key2", "Value2"), kv)));
            }
        }
    }
}