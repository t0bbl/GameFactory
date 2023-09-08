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
    }
}