using ImportOMatic3000.Specs;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImportOMatic3000
{
    public class FixedWidthFileType : IFileType, IEncodable, IYamlVisitorProvider
    {
        public string Format => "Fixed Width";

        public Encoding Encoding { get; set; }

        public IEnumerable<IYamlNodeVisitor> GetVisitors() => new IYamlNodeVisitor[] { new EncodingVisitor() };

        public IRowReader RowReader(Stream stream)
        {
            return new FixedWidthRowReader(stream, Encoding ?? Encoding.UTF8);
        }
    }
}
