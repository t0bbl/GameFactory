namespace GameFactory.Model
{
    internal class FourW : Match
    {

        public FourW()
        {
            p_winningLength = 4;
            p_rows = 6;
            p_columns = 7;
            p_board = new int[p_rows, p_columns];
        }


        public override void GameMechanic(List<Player> Players)
        {

            Console.WriteLine($"{Players[1]}, input a column number from 0 to {p_columns - 1}");

            if (!int.TryParse(Console.ReadLine(), out int chosenColumn) || chosenColumn < 0 || chosenColumn >= p_columns)
            {
                Console.WriteLine("Invalid number. Try again.");
            }

            int row = FindLowestAvailableRow(chosenColumn);

            if (row != -1)
            {
                SetCell(row, chosenColumn, p_currentPlayerIndex + 1);
                p_currentPlayerIndex = (p_currentPlayerIndex + 1) % Players.Count;
            }
            else
            {
                Console.WriteLine("Column is full. Try again.");
            }

            PrintBoard();

        }
        public int FindLowestAvailableRow(int column)
        {
            for (int row = p_rows - 1; row >= 0; row--)
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