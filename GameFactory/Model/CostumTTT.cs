namespace GameFactory.Model
{
    internal class CostumTTT : Match
    {
        public CostumTTT() : base(AskForRows(), AskForColumns(), AskForWinningLength())
        { }
        public override void GameMechanic(List<Player> Players)
        {
            int chosenCell;
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine($"{Players[p_currentPlayerIndex].p_name}, input a number from 1 to {p_rows * p_columns}");

                if (TryGetValidInput(out chosenCell, p_rows * p_columns))
                {
                    int row = (chosenCell - 1) / p_columns;
                    int col = (chosenCell - 1) % p_columns;

                    if (GetCell(row, col) == 0)
                    {
                        SetCell(row, col, p_currentPlayerIndex + 1);
                        p_currentPlayerIndex = (p_currentPlayerIndex + 1) % Players.Count;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
            }
            PrintBoard(true, true);
        }
        private static int AskForRows()
        {
            Console.Write("Enter the number of rows: ");
            int rows;
            if (int.TryParse(Console.ReadLine(), out rows))
            {
                return rows;
            }
            Console.WriteLine("Invalid input for rows. Using default value of 3.");
            return 3;
        }

        private static int AskForColumns()
        {
            Console.Write("Enter the number of columns: ");
            int columns;
            if (int.TryParse(Console.ReadLine(), out columns))
            {
                return columns;
            }
            Console.WriteLine("Invalid input for columns. Using default value of 3.");
            return 3;
        }

        private static int AskForWinningLength()
        {
            Console.Write("Enter the winning length: ");
            int winningLength;
            if (int.TryParse(Console.ReadLine(), out winningLength))
            {
                return winningLength;
            }
            Console.WriteLine("Invalid input for winning length. Using default value of 3.");
            return 3;
        }
    }
}
