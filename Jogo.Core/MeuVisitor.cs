using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Microsoft.VisualBasic;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        private const int DELAY_TRANSICAO_CENA = 1000;

        // 1. Memória Global
        private Dictionary<string, object> _memoria = new Dictionary<string, object>();

        // 2. Pilha de Escopos Locais
        private Stack<Dictionary<string, object>> _escoposLocais = new Stack<Dictionary<string, object>>();

        // 3. Gaveta para guardar as funções criadas pelo player
        private Dictionary<string, LinguagemParser.DeclaracaoFuncaoContext> _funcoesJogador = new Dictionary<string, LinguagemParser.DeclaracaoFuncaoContext>();
        private readonly IAcoesDoJogo _jogo;

        // ==========================================
        // CATEGORIAS DE "ENUMS" E FUNÇÕES
        // ==========================================
        private HashSet<string> _direcoes = new HashSet<string> { "Cima", "Baixo", "Direita", "Esquerda" };
        private HashSet<string> _ataques = new HashSet<string> { "EsferaAzul", "EsferaVermelha", "Raio", "Gelo", "Fogo", "ExplosaoFogo", "ExplosaoGelo", "Alho" };
        private HashSet<string> _recursos = new HashSet<string> { "Moeda", "Osso", "Couro", "Magma", "Cristal", "Plasma", "Sangue", "Safira", "Esmeralda", "Diamante" };
        private HashSet<string> _inimigos = new HashSet<string> { "Goblin", "Esqueleto", "SlimeDeFogo", "SlimeDeGelo", "Lobisomem", "Orc", "Fantasma", "Vampiro" };
        private HashSet<string> _arenas = new HashSet<string> { "Campos", "Floresta", "Labirinto" };
        private HashSet<string> _itens = new HashSet<string> { "PocaoDeVida" };

        
        // NOVO: Lista de todas as funções e objetos nativos do jogo
        private HashSet<string> _funcoesEObjetosNativos = new HashSet<string> { 
            "mover", "podeMover", "atacar", "nomeInimigo", "tempo", "vidaAtual", 
            "inimigoMaisProximo", "escanearArea", "posicaoX", "posicaoY", 
            "tesouroX", "tesouroY", "escapar", "arena", "comprar", 
            "cinto", "mochila", "venderTudo"
        };
        
        private HashSet<string> _palavrasReservadas;

        public MeuVisitor(IAcoesDoJogo acoesDoJogo)
        {
            _jogo = acoesDoJogo;

            _palavrasReservadas = new HashSet<string> { "Verdadeiro", "Falso", "Inimigo", "Arena", "Ataque", "Direcao" };
            _palavrasReservadas.UnionWith(_direcoes);
            _palavrasReservadas.UnionWith(_ataques);
            _palavrasReservadas.UnionWith(_recursos);
            _palavrasReservadas.UnionWith(_inimigos);
            _palavrasReservadas.UnionWith(_arenas);
            _palavrasReservadas.UnionWith(_itens);
            
            // Adiciona as funções à lista gigante de palavras proibidas
            _palavrasReservadas.UnionWith(_funcoesEObjetosNativos);

            foreach (var constante in _palavrasReservadas)
            {
                _memoria[constante] = constante;
            }
        }

        public override object VisitExpressao([NotNull] LinguagemParser.ExpressaoContext context)
        {
            if (context.chamadaFuncao() != null) return Visit(context.chamadaFuncao());
            if (context.NUMERO_INT() != null) return int.Parse(context.NUMERO_INT().GetText());
            if (context.NUMERO_FLOAT() != null) return float.Parse(context.NUMERO_FLOAT().GetText(), System.Globalization.CultureInfo.InvariantCulture);
            if (context.STRING_LIT() != null) return context.STRING_LIT().GetText().Trim('"');
            if (context.BOOLEANO() != null) return context.BOOLEANO().GetText() == "Verdadeiro";

            if (context.ChildCount == 3 && context.GetChild(0).GetText() == "(")
                return Visit(context.expressao(0)); 

            if (context.ID() != null)
            {
                string nomeVar = context.ID().GetText();

                // 1. Procura primeiro no Escopo Local
                if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeVar))
                {
                    return _escoposLocais.Peek()[nomeVar];
                }

                // 2. Se não achou, procura na Memória Global
                if (_memoria.ContainsKey(nomeVar))
                {
                    return _memoria[nomeVar];
                }

                // Se não achou em lugar nenhum, o jogador digitou uma variável que não existe
                throw new Exception($"L:{context.Start.Line}|A variável '{nomeVar}' não foi declarada.");
            }

            // Se o antlr achar o !
            if (context.NAO() != null)
            {
                // Resolve a expressao na direita
                object valor = Visit(context.expressao(0)); 
                
                if (valor is bool condicaoBooleana)
                {
                    return !condicaoBooleana; 
                }
                
                // Se o jogador tentou fazer algo absurdo tipo !5 ou !"texto"
                throw new Exception($"L:{context.Start.Line}|O operador '!' só pode ser usado com valores Verdadeiro ou Falso.");
            }
            
            // Redireciona para as funções especializadas de lista
            if (context.lista() != null) return Visit(context.lista());
            if (context.acessoLista() != null) return Visit(context.acessoLista());


            if (context.expressao().Length == 2)
            {
                object esquerdo = Visit(context.expressao(0));
                object direito = Visit(context.expressao(1));

                if (context.E() != null || context.OU() != null)
                {
                    if (esquerdo is bool boolEsq && direito is bool boolDir)
                    {
                        if (context.E() != null) return boolEsq && boolDir;
                        if (context.OU() != null) return boolEsq || boolDir;
                    }
                    throw new Exception($"L:{context.Start.Line}|Operadores 'e'/'ou' só funcionam com valores lógicos (Verdadeiro/Falso).");
                }

                if (context.IGUAL() != null) return esquerdo.Equals(direito); 

                bool esqEhNumero = esquerdo is int || esquerdo is float;
                bool dirEhNumero = direito is int || direito is float;

                if (esqEhNumero && dirEhNumero)
                {
                    if (esquerdo is float || direito is float)
                    {
                        float fEsq = Convert.ToSingle(esquerdo);
                        float fDir = Convert.ToSingle(direito);

                        if (context.SOMA() != null) return fEsq + fDir;
                        if (context.SUB() != null) return fEsq - fDir;
                        if (context.MULT() != null) return fEsq * fDir;
                        if (context.DIV() != null) return fEsq / fDir;
                        if (context.MOD() != null) return fEsq % fDir;

                        if (context.MAIOR_IGUAL() != null) return fEsq >= fDir;
                        if (context.MENOR_IGUAL() != null) return fEsq <= fDir;
                        if (context.MAIOR() != null) return fEsq > fDir;
                        if (context.MENOR() != null) return fEsq < fDir;
                    }
                    else
                    {
                        int iEsq = (int)esquerdo;
                        int iDir = (int)direito;

                        if (context.SOMA() != null) return iEsq + iDir;
                        if (context.SUB() != null) return iEsq - iDir;
                        if (context.MULT() != null) return iEsq * iDir;
                        if (context.DIV() != null) return iEsq / iDir;
                        if (context.MOD() != null) return iEsq % iDir;

                        if (context.MAIOR_IGUAL() != null) return iEsq >= iDir;
                        if (context.MENOR_IGUAL() != null) return iEsq <= iDir;
                        if (context.MAIOR() != null) return iEsq > iDir;
                        if (context.MENOR() != null) return iEsq < iDir;
                    }
                }

                if (context.SOMA() != null && (esquerdo is string || direito is string))
                    return esquerdo.ToString() + direito.ToString();

                throw new Exception($"L:{context.Start.Line}|Não é possível calcular '{esquerdo.GetType().Name}' com '{direito.GetType().Name}'.");
            }

            return null!;
        }

        public override object VisitDeclaracaoVariavel([NotNull] LinguagemParser.DeclaracaoVariavelContext context)
        {
            string tipoDeclarado = context.TIPO().GetText();
            string nomeDaVariavel = context.ID().GetText();
            
            if (tipoDeclarado == "vazio")
            {
                throw new Exception($"L:{context.Start.Line}|Não é possível criar uma variável do tipo 'vazio'. Esse tipo é exclusivo para funções e procedimentos.");
            }

            // Essa é a linha que vai barrar o jogador se ele tentar: int mover = 10
            if (_palavrasReservadas.Contains(nomeDaVariavel)) {
                throw new Exception($"L:{context.Start.Line}|'{nomeDaVariavel}' é uma função ou palavra reservada do jogo e não pode ser usada como nome de variável.");
            }

            object valorResolvido = Visit(context.expressao());
            VerificarTipo(tipoDeclarado, valorResolvido, nomeDaVariavel, context.Start.Line);

            if (_escoposLocais.Count > 0)
            {
                // Cria a variável restrita à função
                _escoposLocais.Peek()[nomeDaVariavel] = valorResolvido;
            }
            else
            {
                // Se a pilha está vazia, cria a variável de forma global para o jogo inteiro ver
                _memoria[nomeDaVariavel] = valorResolvido;
            }

            return null!;
        }

        private void VerificarTipo(string tipoEsperado, object valor, string nomeVar, int linha)
        {
            // VERIFICAÇÃO ESPECIAL PARA LISTAS
            if (valor is List<object> listaDeValores)
            {
                if (listaDeValores.Count == 0) return; // Se for lista vazia []

                // Inspeciona item por item dentro da lista
                foreach (var item in listaDeValores)
                {
                    bool itemValido = false;
                    switch (tipoEsperado)
                    {
                        case "int": itemValido = item is int; break;
                        // Uma lista de floats pode receber números inteiros também
                        case "float": itemValido = item is float || item is int; break; 
                        case "bool": itemValido = item is bool; break;
                        case "string":
                        case "Direcao":
                        case "Inimigo":
                        case "Ataque":
                        case "Arena":
                            itemValido = item is string; break;
                    }
                    if (!itemValido) 
                        throw new Exception($"L:{linha}|A lista '{nomeVar}' espera itens do tipo '{tipoEsperado}', mas encontrou um intruso do tipo '{item.GetType().Name}'.");
                }
                
                return; 
            }
            // VERIFICAÇÃO NORMAL (Código Original)
            bool tipoValido = false;
            switch (tipoEsperado)
            {
                case "int": tipoValido = valor is int; break;
                case "float": tipoValido = valor is float || valor is int; break;
                case "bool": tipoValido = valor is bool; break;
                case "string":
                case "Direcao":
                case "Inimigo":
                case "Ataque":
                case "Arena":
                    tipoValido = valor is string; break;
            }
            
            if (!tipoValido) throw new Exception($"L:{linha}|O valor passado para '{nomeVar}' não corresponde ao tipo '{tipoEsperado}'.");
        }

        public override object VisitAtribuicao([NotNull] LinguagemParser.AtribuicaoContext context)
        {
            string nomeDaVariavel = context.ID().GetText();

            // Barra o jogador se ele tentar reatribuir: mover = 10
            if (_palavrasReservadas.Contains(nomeDaVariavel)) {
                throw new Exception($"L:{context.Start.Line}|A palavra '{nomeDaVariavel}' é reservada pelo sistema e não pode ser alterada.");
            }
            if (!_memoria.ContainsKey(nomeDaVariavel)) {
                throw new Exception($"L:{context.Start.Line}|A variável '{nomeDaVariavel}' não foi criada. Declare seu tipo antes (ex: int {nomeDaVariavel} = 0).");
            }

            object novoValor = Visit(context.expressao());
            object valorAntigo = _memoria[nomeDaVariavel];

            if (valorAntigo.GetType() != novoValor.GetType()) {
                throw new Exception($"L:{context.Start.Line}|Erro de Tipo: A variável foi criada como '{valorAntigo.GetType().Name}', não pode receber '{novoValor.GetType().Name}'.");
            }
            
            if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeDaVariavel))
            {
                // Atualiza a variável caso ela seja local
                _escoposLocais.Peek()[nomeDaVariavel] = novoValor;
                return null!;
            }
            
            _memoria[nomeDaVariavel] = novoValor;
            return null!;
        }

        public override object VisitEstruturaSe([NotNull] LinguagemParser.EstruturaSeContext context)
        {
            // Teste do bloco 'SE' principal
            object resultadoCondicaoSe = Visit(context.expressao());

            if (!(resultadoCondicaoSe is bool condicaoSe))
                throw new Exception("Erro de Tipo: A condição do 'se' precisa ser Verdadeiro ou Falso.");

            if (condicaoSe)
            {
                Console.WriteLine("[Controle de Fluxo] O 'se' é Verdadeiro! Executando bloco principal.");
                foreach (var cmd in context.comando()) Visit(cmd);

                // O return null! encerra a função. Isso garante que nenhum 'senão se' ou 'senão' será lido.
                return null!; 
            }

            // Executa todos Senao Se (0 ou +)
            if (context.estruturaSenaoSe() != null)
            {
                foreach (var senaoSeContext in context.estruturaSenaoSe())
                {
                    object resultadoSenaoSe = Visit(senaoSeContext.expressao());

                    if (!(resultadoSenaoSe is bool condicaoSenaoSe))
                        throw new Exception("Erro de Tipo: A condição do 'senão se' precisa ser Verdadeiro ou Falso.");

                    // Procura pelo 'Senao Se' vdd e retorna se achar.
                    if (condicaoSenaoSe)
                    {
                        Console.WriteLine("[Controle de Fluxo] Um 'senão se' é Verdadeiro! Executando bloco.");
                        foreach (var cmd in senaoSeContext.comando()) Visit(cmd);

                        return null!; // Encerra a função. Ignora os próximos 'senão se' e o 'senão'.
                    }
                }
            }

            // Verifica o Senao.
            if (context.estruturaSenao() != null)
            {
                Console.WriteLine("[Controle de Fluxo] Tudo foi falso. Executando bloco 'senão'.");
                foreach (var cmd in context.estruturaSenao().comando()) Visit(cmd);
            }

            return null!; 
        }
        
        public override object VisitEstruturaEnquanto([NotNull] LinguagemParser.EstruturaEnquantoContext context)
        {
            object resultadoCondicao = Visit(context.expressao());

            if (resultadoCondicao is bool condicao)
            {
                while (condicao)
                {
                    foreach (var cmd in context.comando()) Visit(cmd);

                    resultadoCondicao = Visit(context.expressao());
                    if (resultadoCondicao is bool novaCondicao) condicao = novaCondicao;
                    else throw new Exception($"L:{context.Start.Line}|A condição do 'enquanto' deixou de ser lógica no meio do loop.");
                }
            }
            else throw new Exception($"L:{context.Start.Line}|A condição do 'enquanto' precisa ser Verdadeiro ou Falso.");

            return null!;
        }
        
        public override object VisitChamadaFuncao([NotNull] LinguagemParser.ChamadaFuncaoContext context)
        {
            string? prefixo = context.objeto != null ? context.objeto.Text : null;
            string nomeFuncao = context.funcao.Text;
            string nomeCompleto = prefixo != null ? $"{prefixo}.{nomeFuncao}" : nomeFuncao;
        
            List<object> args = new List<object>();
            if (context.expressao() != null)
            {
                foreach (var exp in context.expressao()) args.Add(Visit(exp));
            }
        
            switch (nomeCompleto)
            {
            // ==========================================
            // FUNÇÕES BUILT-IN
            // ==========================================

                case "mover":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'mover()' precisa de 1 Direção.");
                    string dirMover = args[0].ToString()!;
                    if (!_direcoes.Contains(dirMover)) throw new Exception($"L:{context.Start.Line}|Direção '{dirMover}' inválida. Use Cima, Baixo, Esquerda ou Direita.");
                    _jogo.Mover(dirMover);
                    return null!;
        
                case "podeMover":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'podeMover()' precisa de 1 Direção.");
                    string dirPode = args[0].ToString()!;
                    if (!_direcoes.Contains(dirPode)) throw new Exception($"L:{context.Start.Line}|Direção '{dirPode}' inválida.");
                    return _jogo.PodeMover(dirPode);
        
                case "atacar":
                    if (args.Count != 2) { throw new Exception($"L:{context.Start.Line}| 'atacar' exige 2 argumentos."); }

                    string alvoStr = args[0].ToString()!;
                    string elemento = args[1].ToString()!;

                    var elementosValidos = _ataques;
                    if (!elementosValidos.Contains(elemento)) throw new Exception($"L:{context.Start.Line}| O ataque '{elemento}' é inválido ou você não possui.");

                    _jogo.Atacar(alvoStr, elemento);
                    return null!;
        
                case "nomeInimigo":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'nomeInimigo()' precisa de um alvo.");
                    return _jogo.GetNomeInimigo(args[0].ToString()!);
        
                case "tempo":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'tempo()' não recebe parâmetros.");
                    return _jogo.GetTempo();
        
                case "vidaAtual":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'vidaAtual()' não recebe parâmetros.");
                    return _jogo.GetVidaAtual();
        
                case "inimigoMaisProximo":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'inimigoMaisProximo()' não recebe parâmetros.");
                    return _jogo.InimigoMaisProximo();

                // ==========================================
                // SISTEMA RELACIONADOS A ARENA
                // ==========================================
        
                case "escanearArea":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'escanearArea()' não recebe parâmetros.");
                    return _jogo.EscanearArea();
        
                case "posicaoX":
                    if (args.Count != 0) { throw new Exception($"L:{context.Start.Line}|'posicaoX' não aceita argumentos.");}
                    return _jogo.GetPosicaoPlayerX();

                case "posicaoY":
                    if (args.Count != 0) { throw new Exception($"L:{context.Start.Line}|'posicaoY' não aceita argumentos.");}
                    return _jogo.GetPosicaoPlayerY();

                case "tesouroX":
                    if (args.Count != 0) { throw new Exception($"L:{context.Start.Line}|'tesouroX' não aceita argumentos.");}
                    return _jogo.GetPosicaoTesouroX();

                case "tesouroY":
                    if (args.Count != 0) { throw new Exception($"L:{context.Start.Line}|'tesouroY' não aceita argumentos.");}
                    return _jogo.GetPosicaoTesouroY();
        
                case "escapar":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'escapar()' não recebe parâmetros.");
                    _jogo.Escapar();
                    System.Threading.Thread.Sleep(DELAY_TRANSICAO_CENA);
                    return null!;
        
                case "arena":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'arena()' precisa do nome da arena.");
                    string arenaDigitada = args[0].ToString()!;
                    if (!_arenas.Contains(arenaDigitada)) throw new Exception($"L:{context.Start.Line}|A arena '{arenaDigitada}' não existe.");
                    _jogo.EntrarArena(arenaDigitada);
                    System.Threading.Thread.Sleep(DELAY_TRANSICAO_CENA);
                    return null!;

                // ==========================================
                // SISTEMA DE INVENTÁRIO E LOJA
                // ==========================================

                case "cinto.usarItem":
                    if (args.Count != 1 || !(args[0] is int)) throw new Exception($"L:{context.Start.Line}|'cinto.usarItem' exige 1 índice numérico inteiro.");
                    
                    Console.WriteLine($"[Chamada de Função] Função 'cinto.usarItem' acionada (Índice: {args[0]})");
                    _jogo.UsarItemCinto((int)args[0]);
                    return null!;
        
                case "cinto.colocarItem":
                    if (args.Count != 2 || !(args[1] is int)) throw new Exception($"L:{context.Start.Line}|'cinto.colocarItem()' exige o Item e o Índice numérico.");
                    _jogo.ColocarItemCinto(args[0].ToString()!, (int)args[1]);
                    return null!;
        
                case "mochila.colocarItem":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'mochila.colocarItem()' exige 1 Item.");
                    _jogo.ColocarItemMochila(args[0].ToString()!);
                    return null!;

                case "mochila.usarItem":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'mochila.usarItem' não aceita argumentos.");
                    
                    Console.WriteLine($"[Chamada de Função] Função 'mochila.usarItem' acionada.");
                    _jogo.UsarItemMochila();
                    return null!;
        
                case "comprar":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'comprar()' precisa do nome do produto.");

                    string produtoDigitado = args[0].ToString()!;
                    // TODO Futuro: if (!_produtos.Contains(produtoDigitado)) throw new Exception(...);
                    _jogo.Comprar(produtoDigitado);
                    return null!;
        
                case "venderTudo":
                    if (args.Count != 0) 
                        throw new Exception($"L:{context.Start.Line}|'venderTudo' não aceita argumentos.");
                    
                    Console.WriteLine($"[Chamada de Função] Função 'venderTudo' acionada.");
                    _jogo.VenderTudo();
                    return null!;

                default:
                    if (_funcoesJogador.ContainsKey(nomeCompleto))
                    {
                        var funcaoContext = _funcoesJogador[nomeCompleto];
                        string tipoDeRetorno = funcaoContext.TIPO().GetText();

                        // 1. DESCOBRE OS PARÂMETROS ESPERADOS (Nome e Tipo)
                        var parametrosEsperados = new List<(string Nome, string Tipo)>();
                        if (funcaoContext.parametro() != null)
                        {
                            foreach (var p in funcaoContext.parametro())
                            {
                                parametrosEsperados.Add((p.ID().GetText(), p.TIPO().GetText()));
                            }
                        }

                        if (args.Count != parametrosEsperados.Count)
                            throw new Exception($"L:{context.Start.Line}|A função '{nomeCompleto}' espera {parametrosEsperados.Count} argumento(s), mas recebeu {args.Count}.");

                        // 2. CRIA O ESCOPO LOCAL E INJETA ARGUMENTOS
                        var escopoLocal = new Dictionary<string, object>();

                        for (int i = 0; i < args.Count; i++)
                        {
                            var argumento = args[i];
                            string paramNome = parametrosEsperados[i].Nome;
                            string paramTipo = parametrosEsperados[i].Tipo;

                            // Validação de Tipagem dos Parâmetros
                            bool tipoInvalido = false;
                            if (paramTipo == "int" && !(argumento is int)) tipoInvalido = true;
                            if (paramTipo == "float" && !(argumento is float || argumento is int)) tipoInvalido = true; 
                            if (paramTipo == "string" && !(argumento is string)) tipoInvalido = true;
                            if (paramTipo == "bool" && !(argumento is bool)) tipoInvalido = true;

                            if (paramTipo == "vazio")
                                throw new Exception($"L:{context.Start.Line}|O parâmetro '{paramNome}' não pode ser do tipo 'vazio'.");

                            if (tipoInvalido)
                                throw new Exception($"L:{context.Start.Line}|O argumento passado para '{paramNome}' deveria ser do tipo '{paramTipo}'.");

                            escopoLocal[paramNome] = argumento; // Salva o valor na gaveta local
                        }

                        // Coloca o escopo local atual no topo da pilha
                        _escoposLocais.Push(escopoLocal); 

                        object? valorRetornado = null;

                        try
                        {
                            // 3. RODA A FUNÇÃO DO JOGADOR
                            foreach (var cmd in funcaoContext.comando()) Visit(cmd);
                        }
                        catch (ExcecaoRetorno retornoException)
                        {
                            // Ocorreu um "retorna"! O fluxo parou e caiu aqui com o valor.
                            valorRetornado = retornoException.Valor;
                        }
                        finally
                        {
                            // 4. LIMPEZA: Tira o escopo local da pilha e destrói as variáveis
                            _escoposLocais.Pop(); 
                        }

                        // ==========================================
                        // 5. VALIDAÇÕES DO GDD PARA O TIPO DE RETORNO
                        // ==========================================

                        if (tipoDeRetorno == "vazio" && valorRetornado != null)
                            throw new Exception($"L:{context.Start.Line}|O procedimento '{nomeCompleto}' é do tipo 'vazio' e não deve retornar valores.");

                        if (tipoDeRetorno != "vazio" && valorRetornado == null)
                            throw new Exception($"L:{context.Start.Line}|A função '{nomeCompleto}' exige um retorno '{tipoDeRetorno}', mas retornou vazio.");

                        if (valorRetornado != null && tipoDeRetorno != "vazio")
                        {
                            bool retornoInvalido = false;
                            if (tipoDeRetorno == "int" && !(valorRetornado is int)) retornoInvalido = true;
                            if (tipoDeRetorno == "float" && !(valorRetornado is float || valorRetornado is int)) retornoInvalido = true;
                            if (tipoDeRetorno == "string" && !(valorRetornado is string)) retornoInvalido = true;
                            if (tipoDeRetorno == "bool" && !(valorRetornado is bool)) retornoInvalido = true;

                            if (retornoInvalido)
                                throw new Exception($"L:{context.Start.Line}|A função '{nomeCompleto}' tentou retornar um tipo incorreto. Esperado: '{tipoDeRetorno}'.");
                        }

                        Console.WriteLine($"[Chamada de Função] Executou função do player '{nomeCompleto}'. Retornou: {valorRetornado ?? "vazio"}");
                        return valorRetornado!;
                    }

                    // Se o interpretador não achar nem função nativa nem função do jogador
                    throw new Exception($"L:{context.Start.Line}|O comando '{nomeCompleto}' não é reconhecido.");
                    }
        }
        
        public override object VisitDeclaracaoFuncao([NotNull] LinguagemParser.DeclaracaoFuncaoContext context)
        {
            string nomeDaFuncao = context.ID().GetText(); 
        
            // Bloqueia se tentar usar nome do sistema (Ex: mover, atacar)
            if (_palavrasReservadas.Contains(nomeDaFuncao))
                throw new Exception($"L:{context.Start.Line}|A palavra '{nomeDaFuncao}' é reservada pelo sistema e não pode ser usada como nome de função.");
        
            // Armazena função
            _funcoesJogador[nomeDaFuncao] = context;
            
            return null!;
        }
        
        public override object VisitComandoRetorno([NotNull] LinguagemParser.ComandoRetornoContext context)
        {
            // Se tiver algo na frente do 'retorna', avaliamos. Se for só 'retorna', fica nulo.
            object? valorDeRetorno = context.expressao() != null ? Visit(context.expressao()) : null;
        
            // Atira o valor para cima para interromper o fluxo da função!
            throw new ExcecaoRetorno(valorDeRetorno);
        }
       public override object VisitLista([NotNull] LinguagemParser.ListaContext context)
        {
            var lista = new List<object>(); 
            
            // Se a lista não for vazia, adiciona os itens
            if (context.expressao() != null)
            {
                foreach (var exp in context.expressao())
                {
                    lista.Add(Visit(exp));
                }
            }
            return lista; 
        }

        public override object VisitAcessoLista([NotNull] LinguagemParser.AcessoListaContext context)
        {
            string nomeVar = context.ID().GetText();

            // 1. Procura a variável na memória local ou global
            object? valorVar = null;
            if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeVar))
                valorVar = _escoposLocais.Peek()[nomeVar];
            else if (_memoria.ContainsKey(nomeVar))
                valorVar = _memoria[nomeVar];
            else
                throw new Exception($"L:{context.Start.Line}|A lista '{nomeVar}' não foi declarada.");

            // 2. Garante que ela é uma lista mesmo
            if (valorVar is List<object> lista)
            {
                object indiceObj = Visit(context.expressao()); // Pega a expressão dentro do colchete
                
                if (indiceObj is int indice)
                {
                    if (indice < 0 || indice >= lista.Count)
                        throw new Exception($"L:{context.Start.Line}|Índice {indice} fora dos limites. A lista '{nomeVar}' tem tamanho {lista.Count}.");
                    
                    return lista[indice]; 
                }
                throw new Exception($"L:{context.Start.Line}|O índice da lista deve ser um número inteiro.");
            }
            throw new Exception($"L:{context.Start.Line}|A variável '{nomeVar}' não é uma lista.");
        }
    }
    // Classe para carregar o valor do 'retorna' para fora da função
    public class ExcecaoRetorno : Exception
    {
        public object? Valor { get; }
        public ExcecaoRetorno(object? valor) { Valor = valor; }
    }
}