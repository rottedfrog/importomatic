using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportOMatic3000.Test
{
    class LineEventHandler : ILineEventListener
    {
        private LineProcessorResults _result = new LineProcessorResults { Errors = new List<RowException>(), Results = new List<string[]>() };
        TaskCompletionSource<LineProcessorResults> _taskSource = new TaskCompletionSource<LineProcessorResults>();
        public Task<LineProcessorResults> Task => _taskSource.Task;
        public void OnError(RowException ex) => _result.Errors.Add(ex);

        public void OnOutputLine(object[] values) => _result.Results.Add(values.Select(x => x.ToString()).ToArray());

        public void OnCompleted() => _taskSource.SetResult(_result);
    }
}
