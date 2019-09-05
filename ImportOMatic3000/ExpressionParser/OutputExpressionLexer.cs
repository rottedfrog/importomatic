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

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
partial class OutputExpressionLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, NUMBER=19, STRING=20, DATE=21, TRUE=22, FALSE=23, IDENTIFIER=24, 
		WHITESPACE=25;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "NUMBER", "STRING", "DATE", "TRUE", "FALSE", "IDENTIFIER", "WHITESPACE"
	};


	public OutputExpressionLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public OutputExpressionLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'!'", "'-'", "'('", "')'", "'*'", "'/'", "'+'", "'<'", "'>'", "'<='", 
		"'>='", "'='", "'!='", "'&'", "'|'", "'If('", "','", "'()'", null, null, 
		null, "'true'", "'false'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, "NUMBER", "STRING", "DATE", 
		"TRUE", "FALSE", "IDENTIFIER", "WHITESPACE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "OutputExpression.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static OutputExpressionLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '\x1B', '\x9A', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x3', '\x3', '\x3', '\x3', '\x4', '\x3', 
		'\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', 
		'\a', '\x3', '\a', '\x3', '\b', '\x3', '\b', '\x3', '\t', '\x3', '\t', 
		'\x3', '\n', '\x3', '\n', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', 
		'\f', '\x3', '\f', '\x3', '\f', '\x3', '\r', '\x3', '\r', '\x3', '\xE', 
		'\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', '\xF', '\x3', '\x10', 
		'\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', 
		'\x3', '\x12', '\x3', '\x12', '\x3', '\x13', '\x3', '\x13', '\x3', '\x13', 
		'\x3', '\x14', '\x6', '\x14', '\x61', '\n', '\x14', '\r', '\x14', '\xE', 
		'\x14', '\x62', '\x3', '\x14', '\x3', '\x14', '\x6', '\x14', 'g', '\n', 
		'\x14', '\r', '\x14', '\xE', '\x14', 'h', '\x5', '\x14', 'k', '\n', '\x14', 
		'\x3', '\x15', '\x3', '\x15', '\x3', '\x15', '\x3', '\x15', '\a', '\x15', 
		'q', '\n', '\x15', '\f', '\x15', '\xE', '\x15', 't', '\v', '\x15', '\x3', 
		'\x15', '\x3', '\x15', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', 
		'\x16', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', 
		'\x16', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', 
		'\x17', '\x3', '\x17', '\x3', '\x17', '\x3', '\x17', '\x3', '\x17', '\x3', 
		'\x18', '\x3', '\x18', '\x3', '\x18', '\x3', '\x18', '\x3', '\x18', '\x3', 
		'\x18', '\x3', '\x19', '\x3', '\x19', '\a', '\x19', '\x92', '\n', '\x19', 
		'\f', '\x19', '\xE', '\x19', '\x95', '\v', '\x19', '\x3', '\x1A', '\x3', 
		'\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x2', '\x2', '\x1B', '\x3', '\x3', 
		'\x5', '\x4', '\a', '\x5', '\t', '\x6', '\v', '\a', '\r', '\b', '\xF', 
		'\t', '\x11', '\n', '\x13', '\v', '\x15', '\f', '\x17', '\r', '\x19', 
		'\xE', '\x1B', '\xF', '\x1D', '\x10', '\x1F', '\x11', '!', '\x12', '#', 
		'\x13', '%', '\x14', '\'', '\x15', ')', '\x16', '+', '\x17', '-', '\x18', 
		'/', '\x19', '\x31', '\x1A', '\x33', '\x1B', '\x3', '\x2', '\a', '\x3', 
		'\x2', '\x32', ';', '\x3', '\x2', '$', '$', '\x6', '\x2', '&', '&', '\x43', 
		'\\', '\x61', '\x61', '\x63', '|', '\a', '\x2', '&', '&', '\x32', ';', 
		'\x43', '\\', '\x61', '\x61', '\x63', '|', '\x5', '\x2', '\v', '\f', '\xF', 
		'\xF', '\"', '\"', '\x2', '\x9F', '\x2', '\x3', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x5', '\x3', '\x2', '\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x13', '\x3', '\x2', '\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x17', '\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x1D', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '!', '\x3', '\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '%', '\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', 
		'\x2', '\x2', '\x2', '\x2', ')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'/', '\x3', '\x2', '\x2', '\x2', '\x2', '\x31', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\x33', '\x3', '\x2', '\x2', '\x2', '\x3', '\x35', '\x3', '\x2', 
		'\x2', '\x2', '\x5', '\x37', '\x3', '\x2', '\x2', '\x2', '\a', '\x39', 
		'\x3', '\x2', '\x2', '\x2', '\t', ';', '\x3', '\x2', '\x2', '\x2', '\v', 
		'=', '\x3', '\x2', '\x2', '\x2', '\r', '?', '\x3', '\x2', '\x2', '\x2', 
		'\xF', '\x41', '\x3', '\x2', '\x2', '\x2', '\x11', '\x43', '\x3', '\x2', 
		'\x2', '\x2', '\x13', '\x45', '\x3', '\x2', '\x2', '\x2', '\x15', 'G', 
		'\x3', '\x2', '\x2', '\x2', '\x17', 'J', '\x3', '\x2', '\x2', '\x2', '\x19', 
		'M', '\x3', '\x2', '\x2', '\x2', '\x1B', 'O', '\x3', '\x2', '\x2', '\x2', 
		'\x1D', 'R', '\x3', '\x2', '\x2', '\x2', '\x1F', 'T', '\x3', '\x2', '\x2', 
		'\x2', '!', 'V', '\x3', '\x2', '\x2', '\x2', '#', 'Z', '\x3', '\x2', '\x2', 
		'\x2', '%', '\\', '\x3', '\x2', '\x2', '\x2', '\'', '`', '\x3', '\x2', 
		'\x2', '\x2', ')', 'l', '\x3', '\x2', '\x2', '\x2', '+', 'w', '\x3', '\x2', 
		'\x2', '\x2', '-', '\x84', '\x3', '\x2', '\x2', '\x2', '/', '\x89', '\x3', 
		'\x2', '\x2', '\x2', '\x31', '\x8F', '\x3', '\x2', '\x2', '\x2', '\x33', 
		'\x96', '\x3', '\x2', '\x2', '\x2', '\x35', '\x36', '\a', '#', '\x2', 
		'\x2', '\x36', '\x4', '\x3', '\x2', '\x2', '\x2', '\x37', '\x38', '\a', 
		'/', '\x2', '\x2', '\x38', '\x6', '\x3', '\x2', '\x2', '\x2', '\x39', 
		':', '\a', '*', '\x2', '\x2', ':', '\b', '\x3', '\x2', '\x2', '\x2', ';', 
		'<', '\a', '+', '\x2', '\x2', '<', '\n', '\x3', '\x2', '\x2', '\x2', '=', 
		'>', '\a', ',', '\x2', '\x2', '>', '\f', '\x3', '\x2', '\x2', '\x2', '?', 
		'@', '\a', '\x31', '\x2', '\x2', '@', '\xE', '\x3', '\x2', '\x2', '\x2', 
		'\x41', '\x42', '\a', '-', '\x2', '\x2', '\x42', '\x10', '\x3', '\x2', 
		'\x2', '\x2', '\x43', '\x44', '\a', '>', '\x2', '\x2', '\x44', '\x12', 
		'\x3', '\x2', '\x2', '\x2', '\x45', '\x46', '\a', '@', '\x2', '\x2', '\x46', 
		'\x14', '\x3', '\x2', '\x2', '\x2', 'G', 'H', '\a', '>', '\x2', '\x2', 
		'H', 'I', '\a', '?', '\x2', '\x2', 'I', '\x16', '\x3', '\x2', '\x2', '\x2', 
		'J', 'K', '\a', '@', '\x2', '\x2', 'K', 'L', '\a', '?', '\x2', '\x2', 
		'L', '\x18', '\x3', '\x2', '\x2', '\x2', 'M', 'N', '\a', '?', '\x2', '\x2', 
		'N', '\x1A', '\x3', '\x2', '\x2', '\x2', 'O', 'P', '\a', '#', '\x2', '\x2', 
		'P', 'Q', '\a', '?', '\x2', '\x2', 'Q', '\x1C', '\x3', '\x2', '\x2', '\x2', 
		'R', 'S', '\a', '(', '\x2', '\x2', 'S', '\x1E', '\x3', '\x2', '\x2', '\x2', 
		'T', 'U', '\a', '~', '\x2', '\x2', 'U', ' ', '\x3', '\x2', '\x2', '\x2', 
		'V', 'W', '\a', 'K', '\x2', '\x2', 'W', 'X', '\a', 'h', '\x2', '\x2', 
		'X', 'Y', '\a', '*', '\x2', '\x2', 'Y', '\"', '\x3', '\x2', '\x2', '\x2', 
		'Z', '[', '\a', '.', '\x2', '\x2', '[', '$', '\x3', '\x2', '\x2', '\x2', 
		'\\', ']', '\a', '*', '\x2', '\x2', ']', '^', '\a', '+', '\x2', '\x2', 
		'^', '&', '\x3', '\x2', '\x2', '\x2', '_', '\x61', '\t', '\x2', '\x2', 
		'\x2', '`', '_', '\x3', '\x2', '\x2', '\x2', '\x61', '\x62', '\x3', '\x2', 
		'\x2', '\x2', '\x62', '`', '\x3', '\x2', '\x2', '\x2', '\x62', '\x63', 
		'\x3', '\x2', '\x2', '\x2', '\x63', 'j', '\x3', '\x2', '\x2', '\x2', '\x64', 
		'\x66', '\a', '\x30', '\x2', '\x2', '\x65', 'g', '\t', '\x2', '\x2', '\x2', 
		'\x66', '\x65', '\x3', '\x2', '\x2', '\x2', 'g', 'h', '\x3', '\x2', '\x2', 
		'\x2', 'h', '\x66', '\x3', '\x2', '\x2', '\x2', 'h', 'i', '\x3', '\x2', 
		'\x2', '\x2', 'i', 'k', '\x3', '\x2', '\x2', '\x2', 'j', '\x64', '\x3', 
		'\x2', '\x2', '\x2', 'j', 'k', '\x3', '\x2', '\x2', '\x2', 'k', '(', '\x3', 
		'\x2', '\x2', '\x2', 'l', 'r', '\a', '$', '\x2', '\x2', 'm', 'q', '\n', 
		'\x3', '\x2', '\x2', 'n', 'o', '\a', '$', '\x2', '\x2', 'o', 'q', '\a', 
		'$', '\x2', '\x2', 'p', 'm', '\x3', '\x2', '\x2', '\x2', 'p', 'n', '\x3', 
		'\x2', '\x2', '\x2', 'q', 't', '\x3', '\x2', '\x2', '\x2', 'r', 'p', '\x3', 
		'\x2', '\x2', '\x2', 'r', 's', '\x3', '\x2', '\x2', '\x2', 's', 'u', '\x3', 
		'\x2', '\x2', '\x2', 't', 'r', '\x3', '\x2', '\x2', '\x2', 'u', 'v', '\a', 
		'$', '\x2', '\x2', 'v', '*', '\x3', '\x2', '\x2', '\x2', 'w', 'x', '\a', 
		')', '\x2', '\x2', 'x', 'y', '\t', '\x2', '\x2', '\x2', 'y', 'z', '\t', 
		'\x2', '\x2', '\x2', 'z', '{', '\t', '\x2', '\x2', '\x2', '{', '|', '\t', 
		'\x2', '\x2', '\x2', '|', '}', '\a', '/', '\x2', '\x2', '}', '~', '\t', 
		'\x2', '\x2', '\x2', '~', '\x7F', '\t', '\x2', '\x2', '\x2', '\x7F', '\x80', 
		'\a', '/', '\x2', '\x2', '\x80', '\x81', '\t', '\x2', '\x2', '\x2', '\x81', 
		'\x82', '\t', '\x2', '\x2', '\x2', '\x82', '\x83', '\a', ')', '\x2', '\x2', 
		'\x83', ',', '\x3', '\x2', '\x2', '\x2', '\x84', '\x85', '\a', 'v', '\x2', 
		'\x2', '\x85', '\x86', '\a', 't', '\x2', '\x2', '\x86', '\x87', '\a', 
		'w', '\x2', '\x2', '\x87', '\x88', '\a', 'g', '\x2', '\x2', '\x88', '.', 
		'\x3', '\x2', '\x2', '\x2', '\x89', '\x8A', '\a', 'h', '\x2', '\x2', '\x8A', 
		'\x8B', '\a', '\x63', '\x2', '\x2', '\x8B', '\x8C', '\a', 'n', '\x2', 
		'\x2', '\x8C', '\x8D', '\a', 'u', '\x2', '\x2', '\x8D', '\x8E', '\a', 
		'g', '\x2', '\x2', '\x8E', '\x30', '\x3', '\x2', '\x2', '\x2', '\x8F', 
		'\x93', '\t', '\x4', '\x2', '\x2', '\x90', '\x92', '\t', '\x5', '\x2', 
		'\x2', '\x91', '\x90', '\x3', '\x2', '\x2', '\x2', '\x92', '\x95', '\x3', 
		'\x2', '\x2', '\x2', '\x93', '\x91', '\x3', '\x2', '\x2', '\x2', '\x93', 
		'\x94', '\x3', '\x2', '\x2', '\x2', '\x94', '\x32', '\x3', '\x2', '\x2', 
		'\x2', '\x95', '\x93', '\x3', '\x2', '\x2', '\x2', '\x96', '\x97', '\t', 
		'\x6', '\x2', '\x2', '\x97', '\x98', '\x3', '\x2', '\x2', '\x2', '\x98', 
		'\x99', '\b', '\x1A', '\x2', '\x2', '\x99', '\x34', '\x3', '\x2', '\x2', 
		'\x2', '\t', '\x2', '\x62', 'h', 'j', 'p', 'r', '\x93', '\x3', '\b', '\x2', 
		'\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
