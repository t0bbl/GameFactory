using TicTacToeC;

namespace TicTacToeC.Model
{
    internal class FourW : Game
    {

        public FourW(Player[] players) : base(players)
        {
            winningLength = 4;
            rows = 6;
            columns = 7;
            board = new int[rows, columns];
        }
        public override (Player[] players, int draw) UpdateStats(Player[] players, int winnerNumber, int draw)
        {
            Player.UpdateFourW(players, winnerNumber, draw);
            return (players, draw);
        }
    }
}