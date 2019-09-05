using System;
using System.Runtime.Serialization;

namespace ImportOMatic3000
{
    public class LineClassException : ExtractedValueException
    {
        public string LineClass { get; }

        public override string Message => $"Error({InitialFieldValue.GetSourceField(Row)}, Line Type \"{LineClass}\") extracting value { ExtractedValue.Name }: {InnerException.Message}";
        public LineClassException(string lineClass, ExtractedValueException innerException) 
            : base(innerException.ExtractedValue, innerException.Row, innerException.InitialFieldValue, innerException)
        {
            LineClass = lineClass;
        }
    }
}