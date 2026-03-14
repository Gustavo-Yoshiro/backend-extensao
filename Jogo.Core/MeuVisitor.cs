using System;
using Antlr4.Runtime.Misc;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        private readonly IAcoesDoJogo _acoesDoJogo;

        public MeuVisitor(IAcoesDoJogo acoesDoJogo)
        {
            _acoesDoJogo = acoesDoJogo;
        }

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