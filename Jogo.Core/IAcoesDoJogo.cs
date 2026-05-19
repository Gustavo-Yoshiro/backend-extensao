using System.Collections.Generic; // Necessário para a List<>

namespace Jogo.Core
{
    public interface IAcoesDoJogo
    {
        // AÇÕES PRINCIPAIS DO JOGADOR
        void Mover(string direcao);
        void Atacar(string alvo, string tipo);
        void Escapar();
        void EntrarArena(string arena);

        // SISTEMA E FRONTEND
        void NotificarErro(string mensagem); 
        void Escreva(string texto);
        void DestacarLinhaAtual(int linha, string categoria = "");

        // SENSORES E INFORMAÇÕES DO AMBIENTE
        string InimigoMaisProximo();
        bool InimigoExiste(string inimigoId);
        bool PodeMover(string direcao);
        int GetTempo();
        int GetVidaAtual();
        List<string> EscanearArea(); 
        
        // SISTEMA DE COORDENADAS
        int GetPosicaoPlayerX();
        int GetPosicaoPlayerY();
        int GetPosicaoTesouroX();
        int GetPosicaoTesouroY();
        
        // INVENTÁRIO E ECONOMIA
        void UsarItemCinto(int indice);
        void UsarItemMochila();
        void Comprar(string item);
        void VenderTudo();
        void ColocarItemMochila(string item);
        void ColocarItemCinto(string item, int idx);


        // ATRIBUTOS DE OBJETOS (Ex: alvo.nome)
        string ObterNomeInimigo(string inimigoId);
        int ObterPosicaoXInimigo(string inimigoId);
        int ObterPosicaoYInimigo(string inimigoId);
        float ObterVidaInimigo(string inimigoId);

    }
}