using System;
using System.IO;
using Antlr4.Runtime;

namespace Jogo.Core
{
    public class ErroSintaxeListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol,
            int line, int charPositionInLine, string msg, RecognitionException e)
            => throw new Exception($"L:{line}|Erro de sintaxe: {msg}");

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol,
            int line, int charPositionInLine, string msg, RecognitionException e)
            => throw new Exception($"L:{line}|Caractere inválido: {msg}");
    }
}