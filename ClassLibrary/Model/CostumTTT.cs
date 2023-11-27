namespace ClassLibrary
{
    public class CustomTTT : Match
    {
        #region Variables
        public bool p_Twist { get; set; }
        public bool p_TwistStat { get; set; }

        private Random p_random = new Random();
        #endregion

        /// <summary>
        /// Initializes a new instance of the CustomTTT class, allowing for a twist mode or a custom-defined game setup. The twist mode uses predefined board dimensions, whereas the custom setup prompts the user for dimensions. Game type is set based on the selected mode and the board is reset accordingly.
        /// </summary>
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

        /// <summary>
        /// Implements the primary game mechanics for the current game session. It prompts players for their move, validates the input, and updates the game board. Players take turns inputting coordinates for their move. If the twist mode is active, specific game dynamics are applied after a valid move. Player moves and game states are saved for record-keeping.
        /// </summary>
        public override void GameMechanic(List<Player> p_Player)
        {
            base.GameMechanic(p_Player);

            if (FirstTurn)
            {
                FirstTurn = false;
            }
            bool p_validInput = false;
            while (!p_validInput)
            {

                SavePlayerToMatch(p_Player[CurrentPlayerIndex].Ident, MatchId);
                string p_input = Console.ReadLine();
                string[] p_parts = p_input.Split('/');

                if (p_parts.Length == 2
                    && int.TryParse(p_parts[0], out int p_Row)
                    && int.TryParse(p_parts[1], out int p_Col)
                    && p_Row >= 1 && p_Row <= Rows
                    && p_Col >= 1 && p_Col <= Columns)
                {
                    p_Row--;
                    p_Col--;

                    if (GetCell(p_Row, p_Col) == '0')
                    {
                        SetCell(p_Row, p_Col, p_Player[CurrentPlayerIndex].Icon);
                        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % p_Player.Count;
                        p_validInput = true;
                        if (p_Twist)
                        {
                            TwistStat = TwistColumn(p_Col);
                        }
                        SaveMoveHistory(p_Player[CurrentPlayerIndex].Ident, p_input, MatchId, TwistStat);

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

        }

        #region CustomSetup
        /// <summary>
        /// Prompts the user to specify the number of rows for the game board. If the input is valid, it returns the specified number; otherwise, it defaults to 3 rows.
        /// </summary>
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
        /// <summary>
        /// Prompts the user to specify the number of columns for the game board. If the input is valid, it returns the specified number; otherwise, it defaults to 3 columns.
        /// </summary>
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
        /// <summary>
        /// Prompts the user to specify the required consecutive markers for a win. If the input is valid, it returns the specified number; otherwise, it defaults to a winning length of 3.
        /// </summary>
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
        /// <summary>
        /// Introduces a twist mechanism to the game by potentially reversing the contents of the chosen column. The decision to twist is made randomly. If a twist occurs, the function flips the contents of the selected column and returns true. Otherwise, it returns false.
        /// </summary>
        public bool TwistColumn(int p_chosenColumn)
        {
            bool ShouldTwist = p_random.Next(0, 2) == 0;
            if (ShouldTwist)
            {
                Console.WriteLine("Twist!");

                List<char> TempColumn = new List<char>();
                for (int RowIndex = 0; RowIndex < Rows; RowIndex++)
                {
                    TempColumn.Add(GetCell(RowIndex, p_chosenColumn));
                }

                TempColumn.Reverse();

                for (int RowIndex = 0; RowIndex < Rows; RowIndex++)
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
