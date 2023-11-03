namespace ClassLibrary
{
    internal class CustomTTT : Match
    {
        #region Variables
        public bool p_Twist { get; set; }
        public bool p_TwistStat { get; set; }

        private Random p_random = new Random();
        #endregion

        public CustomTTT(bool p_Twist)
            : base(
                  p_Twist ? 8 : AskForRows(),
                  p_Twist ? 5 : AskForColumns(),
                  p_Twist ? 4 : AskForWinningLength()
              )
        {
            p_GameType = p_Twist ? "Twist" : "CustomTTT";
            this.p_Twist = p_Twist;
            ResetBoard();
        }
        public override void GameMechanic(List<Player> p_Player)
        {
            base.GameMechanic(p_Player);

            if (p_FirstTurn)
            {
                PrintBoard(true, true, p_Player);
                p_FirstTurn = false;
            }
            bool p_validInput = false;
            while (!p_validInput)
            {
                Console.WriteLine();
                Console.WriteLine($"{p_Player[p_CurrentPlayerIndex].Name}, input a coordinate X/Y");
                SavePlayerToMatch(p_Player[p_CurrentPlayerIndex].Ident, p_MatchId);
                string p_input = Console.ReadLine();
                string[] p_parts = p_input.Split('/');

                if (p_parts.Length == 2
                    && int.TryParse(p_parts[0], out int p_Row)
                    && int.TryParse(p_parts[1], out int p_Col)
                    && p_Row >= 1 && p_Row <= p_Rows
                    && p_Col >= 1 && p_Col <= p_Columns)
                {
                    p_Row--;
                    p_Col--;

                    if (GetCell(p_Row, p_Col) == '0')
                    {
                        SetCell(p_Row, p_Col, p_Player[p_CurrentPlayerIndex].Icon);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Player.Count;
                        p_validInput = true;
                        if (p_Twist)
                        {
                            p_TwistStat = TwistColumn(p_Col);
                        }
                        SaveMoveHistory(p_Player[p_CurrentPlayerIndex].Ident, p_input, p_MatchId, p_TwistStat);

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

            PrintBoard(true, true, p_Player);
        }

        #region CustomSetup
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
        public bool TwistColumn(int p_chosenColumn)
        {
            bool ShouldTwist = p_random.Next(0, 2) == 0;
            if (ShouldTwist)
            {
                Console.WriteLine("Twist!");

                List<char> TempColumn = new List<char>();
                for (int RowIndex = 0; RowIndex < p_Rows; RowIndex++)
                {
                    TempColumn.Add(GetCell(RowIndex, p_chosenColumn));
                }

                TempColumn.Reverse();

                for (int RowIndex = 0; RowIndex < p_Rows; RowIndex++)
                {
                    SetCell(RowIndex, p_chosenColumn, TempColumn[RowIndex]);
                }
                return true;
            }
            return false;
        }

        #endregion






    }
}
