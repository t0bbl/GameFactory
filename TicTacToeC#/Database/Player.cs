namespace TicTacToe
{
    public class Player
    {
        public string Name;
        public int Score { get; set; }
        public int WinsTTT { get; set; }
        public int LossesTTT { get; set; }
        public int DrawsTTT { get; set; }
        public int WinsFourWins { get; set; }
        public int LossesFourWins { get; set; }
        public int DrawsFourWins { get; set; }
        public int Number { get; set; }

        public Guid Id { get; private set; }

        public Player()
        {
            Id = Guid.NewGuid();
        }
    }
}