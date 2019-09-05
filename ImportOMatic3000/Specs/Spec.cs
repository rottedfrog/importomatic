using System.Collections.Generic;
using System.IO;

namespace ImportOMatic3000.Specs
{
    public class Spec
    {
        public ImportSpec Import;
        public OutputSpec Output;
        public Dictionary<string, Dictionary<string, string>> Lookups; 
    }

    public class ImportSpec
    {
        public IFileType FileType { get; set; }
        public List<LineClass> LineClasses { get; set; }
    }

    public class OutputSpec
    {
        public List<OutputField> Fields;
    }
}