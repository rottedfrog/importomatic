//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ExpressionParser\OutputExpression.g4 by ANTLR 4.7.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="OutputExpressionParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
interface IOutputExpressionVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by the <c>IfExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfExpression([NotNull] OutputExpressionParser.IfExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>LiteralExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralExpression([NotNull] OutputExpressionParser.LiteralExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BracketedExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBracketedExpression([NotNull] OutputExpressionParser.BracketedExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>NotExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNotExpression([NotNull] OutputExpressionParser.NotExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>OrExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrExpression([NotNull] OutputExpressionParser.OrExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>FieldExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldExpression([NotNull] OutputExpressionParser.FieldExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>NegateExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNegateExpression([NotNull] OutputExpressionParser.NegateExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>AddSubExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddSubExpression([NotNull] OutputExpressionParser.AddSubExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ComparisonExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisonExpression([NotNull] OutputExpressionParser.ComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>FunctionExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionExpression([NotNull] OutputExpressionParser.FunctionExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>AndExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAndExpression([NotNull] OutputExpressionParser.AndExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>EqualityExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEqualityExpression([NotNull] OutputExpressionParser.EqualityExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MulDivExpression</c>
	/// labeled alternative in <see cref="OutputExpressionParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMulDivExpression([NotNull] OutputExpressionParser.MulDivExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="OutputExpressionParser.ifexpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfexpr([NotNull] OutputExpressionParser.IfexprContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="OutputExpressionParser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunction([NotNull] OutputExpressionParser.FunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="OutputExpressionParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] OutputExpressionParser.LiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="OutputExpressionParser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitField([NotNull] OutputExpressionParser.FieldContext context);
}
