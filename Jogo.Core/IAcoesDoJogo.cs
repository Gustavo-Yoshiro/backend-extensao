using System.Collections.Generic; // Necessário para a List<>

namespace Jogo.Core
{
    // Este é o "controle remoto" que o Cérebro usa.
    // Ele define as ações que o jogo tem
    public interface IAcoesDoJogo
    {
        // AÇÕES PRINCIPAIS DO JOGADOR
        void Mover(string direcao);
        void Atacar(string alvo, string tipo); // Usa string (ID do alvo) em vez de object
        void Escapar();
        void EntrarArena(string arena);

        // SISTEMA E FRONTEND
        void NotificarErro(string mensagem); 
        void Escreva(string texto);

        // SENSORES E INFORMAÇÕES DO AMBIENTE
        string InimigoMaisProximo(); // Retorna o ID (string), não o objeto
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
        void ColocarItemMochila(string item);
        void ColocarItemCinto(string item, int idx);

        // ATRIBUTOS DE OBJETOS (Ex: alvo.nome)
        string ObterNomeInimigo(string inimigoId);
        float ObterVelocidadeInimigo(string inimigoId);
        int ObterPosicaoXInimigo(string inimigoId);
        int ObterPosicaoYInimigo(string inimigoId);

        // OBS: Função legada, mantida para evitar conflitos temporários com o Godot.
        // O jogador não consegue mais usar nomeInimigo() na linguagem, ele usa alvo.nome.
        string GetNomeInimigo(string alvo); 
        void VenderTudo();
    }
}