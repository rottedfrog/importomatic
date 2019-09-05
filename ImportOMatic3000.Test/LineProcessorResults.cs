using System.Collections.Generic;

namespace ImportOMatic3000.Test
{
    class LineProcessorResults
    {
        public List<string[]> Results;
        public List<RowException> Errors;
        public void Deconstruct(out List<string[]> results, out List<RowException> errors)
        {
            results = Results;
            errors = Errors;
        }
    }
}
