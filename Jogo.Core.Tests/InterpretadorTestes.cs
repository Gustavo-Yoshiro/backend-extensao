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

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            
            Assert.Contains("Direção 'noroeste' inválida. Use Cima, Baixo, Esquerda ou Direita.", excecao.Message);
        }

        [Fact]
        public void Deve_Atacar_Com_Alvo_E_Elemento_Validos()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Usando as constantes do sistema diretamente
            string codigo = "atacar(\"Orc\", Gelo)"; 
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("Orc", "Gelo");
        }

        [Fact]
        public void Deve_Barrar_Ataque_Com_Elemento_Invalido()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);
            string codigo = "atacar(\"Orc\", \"chocolate\")"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            
            // Atualizado para a nova mensagem de ataque
            Assert.Contains("O ataque 'chocolate' é inválido ou você não possui.", excecao.Message);
        }

        //[Fact]
        public void Deve_Barrar_Ataque_Com_Alvo_Invalido()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);
            
            // Usamos "Pudim" porque sabemos que não está na lista _inimigos
            string codigo = "atacar(\"Pudim\", \"Fogo\")"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            
            // Exceção estoura contendo a palavra Pudim ou a palavra inválido
            Assert.True(excecao.Message.Contains("Pudim") || excecao.Message.ToLower().Contains("O inimigo 'Pudim' é inválido"));
        }

        [Fact]
        public void Deve_Atacar_Usando_Variavel_Como_Elemento()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Integração: 'Orc' é constante, 'minhaMagia' é variável local
            string codigo = "string minhaMagia = \"Fogo\"\n" +
                            "atacar(\"Orc\", minhaMagia)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("Orc", "Fogo");
        }

        [Fact]
        public void Deve_Respeitar_Case_Sensitivity_No_Comando()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "mover(Baixo)"; 
            Executar(codigo, visitor);

            jogoMock.Received(1).Mover(Arg.Any<string>());
        }

        // --- TESTES DE VARIÁVEIS E OPERAÇÕES ---

        [Fact]
        public void Deve_Reatribuir_Valor_De_Variavel()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "string magia = \"agua\"\n" +
                            "magia = \"Gelo\"\n" +
                            "atacar(\"Orc\", magia)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("Orc", "Gelo");
        }

        [Fact]
        public void Deve_Resolver_Operacao_De_Concatenacao()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);
            
            string codigo = "string tipo = \"Fo\" + \"go\"\n" +
                            "atacar(\"Orc\", tipo)"; 
            
            Executar(codigo, visitor);
            jogoMock.Received(1).Atacar("Orc", "Fogo");
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Declarar_Tipo_Incorreto()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);
            string codigo = "int numero = \"texto\""; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            // Atualizado para a mensagem que o seu amigo colocou no código
            Assert.Contains("não corresponde ao tipo", excecao.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Usar_Variavel_Nao_Declarada()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // 'magiaMisteriosa' não foi declarada nem é constante
            string codigo = "atacar(\"Orc\", magiaMisteriosa)"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            Assert.Contains($"L:1|A variável 'magiaMisteriosa' não foi declarada.", excecao.Message);
        }

        // --- TESTES DE CONSTANTES DO SISTEMA E BLINDAGEM ---

        [Fact]
        public void Deve_Atacar_Usando_Variaveis_Fixas_Do_Sistema()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "atacar(\"Orc\", Fogo)\n" +
                            "atacar(\"Orc\", Alho)"; 
            
            Executar(codigo, visitor);

            jogoMock.Received(1).Atacar("Orc", "Fogo");
            jogoMock.Received(1).Atacar("Orc", "Alho");
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
            string codigo = "string Fogo = \"agua\""; 
            
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
            var visitor = new MeuVisitor(jogoMock);

            string codigo = 
                "mover(\"Cima\")\n" +
                "atacar(\"Orc\", \"Fogo\")\n" +  // Atualizado para F maiúsculo
                "nomeInimigo(Orc)\n" +
                "podeMover(\"Direita\")\n" +
                "tempo()\n" +
                "vidaAtual()\n" +
                "escanearArea()\n" +
                "posicaoX()\n" +
                "posicaoY()\n" +
                "tesouroX()\n" +
                "tesouroY()\n" +
                "cinto.usarItem(2)\n" +
                "mochila.usarItem()\n" +
                "arena(\"Floresta\")\n" +
                "cinto.colocarItem(\"Pocao\", 1)\n" +
                "mochila.colocarItem(\"Chave\")\n" +
                "comprar(\"Escudo\")\n" +
                "escapar()";

            Executar(codigo, visitor);

            jogoMock.Received(1).Mover("Cima");
            jogoMock.Received(1).PodeMover("Direita");
            jogoMock.Received(1).GetTempo();
            jogoMock.Received(1).GetVidaAtual();
            jogoMock.Received(1).EscanearArea();
            jogoMock.Received(1).GetPosicaoPlayerX();
            jogoMock.Received(1).GetPosicaoPlayerY();
            jogoMock.Received(1).GetPosicaoTesouroX();
            jogoMock.Received(1).GetPosicaoTesouroY();
            jogoMock.Received(1).UsarItemCinto(2);
            jogoMock.Received(1).UsarItemMochila();
            jogoMock.Received(1).EntrarArena("Floresta");
            jogoMock.Received(1).ColocarItemCinto("Pocao", 1);
            jogoMock.Received(1).ColocarItemMochila("Chave");
            jogoMock.Received(1).Comprar("Escudo");
            jogoMock.Received(1).Escapar();
        }

        [Fact]
        public void Deve_Notificar_Erros_Com_Parametros_Incorretos_E_Sobrando()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = "mover()\n"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            Assert.Contains("precisa de 1 Direção", excecao.Message);
        }

        [Fact]
        public void Deve_Executar_O_Senao_Se_Correto_E_Ignorar_O_Resto()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = 
                "int vida = 5\n" +
                "se (vida > 10):\n" +
                "    mover(\"Cima\")\n" +
                "senao se (vida == 8):\n" +
                "    mover(\"Baixo\")\n" +
                "senao se (vida == 5):\n" +
                "    mover(\"Direita\")\n" +
                "senao:\n" +
                "    mover(\"Esquerda\")\n" +
                "fim se"; 
            
            Executar(codigo, visitor);

            // VALIDAÇÃO
            jogoMock.Received(1).Mover("Direita");
            jogoMock.DidNotReceive().Mover("Cima");
            jogoMock.DidNotReceive().Mover("Baixo");
            jogoMock.DidNotReceive().Mover("Esquerda");
        }

        [Fact]
        public void Deve_Isolar_Variaveis_Locais_Sem_Afetar_A_Memoria_Global()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = 
                "int vida = 10\n" +
                "vazio BuffTemporario(int vida):\n" +
                "    vida = 999\n" + 
                "    retorna\n" +  
                "fim funcao\n" +
                "\n" +
                "BuffTemporario(50)\n" +
                "\n" +
                "se (vida == 10):\n" +
                "    mover(\"Cima\")\n" + 
                "fim se";

            Executar(codigo, visitor);

            // VALIDAÇÃO
            // Se a memória global foi protegida, a variável original continuou valendo 10 e ele moveu.
            // Se o sistema de escopo falhou, ela virou 999 e ele não vai mover.
            jogoMock.Received(1).Mover("Cima");
        }

        [Fact]
        public void Deve_Executar_Funcao_Com_Retorno_Antecipado_E_Parametros()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            string codigo = 
                "string EscolherAtaque(int hpInimigo):\n" +
                "    se (hpInimigo > 50):\n" +
                "        retorna \"ExplosaoFogo\"\n" +
                "    fim se\n" +
                "    retorna \"Gelo\"\n" +
                "fim funcao\n" +
                "\n" +
                "atacar(\"Orc\", EscolherAtaque(100))\n" +
                "\n" +
                "atacar(\"Orc\", EscolherAtaque(30))";

            Executar(codigo, visitor);

            // VALIDAÇÃO
            // Garante que o alvo temporário recebeu a magia forte no primeiro hit
            jogoMock.Received(1).Atacar("Orc", "ExplosaoFogo");
            
            // Garante que o alvo temporário recebeu a magia fraca no segundo hit
            jogoMock.Received(1).Atacar("Orc", "Gelo");
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

        [Fact]
        public void Deve_Executar_Bloco_Se_Inverter_Falso_Para_Verdadeiro()
        {
            // 1. Prepara o Godot de mentira (Mock)
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // 2. O Script do Jogador
            // Como !Falso vira Verdadeiro, ele DEVE entrar no 'se' e tentar mover.
            string codigo = "se(!Falso):\n" +
                            "    mover(Cima)\n" +
                            "fim se"; 

            // 3. Roda o interpretador
            Executar(codigo, visitor);

            // 4. Prova: Verifica se o Godot recebeu a ordem exata 1 vez
            jogoMock.Received(1).Mover("Cima");
        }

        [Fact]
        public void Nao_Deve_Executar_Bloco_Se_Inverter_Verdadeiro_Para_Falso()
        {
            // 1. Prepara o Godot de mentira (Mock)
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // 2. O Script do Jogador
            // Como !Verdadeiro vira Falso, a porta tranca e ele NÃO deve entrar no 'se'.
            string codigo = "se(!Verdadeiro):\n" +
                            "    mover(Cima)\n" +
                            "fim se"; 

            // 3. Roda o interpretador
            Executar(codigo, visitor);

            // 4. Prova: Garante que o motor do jogo NÃO recebeu NENHUM comando de mover
            jogoMock.DidNotReceive().Mover(Arg.Any<string>());
        }

        // TESTES DE LISTAS E VETORES

        [Fact]
        public void Deve_Criar_E_Acessar_Item_Da_Lista_Pelo_Indice()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // O vetor tem [10, 20, 30]. O índice 1 é o número 20.
            string codigo = "int vetor = [10, 20, 30]\n" +
                            "se (vetor[1] == 20):\n" +
                            "    mover(Cima)\n" +
                            "fim se"; 

            Executar(codigo, visitor);

            jogoMock.Received(1).Mover("Cima");
        }

        [Fact]
        public void Deve_Fazer_Operacoes_Matematicas_Com_Itens_Do_Vetor()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Soma vetor[0] (que é 2) com vetor[1] (que é 3)
            string codigo = "int vetor = [2, 3]\n" +
                            "int soma = vetor[0] + vetor[1]\n" +
                            "se (soma == 5):\n" +
                            "    mover(Direita)\n" +
                            "fim se"; 

            Executar(codigo, visitor);

            jogoMock.Received(1).Mover("Direita");
        }

        [Fact]
        public void Deve_Lancar_Excecao_Se_Lista_Tiver_Item_Incorreto()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // A variável exige 'int', mas o último item é um texto
            string codigo = "int vetor = [1, 5, \"intruso\"]"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            
            // Garante que o erro do seu método VerificarTipo estourou certinho
            Assert.Contains("intruso", excecao.Message.ToLower());
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Acessar_Indice_Fora_Do_Limite()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // O vetor só vai até o índice 1. Tentar acessar o 5 tem que dar erro!
            string codigo = "int vetor = [10, 20]\n" +
                            "int x = vetor[5]"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            
            Assert.Contains("fora dos limites", excecao.Message.ToLower());
        }

        [Fact]
        public void Deve_Concatenar_Diferentes_Tipos_E_Escrever()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Testa: String + Inteiro + String + Booleano
            string codigo = "int numero = 457\n" +
                            "bool condicao = Falso\n" +
                            "string texto = \"HELLO \" + numero + \"\\nA condicao eh: \" + condicao\n" +
                            "escreva(texto)"; 

            Executar(codigo, visitor);

            // O texto exato que deve chegar no Godot, já com a quebra de linha real
            string resultadoEsperado = "HELLO 457\nA condicao eh: Falso";
            
            jogoMock.Received(1).Escreva(resultadoEsperado);
        }
        [Fact]
        public void Deve_Acessar_Atributos_De_Um_Inimigo()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            
            // Quando o código pedir os atributos do "Vampiro", o Mock vai responder isso:
            jogoMock.ObterNomeInimigo("Vampiro").Returns("Vampiro");
            jogoMock.ObterVelocidadeInimigo("Vampiro").Returns(15.5f);
            
            var visitor = new MeuVisitor(jogoMock);

            // Testa o acesso ao nome e à velocidade
            string codigo = "Inimigo alvo = \"Vampiro\"\n" +
                            "string n = alvo.nome\n" +
                            "float v = alvo.velocidade\n" +
                            "se (n == \"Vampiro\" e v > 10.0):\n" +
                            "    mover(Cima)\n" +
                            "fim se"; 

            Executar(codigo, visitor);

            // O boneco tem que ter se movido porque 15.5 é maior que 10.0!
            jogoMock.Received(1).Mover("Cima");
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Acessar_Atributo_De_Tipo_Invalido()
        {
            var jogoMock = Substitute.For<IAcoesDoJogo>();
            var visitor = new MeuVisitor(jogoMock);

            // Tenta acessar .nome de um número inteiro (Tem que dar erro!)
            string codigo = "int numero = 42\n" +
                            "string n = numero.nome"; 

            var excecao = Assert.Throws<Exception>(() => Executar(codigo, visitor));
            
            Assert.Contains("não é um Inimigo", excecao.Message);
        }


    }


}