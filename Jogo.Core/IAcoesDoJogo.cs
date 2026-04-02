namespace Jogo.Core
{
    // Este é o "contrato" ou o "controle remoto" que o Cérebro usa.
    // Ele define as ações que o jogo tem
    public interface IAcoesDoJogo
    {
        void Mover(string direcao);
        void Atacar(object alvo, string tipo);
        void NotificarErro(string mensagem); // erro pro Godot
        object InimigoMaisProximo();
        string GetNomeInimigo(object alvo);
        bool PodeMover(string direcao);
        int GetTempo();
        int GetVidaAtual();
        void Escapar();
        List<object> EscanearArea();
        List<object> GetPosicaoPlayer();
        List<object> GetPosicaoTesouro();
        void UsarItemCinto(int indice);
        void UsarItemMochila();
        void Comprar(string item);
        void EntrarArena(string arena);
        void ColocarItemMochila(string item);
        void ColocarItemCinto(string item, int idx);
    }
}