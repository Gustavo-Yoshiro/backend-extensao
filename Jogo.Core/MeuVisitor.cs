using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        // Dicionário de variáveis (Memória do Jogo)
        private Dictionary<string, object> _memoria = new Dictionary<string, object>();
        private readonly IAcoesDoJogo _jogo;

        // Lista de palavras sagradas que o jogador não pode usar como nome de variável
        private HashSet<string> _palavrasReservadas = new HashSet<string> { 
            // Direções
            "Cima", "Baixo", "Direita", "Esquerda",
            // Elementos
            "fogo", "agua", "gelo", "raio",
            // Ataques
            "EsferaAzul", "EsferaVermelha", "Agua", "Gelo", "Fogo", "ExplosaoFogo", "ExplosaoGelo", "Alho",
            // Recursos
            "Moeda", "Osso", "Couro", "Magma", "Cristal", "Plasma", "Sangue", "Safira", "Esmeralda", "Diamante",
            // Inimigos
            "Goblin", "Esqueleto", "SlimeDeFogo", "SlimeDeGelo", "Lobisomem", "Orc", "Fantasma", "Vampiro",
            // Arenas
            "Campos", "Floresta", "Labirinto",
            // Alvos
            "mais_perto", "aleatorio", "menos_vida",
            // Ações / Comandos
            "mover", "atacar"

        };

        public MeuVisitor(IAcoesDoJogo acoesDoJogo)
        {
            _jogo = acoesDoJogo;

            // VARIÁVEIS FIXAS DO SISTEMA
            foreach (var constante in _palavrasReservadas)
            {
                _memoria[constante] = constante;
            }
            
        }

        public override object VisitExpressao([NotNull] LinguagemParser.ExpressaoContext context)
        {
            // CASCATA DE VERIFICAÇÃO
            if (context.chamadaFuncao() != null) 
            {
                return Visit(context.chamadaFuncao());
            }
            // 1. int
            if (context.NUMERO_INT() != null) return int.Parse(context.NUMERO_INT().GetText());

            // 2. float
            if (context.NUMERO_FLOAT() != null) return float.Parse(context.NUMERO_FLOAT().GetText(), System.Globalization.CultureInfo.InvariantCulture);

            // 3. string
            if (context.STRING_LIT() != null) return context.STRING_LIT().GetText().Trim('"');

            // 4. boolean
            if (context.BOOLEANO() != null) return context.BOOLEANO().GetText() == "Verdadeiro";

            // **PARTE DE OPERAÇÕES**
            // Se a regra tiver 3 filhos e o primeiro for '(', é uma expressão isolada
            if (context.ChildCount == 3 && context.GetChild(0).GetText() == "(")
            {
                // Ignora os parênteses e resolve 
                return Visit(context.expressao(0)); 
            }

            // 5. Busca na memória
            if (context.ID() != null)
            {
                string nomeVar = context.ID().GetText();
                if (_memoria.ContainsKey(nomeVar)) return _memoria[nomeVar];
                throw new Exception($"Erro: A variável '{nomeVar}' não existe!");
            }
            
            // **OPERAÇÕES**

            // 1. PARÊNTESES isolados '( expressao )'
            if (context.ChildCount == 3 && context.GetChild(0).GetText() == "(")
            {
                return Visit(context.expressao(0));
            }

            // 2. OPERAÇÕES BINÁRIAS
            if (context.expressao().Length == 2)
            {
                // Resolvemos o lado esquerdo e o lado direito primeiro
                object esquerdo = Visit(context.expressao(0));
                object direito = Visit(context.expressao(1));

                // --- LÓGICA ('e', 'ou') ---
                if (context.E() != null || context.OU() != null)
                {
                    // Garantir que valEsq e valDir sempre terão valor se entrarem no IF, pra evitar erro
                    if (esquerdo is bool boolEsq && direito is bool boolDir)
                    {
                        if (context.E() != null) return boolEsq && boolDir;
                        if (context.OU() != null) return boolEsq || boolDir;
                    }
                    throw new Exception("Erro de Tipo: Operadores 'e'/'ou' só funcionam com expressões lógicas.");
                }

                // --- IGUALDADE ('==') ---
                if (context.IGUAL() != null)
                {
                    return esquerdo.Equals(direito); 
                }

                // --- MATEMÁTICA E COMPARAÇÃO ---
                bool esqEhNumero = esquerdo is int || esquerdo is float;
                bool dirEhNumero = direito is int || direito is float;

                if (esqEhNumero && dirEhNumero)
                {
                    // Se QUALQUER UM dos lados for float, a conta toda vira float
                    if (esquerdo is float || direito is float)
                    {
                        // Convert.ToSingle transforma int em float
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
                        // Se ambos são int, a conta é int
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

                // --- CONCATENAÇÃO DE TEXTO ---
                if (context.SOMA() != null && (esquerdo is string || direito is string))
                {
                    return esquerdo.ToString() + direito.ToString();
                }

                throw new Exception($"Erro de Operação: Não sei como operar '{esquerdo.GetType().Name}' com '{direito.GetType().Name}'.");
            }

            return null!;
        }

        public override object VisitDeclaracaoVariavel([NotNull] LinguagemParser.DeclaracaoVariavelContext context)
        {
            string tipoDeclarado = context.TIPO().GetText();
            string nomeDaVariavel = context.ID().GetText();

            // -- ESCUDO DE PALAVRAS RESERVADAS --
            if (_palavrasReservadas.Contains(nomeDaVariavel)) {
                throw new Exception($"Erro: '{nomeDaVariavel}' é uma palavra reservada do sistema e não pode ser usada como variável.");
            }

            object valorResolvido = Visit(context.expressao());

            // VERIFICAÇÃO DE TIPO
            VerificarTipo(tipoDeclarado, valorResolvido, nomeDaVariavel);

            // Salva na memória 
            _memoria[nomeDaVariavel] = valorResolvido;

            Console.WriteLine($"[Declaração] Criou a variável '{tipoDeclarado} {nomeDaVariavel}' com valor '{valorResolvido}'");
            return null!;
        }
        // Método auxiliar
        private void VerificarTipo(string tipoEsperado, object valor, string nomeVar)
        {
            bool tipoValido = false;

            // Checar se o tipo real do objeto bate com o tipo que o jogador escreveu
            switch (tipoEsperado)
            {
                case "int":
                    tipoValido = valor is int; // O 'is' do C# verifica se o objeto é daquele tipo
                    break;
                case "string":
                    tipoValido = valor is string;
                    break;
                case "float":
                    tipoValido = valor is float;
                    break;
                case "bool":
                    tipoValido = valor is bool;
                    break;
            }
            if (!tipoValido)
            {
                throw new Exception($"Erro de Tipo: Tentou salvar um valor '{valor.GetType().Name}' na variável '{nomeVar}' que é do tipo '{tipoEsperado}'.");
            }
        }

        public override object VisitAtribuicao([NotNull] LinguagemParser.AtribuicaoContext context)
        {
            string nomeDaVariavel = context.ID().GetText();

            // -- ESCUDO DE CONSTANTES --
            if (_palavrasReservadas.Contains(nomeDaVariavel)) {
                throw new Exception($"Erro: '{nomeDaVariavel}' é uma constante do sistema e não pode ser alterada.");
            }

            if (!_memoria.ContainsKey(nomeDaVariavel))
            {
                throw new Exception($"Erro: A variável '{nomeDaVariavel}' não foi declarada!");
            }

            object novoValor = Visit(context.expressao());

            // Verificação de Segurança (Tipos)
            object valorAntigo = _memoria[nomeDaVariavel];

            // Valor novo diferente do tipo do valor antigo = erro
            if (valorAntigo.GetType() != novoValor.GetType())
            {
                throw new Exception($"Erro de Tipo: A variável '{nomeDaVariavel}' guarda valores do tipo '{valorAntigo.GetType().Name}', mas tentou atribuir um '{novoValor.GetType().Name}'.");
            }

            // Atualiza o valor
            _memoria[nomeDaVariavel] = novoValor;

            Console.WriteLine($"[Atribuição] A variável '{nomeDaVariavel}' foi atualizada para o valor '{novoValor}'");

            return null!;
        }

        public override object VisitEstruturaSe([NotNull] LinguagemParser.EstruturaSeContext context)
        {
            object resultadoCondicao = Visit(context.expressao());
            if (resultadoCondicao is bool condicao)
            {
                if (condicao)
                {
                    Console.WriteLine("[Controle de Fluxo] O 'se' é Verdadeiro! Entrando no bloco...");

                    foreach (var cmd in context.comando())
                    {
                        Visit(cmd);
                    }
                }// Verifica se existe else e executa o código se necessário.
                else if (context.estruturaSenao() != null) 
                {
                    Console.WriteLine("[Controle de Fluxo] O 'se' é Falso. Entrando no bloco 'senao'...");

                    foreach (var cmd in context.estruturaSenao().comando())
                    {
                        Visit(cmd);
                    }
                }
                else
                {
                    Console.WriteLine("[Controle de Fluxo] O 'se' é falso e não foi identificado um 'senao', skipando código");
                }
            }
            else
            {
                throw new Exception($"Erro de Tipo: A condição do 'se' precisa ser Verdadeiro ou Falso, mas você passou um '{resultadoCondicao.GetType().Name}'.");
            }

            return null!;

        }
        
        public override object VisitEstruturaEnquanto([NotNull] LinguagemParser.EstruturaEnquantoContext context)
        {
            int contador = 0;

            object resultadoCondicao = Visit(context.expressao());

            if (resultadoCondicao is bool condicao)
            {
                while (condicao)
                {
                    contador++;
                    foreach (var cmd in context.comando())
                    {
                        Visit(cmd);
                    }

                    resultadoCondicao = Visit(context.expressao());

                    if (resultadoCondicao is bool novaCondicao)
                    {
                        condicao = novaCondicao;
                    }
                    else
                    {
                        throw new Exception("Erro: A condição do 'enquanto' deixou de ser Verdadeiro ou Falso no meio da execução.");
                    }
                }

                Console.WriteLine($"[Controle de Fluxo] O 'enquanto' terminou após {contador} repetições.");
            }
            else
            {
                throw new Exception($"Erro de Tipo: A condição do 'enquanto' precisa ser Verdadeiro ou Falso.");
            }

            return null!;
        }
        
        public override object VisitChamadaFuncao([NotNull] LinguagemParser.ChamadaFuncaoContext context)
        {
            // Verifica se existe um objeto
            string? prefixo = context.objeto != null ? context.objeto.Text : null;
            string nomeFuncao = context.funcao.Text;
        
            // Junta o prefixo e a função para leitura no switch
            string nomeCompleto = prefixo != null ? $"{prefixo}.{nomeFuncao}" : nomeFuncao;
        
            // Avalia e guarda todos os argumentos
            List<object> args = new List<object>();
            if (context.expressao() != null)
            {
                foreach (var exp in context.expressao())
                {
                    args.Add(Visit(exp));
                }
            }
        
            string argsLog = string.Join(", ", args);
        
            // ROTEADOR DE FUNÇÕES DO JOGO
            switch (nomeCompleto)
            {
                // ==========================================
                // AÇÕES BÁSICAS E COMBATE
                // ==========================================
                case "mover":
                    if (args.Count != 1) { _jogo.NotificarErro("'mover' exige exatamente 1 argumento."); return null!; }
                    string dirMover = args[0].ToString()!;
        
                    var direcoesMover = new List<string> { "Cima", "Baixo", "Direita", "Esquerda", "norte", "sul", "leste", "oeste" };
                    if (!direcoesMover.Contains(dirMover)) 
                    { 
                        _jogo.NotificarErro($"A direção '{dirMover}' é inválida para a grade."); 
                        return null!; 
                    }
        
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.Mover(dirMover);
                    return null!;
        
                case "podeMover":
                    if (args.Count != 1) { _jogo.NotificarErro("'podeMover' exige exatamente 1 direção."); return false; }
                    string dirPode = args[0].ToString()!;
                    
                    var direcoesPode = new List<string> { "Cima", "Baixo", "Direita", "Esquerda", "norte", "sul", "leste", "oeste" };
                    if (!direcoesPode.Contains(dirPode)) { _jogo.NotificarErro($"A direção '{dirPode}' é inválida."); return false; }
                    
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.PodeMover(dirPode);
        
                case "atacar":
                    if (args.Count != 2) { _jogo.NotificarErro("'atacar' exige 2 argumentos."); return null!; }
        
                    object alvo = args[0];
                    string elemento = args[1].ToString()!;
        
                    if (alvo is string alvoStr)
                    {
                        var alvosValidos = new List<string> { "mais_perto", "menos_vida", "aleatorio" };
                        if (!alvosValidos.Contains(alvoStr))
                        {
                            _jogo.NotificarErro($"Alvo '{alvoStr}' inválido.");
                            return null!;
                        }
                    }
        
                    var elementosValidos = new List<string> { "fogo", "gelo", "raio", "Agua", "agua" };
                    if (!elementosValidos.Contains(elemento))
                    {
                        _jogo.NotificarErro($"Elemento '{elemento}' inválido.");
                        return null!;
                    }
        
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.Atacar(alvo, elemento);
                    return null!;
        
                case "nomeInimigo":
                    if (args.Count != 1) { _jogo.NotificarErro("'nomeInimigo' exige 1 alvo."); return ""; }
                    
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.GetNomeInimigo(args[0]);
        
                // ==========================================
                // STATUS E MAPA
                // ==========================================
                case "tempo":
                    if (args.Count != 0) { _jogo.NotificarErro("'tempo' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.GetTempo();
        
                case "vidaAtual":
                    if (args.Count != 0) { _jogo.NotificarErro("'vidaAtual' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.GetVidaAtual();
        
                case "inimigoMaisProximo":
                    if (args.Count != 0) { _jogo.NotificarErro("'inimigoMaisProximo' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.InimigoMaisProximo();
        
                case "escanearArea":
                    if (args.Count != 0) { _jogo.NotificarErro("'escanearArea' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.EscanearArea();
        
                case "posicao":
                    if (args.Count != 0) { _jogo.NotificarErro("'posicao' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.GetPosicaoPlayer();
        
                case "tesouro":
                    if (args.Count != 0) { _jogo.NotificarErro("'tesouro' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    return _jogo.GetPosicaoTesouro();
        
                case "escapar":
                    if (args.Count != 0) { _jogo.NotificarErro("'escapar' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.Escapar();
                    return null!;
        
                // ==========================================
                // INVENTÁRIO (Cinto e Mochila)
                // ==========================================
                case "cinto.usarItem": // <-- ESTA FUNÇÃO HAVIA SUMIDO!
                    if (args.Count != 1 || !(args[0] is int)) { _jogo.NotificarErro("'cinto.usarItem' exige 1 índice numérico inteiro."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.UsarItemCinto((int)args[0]);
                    return null!;
        
                case "mochila.usarItem":
                    if (args.Count != 0) { _jogo.NotificarErro("'mochila.usarItem' não aceita argumentos."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.UsarItemMochila();
                    return null!;
        
                case "cinto.colocarItem":
                    if (args.Count != 2 || !(args[1] is int)) { _jogo.NotificarErro("'cinto.colocarItem' exige 1 item (texto) e 1 índice (número inteiro)."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.ColocarItemCinto(args[0].ToString()!, (int)args[1]);
                    return null!;
        
                case "mochila.colocarItem":
                    if (args.Count != 1) { _jogo.NotificarErro("'mochila.colocarItem' exige 1 item (texto)."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.ColocarItemMochila(args[0].ToString()!);
                    return null!;
        
                // ==========================================
                // VILAREJO EXCLUSIVO
                // ==========================================
                case "arena":
                    if (args.Count != 1) { _jogo.NotificarErro("'arena' exige o nome da arena (texto)."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.EntrarArena(args[0].ToString()!);
                    return null!;
        
                case "comprar":
                    if (args.Count != 1) { _jogo.NotificarErro("'comprar' exige o nome do produto (texto)."); return null!; }
                    Console.WriteLine($"[Chamada de Função] Função '{nomeCompleto}' foi chamada com argumentos: [{argsLog}]");
                    _jogo.Comprar(args[0].ToString()!);
                    return null!;
        
                default:
                    _jogo.NotificarErro($"O comando '{nomeCompleto}' não é reconhecido.");
                    return null!;
            }
        }
    }
}