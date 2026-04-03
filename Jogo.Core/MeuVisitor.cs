using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        private const int DELAY_TRANSICAO_CENA = 1000;

        private Dictionary<string, object> _memoria = new Dictionary<string, object>();
        private readonly IAcoesDoJogo _jogo;

        // ==========================================
        // CATEGORIAS DE "ENUMS" E FUNÇÕES
        // ==========================================
        private HashSet<string> _direcoes = new HashSet<string> { "Cima", "Baixo", "Direita", "Esquerda" };
        private HashSet<string> _ataques = new HashSet<string> { "EsferaAzul", "EsferaVermelha", "Agua", "Gelo", "Fogo", "ExplosaoFogo", "ExplosaoGelo", "Alho" };
        private HashSet<string> _recursos = new HashSet<string> { "Moeda", "Osso", "Couro", "Magma", "Cristal", "Plasma", "Sangue", "Safira", "Esmeralda", "Diamante" };
        private HashSet<string> _inimigos = new HashSet<string> { "Goblin", "Esqueleto", "SlimeDeFogo", "SlimeDeGelo", "Lobisomem", "Orc", "Fantasma", "Vampiro" };
        private HashSet<string> _arenas = new HashSet<string> { "Campos", "Floresta", "Labirinto" };
        
        // NOVO: Lista de todas as funções e objetos nativos do jogo
        private HashSet<string> _funcoesEObjetosNativos = new HashSet<string> { 
            "mover", "podeMover", "atacar", "nomeInimigo", "tempo", "vidaAtual", 
            "inimigoMaisProximo", "escanearArea", "posicaoX", "posicaoY", 
            "tesouroX", "tesouroY", "escapar", "arena", "comprar", 
            "cinto", "mochila" // Importante para impedir que digitem: int cinto = 5
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
                if (_memoria.ContainsKey(nomeVar)) return _memoria[nomeVar];
                
                throw new Exception($"L:{context.Start.Line}|A variável '{nomeVar}' não existe.");
            }
            
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

            // Essa é a linha que vai barrar o jogador se ele tentar: int mover = 10
            if (_palavrasReservadas.Contains(nomeDaVariavel)) {
                throw new Exception($"L:{context.Start.Line}|'{nomeDaVariavel}' é uma função ou palavra reservada do jogo e não pode ser usada como nome de variável.");
            }

            object valorResolvido = Visit(context.expressao());
            VerificarTipo(tipoDeclarado, valorResolvido, nomeDaVariavel, context.Start.Line);
            _memoria[nomeDaVariavel] = valorResolvido;

            return null!;
        }

        private void VerificarTipo(string tipoEsperado, object valor, string nomeVar, int linha)
        {
            bool tipoValido = false;
            switch (tipoEsperado)
            {
                case "int": tipoValido = valor is int; break;
                case "float": tipoValido = valor is float; break;
                case "bool": tipoValido = valor is bool; break;
                case "string":
                case "Direcao":
                case "Inimigo":
                case "Ataque":
                case "Arena":
                    tipoValido = valor is string; break;
            }
            
            if (!tipoValido) throw new Exception($"L:{linha}|O valor passado não corresponde ao tipo '{tipoEsperado}'.");
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

            _memoria[nomeDaVariavel] = novoValor;
            return null!;
        }

        public override object VisitEstruturaSe([NotNull] LinguagemParser.EstruturaSeContext context)
        {
            object resultadoCondicao = Visit(context.expressao());
            if (resultadoCondicao is bool condicao)
            {
                if (condicao)
                {
                    foreach (var cmd in context.comando()) Visit(cmd);
                }
                else if (context.estruturaSenao() != null) 
                {
                    foreach (var cmd in context.estruturaSenao().comando()) Visit(cmd);
                }
            }
            else throw new Exception($"L:{context.Start.Line}|A condição do 'se' precisa resultar em Verdadeiro ou Falso.");

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
                    if (args.Count != 2) throw new Exception($"L:{context.Start.Line}|'atacar()' precisa do Alvo e do Tipo de Ataque.");
                    string ataqueDigitado = args[1].ToString()!;
                    if (!_ataques.Contains(ataqueDigitado)) throw new Exception($"L:{context.Start.Line}|O ataque '{ataqueDigitado}' é inválido ou você não possui.");
                    _jogo.Atacar(args[0].ToString()!, ataqueDigitado);
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
        
                case "escanearArea":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'escanearArea()' não recebe parâmetros.");
                    return _jogo.EscanearArea();
        
                case "posicaoX": return _jogo.GetPosicaoPlayerX();
                case "posicaoY": return _jogo.GetPosicaoPlayerY();
                case "tesouroX": return _jogo.GetPosicaoTesouroX();
                case "tesouroY": return _jogo.GetPosicaoTesouroY();
        
                case "escapar":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'escapar()' não recebe parâmetros.");
                    _jogo.Escapar();
                    System.Threading.Thread.Sleep(DELAY_TRANSICAO_CENA);
                    return null!;
        
                case "cinto.usarItem": 
                    if (args.Count != 1 || !(args[0] is int)) throw new Exception($"L:{context.Start.Line}|'cinto.usarItem()' exige 1 índice numérico inteiro.");
                    _jogo.UsarItemCinto((int)args[0]);
                    return null!;
        
                case "mochila.usarItem":
                    if (args.Count != 0) throw new Exception($"L:{context.Start.Line}|'mochila.usarItem()' não recebe parâmetros.");
                    _jogo.UsarItemMochila();
                    return null!;
        
                case "cinto.colocarItem":
                    if (args.Count != 2 || !(args[1] is int)) throw new Exception($"L:{context.Start.Line}|'cinto.colocarItem()' exige o Item e o Índice numérico.");
                    _jogo.ColocarItemCinto(args[0].ToString()!, (int)args[1]);
                    return null!;
        
                case "mochila.colocarItem":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'mochila.colocarItem()' exige 1 Item.");
                    _jogo.ColocarItemMochila(args[0].ToString()!);
                    return null!;
        
                case "arena":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'arena()' precisa do nome da arena.");
                    string arenaDigitada = args[0].ToString()!;
                    if (!_arenas.Contains(arenaDigitada)) throw new Exception($"L:{context.Start.Line}|A arena '{arenaDigitada}' não existe.");
                    _jogo.EntrarArena(arenaDigitada);
                    System.Threading.Thread.Sleep(DELAY_TRANSICAO_CENA);
                    return null!;
        
                case "comprar":
                    if (args.Count != 1) throw new Exception($"L:{context.Start.Line}|'comprar()' precisa do nome do produto.");
                    _jogo.Comprar(args[0].ToString()!);
                    return null!;
        
                default:
                    throw new Exception($"L:{context.Start.Line}|A função '{nomeCompleto}' não foi reconhecida pelo jogo.");
            }
        }
    }
}