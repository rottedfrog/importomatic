using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImportOMatic3000
{
    sealed class FixedWidthRowReader : IRowReader
    {
        Stream _stream;
        Encoding _encoding;
        public FixedWidthRowReader(Stream str, Encoding encoding)
        {
            _stream = str;
            _encoding = encoding;
        }

        public void Dispose() => _stream?.Dispose();

        public IEnumerable<IRow> ReadStream()
        {
            int line = 0;
            using (var reader = new StreamReader(_stream, _encoding))
            {
                var row = reader.ReadLine();
                while (row != null)
                {
                    yield return new FixedWidthRow(++line, new string[] { row });
                    row = reader.ReadLine();
                }
            }
        }

        public IEnumerator<IRow> GetEnumerator() => ReadStream().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
