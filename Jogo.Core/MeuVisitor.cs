using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Microsoft.VisualBasic;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        const int TEMPO_LINHA = 0;
        private const int DELAY_TRANSICAO_CENA = 1000;

        private Dictionary<string, object> _memoria = new Dictionary<string, object>();
        private Dictionary<string, int> _linhasDeclaracaoGlobal = new Dictionary<string, int>();
        private Stack<Dictionary<string, int>> _linhasDeclaracaoLocal = new Stack<Dictionary<string, int>>();

        private Stack<Dictionary<string, object>> _escoposLocais = new Stack<Dictionary<string, object>>();

        private Dictionary<string, LinguagemParser.DeclaracaoFuncaoContext> _funcoesJogador = new Dictionary<string, LinguagemParser.DeclaracaoFuncaoContext>();
        private readonly IAcoesDoJogo _jogo;

        private HashSet<LinguagemParser.DeclaracaoVariavelContext> _declaracoesRegistradas = new HashSet<LinguagemParser.DeclaracaoVariavelContext>();

        // ==========================================
        // CATEGORIAS DE "ENUMS" E FUNÇÕES
        // ==========================================
        private HashSet<string> _direcoes = new HashSet<string> { "Cima", "Baixo", "Direita", "Esquerda" };
        private HashSet<string> _ataques = new HashSet<string> { "EsferaAzul", "EsferaVermelha", "Raio", "Gelo", "Fogo", "ExplosaoFogo", "ExplosaoGelo", "Alho" };
        private HashSet<string> _recursos = new HashSet<string> { "Moeda", "Osso", "Couro", "Magma", "Cristal", "Plasma", "Sangue", "Safira", "Esmeralda", "Diamante" };
        private HashSet<string> _inimigos = new HashSet<string> { "Goblin", "Esqueleto", "SlimeDeFogo", "SlimeDeGelo", "Lobisomem", "Orc", "Fantasma", "Vampiro" };
        private HashSet<string> _arenas = new HashSet<string> { "Campos", "Floresta", "Labirinto" };
        private HashSet<string> _itens = new HashSet<string> { "PocaoDeVida" };

        private HashSet<string> _funcoesEObjetosNativos = new HashSet<string> { 
            "mover", "podeMover", "atacar", "tempo", "vidaAtual", 
            "inimigoMaisProximo", "escanearArea", "posicaoX", "posicaoY", 
            "tesouroX", "tesouroY", "escapar", "arena", "comprar", 
            "cinto", "mochila", "venderTudo", "max", "min", "tamanho", "trunca", "aleatorio"
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
            if (context.NULO() != null) return null!;
            if (context.acessoAtributo() != null) return Visit(context.acessoAtributo());

            if (context.ChildCount == 3 && context.GetChild(0).GetText() == "(")
                return Visit(context.expressao(0)); 

            if (context.ID() != null)
            {
                string nomeVar = context.ID().GetText();
                _jogo.DestacarLinhaAtual(context.Start.Line, "leitura_var");
                System.Threading.Thread.Sleep(TEMPO_LINHA);
                DestacarDeclaracao(nomeVar, "origem_var");
                
                if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeVar))
                    return _escoposLocais.Peek()[nomeVar];

                if (_memoria.ContainsKey(nomeVar))
                    return _memoria[nomeVar];

                throw new Exception($"L:{context.Start.Line}|A variável '{nomeVar}' não foi declarada.");
            }

            if (context.NAO() != null)
            {
                object valor = Visit(context.expressao(0)); 
                if (valor is bool condicaoBooleana) return !condicaoBooleana; 
                throw new Exception($"L:{context.Start.Line}|O operador '!' só pode ser usado com Verdadeiro ou Falso.");
            }
            
            if (context.lista() != null) return Visit(context.lista());
            if (context.acessoLista() != null) return Visit(context.acessoLista());

            if (context.expressao().Length == 2)
            {
                object esquerdo = Visit(context.expressao(0));
                object direito = Visit(context.expressao(1));

                if (context.IGUAL() != null)
                {
                    if (esquerdo == null && direito == null) return true;
                    if (esquerdo == null || direito == null) return false;
                    return esquerdo.Equals(direito);
                }
                if (context.DIFERENTE() != null)
                {
                    if (esquerdo == null && direito == null) return false;
                    if (esquerdo == null || direito == null) return true;
                    return !esquerdo.Equals(direito);
                }

                if (context.E() != null || context.OU() != null)
                {
                    if (esquerdo == null || direito == null)
                        throw new Exception($"L:{context.Start.Line}|Operadores 'e'/'ou' não aceitam 'Nulo'. Use Verdadeiro ou Falso.");
                    
                    if (esquerdo is bool boolEsq && direito is bool boolDir)
                    {
                        if (context.E() != null) return boolEsq && boolDir;
                        if (context.OU() != null) return boolEsq || boolDir;
                    }
                    throw new Exception($"L:{context.Start.Line}|Operadores 'e'/'ou' só funcionam com valores lógicos (Verdadeiro/Falso).");
                }

                if (context.SOMA() != null && (esquerdo is string || direito is string))
                {
                    string strEsq = esquerdo == null ? "Nulo" : (esquerdo is bool bEsq ? (bEsq ? "Verdadeiro" : "Falso") : esquerdo.ToString());
                    string strDir = direito == null ? "Nulo" : (direito is bool bDir ? (bDir ? "Verdadeiro" : "Falso") : direito.ToString());

                    if (esquerdo is float fEsq) strEsq = fEsq.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    if (direito is float fDir) strDir = fDir.ToString(System.Globalization.CultureInfo.InvariantCulture);

                    return strEsq + strDir;
                }

                if (esquerdo == null) esquerdo = 0;
                if (direito == null) direito = 0;

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
                        
                        if (context.DIV() != null) return fDir == 0 ? 0f : fEsq / fDir;
                        if (context.MOD() != null) return fDir == 0 ? 0f : fEsq % fDir;

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
                        
                        if (context.DIV() != null) return iDir == 0 ? 0 : iEsq / iDir;
                        if (context.MOD() != null) return iDir == 0 ? 0 : iEsq % iDir;

                        if (context.MAIOR_IGUAL() != null) return iEsq >= iDir;
                        if (context.MENOR_IGUAL() != null) return iEsq <= iDir;
                        if (context.MAIOR() != null) return iEsq > iDir;
                        if (context.MENOR() != null) return iEsq < iDir;
                    }
                }

                throw new Exception($"L:{context.Start.Line}|Não é possível calcular '{esquerdo.GetType().Name}' com '{direito.GetType().Name}'.");
            }
            
            return null!;
        }

        public override object VisitDeclaracaoVariavel([NotNull] LinguagemParser.DeclaracaoVariavelContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "declaracao_var");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            
            string tipoDeclarado = context.TIPO().GetText();
            string nomeDaVariavel = context.ID().GetText();
            
            if (tipoDeclarado == "vazio")
            {
                throw new Exception($"L:{context.Start.Line}|Não é possível criar uma variável do tipo 'vazio'. Esse tipo é exclusivo para funções e procedimentos.");
            }
        
            if (_palavrasReservadas.Contains(nomeDaVariavel)) 
            {
                throw new Exception($"L:{context.Start.Line}|'{nomeDaVariavel}' é uma função ou palavra reservada do jogo e não pode ser usada como nome de variável.");
            }
            
            object valorResolvido = Visit(context.expressao());
        
            VerificarTipo(tipoDeclarado, valorResolvido, nomeDaVariavel, context.Start.Line);

            if (_escoposLocais.Count > 0)
            {
                var escopoAtual = _escoposLocais.Peek();
        
                if (escopoAtual.ContainsKey(nomeDaVariavel))
                {
                    if (_declaracoesRegistradas.Contains(context))
                    {
                        escopoAtual[nomeDaVariavel] = valorResolvido;
                        return null!;
                    }
                    
                    throw new Exception($"L:{context.Start.Line}| Variável '{nomeDaVariavel}' já foi declarada.");
                }
        
                escopoAtual[nomeDaVariavel] = valorResolvido;
                _linhasDeclaracaoLocal.Peek()[nomeDaVariavel] = context.Start.Line;
                _declaracoesRegistradas.Add(context);
            }
            else
            {
                if (_memoria.ContainsKey(nomeDaVariavel))
                {
                    if (_declaracoesRegistradas.Contains(context))
                    {
                        _memoria[nomeDaVariavel] = valorResolvido;
                        return null!;
                    }
        
                    throw new Exception($"L:{context.Start.Line}| Variável '{nomeDaVariavel}' já foi declarada.");
                }
        
                _memoria[nomeDaVariavel] = valorResolvido;
                _linhasDeclaracaoGlobal[nomeDaVariavel] = context.Start.Line;
                _declaracoesRegistradas.Add(context);
            }
        
            return null!;
        }
        private void VerificarTipo(string tipoEsperado, object valor, string nomeVar, int linha)
        {
            if (valor == null) return;

            if (valor is List<object> listaDeValores)
            {
                if (listaDeValores.Count == 0) return;

                foreach (var item in listaDeValores)
                {
                    bool itemValido = false;
                    switch (tipoEsperado)
                    {
                        case "int": itemValido = item is int; break;
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
            _jogo.DestacarLinhaAtual(context.Start.Line, "atribuicao");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            string nomeDaVariavel = context.ID().GetText();
            object novoValor = Visit(context.expressao());

            bool ehLocal = _escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeDaVariavel);
            bool ehGlobal = _memoria.ContainsKey(nomeDaVariavel);

            DestacarDeclaracao(nomeDaVariavel, "origem_var");
            if (_palavrasReservadas.Contains(nomeDaVariavel)) {
                throw new Exception($"L:{context.Start.Line}|A palavra '{nomeDaVariavel}' é reservada pelo sistema e não pode ser alterada.");
            }

            if (!ehGlobal && !ehLocal) {
                throw new Exception($"L:{context.Start.Line}|A variável '{nomeDaVariavel}' não foi criada. Declare seu tipo antes (ex: int {nomeDaVariavel} = 0).");
            }

            object valorAntigo = ehLocal ? _escoposLocais.Peek()[nomeDaVariavel] : _memoria[nomeDaVariavel];
            Type tipoOriginal = valorAntigo.GetType();
            Type tipoNovo = novoValor.GetType();

            if (tipoOriginal != tipoNovo)
            {
                if (tipoOriginal == typeof(float) && (tipoNovo == typeof(int) || tipoNovo == typeof(double)))
                {
                    novoValor = Convert.ToSingle(novoValor);
                }
                else
                {
                    string nomeOriginal = tipoOriginal.Name.Replace("Int32", "int").Replace("Single", "float").Replace("String", "string").Replace("Boolean", "bool");
                    string nomeNovo = tipoNovo.Name.Replace("Int32", "int").Replace("Single", "float").Replace("String", "string").Replace("Boolean", "bool");

                    throw new Exception($"L:{context.Start.Line}|Erro de tipo: A variável '{nomeDaVariavel}' é do tipo '{nomeOriginal}', mas você tentou atribuir um valor do tipo '{nomeNovo}'.");
                }
            }

            if (ehLocal)
            {
                _escoposLocais.Peek()[nomeDaVariavel] = novoValor;
            }
            else
            {
                _memoria[nomeDaVariavel] = novoValor;
            }
            
            return null!;
        }

        public override object VisitEstruturaSe([NotNull] LinguagemParser.EstruturaSeContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "se_senao");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            object resultadoCondicaoSe = Visit(context.expressao());

            if (!(resultadoCondicaoSe is bool condicaoSe))
                throw new Exception("Erro de Tipo: A condição do 'se' precisa ser Verdadeiro ou Falso.");

            if (condicaoSe)
            {
                Console.WriteLine("[Controle de Fluxo] O 'se' é Verdadeiro! Executando bloco principal.");
                foreach (var cmd in context.comando()) Visit(cmd);

                _jogo.DestacarLinhaAtual(context.Stop.Line, "fim_se");
                System.Threading.Thread.Sleep(TEMPO_LINHA);
                return null!; 
            }

            if (context.estruturaSenaoSe() != null)
            {
                foreach (var senaoSeContext in context.estruturaSenaoSe())
                {
                    _jogo.DestacarLinhaAtual(senaoSeContext.Start.Line, "se_senao");
                    System.Threading.Thread.Sleep(TEMPO_LINHA);
                    object resultadoSenaoSe = Visit(senaoSeContext.expressao());

                    if (!(resultadoSenaoSe is bool condicaoSenaoSe))
                        throw new Exception("Erro de Tipo: A condição do 'senão se' precisa ser Verdadeiro ou Falso.");

                    if (condicaoSenaoSe)
                    {
                        Console.WriteLine("[Controle de Fluxo] Um 'senão se' é Verdadeiro! Executando bloco.");
                        foreach (var cmd in senaoSeContext.comando()) Visit(cmd);
                        _jogo.DestacarLinhaAtual(context.Stop.Line, "fim_se");
                        System.Threading.Thread.Sleep(TEMPO_LINHA);

                        return null!;
                    }
                }
            }

            if (context.estruturaSenao() != null)
            {
                Console.WriteLine("[Controle de Fluxo] Tudo foi falso. Executando bloco 'senão'.");
                _jogo.DestacarLinhaAtual(context.estruturaSenao().Start.Line, "se_senao");
                System.Threading.Thread.Sleep(TEMPO_LINHA);
                foreach (var cmd in context.estruturaSenao().comando()) Visit(cmd);
            }

            _jogo.DestacarLinhaAtual(context.Stop.Line, "fim_se");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            return null!; 
        }
        
        public override object VisitEstruturaEnquanto([NotNull] LinguagemParser.EstruturaEnquantoContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "enquanto");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            object resultadoCondicao = Visit(context.expressao());

            if (resultadoCondicao is bool condicao)
            {
                while (condicao)
                {
                    foreach (var cmd in context.comando()) Visit(cmd);
                    System.Threading.Thread.Sleep(1);
                    _jogo.DestacarLinhaAtual(context.Start.Line, "enquanto");
                    System.Threading.Thread.Sleep(TEMPO_LINHA);
                    resultadoCondicao = Visit(context.expressao());
                    if (resultadoCondicao is bool novaCondicao) condicao = novaCondicao;
                    else throw new Exception($"L:{context.Start.Line}|A condição do 'enquanto' deixou de ser lógica no meio do loop.");
                }
                _jogo.DestacarLinhaAtual(context.Stop.Line, "fim_enquanto");
                System.Threading.Thread.Sleep(TEMPO_LINHA);
            }
            else throw new Exception($"L:{context.Start.Line}|A condição do 'enquanto' precisa ser Verdadeiro ou Falso.");

            return null!;
        }
        
        public override object VisitChamadaFuncao([NotNull] LinguagemParser.ChamadaFuncaoContext context)
        {
            string? prefixo = context.objeto != null ? context.objeto.Text : null;
            string nomeFuncao = context.funcao.Text;
            string nomeCompleto = prefixo != null ? $"{prefixo}.{nomeFuncao}" : nomeFuncao;
        
            HashSet<string> acoesComTick = new HashSet<string> { 
                "mover", "atacar", "escapar", "arena", 
                "cinto.usarItem", "mochila.usarItem", "comprar", "venderTudo" 
            };
            
            bool ehAcaoPendente = acoesComTick.Contains(nomeCompleto);

            if (ehAcaoPendente)
            {
                _jogo.DestacarLinhaAtual(context.Start.Line, "chamada_funcao_pendente");
            }
            
            List<object> args = new List<object>();
            if (context.expressao() != null)
            {
                foreach (var exp in context.expressao()) args.Add(Visit(exp));
            }
        
            string categoriaDestaque = ehAcaoPendente ? "chamada_funcao_pendente" : "chamada_funcao";
            _jogo.DestacarLinhaAtual(context.Start.Line, categoriaDestaque);
            System.Threading.Thread.Sleep(TEMPO_LINHA);

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
        
                case "tempo":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'tempo()' não recebe parâmetros.");
                    return _jogo.GetTempo();
        
                case "vidaAtual":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'vidaAtual()' não recebe parâmetros.");
                    return _jogo.GetVidaAtual();
        
                case "inimigoMaisProximo":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'inimigoMaisProximo()' não recebe parâmetros.");
                    string idInimigo = _jogo.InimigoMaisProximo();
                    if (string.IsNullOrEmpty(idInimigo) || idInimigo == "vazio") return null!;
                    return idInimigo;

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

                case "escreva":
                { 
                    if (args.Count != 1) 
                        throw new Exception($"L:{context.Start.Line}|A função 'escreva()' precisa receber exatamente 1 parâmetro.");

                    object valorParaEscrever = args[0];
                    string textoFinal = valorParaEscrever != null ? valorParaEscrever.ToString() : "";

                    if (valorParaEscrever is bool booleano)
                    {
                        textoFinal = booleano ? "Verdadeiro" : "Falso";
                    }

                    textoFinal = textoFinal.Replace("\\n", "\n");
                    _jogo.Escreva(textoFinal);
                    
                    return null!; 
                }

                case "venderTudo":
                    if (args.Count != 0) 
                        throw new Exception($"L:{context.Start.Line}|'venderTudo' não aceita argumentos.");
                    
                    Console.WriteLine($"[Chamada de Função] Função 'venderTudo' acionada.");
                    _jogo.VenderTudo();
                    return null!;

                case "max":
                    if (args.Count != 2) 
                        throw new Exception($"L:{context.Start.Line}|A função 'max()' exige exatamente 2 argumentos numéricos.");
                    
                    if (!(args[0] is float || args[0] is int) || !(args[1] is float || args[1] is int))
                        throw new Exception($"L:{context.Start.Line}|Os dois argumentos de 'max()' devem ser números.");
        
                    float v1 = Convert.ToSingle(args[0]);
                    float v2 = Convert.ToSingle(args[1]);
        
                    return (v1 >= v2) ? args[0] : args[1];
        
                case "tamanho":
                    if (args.Count != 1) 
                        throw new Exception($"L:{context.Start.Line}|A função 'tamanho()' exige 1 argumento (uma lista/vetor).");
                    
                    if (!(args[0] is List<object>))
                        throw new Exception($"L:{context.Start.Line}|O argumento de 'tamanho()' deve ser uma lista.");
        
                    var lista = (List<object>)args[0];
                    
                    return lista.Count;
        
                case "trunca":
                    if (args.Count != 1) 
                        throw new Exception($"L:{context.Start.Line}|A função 'trunca()' exige 1 argumento numérico.");
                    
                    if (!(args[0] is float || args[0] is int))
                        throw new Exception($"L:{context.Start.Line}|O argumento de 'trunca()' deve ser um número (int ou float).");
        
                    float valorParaTruncar = Convert.ToSingle(args[0]);
                    return (int)Math.Truncate(valorParaTruncar);

                case "min":
                    if (args.Count != 2) 
                        throw new Exception($"L:{context.Start.Line}|A função 'min()' exige exatamente 2 argumentos numéricos.");
                    
                    if (!(args[0] is float || args[0] is int) || !(args[1] is float || args[1] is int))
                        throw new Exception($"L:{context.Start.Line}|Os dois argumentos de 'min()' devem ser números.");

                    float minV1 = Convert.ToSingle(args[0]);
                    float minV2 = Convert.ToSingle(args[1]);

                    return (minV1 <= minV2) ? args[0] : args[1];

                case "aleatorio":
                    if (args.Count != 0) 
                        throw new Exception($"L:{context.Start.Line}|A função 'aleatorio()' não recebe argumentos.");
                    
                    float valorAleatorio = (float)Math.Round(new Random().NextDouble(), 2);
                    return valorAleatorio;

                default:
                    if (_funcoesJogador.ContainsKey(nomeCompleto))
                    {
                        var funcaoContext = _funcoesJogador[nomeCompleto];
                        string tipoDeRetorno = funcaoContext.TIPO().GetText();

                        // 1. DESCOBRE OS PARÂMETROS ESPERADOS (Nome e Tipo)
                        var parametrosEsperados = new List<(string Nome, string Tipo, int Linha)>();
                        if (funcaoContext.parametro() != null)
                        {
                            foreach (var p in funcaoContext.parametro())
                            {
                                parametrosEsperados.Add((p.ID().GetText(), p.TIPO().GetText(), p.Start.Line));
                            }
                        }

                        if (args.Count != parametrosEsperados.Count)
                            throw new Exception($"L:{context.Start.Line}|A função '{nomeCompleto}' espera {parametrosEsperados.Count} argumento(s), mas recebeu {args.Count}.");

                        // 2. CRIA O ESCOPO LOCAL E INJETA ARGUMENTOS
                        var escopoLocal = new Dictionary<string, object>();
                        var escopoLinhas = new Dictionary<string, int>();
                        for (int i = 0; i < args.Count; i++)
                        {
                            var argumento = args[i];
                            string paramNome = parametrosEsperados[i].Nome;
                            string paramTipo = parametrosEsperados[i].Tipo;

                            bool tipoInvalido = false;
                            if (paramTipo == "int" && !(argumento is int)) tipoInvalido = true;
                            if (paramTipo == "float" && !(argumento is float || argumento is int)) tipoInvalido = true; 
                            if (paramTipo == "string" && !(argumento is string)) tipoInvalido = true;
                            if (paramTipo == "bool" && !(argumento is bool)) tipoInvalido = true;

                            if (paramTipo == "vazio")
                                throw new Exception($"L:{context.Start.Line}|O parâmetro '{paramNome}' não pode ser do tipo 'vazio'.");

                            if (tipoInvalido)
                                throw new Exception($"L:{context.Start.Line}|O argumento passado para '{paramNome}' deveria ser do tipo '{paramTipo}'.");

                            escopoLocal[paramNome] = argumento;
                            escopoLinhas[paramNome] = parametrosEsperados[i].Linha;
                        }
                        _linhasDeclaracaoLocal.Push(escopoLinhas);
                        _escoposLocais.Push(escopoLocal); 

                        object? valorRetornado = null;

                        try
                        {
                            _jogo.DestacarLinhaAtual(funcaoContext.Start.Line, "corpo_funcao");
                            System.Threading.Thread.Sleep(TEMPO_LINHA);
                            // 3. RODA A FUNÇÃO DO JOGADOR
                            foreach (var cmd in funcaoContext.comando()) Visit(cmd);
                            _jogo.DestacarLinhaAtual(funcaoContext.Stop.Line, "fim_funcao");
                            System.Threading.Thread.Sleep(TEMPO_LINHA);
                        }
                        catch (ExcecaoRetorno retornoException)
                        {
                            valorRetornado = retornoException.Valor;
                        }
                        finally
                        {
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

                    throw new Exception($"L:{context.Start.Line}|O comando '{nomeCompleto}' não é reconhecido.");
                    }
        }
        
        public override object VisitDeclaracaoFuncao([NotNull] LinguagemParser.DeclaracaoFuncaoContext context)
        {
            string nomeDaFuncao = context.ID().GetText(); 

            if (_palavrasReservadas.Contains(nomeDaFuncao))
                throw new Exception($"L:{context.Start.Line}|A palavra '{nomeDaFuncao}' é reservada pelo sistema e não pode ser usada como nome de função.");

            _funcoesJogador[nomeDaFuncao] = context;
            
            return null!;
        }
        
        public override object VisitComandoRetorno([NotNull] LinguagemParser.ComandoRetornoContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "retorna");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            object? valorDeRetorno = context.expressao() != null ? Visit(context.expressao()) : null;
        
            throw new ExcecaoRetorno(valorDeRetorno);
        }
       public override object VisitLista([NotNull] LinguagemParser.ListaContext context)
        {
            var lista = new List<object>(); 
            
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
            _jogo.DestacarLinhaAtual(context.Start.Line, "leitura_var");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            DestacarDeclaracao(nomeVar, "origem_var");

            object? valorVar = null;
            if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeVar))
                valorVar = _escoposLocais.Peek()[nomeVar];
            else if (_memoria.ContainsKey(nomeVar))
                valorVar = _memoria[nomeVar];
            else
                throw new Exception($"L:{context.Start.Line}|A lista '{nomeVar}' não foi declarada.");

            if (valorVar is List<object> lista)
            {
                object indiceObj = Visit(context.expressao());
                
                if (indiceObj is int indice)
                {
                    if (indice < 0)
                        throw new Exception($"L:{context.Start.Line}|Índice '{indice}' inválido. Não é possível usar índices negativos em listas.");

                    if (indice >= lista.Count)
                    {
                        return null!;
                    }
                    
                    return lista[indice]; 
                }
                throw new Exception($"L:{context.Start.Line}|O índice da lista deve ser um número inteiro.");
            }
            throw new Exception($"L:{context.Start.Line}|A variável '{nomeVar}' não é uma lista.");
        }

        public override object VisitAcessoAtributo([NotNull] LinguagemParser.AcessoAtributoContext context)
        {
            string nomeVar = context.ID(0).GetText();
            string atributo = context.ID(1).GetText();
            _jogo.DestacarLinhaAtual(context.Start.Line, "leitura_var");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            DestacarDeclaracao(nomeVar, "origem_var");
            
            object valorVar = null;
            if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeVar))
                valorVar = _escoposLocais.Peek()[nomeVar];
            else if (_memoria.ContainsKey(nomeVar))
                valorVar = _memoria[nomeVar];
            else
                throw new Exception($"L:{context.Start.Line}|A variável '{nomeVar}' não foi declarada.");

            if (valorVar is string idInimigo)
            {

                if (!_jogo.InimigoExiste(idInimigo))
                {
                    return null!; 
                }

                switch (atributo)
                {
                    case "nome": return _jogo.ObterNomeInimigo(idInimigo);
                    case "posicaoX": return _jogo.ObterPosicaoXInimigo(idInimigo);
                    case "posicaoY": return _jogo.ObterPosicaoYInimigo(idInimigo);
                    case "vida": return _jogo.ObterVidaInimigo(idInimigo);
                    default:
                        throw new Exception($"L:{context.Start.Line}|O atributo '{atributo}' não existe em um Inimigo.");
                }
            }

            throw new Exception($"L:{context.Start.Line}|Não é possível acessar atributos de '{nomeVar}', pois ele não é um Inimigo.");
        }

        private void DestacarDeclaracao(string nomeVar, string categoria)
        {
            if (_linhasDeclaracaoLocal.Count > 0 && _linhasDeclaracaoLocal.Peek().TryGetValue(nomeVar, out int linhaLocal))
            {
                _jogo.DestacarLinhaAtual(linhaLocal, categoria);
                System.Threading.Thread.Sleep(TEMPO_LINHA);
                return;
            }
            if (_linhasDeclaracaoGlobal.TryGetValue(nomeVar, out int linhaGlobal))
            {
                _jogo.DestacarLinhaAtual(linhaGlobal, categoria);
                System.Threading.Thread.Sleep(TEMPO_LINHA);
            }
        }
        
        public override object VisitAtribuicaoLista([NotNull] LinguagemParser.AtribuicaoListaContext context)
        {
            string nomeLista = context.ID().GetText();
            
            object objIndice = Visit(context.expressao(0));
            int indice = Convert.ToInt32(objIndice); 
            
            object novoValor = Visit(context.expressao(1)); 
        
            List<object>? listaAlvo = null;
        
            if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeLista))
            {
                listaAlvo = _escoposLocais.Peek()[nomeLista] as List<object>;
            }
            else if (_memoria.ContainsKey(nomeLista))
            {
                listaAlvo = _memoria[nomeLista] as List<object>;
            }
        
            if (listaAlvo == null)
                throw new Exception($"L:{context.Start.Line}|A variável '{nomeLista}' não é uma lista válida ou não foi declarada.");
        
            if (indice < 0)
                throw new Exception($"L:{context.Start.Line}|Índice '{indice}' inválido. Não é possível usar índices negativos em listas.");
        
            while (indice >= listaAlvo.Count)
            {
                listaAlvo.Add(null!);
            }
        
            listaAlvo[indice] = novoValor;
        
            return null!;
        }

    }

    public class ExcecaoRetorno : Exception
    {
        public object? Valor { get; }
        public ExcecaoRetorno(object? valor) { Valor = valor; }
    }
}