using System;

namespace ImportOMatic3000
{
    public class TypeMismatchException : Exception
    {
        public Type Actual { get; }
        public Type Expected { get; }
        public string Operation { get; }
        public TypeMismatchException(Type actual, Type expected, string operation)
            : base($"Type mismatch in operation '{operation}' - expected '{TypeHelper.TypeName(expected)}', but found '{TypeHelper.TypeName(actual)}'.")
        {
            Actual = actual;
            Expected = expected;
            Operation = operation;
        }
    }
}