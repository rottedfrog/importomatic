using System;
using System.Globalization;

namespace ImportOMatic3000
{
    public class ColumnInitialValue : IInitialFieldValue
    {
        public int Column { get; }

        public ColumnInitialValue(string index)
        {
            if (int.TryParse(index,
                NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite,
                CultureInfo.InvariantCulture, out var column))
            {
                if (column == 0)
                { throw new InvalidOperationException("Column index cannot be zero"); }
                Column = column - 1;
            }
            else
            { Column = index.ToColumnIndex(); }
        }

        public string GetSourceField(IRow row) => row.GetSourceField(Column);

        public string GetValue(IRow row) => row.Fields[Column];
    }

}
