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
            string argumentos = context.expressao() != null ? context.expressao()[0].GetText() : "nenhum";

            // Log de Depuração, logo função call do Godot
            Console.WriteLine($"[Cérebro Ativado] O jogador quer executar a função '{nomeDaFuncao}' com o argumento '{argumentos}'");

            // Continua
            return base.VisitChamadaFuncao(context);
        }
    }
}