
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
        public override void GameMechanic(int currentPlayerIndex)
        {
            do
            {
                Console.WriteLine($"{players[currentPlayerIndex].Name}, input a column number from 0 to {columns - 1}");

                if (!int.TryParse(Console.ReadLine(), out int chosenColumn) || chosenColumn < 0 || chosenColumn >= columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int row = FindLowestAvailableRow(chosenColumn);

                if (row != -1)
                {
                    SetCell(row, chosenColumn, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                }
                else
                {
                    Console.WriteLine("Column is full. Try again.");
                    continue;
                }

                PrintBoard();
            }
            while (CheckWinner(winningLength) == 0);
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