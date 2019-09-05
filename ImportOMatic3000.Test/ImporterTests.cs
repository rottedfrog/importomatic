using ImportOMatic3000.Specs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class ImporterTests
    {
        private Spec Spec { get; set; }

        public ImporterTests()
        {
            Spec = new Spec();
            Spec.Import = new ImportSpec();
            Spec.Output = new OutputSpec();
            Spec.Import.FileType = new FixedWidthFileType();
            Spec.Import.LineClasses = new List<LineClass>();
            Spec.Import.LineClasses.Add(new LineClass
            {
                MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex(".*")) },
                OutputLine = true,
                Name = "Foo",
                ValuesToExtract = new List<ExtractedValue>()
            });
        }

        private LineProcessorResults DoImport(string importData, params Func<IDictionary<string, string>, object>[] outputFunctions)
        {
            Spec.Output.Fields = outputFunctions.Select((f, i) => new OutputField(i.ToColumnRef(), f)).ToList();
            var importer = new Importer(Spec);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(importData)))
            {
                var listener = new LineEventHandler();
                importer.Import(stream, listener);
                return listener.Task.Result;
            }
        }

        [Fact]
        public void ImporterProcessesLineNumber()
        {
            var results = DoImport("Hello\nWorld", fields => fields["$LineNumber"]);
            Assert.Collection(results.Results, x => Assert.Equal("1", x[0]), x => Assert.Equal("2", x[0]));
        }

        [Fact]
        public void ImporterProcessesLineMatch()
        {
            var results = DoImport("Hello", fields => fields["$LineMatch"]);
            Assert.Collection(results.Results, x => Assert.Equal("Foo", x[0]));
        }

        [Fact]
        public void ImporterProcessesSourceLine()
        {
            var results = DoImport("Hello\nWorld", fields => fields["$SourceLine"]);
            Assert.Collection(results.Results, x => Assert.Equal("Line 1", x[0]), x => Assert.Equal("Line 2", x[0]));
        }
    }
}
