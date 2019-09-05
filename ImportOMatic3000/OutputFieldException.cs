using System;
using System.Runtime.Serialization;

namespace ImportOMatic3000
{
    class OutputFieldException : RowException
    {
        private string OutputFieldName;

        public OutputFieldException(string name, IRow row, string message) : base(message, row)
        {

        }

        public OutputFieldException(string outputFieldName, IRow row, Exception ex)
            : base($"Error ({row.GetSourceRow()}) writing output field {outputFieldName}: {ex.Message}", row, ex)
        {
            this.OutputFieldName = outputFieldName;
        }
    }
}