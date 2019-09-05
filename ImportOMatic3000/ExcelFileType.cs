using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ImportOMatic3000
{
    public sealed class ExcelFileType : IFileType, ISheetMatching
    {
        public string Format => "Excel";
        public List<Regex> SheetMatchExpressions { get; set; }
        public IRowReader RowReader(Stream file)
        {
            return new ExcelRowReader(ExcelReaderFactory.CreateReader(file), SheetMatchExpressions);
        }
    }
}
