using System;

namespace ImportOMatic3000
{
    public class IncorrectParameterCountException : Exception
    {
        public string MethodName { get; }
        public int ExpectedCount { get; }
        public int ActualCount { get; }

        public IncorrectParameterCountException(string method, int expected, int actual)
            : base()
        {
            MethodName = method;
            ExpectedCount = expected;
            ActualCount = actual;
        }

        public override string Message
        {
            get
            {
                if (ActualCount > ExpectedCount)
                { return $"Too many parameters for function {MethodName} - expected {ExpectedCount}, but found {ActualCount}."; }
                return $"Not enough parameters for function {MethodName} - expected {ExpectedCount}, but found {ActualCount}.";
            }
        }
    }
}