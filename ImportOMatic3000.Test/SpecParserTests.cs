using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class SpecParserTests
    {
        SpecParser _parser = new SpecParser();

        [Fact]
        public void Parse_RecognisesTrimFilterByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import:\n  Line Classes:\n    - Foo:\n        Extract:\n          Field1: Column A | Trim"));
        }

        [Fact]
        public void Parse_RecognisesTrimStartFilterByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import:\n  Line Classes:\n    - Foo:\n        Extract:\n          Field1: Column A | TrimStart"));
        }

        [Fact]
        public void Parse_RecognisesTrimEndFilterByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import:\n  Line Classes:\n    - Foo:\n        Extract:\n          Field1: Column A | TrimEnd"));
        }

        [Fact]
        public void Parse_RecognisesMatchFilterByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import:\n  Line Classes:\n    - Foo:\n        Extract:\n          Field1: Column A | Match \"[A-Z]+\""));
        }

        [Fact]
        public void Parse_RecognisesSkipFilterByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import:\n  Line Classes:\n    - Foo:\n        Extract:\n          Field1: Column A | Skip 1"));
        }

        [Fact]
        public void Parse_RecognisesSubstringFilterByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import:\n  Line Classes:\n    - Foo:\n        Extract:\n          Field1: Column A | Substring 0 1"));
        }

        [Fact]
        public void Parse_Recognises_ReplaceFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: Replace(\"Foo\", \"Hello\", \"World\")"));
        }

        [Fact]
        public void Parse_Recognises_RegexReplaceFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: RegexReplace(\"Foo\", \"Hello\", \"World\")"));
        }

        [Fact]
        public void Parse_Recognises_SkipStartFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: SkipStart(\"Foo\", 1)"));
        }

        [Fact]
        public void Parse_Recognises_EndFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: End(\"Foo\", 1)"));
        }

        [Fact]
        public void Parse_Recognises_SubstringFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: Substring(\"Foo\", 1, 1)"));
        }

        [Fact]
        public void Parse_Recognises_TrimFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: Trim(\"Foo\")"));
        }

        [Fact]
        public void Parse_Recognises_TrimStartFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: TrimStart(\"Foo\")"));
        }

        [Fact]
        public void Parse_Recognises_TrimEndFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: TrimEnd(\"Foo\")"));
        }

        [Fact]
        public void Parse_Recognises_StartFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: Start(\"Foo\", 2)"));
        }

        [Fact]
        public void Parse_Recognises_StringToDateFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: StringToDate(\"2012-12-31\", \"dd/MM/yyyy\")"));
        }

        [Fact]
        public void Parse_Recognises_StringToNumberFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: StringToNumber(\"2\")"));
        }

        [Fact]
        public void Parse_Recognises_NumberToStringFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: NumberToString(5)"));
        }

        [Fact]
        public void Parse_Recognises_DateToStringFunctionByDefault()
        {
            var spec = _parser.Parse(new StringReader("Output:\n  - Foo: DateToString('2012-12-31', \"dd/MM/yyyy\")"));
        }

        [Fact]
        public void SpecParseRecognizesFixedFilesByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import: \n  File Type: Fixed Width"));
        }

        [Fact]
        public void SpecParseRecognizesCsvFilesByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import: \n  File Type: CSV"));
        }

        [Fact]
        public void SpecParseRecognizesExcelFilesByDefault()
        {
            var spec = _parser.Parse(new StringReader("Import: \n  File Type: Excel"));
        }
    }
}
