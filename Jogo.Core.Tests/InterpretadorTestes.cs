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
        public void Deve_Mover_Corretamente_Para_Cima()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "mover(Cima)"; 
            Executar(codigo, visitor);

            jogoMock.Received(1).Mover("Cima");
        }

        [Fact]
        public void Nao_Deve_Mover_Para_Direcao_Invalida()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

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

            // Alvo literal inválido
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

            string codigo = "Mover(Baixo)"; 
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

            // Tentativa de alterar um alvo "oficial"
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
                            "    mover(Cima)"+
                            "fim se"; 
            Executar(codigo, visitor);

            jogoMock.Received(1).Mover("Cima");
        }

        [Fact]
        public void Nao_Deve_Executar_Comandos_Dentro_Do_Se_Quando_Falso()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int vidas = 0\n" +
                            "se (vidas == 1):\n" +
                            "    mover(Cima)" +
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

            // O jogador quer dar 3 passos para o Cima
            string codigo = "int passos = 0\n" +
                            "enquanto (passos < 3):\n" +
                            "    mover(Cima)\n" +
                            "    passos = passos + 1\n" +
                            "fim enquanto";
            
            Executar(codigo, visitor);

            // O MOCK confere se a função Mover("Cima") foi chamada EXATAMENTE 3 vezes
            jogoMock.Received(3).Mover("Cima");
        }

        [Fact]
        public void Nao_Deve_Entrar_No_Enquanto_Se_Condicao_Inicial_For_Falsa()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int passos = 5\n" +
                            "enquanto (passos < 3):\n" +
                            "    mover(Baixo)\n" +
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
        
        [Fact]
        public void Deve_Ignorar_Senao_Quando_Se_For_Verdadeiro()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int vidas = 1\n" +
                            "se (vidas == 1):\n" +
                            "    mover(Cima)\n" +
                            "senao:\n" +
                            "    mover(Baixo)\n" +
                            "fim se"; 
            
            Executar(codigo, visitor);

            // Garante que moveu pro Cima e ignorou o Baixo
            jogoMock.Received(1).Mover("Cima");
            jogoMock.DidNotReceive().Mover("Baixo");
        }

        [Fact]
        public void Deve_Executar_Senao_Quando_Se_For_Falso()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "int i = 0\n" +
                            "enquanto (i < 10):\n" +
                            "    se (i < 5):\n" +
                            "        mover(Cima)\n" +
                            "    senao:\n" +
                            "        mover(Baixo)\n" +
                            "    fim se\n" +
                            "    i = i + 1 \n" +
                            "fim enquanto";
            
            Executar(codigo, visitor);

            jogoMock.Received(5).Mover("Cima");
            jogoMock.Received(5).Mover("Baixo");
        }

        [Fact]
        public void Deve_Usar_Item_Do_Cinto() {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);
            Executar("cinto.usarItem(0)", visitor);
            jogoMock.Received(1).UsarItemCinto(0);
        }

        [Fact]
        public void Deve_Executar_Todas_As_Funcoes_BuiltIn_Corretamente()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            
            // Criação de um objeto de teste
            var inimigoFake = new object();
            jogoMock.InimigoMaisProximo().Returns(inimigoFake);

            var visitor = new MeuVisitor(jogoMock);

            // CÓDIGO DO JOGADOR: Usando absolutamente todas as funções corretamente
            string codigo = 
                "mover(\"Cima\")\n" +
                "atacar(inimigoMaisProximo(), \"fogo\")\n" +
                "nomeInimigo(inimigoMaisProximo())\n" +
                "podeMover(\"Direita\")\n" +
                "tempo()\n" +
                "vidaAtual()\n" +
                "escanearArea()\n" +
                "posicao()\n" +
                "tesouro()\n" +
                "cinto.usarItem(2)\n" +
                "mochila.usarItem()\n" +
                "arena(\"Floresta\")\n" +
                "cinto.colocarItem(\"Pocao\", 1)\n" +
                "mochila.colocarItem(\"Chave\")\n" +
                "comprar(\"Escudo\")\n" +
                "escapar()";

            Executar(codigo, visitor);

            // VALIDAÇÃO: Garantimos que o motor do jogo recebeu exatamente as chamadas certas
            jogoMock.Received(1).Mover("Cima");
            jogoMock.Received(2).InimigoMaisProximo(); // Foi chamado 2x (no atacar e no nomeInimigo)
            jogoMock.Received(1).Atacar(inimigoFake, "fogo");
            jogoMock.Received(1).GetNomeInimigo(inimigoFake);
            jogoMock.Received(1).PodeMover("Direita");
            jogoMock.Received(1).GetTempo();
            jogoMock.Received(1).GetVidaAtual();
            jogoMock.Received(1).EscanearArea();
            jogoMock.Received(1).GetPosicaoPlayer();
            jogoMock.Received(1).GetPosicaoTesouro();
            jogoMock.Received(1).UsarItemCinto(2);
            jogoMock.Received(1).UsarItemMochila();
            jogoMock.Received(1).EntrarArena("Floresta");
            jogoMock.Received(1).ColocarItemCinto("Pocao", 1);
            jogoMock.Received(1).ColocarItemMochila("Chave");
            jogoMock.Received(1).Comprar("Escudo");
            jogoMock.Received(1).Escapar();

            // Garante que NENHUM erro foi disparado neste teste
            jogoMock.DidNotReceive().NotificarErro(Arg.Any<string>());
        }

        [Fact]
        public void Deve_Notificar_Erros_Com_Parametros_Incorretos_E_Sobrando()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = 
                "mover()\n" +                           // ERRO: Falta direção
                "atacar(\"mais_perto\")\n" +            // ERRO: Falta o elemento
                "cinto.usarItem(\"pocao\")\n" +         // ERRO: Tipo errado (é string, deveria ser int)
                "cinto.colocarItem(\"Pocao\")\n" +      // ERRO: Falta o índice numérico
                "arena()\n" +                           // ERRO: Falta o nome da arena
                "tempo(5)\n" +                          // ERRO: Tem parâmetro, mas não deveria ter
                "vidaAtual(\"teste\", 2)";              // ERRO: Tem 2 parâmetros, mas não deveria ter nenhum

            Executar(codigo, visitor);

            // VALIDAÇÃO: Verifica se o interpretador pegou erros do jogador e gerou os erros internos
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("'mover' exige exatamente 1")));
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("'atacar' exige 2 argumentos")));
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("'cinto.usarItem' exige 1 índice numérico")));
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("'cinto.colocarItem' exige 1 item")));
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("'arena' exige o nome")));
            
            // Validações de funções que não deveriam ter parâmetros
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("'tempo' não aceita argumentos")));
            jogoMock.Received(1).NotificarErro(Arg.Is<string>(s => s.Contains("'vidaAtual' não aceita argumentos")));
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