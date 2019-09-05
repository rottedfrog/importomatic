using System;

namespace ImportOMatic3000
{
    public class SyntaxException : Exception
    {
        public SyntaxException(string message)
            : base(message)
        { }
    }

    public class TypeException : Exception
    {
        public Type Type { get; }
        public string Operation { get; }
        public TypeException(Type type, string operation)
            : base($"Type error in operation '{operation}' - cannot use '{TypeHelper.TypeName(type)}' here.")
        {
            Type = type;
            Operation = operation;
        }
    }
}