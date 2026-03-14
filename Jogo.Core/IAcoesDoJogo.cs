namespace Jogo.Core
{
    // Este é o "contrato" ou o "controle remoto" que o seu Cérebro usa.
    // Ele define as ações que o jogo tem
    public interface IAcoesDoJogo
    {
        void Mover(string direcao);
        void Atacar(string alvo, string tipo);
    }
}