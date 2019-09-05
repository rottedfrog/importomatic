using ImportOMatic3000.Filters;
using ImportOMatic3000.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace ImportOMatic3000.Test
{

    public class LineProcessorTests
    {
        private static readonly LineClass CatchAllOutputLine = new LineClass()
        {
            MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex(".*")) },
            OutputLine = true,
            ValuesToExtract = new List<ExtractedValue> { new ExtractedValue("Foo", new ColumnInitialValue("A")) }
        };

        private static readonly LineClass TaggedSectionLine = new LineClass()
        {
            MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex("^Section:")) },
            OutputLine = false,
            Section = "Section",
            ValuesToExtract = new List<ExtractedValue>()
        };

        private static readonly LineClass SectionedCatchAllOutputLine = new LineClass()
        {
            MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex(".*")) },
            OutputLine = true,
            MatchingSection = "Section",
            ValuesToExtract = new List<ExtractedValue> { new ExtractedValue("Foo", new ColumnInitialValue("A")) }
        };

        private static readonly LineClass BarOutputLineLowerCase = new LineClass()
        {
            MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex(".*")) },
            OutputLine = true,
            ValuesToExtract = new List<ExtractedValue> { new ExtractedValue("bar", new ColumnInitialValue("A")) }
        };

        private static readonly LineClass TaggedOutputLine = new LineClass()
        {
            MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex("^Output:")) },
            OutputLine = true,
            ValuesToExtract = new List<ExtractedValue> { new ExtractedValue("Foo", new ColumnInitialValue("A")) { Filters = new List<IStringFilter> { new SkipFilter(8) } } }
        };

        private static readonly LineClass TaggedHeaderLine = new LineClass()
        {
            MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex("^Header:")) },
            OutputLine = false,
            ValuesToExtract = new List<ExtractedValue> { new ExtractedValue("Bar", new ColumnInitialValue("A")) { Filters = new List<IStringFilter> { new SkipFilter(8) } } }
        };

        private class NameComparer : IEqualityComparer<OutputField>
        {
            public bool Equals(OutputField x, OutputField y) => StringComparer.InvariantCultureIgnoreCase.Equals(x?.Name, y?.Name);
            public int GetHashCode(OutputField obj) => obj.Name.ToLowerInvariant().GetHashCode();
        }

        private static List<OutputField> IdentityFields(IEnumerable<LineClass> lineClasses)
        {
            return lineClasses.SelectMany(x => x.ValuesToExtract).Select(x => new OutputField(x.Name, y => y[x.Name])).Distinct(new NameComparer()).ToList();
        }

        LineProcessor CreateLineProcessor(List<LineClass> lineClasses, List<OutputField> fields = null)
        {
            var spec = new Spec
            {
                Import = new ImportSpec(),
                Output = new OutputSpec()
            };
            spec.Import.LineClasses = lineClasses;
            spec.Output.Fields = fields ?? IdentityFields(lineClasses);
            return new LineProcessor() { Spec = spec };
        }

        private Task<LineProcessorResults> ProcessAsync(LineProcessor processor)
        {
            var observer = new LineEventHandler();
            processor.Process(observer);
            return observer.Task;
        }

        [Fact]
        public void WhenALineIsMarkedAsAnOutputLineItsFieldsAreExtractedAndTransformed()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { CatchAllOutputLine });
            lineProcessor.RowReader = new SimpleRowReader("Hello");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(lines[0], x => Assert.Equal("Hello", x));
            Assert.Empty(errors);
        }

        [Fact]
        public void ExtractedFieldsFromALineAreCombinedWithThePreviousExtractedFieldsBeforeOutput()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { TaggedOutputLine, CatchAllOutputLine });
            lineProcessor.RowReader = new SimpleRowReader("Output: Hello", "World");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(lines, x => Assert.Equal("Hello", x[0]),
                                     x => Assert.Equal("World", x[0]));
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenALineIsNotAnOutputLineItsFieldsAreExtractedAndCombinedWithSubsequentLines()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { TaggedHeaderLine, CatchAllOutputLine });
            lineProcessor.RowReader = new SimpleRowReader("Header: Greeting", "Hello");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(lines[0], x => Assert.Equal("Greeting", x), x => Assert.Equal("Hello", x));
            Assert.Equal(1, lines.Count);
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenASpecialVariableIsAdded_ItIsAvailableInOutputFormulae()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { CatchAllOutputLine });
            lineProcessor.SpecialVariables.Add(("$Chimp", (x, y, z) => "Chimp"));
            lineProcessor.Spec.Output.Fields = new List<OutputField> { new OutputField("Foo", fields => fields["$Chimp"]) };
            lineProcessor.RowReader = new SimpleRowReader("Hello");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(lines[0], x => Assert.Equal("Chimp", x));
            Assert.Empty(errors);
        }

        [Fact]
        public void WhenAnOutputFunctionThrowsTheLineItemShouldWrapTHeExceptionAndReturnTheOutputFieldAndRowSource()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { CatchAllOutputLine });
            lineProcessor.Spec.Output.Fields = new List<OutputField> { new OutputField("Foo", x => throw new Exception("Boom!")) };
            lineProcessor.RowReader = new SimpleRowReader("Hello");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(errors, x => Assert.Equal("Boom!", x.InnermostException().Message));
            Assert.Collection(errors, x => Assert.Equal(1, x.Row.LineNumber));
        }

        [Fact]
        public void WhenALineMatchesNoLineClasses_NothingShouldBeExtractedFromThatLine()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { TaggedHeaderLine });
            lineProcessor.Spec.Output.Fields = new List<OutputField> { new OutputField("Foo", x => throw new Exception("Boom!")) };
            lineProcessor.RowReader = new SimpleRowReader("Hello", "World");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Empty(lines);
            Assert.Empty(errors);
        }

        [Fact]
        public void FieldsToExtractAreCaseInsensitive()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { TaggedHeaderLine, BarOutputLineLowerCase });
            lineProcessor.RowReader = new SimpleRowReader("Header: Greeting", "Hello");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(lines[0], x => Assert.Equal("Hello", x));
            Assert.Equal(1, lines.Count);
            Assert.Empty(errors);
        }

        [Fact]
        public void FieldsThatHaveNotBeenInitialisedAreSetToEmptyStrings()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { TaggedHeaderLine, CatchAllOutputLine });
            lineProcessor.RowReader = new SimpleRowReader("Hello");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(lines[0], x=> Assert.Equal("", x), x => Assert.Equal("Hello", x));
            Assert.Equal(1, lines.Count);
            Assert.Empty(errors);
        }

        [Fact]
        public void LineClassesThatMatchASectionDefinedEarlierAreOutputSuccessfully()
        {
            var lineProcessor = CreateLineProcessor(new List<LineClass> { TaggedSectionLine, SectionedCatchAllOutputLine });
            lineProcessor.RowReader = new SimpleRowReader("Section: Hello", "World");
            var (lines, errors) = ProcessAsync(lineProcessor).Result;
            Assert.Collection(lines[0], x => Assert.Equal("World", x));
            Assert.Equal(1, lines.Count);
            Assert.Empty(errors);
        }
    }
}
