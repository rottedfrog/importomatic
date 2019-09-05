using System;
using System.Runtime.CompilerServices;

namespace ImportOMatic3000
{
    public class CsvException : Exception
    {
        public int Position { get; }
        public CsvException(int oneBasedPosition, string message)
           : base($"CSV Processing error (Position {oneBasedPosition}): {message}")
        {
            Position = oneBasedPosition;
        }
    }
}
