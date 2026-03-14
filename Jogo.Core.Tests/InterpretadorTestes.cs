using Xunit;
using NSubstitute;
using Antlr4.Runtime;
using Jogo.Core;

namespace Jogo.Core.Tests
{
    public class InterpretadorTestes
    {

        [Fact]
        public void Deve_Mover_Corretamente()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "mover(norte)"; 
            
            Executar(codigo, visitor);

            // Verifica se o mover foi chamado com "norte"
            jogoMock.Received(1).Mover("norte");
        }

        [Fact]
        public void Deve_Atacar_Com_Dois_Argumentos()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "atacar(orc, gelo)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("orc", "gelo");
        }

        // TESTES DE ERRO

        [Fact]
        public void Nao_Deve_Aceitar_Comando_Inexistente()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "pular()"; 
            
            Executar(codigo, visitor);

            // Nenhuma ação deve ter sido disparada
            jogoMock.DidNotReceive().Mover(Arg.Any<string>());
            jogoMock.DidNotReceive().Atacar(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Deve_Identificar_Falta_De_Argumentos()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "atacar()"; 
            
            Executar(codigo, visitor);

            // O sistema deve chamar o Atacar mas os argumentos serão "vazio"
            jogoMock.Received(1).Atacar("vazio", "vazio");
        }

        [Fact]
        public void Deve_Respeitar_Case_Sensitivity_No_Mover()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "Mover(sul)"; 
            
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Mover(Arg.Any<string>());
        }

        [Fact]
        public void Teste_Aberto()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "float jao = 2.0"; 
            
            Executar(codigo, visitor);
        }
    

        // Método auxiliar para não repetir código de setup do ANTLR
        private void Executar(string input, MeuVisitor visitor)
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new LinguagemLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new LinguagemParser(tokens);
            var arvore = parser.programa();
            visitor.Visit(arvore);
        }
    }
}