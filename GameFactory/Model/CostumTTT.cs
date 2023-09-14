namespace GameFactory.Model
{
    internal class CustomTTT : Match
    {
        private bool FirstTurn = true;

        public CustomTTT() : base(AskForRows(), AskForColumns(), AskForWinningLength())
        { }
        public override void GameMechanic(List<Player> p_Players)
        {
            if (FirstTurn)
            {
                PrintBoard(true, true, p_Players);
                FirstTurn = false;
            }
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine();
                Console.WriteLine($"{p_Players[p_CurrentPlayerIndex].Name}, input a coordinate X/Y");

                string input = Console.ReadLine();
                string[] parts = input.Split('/');

                if (parts.Length == 2
                    && int.TryParse(parts[0], out int row)
                    && int.TryParse(parts[1], out int col)
                    && row >= 1 && row <= p_rows
                    && col >= 1 && col <= p_Columns)
                {
                    row--; 
                    col--; 

                    if (GetCell(row, col) == 0)
                    {
                        SetCell(row, col, p_CurrentPlayerIndex + 1);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;
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
            PrintBoard(true, true, p_Players);
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
            FirstTurn = true;
        }

    }
}
