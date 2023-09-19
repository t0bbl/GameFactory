namespace GameFactory.Model
{
    internal class CustomTTT : Match
    {
        public bool p_twist;
        private Random p_Random = new Random();


        public CustomTTT(bool p_twist)
        {
            this.p_twist = p_twist;
            if (p_twist)
            {
                p_rows = 8;
                p_Columns = 5;
                p_WinningLength = 4;
            }
            else { 
                p_rows = AskForRows();
                p_Columns = AskForColumns();
                p_WinningLength = AskForWinningLength();
            }
            p_board = new char[p_rows, p_Columns];
            ResetBoard();

        }
        public override void GameMechanic(List<Player> p_player)
        {
            if (p_firstTurn)
            {
                PrintBoard(true, true, p_player);
                p_firstTurn = false;
            }
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine();
                Console.WriteLine($"{p_player[p_CurrentPlayerIndex].Name}, input a coordinate X/Y");

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

                    if (GetCell(row, col) == '0')
                    {
                        SetCell(row, col, p_player[p_CurrentPlayerIndex].Icon);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_player.Count;
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

            PrintBoard(true, true, p_player);
        }

        #region CostumSetup
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
        #endregion


        public void TwistColumn(int p_chosenColumn)
        {
            bool shouldTwist = p_Random.Next(0, 2) == 0;
            if (shouldTwist)
            {
                Console.WriteLine("Twist!");

                List<char> p_tempColumn = new List<char>();
                for (int p_rowIndex = 0; p_rowIndex < p_rows; p_rowIndex++)
                {
                    p_tempColumn.Add(GetCell(p_rowIndex, p_chosenColumn));
                }

                p_tempColumn.Reverse();

                for (int p_rowIndex = 0; p_rowIndex < p_rows; p_rowIndex++)
                {
                    SetCell(p_rowIndex, p_chosenColumn, p_tempColumn[p_rowIndex]);
                }
            }
        }




    }
}
