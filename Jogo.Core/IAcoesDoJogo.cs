namespace Jogo.Core
{
    // Este é o "controle remoto" que o Cérebro usa.
    // Ele define as ações que o jogo tem
    public interface IAcoesDoJogo
    {
        void Mover(string direcao);
        
        // Usar string (ID do alvo) em vez de object facilita para o jogador
        void Atacar(string alvo, string tipo); 
        
        void NotificarErro(string mensagem); 
        
        // Retorna o NOME (string) do inimigo mais próximo, não o objeto Godot
        string InimigoMaisProximo(); 
        
        // Se InimigoMaisProximo retorna o nome, esta função pode não ser mais necessária,
        // mas vamos mantê-la como utilitário caso queira obter detalhes depois.
        string GetNomeInimigo(string alvo); 
        
        bool PodeMover(string direcao);
        int GetTempo();
        int GetVidaAtual();
        void Escapar();
        
        // Pode retornar uma lista de strings (nomes) dos objetos próximos
        List<string> EscanearArea(); 
        
        // --- A Sua Ideia de Refatoração de Coordenadas ---
        int GetPosicaoPlayerX();
        int GetPosicaoPlayerY();
        int GetPosicaoTesouroX();
        int GetPosicaoTesouroY();
        // ---------------------------------------------------
        
        void UsarItemCinto(int indice);
        void UsarItemMochila();
        void Comprar(string item);
        void EntrarArena(string arena);
        void ColocarItemMochila(string item);
        void ColocarItemCinto(string item, int idx);
    }
}