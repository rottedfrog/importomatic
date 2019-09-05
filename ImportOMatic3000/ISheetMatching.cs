using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ImportOMatic3000
{
    public interface ISheetMatching
    {
        List<Regex> SheetMatchExpressions { get; set; }
    }
}
