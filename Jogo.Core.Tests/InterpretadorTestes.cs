using System;
using Xunit;
using NSubstitute;
using Antlr4.Runtime;
using Jogo.Core;

namespace Jogo.Core.Tests
{
    public class InterpretadorTestes
    {
        [Fact]
        public void Deve_Mover_Corretamente_Para_Norte()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "mover(norte)"; 
            Executar(codigo, visitor);

            jogoMock.Received(1).Mover("norte");
        }

        [Fact]
        public void Nao_Deve_Mover_Para_Direcao_Invalida()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // "noroeste" como string literal para não dar erro de variável inexistente, 
            // focando no erro de direção da grade.
            string codigo = "mover(\"noroeste\")"; 
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Mover(Arg.Any<string>());
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("inválida para a grade")));
        }

        [Fact]
        public void Deve_Atacar_Com_Alvo_E_Elemento_Validos()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Usando as constantes do sistema diretamente
            string codigo = "atacar(mais_perto, gelo)"; 
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("mais_perto", "gelo");
        }

        [Fact]
        public void Deve_Barrar_Ataque_Com_Elemento_Invalido()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Alvo válido, mas elemento literal inválido
            string codigo = "atacar(aleatorio, \"chocolate\")"; 
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Atacar(Arg.Any<string>(), Arg.Any<string>());
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("Elemento 'chocolate' inválido")));
        }

        [Fact]
        public void Deve_Barrar_Ataque_Com_Alvo_Invalido()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Alvo literal inválido (não é mais_perto, aleatorio, nem menos_vida)
            string codigo = "atacar(\"orc\", fogo)"; 
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Atacar(Arg.Any<string>(), Arg.Any<string>());
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("Alvo 'orc' inválido")));
        }

        [Fact]
        public void Deve_Barrar_Ataque_Sem_Argumentos()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "atacar()"; 
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Atacar(Arg.Any<string>(), Arg.Any<string>());
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("exige")));
        }

        [Fact]
        public void Deve_Atacar_Usando_Variavel_Como_Elemento()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Integração: 'menos_vida' é constante, 'minhaMagia' é variável local
            string codigo = "string minhaMagia = \"fogo\"\n" +
                            "atacar(menos_vida, minhaMagia)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("menos_vida", "fogo");
        }

        [Fact]
        public void Deve_Respeitar_Case_Sensitivity_No_Comando()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "Mover(sul)"; 
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Mover(Arg.Any<string>());
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("não é reconhecido")));
        }

        // --- TESTES DE VARIÁVEIS E OPERAÇÕES ---

        [Fact]
        public void Deve_Reatribuir_Valor_De_Variavel()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "string magia = \"agua\"\n" +
                            "magia = \"gelo\"\n" +
                            "atacar(aleatorio, magia)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("aleatorio", "gelo");
        }

        [Fact]
        public void Deve_Resolver_Operacao_De_Concatenacao()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "string tipo = \"fo\" + \"go\"\n" +
                            "atacar(mais_perto, tipo)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("mais_perto", "fogo");
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Declarar_Tipo_Incorreto()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int numero = \"isto e um texto\""; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            Assert.Contains("Erro de Tipo", excecao.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Usar_Variavel_Nao_Declarada()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // 'magiaMisteriosa' não foi declarada nem é constante
            string codigo = "atacar(mais_perto, magiaMisteriosa)"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            Assert.Contains("não existe", excecao.Message);
        }

        // --- TESTES DE CONSTANTES DO SISTEMA E BLINDAGEM ---

        [Fact]
        public void Deve_Atacar_Usando_Variaveis_Fixas_Do_Sistema()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "atacar(mais_perto, fogo)\n" +
                            "atacar(menos_vida, raio)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("mais_perto", "fogo");
            jogoMock.Received(1).Atacar("menos_vida", "raio");
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Tentar_Sobrescrever_Constante()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Tentativa de alterar um alvo oficial
            string codigo = "mais_perto = \"aleatorio\""; 
            
            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            Assert.Contains("constante", excecao.Message.ToLower());
        }

        // --- TESTES DE CONTROLE DE FLUXO (SE) ---

        [Fact]
        public void Deve_Executar_Comandos_Dentro_Do_Se_Quando_Verdadeiro()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int vidas = 1\n" +
                            "se (vidas == 1):\n" +
                            "    mover(norte)"+
                            "fim se"; 
            Executar(codigo, visitor);

            jogoMock.Received(1).Mover("norte");
        }

        [Fact]
        public void Nao_Deve_Executar_Comandos_Dentro_Do_Se_Quando_Falso()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int vidas = 0\n" +
                            "se (vidas == 1):\n" +
                            "    mover(norte)" +
                            "fim se"; 
            
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Mover(Arg.Any<string>());
        }

        // --- TESTES DE LAÇO DE REPETIÇÃO (ENQUANTO) ---

        [Fact]
        public void Deve_Repetir_Comandos_Enquanto_Condicao_For_Verdadeira()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // O jogador quer dar 3 passos para o norte
            string codigo = "int passos = 0\n" +
                            "enquanto (passos < 3):\n" +
                            "    mover(norte)\n" +
                            "    passos = passos + 1\n" +
                            "fim enquanto"; 
            
            Executar(codigo, visitor);

            // O MOCK confere se a função Mover("norte") foi chamada EXATAMENTE 3 vezes
            jogoMock.Received(3).Mover("norte");
        }

        [Fact]
        public void Nao_Deve_Entrar_No_Enquanto_Se_Condicao_Inicial_For_Falsa()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int passos = 5\n" +
                            "enquanto (passos < 3):\n" +
                            "    mover(sul)\n" +
                            "    passos = passos + 1\n" +
                            "fim enquanto"; 
            
            Executar(codigo, visitor);

            jogoMock.DidNotReceive().Mover(Arg.Any<string>());
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Declarar_Variavel_Com_Palavra_Reservada()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Tentativa de criar uma variável com nome de constante
            string codigo = "string fogo = \"agua\""; 
            
            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            Assert.Contains("reservada", excecao.Message.ToLower());
        }

        // Método auxiliar
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