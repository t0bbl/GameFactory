namespace TicTacToe
{
    public class Player
    {
        public string Name;
        public int Score;
        public int WinsTTT;
        public int LossesTTT;
        public int DrawsTTT;
        public int WinsFourWins;
        public int LossesFourWins;
        public int DrawsFourWins;
        public int Number;

        public Guid Id { get; private set; }

        public Player()
        {
            Id = Guid.NewGuid();
        }
    }
}