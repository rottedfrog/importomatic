using System;

namespace ImportOMatic3000
{
    public abstract class RowException : Exception
    {
        public IRow Row { get; }
        public RowException(string message, IRow row, Exception innerException)
            : base(message, innerException)
        {
            Row = row;
        }

        public RowException(string message, IRow row)
            : base(message)
        {
            Row = row;
        }

    }
}
