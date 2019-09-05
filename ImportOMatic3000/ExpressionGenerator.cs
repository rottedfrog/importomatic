using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ImportOMatic3000
{
    class ExpressionGenerator
    {
        public HashSet<string> ValidIdentifiers { get; set; }
        private static List<Type> SupportedTypes;
        public Dictionary<string, Dictionary<string, string>> Lookups { get; set; }
        static ExpressionGenerator()
        {
            SupportedTypes = new List<Type>()
            {
                typeof(bool),
                typeof(decimal),
                typeof(string),
                typeof(DateTime),
                typeof(int),
                typeof(IFormatProvider),
                typeof(Dictionary<string, string>)
            };
        }

        public void AddFunctions(Type type)
        {
            foreach(var methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                if (methodInfo.GetParameters().All(x => SupportedTypes.Contains(x.ParameterType)) && 
                    SupportedTypes.Contains(methodInfo.ReturnType))
                {
                    _supportedFunctions.Add(methodInfo.Name, methodInfo);
                }
            }
        }

        private readonly Dictionary<string, MethodInfo> _supportedFunctions = new Dictionary<string, MethodInfo>(StringComparer.InvariantCultureIgnoreCase);
        public IReadOnlyDictionary<string, MethodInfo> SupportedFunctions => _supportedFunctions;
        public Func<IDictionary<string, string>, object> Generate(string formula)
        {
            var lexer = new OutputExpressionLexer(new AntlrInputStream(formula));
            var parser = new OutputExpressionParser(new CommonTokenStream(lexer));
            parser.RemoveParseListeners();
            parser.AddErrorListener(new ErrorListener());
            var parameter = Expression.Parameter(typeof(IDictionary<string, string>));
            var visitor = new ExpressionVisitor(ValidIdentifiers, parameter, _supportedFunctions, Lookups);
            var expression = parser.expr().Accept(visitor);
            expression = Expression.Convert(expression, typeof(object));
            return (Func<IDictionary<string, string>, object>)Expression.Lambda(expression, parameter).Compile();
        }
    }
}
