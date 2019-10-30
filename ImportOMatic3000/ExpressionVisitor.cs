using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Antlr4.Runtime.Misc;
using static OutputExpressionParser;

namespace ImportOMatic3000
{
    class ExpressionVisitor : OutputExpressionBaseVisitor<Expression>
    {
        private readonly HashSet<string> _validIdentifiers;
        private static readonly MethodInfo StringConcat = typeof(string).GetMethod("Concat", new[] { typeof(string[]) });
        private static readonly MethodInfo StringCompare = typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string) });
        private static readonly MethodInfo DateCompare = typeof(DateTime).GetMethod("Compare", new[] { typeof(DateTime), typeof(DateTime) });
        private static readonly ConstructorInfo CultureInfoCtor = typeof(CultureInfo).GetConstructor(new[] { typeof(string) });
        private static readonly MethodInfo GetLookup = typeof(ExpressionVisitor).GetMethod("GetLookupTable", new[] { typeof(Dictionary<string, Dictionary<string, string>>), typeof(string) });
        private readonly Dictionary<string, MethodInfo> _supportedFunctions;
        private readonly Dictionary<string, Dictionary<string, string>> _lookups;

        private Dictionary<(Type From, Type To), Func<Expression, Expression>> TypeConverters;

        public ExpressionVisitor(HashSet<string> validIdentifiers, ParameterExpression fieldDictionary, Dictionary<string, MethodInfo> supportedFunctions, Dictionary<string, Dictionary<string, string>> lookups)
        {
            _validIdentifiers = validIdentifiers;
            FieldDictionaryParameter = fieldDictionary;
            _supportedFunctions = supportedFunctions;
            _lookups = lookups;
            TypeConverters = new Dictionary<(Type src, Type dest), Func<Expression, Expression>>
            {
                [(typeof(decimal), typeof(int))] = ex => Expression.Convert(ex, typeof(int)),
                [(typeof(string), typeof(IFormatProvider))] = ex => Expression.New(CultureInfoCtor, ex),
                [(typeof(string), typeof(Dictionary<string, string>))] = ex => Expression.Call(GetLookup, Expression.Constant(_lookups), ex)
            };
        }

        public ParameterExpression FieldDictionaryParameter { get; }

        public static Dictionary<string, string> GetLookupTable(Dictionary<string, Dictionary<string, string>> lookups, string table)
        {
            if (lookups.TryGetValue(table, out var result))
            { return result; }
            throw new KeyNotFoundException($"Could not find a lookup table called {table}.");
        }

        public void ThrowIfNotExpectedType(Type actual, Type expected, string operation)
        {
            if (!expected.IsAssignableFrom(actual))
            { throw new TypeMismatchException(actual, expected, operation); }
        }

        private object[] GetOptionalParameterDefaults(MethodInfo method)
        {
            var p = method.GetParameters();
            if (p.Length > 0 && p[p.Length - 1].ParameterType == typeof(IFormatProvider))
            { return new object[] { CultureInfo.InvariantCulture }; }
            return new object[0];
        }

        public override Expression VisitFunction([NotNull] FunctionContext context)
        {
            var functionName = context.GetChild(0).GetText();
            if (!_supportedFunctions.TryGetValue(functionName, out var method))
            {
                throw new UnknownFunctionException(functionName);
            }
            var paramCount = (context.ChildCount - 2) / 2;
            var parametersInfo = method.GetParameters();
            var defaults = GetOptionalParameterDefaults(method);
            var minParams = parametersInfo.Length - defaults.Length;
            if (minParams > paramCount)
            { throw new IncorrectParameterCountException(method.Name, minParams, paramCount); }
            if (parametersInfo.Length < paramCount)
            { throw new IncorrectParameterCountException(method.Name, parametersInfo.Length, paramCount); }
            var parameterExpressions = new Expression[parametersInfo.Length];
            for (int i = 0; i < parametersInfo.Length; i++)
            {
                if (i < paramCount)
                { parameterExpressions[i] = context.GetChild((i + 1) * 2).Accept(this); }
                else
                { parameterExpressions[i] = Expression.Constant(defaults[i - minParams]); }

                if (TypeConverters.TryGetValue((parameterExpressions[i].Type, parametersInfo[i].ParameterType), out var convertFunc))
                { parameterExpressions[i] = convertFunc(parameterExpressions[i]); }
                ThrowIfNotExpectedType(parameterExpressions[i].Type, parametersInfo[i].ParameterType, functionName);
            }
            return Expression.Call(null, method, parameterExpressions);
        }

        public override Expression VisitField([NotNull] FieldContext context)
        {
            var field = context.IDENTIFIER().GetText();
            if (!_validIdentifiers.Contains(field))
            { throw new KeyNotFoundException($"Field {field} not extracted from any Line Items. Please check your spec."); }
            var propertyInfo = FieldDictionaryParameter.Type.GetProperty("Item");
            return Expression.MakeIndex(FieldDictionaryParameter, propertyInfo, 
                new[] { Expression.Constant(field) });
        }

        public override Expression VisitLiteral([NotNull] LiteralContext context)
        {
            if (context.DATE() != null)
            {
                var date = context.DATE().GetText();
                return Expression.Constant(DateTime.ParseExact(date.Substring(1, date.Length - 2), "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            if (context.NUMBER() != null)
            { return Expression.Constant(decimal.Parse(context.NUMBER().GetText(), NumberStyles.Number, CultureInfo.InvariantCulture)); }
            if (context.TRUE() != null)
            { return Expression.Constant(true); }
            if (context.FALSE() != null)
            { return Expression.Constant(false); }
            var str = context.STRING().GetText();
            return Expression.Constant(str.Substring(1, str.Length - 2));
        }

        public override Expression VisitAddSubExpression([NotNull] AddSubExpressionContext context)
        {
            var param1 = context.GetChild(0).Accept(this);
            var param2 = context.GetChild(2).Accept(this);
            var operation = context.GetChild(1).GetText();

            if (operation == "-")
            {
                if (param1.Type != typeof(decimal))
                { throw new TypeMismatchException(param1.Type, typeof(decimal), operation); }
                if (param2.Type != typeof(decimal))
                { throw new TypeMismatchException(param2.Type, typeof(decimal), operation); }
                return Expression.Subtract(param1, param2);
            }

            if (param1.Type == typeof(DateTime))
            { throw new TypeException(param1.Type, operation); }

            if (param2.Type == typeof(DateTime))
            { throw new TypeException(param2.Type, operation); }

            if (param1.Type != param2.Type)
            { throw new TypesNotSameException(param1.Type, param2.Type, operation); }

            if (param1.Type == typeof(string) && param2.Type == typeof(string))
            {
                return Expression.Call(null, StringConcat, Expression.NewArrayInit(typeof(string), param1, param2));
            }
            return Expression.Add(param1, param2);
        }

        public override Expression VisitMulDivExpression([NotNull] MulDivExpressionContext context)
        {
            var param1 = context.GetChild(0).Accept(this);
            var param2 = context.GetChild(2).Accept(this);
            var operation = context.GetChild(1).GetText();

            if (param1.Type != typeof(decimal))
            { throw new TypeMismatchException(param1.Type, typeof(decimal), operation); }
            if (param2.Type != typeof(decimal))
            { throw new TypeMismatchException(param2.Type, typeof(decimal), operation); }

            if (operation == "*")
            {
                return Expression.Multiply(param1, param2);
            }
            
            return Expression.Divide(param1, param2);
        }

        public override Expression VisitBracketedExpression([NotNull] BracketedExpressionContext context)
        {
            return context.GetChild(1).Accept(this);
        }

        public override Expression VisitComparisonExpression([NotNull] ComparisonExpressionContext context)
        {
            var param1 = context.GetChild(0).Accept(this);
            var param2 = context.GetChild(2).Accept(this);
            var operation = context.GetChild(1).GetText();

            if (param1.Type != param2.Type)
            { throw new TypesNotSameException(param1.Type, param2.Type, operation); }

            if (param1.Type == typeof(string))
            {
                param1 = Expression.Call(null, StringCompare, param1, param2);
                param2 = Expression.Constant(0);
            }
            else if (param1.Type == typeof(DateTime))
            {
                param1 = Expression.Call(null, DateCompare, param1, param2);
                param2 = Expression.Constant(0);
            }
            if (operation == "<")
            { return Expression.LessThan(param1, param2); }
            if (operation == "<=")
            { return Expression.LessThanOrEqual(param1, param2); }
            if (operation == ">")
            { return Expression.GreaterThan(param1, param2); }
            return Expression.GreaterThanOrEqual(param1, param2);
        }

        public override Expression VisitIfExpression([NotNull] IfExpressionContext context)
        {
            var ifContext = context.ifexpr();
            var condition = ifContext.GetChild(1).Accept(this);
            if (condition.Type != typeof(bool))
            { throw new TypeMismatchException(condition.Type, typeof(bool), "If"); }
            var trueExpr = ifContext.GetChild(3).Accept(this);
            var falseExpr = ifContext.GetChild(5).Accept(this);
            if (trueExpr.Type != falseExpr.Type)
            { throw new TypesNotSameException(trueExpr.Type, falseExpr.Type, "If", "If result expressions must both be of the same type."); }
            return Expression.Condition(condition, trueExpr, falseExpr);
        }

        public override Expression VisitEqualityExpression([NotNull] EqualityExpressionContext context)
        {
            var param1 = context.GetChild(0).Accept(this);
            var param2 = context.GetChild(2).Accept(this);
            var operation = context.GetChild(1).GetText();

            if (param1.Type != param2.Type)
            { throw new TypesNotSameException(param1.Type, param2.Type, operation); }
            if (operation == "=")
            { return Expression.Equal(param1, param2); }
            return Expression.NotEqual(param1, param2);
        }

        public override Expression VisitOrExpression([NotNull] OrExpressionContext context)
        {
            var param1 = context.GetChild(0).Accept(this);
            var param2 = context.GetChild(2).Accept(this);
            var operation = context.GetChild(1).GetText();

            if (param1.Type != typeof(bool))
            { throw new TypeMismatchException(param1.Type, typeof(bool), operation); }
            if (param2.Type != typeof(bool))
            { throw new TypeMismatchException(param2.Type, typeof(bool), operation); }
            return Expression.Or(param1, param2);
        }

        public override Expression VisitAndExpression([NotNull] AndExpressionContext context)
        {
            var param1 = context.GetChild(0).Accept(this);
            var param2 = context.GetChild(2).Accept(this);
            var operation = context.GetChild(1).GetText();

            if (param1.Type != typeof(bool))
            { throw new TypeMismatchException(param1.Type, typeof(bool), operation); }
            if (param2.Type != typeof(bool))
            { throw new TypeMismatchException(param2.Type, typeof(bool), operation); }
            return Expression.And(param1, param2);
        }

        public override Expression VisitNotExpression([NotNull] NotExpressionContext context)
        {
            var operation = context.GetChild(0).GetText();
            var param = context.GetChild(1).Accept(this);
            if (param.Type != typeof(bool))
            { throw new TypeMismatchException(param.Type, typeof(bool), operation); }
            return Expression.Not(param);
        }
        
        public override Expression VisitNegateExpression([NotNull] NegateExpressionContext context)
        {
            var operation = context.GetChild(0).GetText();
            var param = context.GetChild(1).Accept(this);
            if (param.Type != typeof(decimal))
            { throw new TypeMismatchException(param.Type, typeof(decimal), operation); }
            return Expression.Negate(param);
        }
    }
}
