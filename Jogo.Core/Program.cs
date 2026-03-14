using System;
using Antlr4.Runtime; // Traz as ferramentas do ANTLR

namespace Jogo.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Simulamos o texto que o jogador digitaria no jogo
            string codigoDoJogador = "Atacar(inimigo, fogo)";
            Console.WriteLine("--- Iniciando o Interpretador ---");

            // 2. Passamos o texto para o Lexer (que corta em palavras)
            var inputStream = new AntlrInputStream(codigoDoJogador);
            var lexer = new LinguagemLexer(inputStream); // Classe gerada pelo ANTLR
            var tokens = new CommonTokenStream(lexer);

            // 3. Passamos as palavras para o Parser (que entende a gramática)
            var parser = new LinguagemParser(tokens); // Classe gerada pelo ANTLR
            
            // 4. Mandamos o Parser ler a regra principal 'programa' gerando a Árvore
            var arvore = parser.programa(); 

            // 5. Chamamos o seu Visitor para passear pela Árvore
            MeuVisitor visitor = new MeuVisitor();
            visitor.Visit(arvore);

            Console.WriteLine("--- Execução Terminada ---");
        }
    }
}