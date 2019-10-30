using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImportOMatic3000
{
    /*
     * TODO:
     * Paralllellllism
     * Excel Reading
     * Csv Reading
     * locale for Excel
     * Output validation
     */

    public sealed class CsvRowReader : IRowReader
    {
        Stream _stream;
        Encoding _encoding;

        public char Separator { get; }
        public char Qualifier { get; }

        public CsvRowReader(Stream stream, Encoding encoding, char separator, char qualifier)
        {
            _stream = stream;
            _encoding = encoding;
            Separator = separator;
            Qualifier = qualifier;
        }

        enum State
        {

            InField,
            InQuotedField,
            Quote,
            Separator,
        }

        private IEnumerable<string> GetFields(StreamReader sr)
        {
            var state = State.Separator;
            StringBuilder pendingField = new StringBuilder();
            do
            {
                if (state == State.InQuotedField)
                { pendingField.Append("\r\n"); }
                var line = sr.ReadLine();
                for (var i = 0; i < line.Length; ++i)
                {
                    var ch = line[i];
                    if (ch == Qualifier)
                    {
                        switch (state)
                        {
                            case State.Separator:
                                state = State.InQuotedField;
                                continue;
                            case State.Quote:
                                state = State.InQuotedField;
                                break;
                            case State.InQuotedField:
                                state = State.Quote;
                                continue;
                            case State.InField:
                                throw new CsvException(i + 1, $"Unexpected character in unqualified field: {Qualifier}.");
                        }
                    }
                    else if (ch == Separator)
                    {
                        switch (state)
                        {
                            case State.Separator:
                            case State.Quote:
                            case State.InField:
                                yield return pendingField.ToString();
                                pendingField.Clear();
                                state = State.Separator;
                                continue;
                        }
                    }
                    else if (state == State.Separator)
                    { state = State.InField; }
                    else if (state == State.Quote)
                    { throw new CsvException(i, string.Format("Expected {0}{0}, but found {0}.", Qualifier)); }
                    pendingField.Append(ch);
                }
            } while (state == State.InQuotedField && !sr.EndOfStream);

            if (state == State.Separator || state == State.Quote || state == State.InField)
            { yield return pendingField.ToString(); }
        }

        private IEnumerable<IRow> GetRows()
        {
            using (StreamReader sr = new StreamReader(_stream))
            {
                while (!sr.EndOfStream)
                {
                    yield return new CsvRow(1, GetFields(sr).ToArray());
                }
            }
        }

        public IEnumerator<IRow> GetEnumerator() => GetRows().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetRows().GetEnumerator();
        public void Dispose() => _stream?.Dispose();
    }
}
