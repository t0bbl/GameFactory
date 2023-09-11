
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
            return (players, draw);
        }
        public override void GameMechanic()
        {
            base.GameMechanic();            
            do
            {
                Console.WriteLine($"{players[currentPlayerIndex].Name}, input a number from 0 to {rows * columns - 1}");

                if (!int.TryParse(Console.ReadLine(), out int chosenCell) || chosenCell < 0 || chosenCell >= rows * columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int row = chosenCell / columns;
                int col = chosenCell % columns;

                if (GetCell(row, col) == 0)
                {
                    SetCell(row, col, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                    continue;
                }

                PrintBoard();
            }
            while (CheckWinner(winningLength) == 0);
        }
    }
}