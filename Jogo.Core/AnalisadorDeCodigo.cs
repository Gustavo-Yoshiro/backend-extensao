using Antlr4.Runtime;

namespace Jogo.Core
{
    public static class AnalisadorDeCodigo
    {
        // Converte o texto do jogador em árvore. Lança Exception no 1º erro de sintaxe.
        public static LinguagemParser.ProgramaContext Analisar(string codigo)
        {
            var erros = new ErroSintaxeListener();

            var lexer = new LinguagemLexer(new AntlrInputStream(codigo));
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(erros);

            var parser = new LinguagemParser(new CommonTokenStream(lexer));
            parser.RemoveErrorListeners();
            parser.AddErrorListener(erros);

            return parser.programa();
        }
    }
}