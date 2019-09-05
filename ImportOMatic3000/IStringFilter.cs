using System;
using System.Text.RegularExpressions;

namespace ImportOMatic3000
{
    public interface IStringFilter
    {
        string Apply(string value);
    }
}
