namespace GameFactory.Model
{
    internal class CostumTTT : Match
    {
        private bool firstTurn = true;

        public CostumTTT() : base(AskForRows(), AskForColumns(), AskForWinningLength())
        { }
        public override void GameMechanic(List<Player> Players)
        {
            if (firstTurn)
            {
                PrintBoard(true, true);
                firstTurn = false;
            }
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine();
                Console.WriteLine($"{Players[p_currentPlayerIndex].p_name}, input a coordinate X/Y");

                string input = Console.ReadLine();
                string[] parts = input.Split('/');

                if (parts.Length == 2
                    && int.TryParse(parts[0], out int row)
                    && int.TryParse(parts[1], out int col)
                    && row >= 1 && row <= p_rows
                    && col >= 1 && col <= p_columns)
                {
                    row--; // Adjust for 0-based index
                    col--; // Adjust for 0-based index

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
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
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
        public override void ResetFirstTurn()
        {
            firstTurn = true;
        }

    }
}
