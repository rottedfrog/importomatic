using System;

namespace ImportOMatic3000
{
    public class TypesNotSameException : Exception
    {
        public Type FirstType { get; }
        public Type SecondType { get; }
        public string Operation { get; }
        public TypesNotSameException(Type first, Type second, string operation)
            : base($"Type mismatch in operation '{operation}' - types must be the same. Types are '{TypeHelper.TypeName(first)}' and '{TypeHelper.TypeName(second)}'.")
        {
            FirstType = first;
            SecondType = second;
            Operation = operation;
        }

        public TypesNotSameException(Type first, Type second, string operation, string message)
            : base(message + " Types are '{TypeHelper.TypeName(first)}' and '{TypeHelper.TypeName(second)}'.")
        {
            FirstType = first;
            SecondType = second;
            Operation = operation;
        }
    }
}