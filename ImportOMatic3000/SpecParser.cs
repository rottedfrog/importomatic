using ImportOMatic3000.Specs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;


namespace ImportOMatic3000
{
    public sealed class SpecParser
    {
        static Dictionary<string, IFileType> DefaultFileTypes;
        static Dictionary<string, Func<string[], IInitialFieldValue>> DefaultInitialValues;
        static Dictionary<string, Func<string[], IStringFilter>> DefaultFilters;

        static SpecParser()
        {
            
            var emptyTypeArray = new Type[0];
            var emptyObjectArray = new object[0];
            DefaultFileTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterface("IFileType") != null)
                                                                         .Select(x => x.GetConstructor(emptyTypeArray))
                                                                         .Where(x => x != null)
                                                                         .Select(x => (IFileType)x.Invoke(emptyObjectArray))
                                                                         .ToDictionary(x => x.Format, x => x);
            var initialValueGenerator = new StringArgFactoryGenerator<IInitialFieldValue>();
            DefaultInitialValues = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterface("IInitialFieldValue") != null)
                                                                       .ToDictionary(x => Regex.Replace(x.Name, "InitialValue$", ""),
                                                                                     x => initialValueGenerator.Generate(x));
            var filterGenerator = new StringArgFactoryGenerator<IStringFilter>();
            DefaultFilters = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterface("IStringFilter") != null)
                                                                       .ToDictionary(x => Regex.Replace(x.Name, "Filter$", ""),
                                                                                     x => filterGenerator.Generate(x));
        }

        public HashSet<string> SupportedVariables { get; set; } = new HashSet<string> { "$SourceLine", "$LineNumber", "$LineMatch" };
        public Dictionary<string, IFileType> SupportedFileTypes { get; set; } = new Dictionary<string, IFileType>(DefaultFileTypes);
        public Dictionary<string, Func<string[], IInitialFieldValue>> SupportedInitialValues { get; set; } = new Dictionary<string, Func<string[], IInitialFieldValue>>(DefaultInitialValues);
        public Dictionary<string, Func<string[], IStringFilter>> SupportedFilters { get; set; } = new Dictionary<string, Func<string[], IStringFilter>>(DefaultFilters);
        public HashSet<Type> SupportedFormulaFunctions { get; set; } = new HashSet<Type> { typeof(Formulae) };

        public SpecParser()
        {
            SupportedFileTypes = DefaultFileTypes;
            SupportedFilters = DefaultFilters;
            SupportedInitialValues = DefaultInitialValues;
        }

        public Spec Parse(TextReader yamlSpec)
        {
            var filterParser = new FilterParser();
            foreach(var initialValueKV in SupportedInitialValues)
            { filterParser.AddInitialValueFactory(initialValueKV.Key, initialValueKV.Value); }
            foreach (var filterKV in SupportedFilters)
            { filterParser.AddFilterFactory(filterKV.Key, filterKV.Value); }
            var reader = new YamlSpecReader()
            {
                Visitors = new List<IYamlNodeVisitor>
                {
                    new ImportSpecVisitor(),
                    new FileTypeVisitor(SupportedFileTypes),
                    new EncodingVisitor(),
                    new SeparatorVisitor(),
                    new QualifierVisitor(),
                    new SheetsVisitor(),
                    new SheetSequenceVisitor(),
                    new LookupVisitor(),
                    new LookupMapVisitor(),
                    new LineClassVisitor(),
                    new IndividualLineClassVisitor(),
                    new OutputRowVisitor(),
                    new MatchVisitor(),
                    new MatchCellsVisitor(),
                    new MatchSheetsVisitor(),
                    new SectionVisitor(),
                    new MatchingSectionVisitor(),
                    new ExtractVisitor(),
                    new ExtractableValuesVisitor(filterParser),
                    new OutputSpecVisitor(SupportedFormulaFunctions.ToArray(), SupportedVariables)
                }
            };
            return reader.Parse(yamlSpec);
        }
    }
}
