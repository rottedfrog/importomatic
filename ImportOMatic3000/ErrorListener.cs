using System.IO;
using Antlr4.Runtime;

namespace ImportOMatic3000
{
    class ErrorListener : IAntlrErrorListener<IToken>
    {
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SyntaxException(msg);
        }
    }
}
