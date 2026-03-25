using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        // Dicionário de variáveis (Memória do Jogo)
        private Dictionary<string, object> _memoria = new Dictionary<string, object>();
        private readonly IAcoesDoJogo _acoesDoJogo;

        // Lista de palavras sagradas que o jogador não pode usar como nome de variável
        private HashSet<string> _palavrasReservadas = new HashSet<string> { 
            // Direções
            "norte", "sul", "leste", "oeste", 
            // Elementos
            "fogo", "agua", "gelo", "raio",
            // Alvos
            "mais_perto", "aleatorio", "menos_vida",
            // Ações / Comandos
            "mover", "atacar"
        };

        public MeuVisitor(IAcoesDoJogo acoesDoJogo)
        {
            _acoesDoJogo = acoesDoJogo;

            // VARIÁVEIS FIXAS DO SISTEMA 
            // Direções
            _memoria["norte"] = "norte";
            _memoria["sul"] = "sul";
            _memoria["leste"] = "leste";
            _memoria["oeste"] = "oeste";

            // Elementos
            _memoria["fogo"] = "fogo";
            _memoria["agua"] = "agua";
            _memoria["gelo"] = "gelo";
            _memoria["raio"] = "raio";

            // Alvos
            _memoria["mais_perto"] = "mais_perto";
            _memoria["aleatorio"] = "aleatorio";
            _memoria["menos_vida"] = "menos_vida";

            
        }

        public override object VisitExpressao([NotNull] LinguagemParser.ExpressaoContext context)
        {
            // CASCATA DE VERIFICAÇÃO

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

            Console.WriteLine($"[Sucesso] Criou a variável '{tipoDeclarado} {nomeDaVariavel}' com valor '{valorResolvido}'");
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
                }
                else
                {
                    Console.WriteLine("[Controle de Fluxo] O 'se' é Falso. Ignorando os comandos do bloco...");
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
            string nomeDaFuncao = context.ID().GetText();
            int qtdArgs = context.expressao().Length;

            // Resolve os valores usando o Visit (permite usar as variáveis e constantes)
            object arg1Raw = qtdArgs > 0 ? Visit(context.expressao(0)) : "vazio";
            object arg2Raw = qtdArgs > 1 ? Visit(context.expressao(1)) : "vazio";

            // Transforma em string segura para não dar erro nulo
            string arg1 = arg1Raw?.ToString() ?? "vazio";
            string arg2 = arg2Raw?.ToString() ?? "vazio";

            Console.WriteLine($"[DEBUG] Comando Detectado: {nomeDaFuncao}({arg1}, {arg2})");

            switch (nomeDaFuncao)
            {
                case "atacar":
                    if (qtdArgs < 2) 
                    {
                        _acoesDoJogo.NotificarErro("Erro: 'atacar' exige (alvo, elemento).");
                    } 
                    else 
                    {
                        bool alvoValido = arg1 == "mais_perto" || arg1 == "aleatorio" || arg1 == "menos_vida";
                        bool elementoValido = arg2 == "fogo" || arg2 == "agua" || arg2 == "gelo" || arg2 == "raio";

                        if (!alvoValido)
                        {
                            _acoesDoJogo.NotificarErro($"Erro: Alvo '{arg1}' inválido. Use: mais_perto, aleatorio, menos_vida.");
                        }
                        else if (!elementoValido) 
                        {
                            _acoesDoJogo.NotificarErro($"Erro: Elemento '{arg2}' inválido. Use: fogo, agua, gelo, raio.");
                        }
                        else 
                        {
                            Console.WriteLine($"[SUCESSO] Godot -> Atacar({arg1}, {arg2})");
                            _acoesDoJogo.Atacar(arg1, arg2);
                        }
                    }
                    break;

                case "mover":
                    if (qtdArgs < 1) 
                    {
                        _acoesDoJogo.NotificarErro("Erro: 'mover' precisa de uma direção.");
                    } 
                    else 
                    {
                        if (arg1 == "norte" || arg1 == "sul" || arg1 == "leste" || arg1 == "oeste") 
                        {
                            Console.WriteLine($"[SUCESSO] Godot -> Mover({arg1})");
                            _acoesDoJogo.Mover(arg1);
                        } 
                        else 
                        {
                            _acoesDoJogo.NotificarErro($"Erro: Direção '{arg1}' inválida para a grade.");
                        }
                    }
                    break;

                default:
                    _acoesDoJogo.NotificarErro($"Erro: O comando '{nomeDaFuncao}' não é reconhecido.");
                    break;
            }

            return null!; 
        }
    }
}