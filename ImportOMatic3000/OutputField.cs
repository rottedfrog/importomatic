using System;
using System.Collections.Generic;

namespace ImportOMatic3000
{
    public class OutputField
    {
        public OutputField(string name, Func<IDictionary<string, string>, object> outputFormula)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            OutputFormula = outputFormula ?? throw new ArgumentNullException(nameof(outputFormula));
        }

        public string Name { get; }
        public Func<IDictionary<string, string>, object> OutputFormula { get; }
    }
}
