using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace ImportOMatic3000.Test
{
    public static class MockFormulae
    {
        public static decimal One() => 1m;
        public static bool Not(bool b) => !b;
        public static decimal Add(decimal a, decimal b) => a + b;
        public static string AddMoo(string prefix) => prefix + "Moo";
        public static DateTime AddDay(DateTime date) => date.AddDays(1);
        public static string GoBoom() => throw new Exception("Boom!");
        public static string NumberToString(decimal number, IFormatProvider fmt) => number.ToString(fmt);
        public static string Lookup(Dictionary<string, string> lookupTable, string key) => lookupTable[key];
    }

    public static class InvalidFunctions
    {
        public static Regex ReturnRegex() => null;
        public static string RegexToString(Regex r) => r.ToString();
    }

    public class ExpressionGeneratorTests
    {
        [Fact]
        public void NumberIsAValidExpression()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("5");
            var result = func(new Dictionary<string, string>());
            Assert.Equal(5, (decimal)result);
        }
        [Fact]
        public void StringIsAValidExpression()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"hello\"");
            var result = func(new Dictionary<string, string>());
            Assert.Equal("hello", (string)result);
        }

        [Fact]
        public void DateIsAValidExpression()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("'2010-12-31'");
            var result = func(new Dictionary<string, string>());
            Assert.Equal(new DateTime(2010, 12, 31), (DateTime)result);
        }

        [Fact]
        public void IdentifierIsAValidExpression()
        {
            var generator = new ExpressionGenerator();
            generator.ValidIdentifiers = new HashSet<string> { "Hello" };
            var func = generator.Generate("Hello");
            var result = func(new Dictionary<string, string> { ["Hello"] = "5" });
            Assert.Equal("5", (string)result);
        }

        [Fact]
        public void CanAddTwoNumbersTogether()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("5 + 4");
            var result = func(null);
            Assert.Equal(9m, (decimal)result);
        }

        [Fact]
        public void CanSubtractTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("5 - 4");
            var result = func(null);
            Assert.Equal(1m, (decimal)result);
        }

        [Fact]
        public void CanConcatenateTwoStrings()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"Hello\" + \", World\"");
            var result = func(null);
            Assert.Equal("Hello, World", (string)result);
        }

        [Fact]
        public void AddWithDifferentTypesThrowsTypesNotEqualExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("\"Hello\" + 5"));
        }

        [Fact]
        public void AddOnDateTimesThrowsTypeExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypeException>(() => generator.Generate("'2012-12-31' + '2012-12-31'"));
        }


        [Fact]
        public void SubtractOnDateTimesThrowsTypeExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("'2012-12-31' - '2012-12-31'"));
            Assert.Equal(typeof(decimal), ex.Expected);
            Assert.Equal(typeof(DateTime), ex.Actual);
        }

        [Fact]
        public void SubtractOnStringThrowsExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("\"Hello\" - \"World\""));
            Assert.Equal(typeof(decimal), ex.Expected);
            Assert.Equal(typeof(string), ex.Actual);
        }

        [Fact]
        public void DivideOnDateTimesThrowsExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("'2012-12-31' / '2012-12-31'"));
            Assert.Equal(typeof(decimal), ex.Expected);
            Assert.Equal(typeof(DateTime), ex.Actual);
        }

        [Fact]
        public void DivideOnStringThrowsExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("\"Hello\" / \"World\""));
            Assert.Equal(typeof(decimal), ex.Expected);
            Assert.Equal(typeof(string), ex.Actual);
        }

        [Fact]
        public void MultiplyOnDateTimeThrowsExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("'2012-12-31' * '2012-12-31'"));
            Assert.Equal(typeof(decimal), ex.Expected);
            Assert.Equal(typeof(DateTime), ex.Actual);
        }

        [Fact]
        public void MultiplyOnStringThrowsTypeExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("\"Hello\" * \"World\""));
            Assert.Equal(typeof(decimal), ex.Expected);
            Assert.Equal(typeof(string), ex.Actual);
        }

        [Fact]
        public void CanDivideTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("1 / 10");
            var result = func(null);
            Assert.Equal(0.1m, (decimal)result);
        }

        [Fact]
        public void CanMultiplyTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("2 * 0.5");
            var result = func(null);
            Assert.Equal(1m, (decimal)result);
        }

        [Fact]
        public void GreaterThanWithDifferentTypesThrowsTypeMismatchExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("\"Hello\" > 5"));
        }

        [Fact]
        public void GreaterEqualWithDifferentTypesThrowsTypeMismatchExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("\"Hello\" >= 5"));
        }

        [Fact]
        public void LessThanWithDifferentTypesThrowsTypeMismatchExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("\"Hello\" < 5"));
        }

        [Fact]
        public void LessEqualWithDifferentTypesThrowsTypeMismatchExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("\"Hello\" <= 5"));
        }

        [Fact]
        public void GreaterOfTwoStringsDoesCaseSensitiveCompare()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"world\" > \"World\"");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void GreaterEqualOfTwoStringsDoesCaseSensitiveCompare()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"world\" >= \"World\"");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void LessOfTwoStringsDoesCaseSensitiveCompare()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"world\" < \"World\"");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void LessEqualOfTwoStringsDoesCaseSensitiveCompare()
        {

            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"world\" <= \"World\"");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void CanDoGreaterOfTwoDates()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("'2010-12-30' > '2010-12-31'");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void CanDoGreaterEqualOfTwoDates()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("'2010-12-30' >= '2010-12-31'");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void CanDoLessOfTwoDates()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("'2010-12-30' < '2010-12-31'");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void CanDoLessEqualOfTwoDates()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("'2010-12-30' <= '2010-12-31'");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void CanDoGreaterOfTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("5.3 > 8.4");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void CanDoGreaterEqualOfTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("5.3 >= 8.4");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void CanDoLessOfTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("5.3 < 8.4");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void CanDoLessEqualOfTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("5.3 <= 8.4");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void IfReturnsFirstResultWhenConditionIsTrue()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("If(1 = 1, 8, 9)");
            var result = func(null);
            Assert.Equal(8m, (decimal)result);
        }

        [Fact]
        public void IfReturnsSecondResultWhenConditionIsFalse()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("If(2 = 1, 8, 9)");
            var result = func(null);
            Assert.Equal(9m, (decimal)result);
        }


        [Fact]
        public void IfThrowsExceptionIfResultTypesAreDifferent()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("If(1=2, \"Hello\", 5)"));
        }

        [Fact]
        public void IfThrowsExceptionIfConditionisNotABooleanExpression()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("If(1, 8, 9)"));
            Assert.Equal(typeof(bool), ex.Expected);
            Assert.Equal(typeof(decimal), ex.Actual);
        }

        [Fact]
        public void IfOnlyEvaluatesTheUsedResultParameter()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var func = generator.Generate("If(1=2, GoBoom(), \"Success\")");
            var result = func(null);
            Assert.Equal("Success", result);
        }

        [Fact]
        public void EqualWithDifferentTypesThrowsTypeMismatchExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("\"Hello\" = 5"));
        }

        [Fact]
        public void CanDoEqualOnTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("2 = 1");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void CanDoEqualOnTwoStrings()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"Hello\" = \"World\"");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void CanDoEqualOnTwoDates()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("'2012-12-31' = '2010-06-05'");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void NotEqualWithDifferentTypesThrowsExceptionOnGeneration()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypesNotSameException>(() => generator.Generate("\"Hello\" != 5"));
        }

        [Fact]
        public void CanDoNotEqualOnTwoNumbers()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("2 != 1");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void CanDoNotEqualOnTwoStrings()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("\"Hello\" != \"World\"");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void CanDoNotEqualOnTwoDates()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("'2012-12-31' != '2010-06-05'");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void NotOperatorThrowsTypeExceptionIfExpressionIsNotBoolean()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<TypeMismatchException>(() => generator.Generate("!\"Hello\""));
        }

        [Fact]
        public void NotOperatorInvertsABooleanExpression()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("!(1 = 0)");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void GeneratorExposesStaticFunctionsFromSpecifiedTypes()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var func = generator.Generate("One()");
            var result = func(null);
            Assert.Equal(1m, (decimal)result);
        }

        [Fact]
        public void GeneratorExposesStaticFunctionsWithStringParameters()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var func = generator.Generate("AddMoo(\"Foo\")");
            var result = func(null);
            Assert.Equal("FooMoo", (string)result);
        }

        [Fact]
        public void GeneratorExposesStaticFunctionsWithDateParameters()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var func = generator.Generate("AddDay('2012-12-30')");
            var result = func(null);
            Assert.Equal(new DateTime(2012, 12, 31), (DateTime)result);
        }

        [Fact]
        public void GeneratorExposesStaticFunctionsWithNumberParameters()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var func = generator.Generate("Add(5, 7)");
            var result = func(null);
            Assert.Equal(12m, (decimal)result);
        }

        [Fact]
        public void GeneratorExposesStaticFunctionsWithBooleangParameters()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var func = generator.Generate("Not(\"Foo\" = \"Bar\")");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void GeneratorDoesNotExposeStaticFunctionsWithUnsupportedReturnTypes()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(InvalidFunctions));
            Assert.Throws<UnknownFunctionException>(() => generator.Generate("ReturnRegex()"));
        }

        [Fact]
        public void GeneratorDoesNotExposeStaticFunctionsWithUnsupportedTypeParameters()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(InvalidFunctions));
            Assert.Throws<UnknownFunctionException>(() => generator.Generate("RegexToString(\".*\")"));
        }

        [Fact]
        public void AndExpressionPerformsLogicalAnd()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("(1 = 0) & (1 = 1)");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void AndExpressionThrowsExceptionIfLhsIsNotBoolean()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("0 & (1 = 1)"));
            Assert.Equal(typeof(bool), ex.Expected);
            Assert.Equal(typeof(decimal), ex.Actual);
        }

        [Fact]
        public void AndExpressionThrowsExceptionIfRhsIsNotBoolean()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("(1 = 0) & 1)"));
            Assert.Equal(typeof(bool), ex.Expected);
            Assert.Equal(typeof(decimal), ex.Actual);
        }

        [Fact]
        public void OrExpressionPerformsLogicalOr()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("(1 = 0) | (1 = 1)");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void OrExpressionThrowsExceptionIfLhsIsNotBoolean()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("0 | (1 = 1)"));
            Assert.Equal(typeof(bool), ex.Expected);
            Assert.Equal(typeof(decimal), ex.Actual);
        }

        [Fact]
        public void OrExpressionThrowsExceptionIfRhsIsNotBoolean()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("(1 = 0) | 1)"));
            Assert.Equal(typeof(bool), ex.Expected);
            Assert.Equal(typeof(decimal), ex.Actual);
        }

        [Fact]
        public void SyntaxErrorsThrowSyntaxExceptions()
        {
            var generator = new ExpressionGenerator();
            Assert.Throws<SyntaxException>(() => generator.Generate("1 ="));
        }

        [Fact]
        public void CallingAFunctionWithAnInvalidParameterThrowsAnException()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("AddDay(\"Hello\")"));
            Assert.Equal(typeof(DateTime), ex.Expected);
            Assert.Equal(typeof(string), ex.Actual);
        }

        [Fact]
        public void BracketExpressionsEvaluateTheContentsOfTheBracket()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("(5)");
            var result = func(null);
            Assert.Equal(5m, (decimal)result);
        }

        [Fact]
        public void TrueReturnsTrue()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("true");
            var result = func(null);
            Assert.True((bool)result);
        }

        [Fact]
        public void FalseReturnsFalse()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("false");
            var result = func(null);
            Assert.False((bool)result);
        }

        [Fact]
        public void UnaryNegationOperatorReversesTheSignOfANumber()
        {
            var generator = new ExpressionGenerator();
            var func = generator.Generate("-(1)");
            var result = func(null);
            Assert.Equal(-1m, (decimal)result);
        }

        [Fact]
        public void UnaryNegationExpressionThrowsExceptionWhenExpressionIsNotANumber()
        {
            var generator = new ExpressionGenerator();
            var ex = Assert.Throws<TypeMismatchException>(() => generator.Generate("-\"String\""));
            Assert.Equal(typeof(decimal), ex.Expected);
            Assert.Equal(typeof(string), ex.Actual);
        }

        [Fact]
        public void FunctionsNamesAreCaseInsensitive()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            generator.Generate("ONE()");
        }

        [Fact]
        public void WhenTooManyParametersAreSupplied_ThrowsIncorrectParameterCountException()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            Assert.Throws<IncorrectParameterCountException>(() => generator.Generate("One(5)"));
        }

        [Fact]
        public void WhenTooFewParametersAreSupplied_ThrowsIncorrectParameterCountException()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            Assert.Throws<IncorrectParameterCountException>(() => generator.Generate("Not()"));
        }

        [Fact]
        public void WhenLastParametersIsAnIFormatProvider_ThenItIsOptional()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var result = (string)generator.Generate("NumberToString(5)")(null);
            Assert.Equal("5", result);
        }

        [Fact]
        public void GivenAFunctionWithAnIFormatProviderAsTheLastParameter_WhenPassingAStringAsTheLocale_ThenItConvertsTheString()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));
            var result = (string)generator.Generate("NumberToString(5.3, \"fr-FR\")")(null);
            Assert.Equal("5,3", result);
        }

        [Fact]
        public void GivenAFunctionWithAParameterOfDictionaryStringString_WhenAStringIsPassedToTheFunction_ReturnsALookupTableFromTheList()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));

            generator.Lookups = new Dictionary<string, Dictionary<string, string>>
            {
                ["FooLookup"] = new Dictionary<string, string> { ["Bar"] = "Foo" }
            };
            var result = (string)generator.Generate("Lookup(\"FooLookup\", \"Bar\")")(null);
            Assert.Equal("Foo", result);
        }

        [Fact]
        public void GivenAFunctionWithAParameterOfDictionaryStringString_WhenAnIncorrectStringIsPassedToTheFunction_ThrowsAKeyNotFoundException()
        {
            var generator = new ExpressionGenerator();
            generator.AddFunctions(typeof(MockFormulae));

            generator.Lookups = new Dictionary<string, Dictionary<string, string>>
            {
                ["FooLookup"] = new Dictionary<string, string> { ["Bar"] = "Foo" }
            };
            var func = generator.Generate("Lookup(\"NotALookup\", \"Bar\")");
            Assert.Throws<KeyNotFoundException>(() => func(null));
        }
    }
}
