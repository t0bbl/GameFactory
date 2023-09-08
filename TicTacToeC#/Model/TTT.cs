using TicTacToeC;

namespace TicTacToeC.Model
{
    internal class TTT : Game
    {

        public TTT(Player[] players) : base(players)
        {
            winningLength = 3;
            rows = 3;
            columns = 3;
            board = new int[rows, columns];
        }
        public override (Player[] players, int draw) UpdateStats(Player[] players, int winnerNumber, int draw)
        {
            Player.UpdateTTT(players, winnerNumber, draw);
            Console.WriteLine($"From inside updatestats in override, {players}, {draw}");
            return (players, draw);
        }
    }
}