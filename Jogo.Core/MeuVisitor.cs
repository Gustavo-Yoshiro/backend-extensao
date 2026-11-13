using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Microsoft.VisualBasic;

namespace Jogo.Core
{
    public class Vetor : List<object>
    {
        public string TipoElemento { get; }
        public Vetor(string tipoElemento, IEnumerable<object> itens) : base(itens)
        {
            TipoElemento = tipoElemento;
        }
    }
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        const int TEMPO_LINHA = 0;
        private const int DELAY_TRANSICAO_CENA = 1000;
        private const int MAX_PROFUNDIDADE_RECURSAO = 100;
        private const int MAX_TAMANHO_LISTA = 5000;

        private Dictionary<string, object> _memoria = new Dictionary<string, object>();
        private Dictionary<string, int> _linhasDeclaracaoGlobal = new Dictionary<string, int>();
        private Stack<Dictionary<string, int>> _linhasDeclaracaoLocal = new Stack<Dictionary<string, int>>();

        private Stack<Dictionary<string, object>> _escoposLocais = new Stack<Dictionary<string, object>>();

        private Dictionary<string, string> _tiposGlobais = new Dictionary<string, string>();
        
        private Stack<Dictionary<string, string>> _tiposLocais = new Stack<Dictionary<string, string>>();

        private Dictionary<string, LinguagemParser.DeclaracaoFuncaoContext> _funcoesJogador = new Dictionary<string, LinguagemParser.DeclaracaoFuncaoContext>();
        private readonly IAcoesDoJogo _jogo;

        private HashSet<Antlr4.Runtime.ParserRuleContext> _declaracoesRegistradas = new HashSet<Antlr4.Runtime.ParserRuleContext>();

        // ==========================================
        // CATEGORIAS DE "ENUMS" E FUNÇÕES
        // ==========================================
        private HashSet<string> _direcoes = new HashSet<string> { "Cima", "Baixo", "Direita", "Esquerda" };
        private HashSet<string> _ataques = new HashSet<string> { "EsferaAzul", "EsferaVermelha", "Raio", "Gelo", "Fogo", "ExplosaoFogo", "ExplosaoGelo", "Alho" };
        private HashSet<string> _recursos = new HashSet<string> { "Moeda", "Osso", "Couro", "Magma", "Cristal", "Plasma", "Sangue", "Safira", "Esmeralda", "Diamante" };
        private HashSet<string> _inimigos = new HashSet<string> { "Goblin", "Esqueleto", "SlimeDeFogo", "SlimeDeGelo", "Lobisomem", "Orc", "Fantasma", "Vampiro" };
        private HashSet<string> _arenas = new HashSet<string> { "Campos", "Floresta", "Labirinto" };
        private HashSet<string> _itens = new HashSet<string> { "PocaoDeVida" };
        private readonly HashSet<string> _tiposValidos = new HashSet<string>
        { "int", "float", "bool", "string", "Inimigo", "Arena", "Ataque", "Direcao" };

        private HashSet<string> _funcoesEObjetosNativos = new HashSet<string> { 
            "mover", "podeMover", "atacar", "tempo", "vidaAtual", 
            "inimigoMaisProximo", "escanearArea", "posicaoX", "posicaoY", 
            "tesouroX", "tesouroY", "escapar", "arena", "comprar", 
            "cinto", "mochila", "venderTudo", "max", "min", "tamanho", "trunca", "aleatorio", "Vetor", "escreva"
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
        private static int ArgInt(List<object> args, int i, string funcao, int linha)
            => args[i] is int n ? n
            : throw new Exception($"L:{linha}|O argumento {i + 1} de '{funcao}' precisa ser um número inteiro.");
        private static void ExigirLogico(object? valor, int linha)
        {
            if (valor == null)
                throw new Exception($"L:{linha}|Operadores 'e'/'ou' não aceitam 'Nulo'. Use Verdadeiro ou Falso.");
            if (valor is not bool)
                throw new Exception($"L:{linha}|Operadores 'e'/'ou' só funcionam com valores lógicos (Verdadeiro/Falso).");
        }

        // Tem um problema que ta acontecendo que msg de erro não retorna o tipo do jogo mas sim do c# (ex: Int32). Essa função é para resolver isso.
        private static string NomeTipo(object? v) => v switch
        {
            null => "Nulo",
            int => "int",
            float => "float",
            bool => "bool",
            string => "string",
            Vetor vt => $"Vetor de {vt.TipoElemento}",
            List<object> => "vetor",
            _ => v.GetType().Name
        };
        private readonly record struct TipoDecl(string Elemento, bool EhVetor)
        {
            public override string ToString() => EhVetor ? $"Vetor de {Elemento}" : Elemento;
        }

        private static TipoDecl LerTipo(LinguagemParser.TipoCompletoContext t)
            => new TipoDecl(t.TIPO().GetText(), t.ChildCount > 1);

        // Valida valor contra um tipo que pode ser escalar ou vetor (parâmetros e retornos)
        private void VerificarTipoCompleto(TipoDecl tipo, object? valor, string nome, int linha)
        {
            if (!tipo.EhVetor)
            {
                if (valor is List<object>)
                    throw new Exception($"L:{linha}|'{nome}' espera '{tipo}', mas recebeu um vetor.");

                VerificarTipo(tipo.Elemento, valor!, nome, linha);
                return;
            }

            if (!_tiposValidos.Contains(tipo.Elemento))
                throw new Exception($"L:{linha}|O tipo '{tipo.Elemento}' não existe.");

            if (valor is not List<object> lista)
                throw new Exception($"L:{linha}|'{nome}' espera '{tipo}', mas recebeu '{NomeTipo(valor)}'.");

            if (lista is Vetor vetor && vetor.TipoElemento != tipo.Elemento)
                throw new Exception($"L:{linha}|'{nome}' espera '{tipo}', mas recebeu '{NomeTipo(vetor)}'.");

            VerificarTipo(tipo.Elemento, lista, nome, linha);   // confere os itens
        }
        
        private static bool SaoIguais(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if ((a is int || a is float) && (b is int || b is float))
                return Convert.ToDouble(a) == Convert.ToDouble(b);
            return a.Equals(b);
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

            if (context.SUB() != null && context.expressao().Length == 1)
            {
                object valorNeg = Visit(context.expressao(0));
                if (valorNeg is int i) return -i;
                if (valorNeg is float f) return -f;
                throw new Exception($"L:{context.Start.Line}|O operador '-' só pode ser usado com números.");
            }
            
            if (context.lista() != null) return Visit(context.lista());
            if (context.acessoLista() != null) return Visit(context.acessoLista());

            // Curto-circuito
            if (context.expressao().Length == 2 && (context.E() != null || context.OU() != null))
            {
                bool ehE = context.E() != null;
                int linhaLogica = context.Start.Line;

                object esq = Visit(context.expressao(0));
                ExigirLogico(esq, linhaLogica);
                bool valorEsq = (bool)esq;

                if (ehE && !valorEsq) return false;   
                if (!ehE && valorEsq) return true;     

                object dir = Visit(context.expressao(1));
                ExigirLogico(dir, linhaLogica);
                return (bool)dir;
            }

            if (context.expressao().Length == 2)
            {
                object esquerdo = Visit(context.expressao(0));
                object direito = Visit(context.expressao(1));

                if (context.IGUAL() != null)     return SaoIguais(esquerdo, direito);

                if (context.DIFERENTE() != null) return !SaoIguais(esquerdo, direito);


                if (context.SOMA() != null && (esquerdo is string || direito is string))
                {
                    string strEsq = ConverterParaStringVisivel(esquerdo);
                    string strDir = ConverterParaStringVisivel(direito);

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

                throw new Exception($"L:{context.Start.Line}|Não é possível calcular '{NomeTipo(esquerdo)}' com '{NomeTipo(direito)}'.");
            }
            
            return null!;
        }

        public override object VisitDeclaracaoVariavel([NotNull] LinguagemParser.DeclaracaoVariavelContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "declaracao_var");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
        
            string tipoDeclarado = context.TIPO().GetText();
            string nomeDaVariavel = context.ID().GetText();
            int linha = context.Start.Line;
        
            if (tipoDeclarado == "vazio")
                throw new Exception($"L:{linha}|Não é possível criar uma variável do tipo 'vazio'. Esse tipo é exclusivo para funções e procedimentos.");
        
            if (_palavrasReservadas.Contains(nomeDaVariavel))
                throw new Exception($"L:{linha}|'{nomeDaVariavel}' é uma função ou palavra reservada do jogo e não pode ser usada como nome de variável.");
        
            if (_funcoesJogador.ContainsKey(nomeDaVariavel))
                throw new Exception($"L:{linha}|'{nomeDaVariavel}' já é o nome de uma função.");
        
            object valorResolvido = Visit(context.expressao());
        
            if (valorResolvido is List<object>)
                throw new Exception($"L:{linha}|Para criar uma lista, use: Vetor de {tipoDeclarado} {nomeDaVariavel} = [...]");
        
            VerificarTipo(tipoDeclarado, valorResolvido, nomeDaVariavel, linha);
            valorResolvido = Normalizar(tipoDeclarado, valorResolvido);
        
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
                    throw new Exception($"L:{linha}| Variável '{nomeDaVariavel}' já foi declarada.");
                }
        
                escopoAtual[nomeDaVariavel] = valorResolvido;
                _tiposLocais.Peek()[nomeDaVariavel] = tipoDeclarado;          // registro do tipo
                _linhasDeclaracaoLocal.Peek()[nomeDaVariavel] = linha;
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
                    throw new Exception($"L:{linha}| Variável '{nomeDaVariavel}' já foi declarada.");
                }
        
                _memoria[nomeDaVariavel] = valorResolvido;
                _tiposGlobais[nomeDaVariavel] = tipoDeclarado;                // registro do tipo
                _linhasDeclaracaoGlobal[nomeDaVariavel] = linha;
                _declaracoesRegistradas.Add(context);
            }
        
            return null!;
        }
        private bool ValorCombinaComTipo(string tipo, object valor)
        {
            switch (tipo)
            {
                case "int":     return valor is int;
                case "float":   return valor is float || valor is int;
                case "bool":    return valor is bool;
                case "string":  return valor is string;
                case "Direcao": return valor is string d && _direcoes.Contains(d);
                case "Ataque":  return valor is string a && _ataques.Contains(a);
                case "Arena":   return valor is string r && _arenas.Contains(r);
                case "Inimigo": return valor is string;
                default:        return false;
            }
        }
        
        // Função auxiliar 
        private static object Normalizar(string tipo, object valor)
            => (tipo == "float" && valor is int i) ? (object)(float)i : valor;

        private void VerificarTipo(string tipoEsperado, object valor, string nomeVar, int linha)
        {
            if (!_tiposValidos.Contains(tipoEsperado))
                throw new Exception($"L:{linha}|O tipo '{tipoEsperado}' não existe.");

            if (valor == null) return;

            if (valor is List<object> lista)
            {
                foreach (var item in lista)
                {
                    if (item != null && !ValorCombinaComTipo(tipoEsperado, item))
                        throw new Exception($"L:{linha}|A lista '{nomeVar}' espera itens do tipo '{tipoEsperado}', mas encontrou um intruso do tipo '{NomeTipo(item)}'.");
                }
                return;
            }

            if (!ValorCombinaComTipo(tipoEsperado, valor))
                throw new Exception($"L:{linha}|O valor passado para '{nomeVar}' não corresponde ao tipo '{tipoEsperado}' (recebeu '{NomeTipo(valor)}').");
        }

        

        public override object VisitAtribuicao([NotNull] LinguagemParser.AtribuicaoContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "atribuicao");
            System.Threading.Thread.Sleep(TEMPO_LINHA);

            string nomeDaVariavel = context.ID().GetText();
            int linha = context.Start.Line;
            object novoValor = Visit(context.expressao());

            bool ehLocal = _escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeDaVariavel);
            bool ehGlobal = _memoria.ContainsKey(nomeDaVariavel);

            DestacarDeclaracao(nomeDaVariavel, "origem_var");

            if (_palavrasReservadas.Contains(nomeDaVariavel))
                throw new Exception($"L:{linha}|A palavra '{nomeDaVariavel}' é reservada pelo sistema e não pode ser alterada.");

            if (!ehGlobal && !ehLocal)
                throw new Exception($"L:{linha}|A variável '{nomeDaVariavel}' não foi criada. Declare seu tipo antes (ex: int {nomeDaVariavel} = 0).");

            object valorAntigo = ehLocal ? _escoposLocais.Peek()[nomeDaVariavel] : _memoria[nomeDaVariavel];

            if (valorAntigo is Vetor vetorAntigo)
            {
                if (novoValor is not List<object> novaLista)
                    throw new Exception($"L:{linha}|'{nomeDaVariavel}' é um vetor e só pode receber outra lista.");

                VerificarTipo(vetorAntigo.TipoElemento, novaLista, nomeDaVariavel, linha);
                novoValor = new Vetor(vetorAntigo.TipoElemento, novaLista);
            }
            else
            {
                if (novoValor is List<object>)
                    throw new Exception($"L:{linha}|'{nomeDaVariavel}' não é um vetor. Para criar uma lista, use: Vetor de <tipo> {nomeDaVariavel} = [...]");

                string? tipoDeclarado = null;
                if (ehLocal) _tiposLocais.Peek().TryGetValue(nomeDaVariavel, out tipoDeclarado);
                else         _tiposGlobais.TryGetValue(nomeDaVariavel, out tipoDeclarado);

                if (tipoDeclarado == null)
                    throw new Exception($"L:{linha}|Erro interno: o tipo da variável '{nomeDaVariavel}' não foi registrado.");

                VerificarTipo(tipoDeclarado, novoValor, nomeDaVariavel, linha);
                novoValor = Normalizar(tipoDeclarado, novoValor);
            }

            if (ehLocal) _escoposLocais.Peek()[nomeDaVariavel] = novoValor;
            else         _memoria[nomeDaVariavel] = novoValor;

            return null!;
        }

        public override object VisitEstruturaSe([NotNull] LinguagemParser.EstruturaSeContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "se_senao");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            object resultadoCondicaoSe = Visit(context.expressao());

            if (!(resultadoCondicaoSe is bool condicaoSe))
                throw new Exception($"L:{context.Start.Line}|A condição do 'se' precisa ser Verdadeiro ou Falso.");

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
                        throw new Exception($"L:{senaoSeContext.Start.Line}|A condição do 'senao se' precisa ser Verdadeiro ou Falso.");

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
        
        private static string ArgTexto(List<object> args, int i, string funcao, int linha)
            => args[i]?.ToString() 
                ?? throw new Exception($"L:{linha}|O argumento {i + 1} de '{funcao}' não pode ser Nulo.");

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
                    string dirMover = ArgTexto(args, 0, "mover", context.Start.Line);
                    if (!_direcoes.Contains(dirMover)) throw new Exception($"L:{context.Start.Line}|Direção '{dirMover}' inválida. Use Cima, Baixo, Esquerda ou Direita.");
                    _jogo.Mover(dirMover);
                    return null!;
        
                case "podeMover":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'podeMover()' precisa de 1 Direção.");
                    string dirPode = ArgTexto(args, 0, "podeMover", context.Start.Line);
                    if (!_direcoes.Contains(dirPode)) throw new Exception($"L:{context.Start.Line}|Direção '{dirPode}' inválida.");
                    return _jogo.PodeMover(dirPode);
        
                case "atacar":
                    if (args.Count != 2) { throw new Exception($"L:{context.Start.Line}| 'atacar' exige 2 argumentos."); }

                    string alvoStr = args[0] == null ? "Nulo" : args[0].ToString()!;

                    if (args[1] == null) {
                        throw new Exception($"L:{context.Start.Line}| O ataque passado não pode ser Nulo!");
                    }
                    
                    string elemento = ArgTexto(args, 1, "atacar", context.Start.Line);
                    
                    if (!_ataques.Contains(elemento)) {
                        throw new Exception($"L:{context.Start.Line}| O ataque '{elemento}' é inválido ou você não o possui.");
                    }

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

                    List<string> inimigosDetectados = _jogo.EscanearArea() ?? new List<string>();

                    List<object> listaParaOJogador = new List<object>();

                    foreach(string inimigo in inimigosDetectados)
                    {
                        listaParaOJogador.Add(inimigo);
                    }
                    
                    return new Vetor("Inimigo", listaParaOJogador);
        
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
                    string arenaDigitada = ArgTexto(args, 0, "arena", context.Start.Line);
                    if (!_arenas.Contains(arenaDigitada)) throw new Exception($"L:{context.Start.Line}|A arena '{arenaDigitada}' não existe.");
                    _jogo.EntrarArena(arenaDigitada);
                    System.Threading.Thread.Sleep(DELAY_TRANSICAO_CENA);
                    return null!;

                // ==========================================
                // SISTEMA DE INVENTÁRIO E LOJA
                // ==========================================

                case "cinto.usarItem":
                    ArgInt(args, 0, "cinto.usarItem", context.Start.Line);
                    
                    Console.WriteLine($"[Chamada de Função] Função 'cinto.usarItem' acionada (Índice: {args[0]})");
                    _jogo.UsarItemCinto((int)args[0]);
                    return null!;
        
                case "cinto.colocarItem":
                    ArgInt(args, 1, "cinto.colocarItem", context.Start.Line);
                    _jogo.ColocarItemCinto(ArgTexto(args, 0, "cinto.colocarItem", context.Start.Line), (int)args[1]);
                    return null!;
        
                case "mochila.colocarItem":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'mochila.colocarItem()' exige 1 Item.");
                    _jogo.ColocarItemMochila(ArgTexto(args, 0, "mochila.colocarItem", context.Start.Line));
                    return null!;

                case "mochila.usarItem":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'mochila.usarItem' não aceita argumentos.");
                    
                    Console.WriteLine($"[Chamada de Função] Função 'mochila.usarItem' acionada.");
                    _jogo.UsarItemMochila();
                    return null!;
        
                case "comprar":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'comprar()' precisa do nome do produto.");

                    string produtoDigitado = ArgTexto(args, 0, "comprar", context.Start.Line);
                    // TODO Futuro: if (!_produtos.Contains(produtoDigitado)) throw new Exception(...);
                    _jogo.Comprar(produtoDigitado);
                    return null!;

                case "escreva":
                { 
                    if (args.Count != 1) 
                        throw new Exception($"L:{context.Start.Line}|A função 'escreva()' precisa receber exatamente 1 parâmetro.");

                    string textoFinal = ConverterParaStringVisivel(args[0]).Replace("\\n", "\n");
                    
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
                    
                    float valorAleatorio = (float)Math.Round(Random.Shared.NextDouble(), 2);
                    return valorAleatorio;

                default:
                    if (_funcoesJogador.ContainsKey(nomeCompleto))
                    {
                        var funcaoContext = _funcoesJogador[nomeCompleto];
                        TipoDecl tipoDeRetorno = LerTipo(funcaoContext.tipoCompleto());
                        bool ehProcedimento = !tipoDeRetorno.EhVetor && tipoDeRetorno.Elemento == "vazio";

                        // 1. Parâmetros esperados
                        var parametrosEsperados = new List<(string Nome, TipoDecl Tipo, int Linha)>();
                        foreach (var p in funcaoContext.parametro())
                            parametrosEsperados.Add((p.ID().GetText(), LerTipo(p.tipoCompleto()), p.Start.Line));

                        if (args.Count != parametrosEsperados.Count)
                            throw new Exception($"L:{context.Start.Line}|A função '{nomeCompleto}' espera {parametrosEsperados.Count} argumento(s), mas recebeu {args.Count}.");

                        if (_escoposLocais.Count >= MAX_PROFUNDIDADE_RECURSAO)
                            throw new Exception($"L:{context.Start.Line}|Erro Crítico: A função '{nomeCompleto}' causou um loop de recursão infinito (limite de {MAX_PROFUNDIDADE_RECURSAO} chamadas excedido).");

                        // 2. Escopo local com os argumentos
                        var escopoLocal = new Dictionary<string, object>();
                        var escopoLinhas = new Dictionary<string, int>();
                        var escopoTipos = new Dictionary<string, string>();

                        for (int i = 0; i < args.Count; i++)
                        {
                            var argumento = args[i];
                            var (paramNome, paramTipo, paramLinha) = parametrosEsperados[i];

                            VerificarTipoCompleto(paramTipo, argumento, paramNome, context.Start.Line);

                            // Vetor entra como está (o próprio Vetor guarda o tipo); escalar é normalizado e tem o tipo registrado
                            escopoLocal[paramNome] = paramTipo.EhVetor ? argumento : Normalizar(paramTipo.Elemento, argumento);
                            escopoLinhas[paramNome] = paramLinha;
                            if (!paramTipo.EhVetor) escopoTipos[paramNome] = paramTipo.Elemento;
                        }

                        _linhasDeclaracaoLocal.Push(escopoLinhas);
                        _tiposLocais.Push(escopoTipos);
                        _escoposLocais.Push(escopoLocal);

                        object? valorRetornado = null;
                        bool retornouComValor = false;

                        try
                        {
                            _jogo.DestacarLinhaAtual(funcaoContext.Start.Line, "corpo_funcao");
                            System.Threading.Thread.Sleep(TEMPO_LINHA);
                            foreach (var cmd in funcaoContext.comando()) Visit(cmd);
                            _jogo.DestacarLinhaAtual(funcaoContext.Stop.Line, "fim_funcao");
                            System.Threading.Thread.Sleep(TEMPO_LINHA);
                        }
                        catch (ExcecaoRetorno retornoException)
                        {
                            valorRetornado = retornoException.Valor;
                            retornouComValor = retornoException.TemValor;
                        }
                        finally
                        {
                            _escoposLocais.Pop();
                            _tiposLocais.Pop();
                            _linhasDeclaracaoLocal.Pop();
                        }

                        // 3. Validação do retorno
                        if (ehProcedimento && retornouComValor)
                            throw new Exception($"L:{context.Start.Line}|O procedimento '{nomeCompleto}' é do tipo 'vazio' e não deve retornar valores.");

                        if (!ehProcedimento)
                        {
                            if (!retornouComValor)
                                throw new Exception($"L:{context.Start.Line}|A função '{nomeCompleto}' exige um retorno '{tipoDeRetorno}', mas terminou sem retornar nada.");

                            VerificarTipoCompleto(tipoDeRetorno, valorRetornado, $"retorno_de_{nomeCompleto}", context.Start.Line);

                            if (!tipoDeRetorno.EhVetor)
                                valorRetornado = Normalizar(tipoDeRetorno.Elemento, valorRetornado!);
                        }

                        return valorRetornado!;
                    }

                    throw new Exception($"L:{context.Start.Line}|O comando '{nomeCompleto}' não é reconhecido. (Funções devem ser declaradas antes de serem chamadas)");
                }
        }
        
        public override object VisitDeclaracaoFuncao([NotNull] LinguagemParser.DeclaracaoFuncaoContext context)
        {
            string nomeDaFuncao = context.ID().GetText();
            int linha = context.Start.Line;

            if (_palavrasReservadas.Contains(nomeDaFuncao))
                throw new Exception($"L:{linha}|A palavra '{nomeDaFuncao}' é reservada pelo sistema e não pode ser usada como nome de função.");

            if (_funcoesJogador.TryGetValue(nomeDaFuncao, out var existente) && existente != context)
                throw new Exception($"L:{linha}|A função '{nomeDaFuncao}' já foi declarada.");

            if (_memoria.ContainsKey(nomeDaFuncao) || (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeDaFuncao)))
                throw new Exception($"L:{linha}|'{nomeDaFuncao}' já é o nome de uma variável.");

            // Validação de parâmetros
            var vistos = new HashSet<string>();
            foreach (var p in context.parametro())
            {
                string pNome = p.ID().GetText();

                if (LerTipo(p.tipoCompleto()).Elemento == "vazio")
                    throw new Exception($"L:{p.Start.Line}|O parâmetro '{pNome}' não pode ser do tipo 'vazio'.");

                if (_palavrasReservadas.Contains(pNome))
                    throw new Exception($"L:{p.Start.Line}|'{pNome}' é uma palavra reservada e não pode ser nome de parâmetro.");

                if (!vistos.Add(pNome))
                    throw new Exception($"L:{p.Start.Line}|O parâmetro '{pNome}' está repetido.");
            }
            var tipoRetorno = LerTipo(context.tipoCompleto());
            if (tipoRetorno.EhVetor && tipoRetorno.Elemento == "vazio")
                throw new Exception($"L:{linha}|Uma função não pode retornar 'Vetor de vazio'.");

            _funcoesJogador[nomeDaFuncao] = context;
            return null!;
        }
        
        public override object VisitComandoRetorno([NotNull] LinguagemParser.ComandoRetornoContext context)
        {
            if (_escoposLocais.Count == 0)
                throw new Exception($"L:{context.Start.Line}|'retorna' só pode ser usado dentro de uma função.");

            _jogo.DestacarLinhaAtual(context.Start.Line, "retorna");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
            object? valorDeRetorno = context.expressao() != null ? Visit(context.expressao()) : null;

            throw new ExcecaoRetorno(valorDeRetorno, context.expressao() != null);
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
            if (lista.Count > MAX_TAMANHO_LISTA)
                throw new Exception($"L:{context.Start.Line}|Limite de memória excedido! As listas do jogo suportam no máximo {MAX_TAMANHO_LISTA} espaços.");
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
            object baseObj = null!;
            string nomeBase = "";

            // 1. Identifica a base: É uma função, um item de vetor ou uma variável?
            if (context.chamadaFuncao() != null)
            {
                // Caso: funcao().atributo
                baseObj = Visit(context.chamadaFuncao());
                nomeBase = context.chamadaFuncao().GetText();
            }
            else if (context.acessoLista() != null)
            {
                // Caso: area[1].atributo
                baseObj = Visit(context.acessoLista());
                nomeBase = context.acessoLista().GetText();
            }
            else if (context.ID().Length == 2)
            {
                // Caso: variavel.atributo
                nomeBase = context.ID(0).GetText();
                _jogo.DestacarLinhaAtual(context.Start.Line, "leitura_var");
                System.Threading.Thread.Sleep(TEMPO_LINHA);
                DestacarDeclaracao(nomeBase, "origem_var");
                
                if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeBase))
                    baseObj = _escoposLocais.Peek()[nomeBase];
                else if (_memoria.ContainsKey(nomeBase))
                    baseObj = _memoria[nomeBase];
                else
                    throw new Exception($"L:{context.Start.Line}|A variável '{nomeBase}' não foi declarada.");
            }

            // 2. Extrai o nome do atributo (na nossa regra, o atributo é SEMPRE o último ID)
            string atributo = context.ID(context.ID().Length - 1).GetText();

            if (baseObj == null) return null!;

            // 3. Lógica de acesso (já configurada para Inimigos)
            if (baseObj is string idInimigo)
            {
                if (!_jogo.InimigoExiste(idInimigo)) return null!;

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

            throw new Exception($"L:{context.Start.Line}|Não é possível acessar atributos de '{nomeBase}', pois ele não é um Inimigo.");
        }

        private string ConverterParaStringVisivel(object? valor)
        {
            if (valor == null) return "Nulo";
            if (valor is bool b) return b ? "Verdadeiro" : "Falso";
            if (valor is float f) return f.ToString(System.Globalization.CultureInfo.InvariantCulture);

            if (valor is List<object> lista)
            {
                List<string> itensFormatados = new List<string>();
                foreach (var item in lista)
                {
                    itensFormatados.Add(ConverterParaStringVisivel(item));
                }
                return "[" + string.Join(", ", itensFormatados) + "]";
            }
            
            if (valor is string texto)
            {
                if (_jogo.InimigoExiste(texto))
                {
                    return _jogo.ObterNomeInimigo(texto);
                }
                return texto;
            }
            
            return valor.ToString()!;
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
            int linha = context.Start.Line;
            string nomeLista = context.ID().GetText();
        
            object objIndice = Visit(context.expressao(0));
            if (objIndice is not int indice)
                throw new Exception($"L:{linha}|O índice da lista deve ser um número inteiro.");
        
            object novoValor = Visit(context.expressao(1));
        
            // Localiza a lista
            List<object>? listaAlvo = null;
            if (_escoposLocais.Count > 0 && _escoposLocais.Peek().ContainsKey(nomeLista))
                listaAlvo = _escoposLocais.Peek()[nomeLista] as List<object>;
            else if (_memoria.ContainsKey(nomeLista))
                listaAlvo = _memoria[nomeLista] as List<object>;
        
            if (listaAlvo == null)
                throw new Exception($"L:{linha}|A variável '{nomeLista}' não é uma lista válida ou não foi declarada.");
        
            // Valida índice
            if (indice < 0)
                throw new Exception($"L:{linha}|Índice '{indice}' inválido. Não é possível usar índices negativos em listas.");
        
            if (indice >= MAX_TAMANHO_LISTA)
                throw new Exception($"L:{linha}|Limite de memória excedido! As listas do jogo suportam no máximo {MAX_TAMANHO_LISTA} espaços.");
        
            // Valida tipo do valor
            if (novoValor is List<object>)
                throw new Exception($"L:{linha}|Não é possível guardar uma lista dentro de um vetor.");
        
            if (listaAlvo is Vetor vetor)
            {
                VerificarTipo(vetor.TipoElemento, novoValor, $"{nomeLista}[{indice}]", linha);
        
                if (vetor.TipoElemento == "float" && novoValor is int n)
                    novoValor = (float)n;
            }
        
            // 4. Grava
            while (indice >= listaAlvo.Count)
                listaAlvo.Add(null!);
        
            listaAlvo[indice] = novoValor;
            return null!;
        }
        public override object VisitDeclaracaoLista([NotNull] LinguagemParser.DeclaracaoListaContext context)
        {
            _jogo.DestacarLinhaAtual(context.Start.Line, "declaracao_var");
            System.Threading.Thread.Sleep(TEMPO_LINHA);
        
            string tipo = context.TIPO().GetText();
            string nome = context.ID().GetText();
            int linha = context.Start.Line;
        
            if (tipo == "vazio")
                throw new Exception($"L:{linha}|Não é possível criar um Vetor do tipo 'vazio'.");
        
            if (_palavrasReservadas.Contains(nome))
                throw new Exception($"L:{linha}|'{nome}' é uma função ou palavra reservada do jogo e não pode ser usada como nome de variável.");
        
            if (_funcoesJogador.ContainsKey(nome))
                throw new Exception($"L:{context.Start.Line}|'{nome}' já é o nome de uma função.");

            object valor = Visit(context.expressao());

            if (valor is not List<object> origem)
                throw new Exception($"L:{linha}|O vetor '{nome}' precisa receber uma lista (ex: [1, 2, 3]) ou uma função que retorne uma lista.");
            
            VerificarTipo(tipo, origem, nome, linha);
            
            if (origem.Count > MAX_TAMANHO_LISTA)
                throw new Exception($"L:{linha}|Limite de memória excedido! As listas do jogo suportam no máximo {MAX_TAMANHO_LISTA} espaços.");
            
            var itens = new List<object>(origem);   // cópia: não altera a lista original

            if (tipo == "float")
                for (int i = 0; i < itens.Count; i++)
                    if (itens[i] is int n) itens[i] = (float)n;
            
            valor = new Vetor(tipo, itens);
        
            if (_escoposLocais.Count > 0)
            {
                var escopoAtual = _escoposLocais.Peek();
        
                if (escopoAtual.ContainsKey(nome))
                {
                    if (_declaracoesRegistradas.Contains(context))
                    {
                        escopoAtual[nome] = valor;
                        return null!;
                    }
                    throw new Exception($"L:{linha}| Variável '{nome}' já foi declarada.");
                }
        
                escopoAtual[nome] = valor;
                _linhasDeclaracaoLocal.Peek()[nome] = linha;
                _declaracoesRegistradas.Add(context);
            }
            else
            {
                if (_memoria.ContainsKey(nome))
                {
                    if (_declaracoesRegistradas.Contains(context))
                    {
                        _memoria[nome] = valor;
                        return null!;
                    }
                    throw new Exception($"L:{linha}| Variável '{nome}' já foi declarada.");
                }
        
                _memoria[nome] = valor;
                _linhasDeclaracaoGlobal[nome] = linha;
                _declaracoesRegistradas.Add(context);
            }
        
            return null!;
        }
        
   
    }

    public class ExcecaoRetorno : Exception
    {
        public object? Valor { get; }
        public bool TemValor { get; }
        public ExcecaoRetorno(object? valor, bool temValor) { Valor = valor; TemValor = temValor; }
    }

}