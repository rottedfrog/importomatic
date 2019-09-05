using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace ImportOMatic3000.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var specParser = new SpecParser();
                var errorCount = 0;
                string outputFile = args[1] + ".output.csv";
                using (var sr = new StreamReader(args[0]))
                using (var data = new FileStream(args[1], FileMode.Open, FileAccess.Read))
                using (var output = new StreamWriter(args[1] + ".output.csv"))
                {
                    var importer = new Importer(specParser.Parse(sr));
                    var listener = new CsvOutputListener(output);
                    importer.Import(data, listener);
                    listener.Task.Wait();
                    var errors = listener.Errors;
                }
                if (errorCount > 0)
                {
                    File.Delete(outputFile);
                    WriteLine("No output created.");
                }
                else
                { WriteLine($"Output in in \"{outputFile}\""); }
            }
            catch (Exception ex)
            {
                WriteLine($"Error: {ex.Message}");
            }
#if DEBUG
            WriteLine("Press any key to continue");
            ReadKey();
#endif
        }
    }

    class CsvOutputListener : ILineEventListener
    {
        StreamWriter _writer;
        TaskCompletionSource<bool> _taskCompletion = new TaskCompletionSource<bool>();

        public Task Task => _taskCompletion.Task;
        public int Errors { get; private set; }
        public DateTime _start = DateTime.Now;
        public int Rows { get; set; }
        public void OnCompleted()
        {
            WriteLine($"Wrote {Rows} rows with {Errors} errors");
            WriteLine($"Time taken: {DateTime.Now - _start}");
            _writer.Close();
            _taskCompletion.SetResult(Errors > 0);
        }

        public CsvOutputListener(StreamWriter sw)
        {
            _writer = sw;
        }
        public void OnError(RowException ex)
        {
            Errors++;
            WriteLine(ex.Message);
        }

        public void OnOutputLine(object[] values)
        {
            string line = string.Join(',', values.Select(x => x.ToString())
                .Select(x => x.Contains(',') || x.Contains('"') ? $"\"{x.Replace("\"", "\"\"")}\"" : x));
            Rows++;
            _writer.WriteLine(line);
        }
    }
}
