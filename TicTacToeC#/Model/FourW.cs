
namespace TicTacToeC.Model
{
    internal class FourW : Game
    {

        public FourW()
        {
            winningLength = 4;
            rows = 6;
            columns = 7;
            board = new int[rows, columns];
        }
    
    
        public override void GameMechanic(List<Player> Players)
        {

            Console.WriteLine($"{Players[1]}, input a column number from 0 to {columns - 1}");

                if (!int.TryParse(Console.ReadLine(), out int chosenColumn) || chosenColumn < 0 || chosenColumn >= columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                }

                int row = FindLowestAvailableRow(chosenColumn);

                if (row != -1)
                {
                    SetCell(row, chosenColumn, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
                }
                else
                {
                    Console.WriteLine("Column is full. Try again.");
                }

                PrintBoard();

        }
        public int FindLowestAvailableRow(int column)
        {
            for (int row = rows - 1; row >= 0; row--)
            {
                if (GetCell(row, column) == 0)
                {
                    return row;
                }
            }
            return -1;
        }

    }

}