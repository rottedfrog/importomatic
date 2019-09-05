using System;
using System.Collections.Generic;

namespace ImportOMatic3000
{
    public interface IRowReader : IEnumerable<IRow>, IDisposable
    { }
}
