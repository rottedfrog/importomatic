using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ImportOMatic3000.Test
{
    public class SimpleRowReader : IRowReader
    {
        string[] _lines;
        public void Dispose() { }
        public SimpleRowReader(params string[] lines) =>_lines = lines;
        public IEnumerator<IRow> GetEnumerator() => _lines.Select(x => new SimpleRow(x.Split(','))).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
