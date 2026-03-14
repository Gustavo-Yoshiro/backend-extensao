using System;
using Antlr4.Runtime.Misc;

namespace Jogo.Core
{
    public class MeuVisitor : LinguagemBaseVisitor<object>
    {
        public override object VisitChamadaFuncao([NotNull] LinguagemParser.ChamadaFuncaoContext context)
        {
            // Pega o nome da função (ex: mover, atacar, escapar)
            string nomeDaFuncao = context.ID().GetText();

            // Pega o que está dentro do parênteses (ex: Norte, Fogo)
            String argumentos;
            if (context.expressao() != null)
            {
                argumentos = context.expressao()[0].GetText();                                    
                if (context.expressao().Length >= 1)
                {
                    for (int i = 1; i < context.expressao().Length; i++)
                    {
                        argumentos += $", {context.expressao()[i].GetText()}";
                    }
                }
            }
            else
            {
                argumentos = "nenhum";
            }
            

            // Log de Depuração, logo função call do Godot
            Console.WriteLine($"[Cérebro Ativado] O jogador quer executar a função '{nomeDaFuncao}' com o argumento(s) '{argumentos}'");

            // Continua
            return base.VisitChamadaFuncao(context);
        }
    }
}