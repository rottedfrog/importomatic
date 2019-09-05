using System.IO;
using System.Text;

namespace ImportOMatic3000
{
    public sealed class CsvFileType : IFileType, IDelimited, IEncodable
    {
        public string Format => "CSV";
        public char Separator { get; set; }
        public char Qualifier { get; set; }
        public Encoding Encoding { get; set; }
        public IRowReader RowReader(Stream file)
        {
            return new CsvRowReader(file, Encoding ?? Encoding.UTF8,Separator, Qualifier);
        }
    }
}
