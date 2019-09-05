using System;

namespace ImportOMatic3000
{
    public class ExtractedValueException : RowException
    {
        public ExtractedValue ExtractedValue { get; }
        public IInitialFieldValue InitialFieldValue { get; }
        public ExtractedValueException(ExtractedValue extractedValue, IRow row, IInitialFieldValue initialFieldValue, Exception innerException)
            : base(innerException.Message, row, innerException)
        {
            ExtractedValue = extractedValue;
            InitialFieldValue = initialFieldValue;
        }
    }
}
