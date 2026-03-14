using System;
using Antlr4.Runtime.Misc;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        // Dicionário de variáveis
        private Dictionary<string, object> _memoria = new Dictionary<string, object>();
        private readonly IAcoesDoJogo _acoesDoJogo;

        public MeuVisitor(IAcoesDoJogo acoesDoJogo)
        {
            _acoesDoJogo = acoesDoJogo;
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
            object valorResolvido = Visit(context.expressao());

            // VERIFICAÇÃO DE TIPO
            VerificarTipo(tipoDeclarado, valorResolvido, nomeDaVariavel);

            // Salva na memória
            _memoria[nomeDaVariavel] = valorResolvido;

            Console.WriteLine($"[Sucesso] Criou a variável '{tipoDeclarado} {nomeDaVariavel}' com valor '{valorResolvido}'");
            return null;
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

            return null;
        }
        
        // **ANALISAR impl VisitFunção!**
        public override object VisitChamadaFuncao([NotNull] LinguagemParser.ChamadaFuncaoContext context)
        {
            string nomeDaFuncao = context.ID().GetText();

            // pega os argumentos separadamente
            string arg1 = context.expressao().Length > 0 ? context.expressao()[0].GetText() : "vazio";
            string arg2 = context.expressao().Length > 1 ? context.expressao()[1].GetText() : "vazio";

            // LOG DE DEPURAÇÃO
            Console.WriteLine($"[Cerebro] Comando detectado: {nomeDaFuncao}({arg1}, {arg2})");

            // caso sensitive
            if (nomeDaFuncao == "atacar")
            {
                //passa alvo e tipo separadamente para a Interface
                _acoesDoJogo.Atacar(arg1, arg2); 
            }
            else if (nomeDaFuncao == "mover")
            {
                _acoesDoJogo.Mover(arg1);
            }
            else 
            {
                Console.WriteLine($"[ERRO] Comando '{nomeDaFuncao}' nao existe ou ainda nao implementada.");
            }

            return base.VisitChamadaFuncao(context);
        }
    }
}