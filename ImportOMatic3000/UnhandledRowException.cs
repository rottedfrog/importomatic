using System;

namespace ImportOMatic3000
{
    /* Line processor:
     * foreach line:
     *  IsMatch
     *  ExtractFields
     *  IsOutput
     *  WriteFields
     */
    public class UnhandledRowException : RowException
    {
        public UnhandledRowException(IRow row, Exception innerException)
            : base($"Error ({row.GetSourceRow()}) - Unhandled system exception: {innerException.Message}", row, innerException)
        { }
    }
}
