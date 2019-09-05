using ImportOMatic3000.Specs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImportOMatic3000
{
    public sealed class Importer
    {
        public Spec Spec { get; set; }

        private List<(string Name, Func<Spec, IRow, LineClass, string> Create)> SpecialVariables = new List<(string Name, Func<Spec, IRow, LineClass, string> Create)>
        {
            ("$SourceLine", (spec, row, lt) => row.GetSourceRow()),
            ("$LineNumber", (spec, row, lt) => row.LineNumber.ToString()),
            ("$LineMatch", (spec, row, lt) => lt.Name),
        };

        static Importer()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public Importer(Spec s)
        {
            Spec = s;
        }

        public void Import(Stream s, ILineEventListener listener)
        {
            if (Spec == null)
            { throw new InvalidOperationException("No Specification has been set"); }
            if (s == null)
            { throw new ArgumentNullException(nameof(s)); }
            if (listener == null)
            { throw new ArgumentNullException(nameof(listener)); }

            var processor = new LineProcessor()
            {
                RowReader = Spec.Import.FileType.RowReader(s),
                Spec = Spec
            };
            processor.SpecialVariables = SpecialVariables;
            processor.Process(listener);
        }
    }
}
