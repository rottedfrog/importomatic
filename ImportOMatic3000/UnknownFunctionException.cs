using System;

namespace ImportOMatic3000
{
    public class UnknownFunctionException : Exception
    {
        public UnknownFunctionException(string name)
            : base($"Unknown function '{name}'.")
        { }
    }
}
