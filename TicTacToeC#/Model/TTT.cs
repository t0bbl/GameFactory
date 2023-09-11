
namespace TicTacToeC.Model
{
    internal class TTT : Game
    {

        public TTT() 
        {
            winningLength = 3;
            rows = 3;
            columns = 3;
            board = new int[rows, columns];
        }

        public override void GameMechanic(List<Player> Players)
        {
            do
            {
                Console.WriteLine($"{Players[0].Name}, input a number from 0 to {rows * columns - 1}");

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
                    currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                    continue;
                }

                PrintBoard();
            }
            while (CheckWinner() == 0);
        }
    }
}