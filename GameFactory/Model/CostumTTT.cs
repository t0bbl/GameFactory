namespace GameFactory.Model
{
    internal class CustomTTT : Match
    {
        private bool FirstTurn = true;
        private bool p_twist;

        public CustomTTT(bool p_twist)
            : base(p_twist ? 8 : AskForRows(),
                   p_twist ? 5 : AskForColumns(),
                   p_twist ? 4 : AskForWinningLength())
        {
            this.p_twist = p_twist;
        }
        public override void GameMechanic(List<Player> p_Players, bool p_gpt)
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
                        if (p_twist)
                        {
                            TwistColumn(col);
                        }
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
        public void TwistColumn(int chosenColumn)
        {
            Random random = new Random();
            bool TwistOrNot = random.Next(0, 2) == 0;
            if (TwistOrNot)
            {
                Console.WriteLine("Twist!");

                List<int> tempColumn = new List<int>();
                for (int i = 0; i < p_rows; i++)
                {
                    tempColumn.Add(GetCell(i, chosenColumn));
                }

                tempColumn.Reverse();

                for (int i = 0; i < p_rows; i++)
                {
                    SetCell(i, chosenColumn, tempColumn[i]);
                }
            }
        }


    }
}
