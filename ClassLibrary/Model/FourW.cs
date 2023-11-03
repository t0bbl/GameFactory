namespace ClassLibrary
{
    internal class FourW : Match
    {
        #region Variables
        internal bool ChatGPT { get; set; }
        #endregion
        /// <summary>
        /// Initializes a new instance of the FourW class with specified board dimensions and winning length.
        /// </summary>
        public FourW() : base(6, 7, 4)
        {
            p_GameType = ChatGPT ? "FourWChatGPT" : "FourW";
        }
        /// <summary>
        /// Handles the game mechanics for FourW.
        /// </summary>
        /// <param name="p_Player">The list of players.</param>
        public override void GameMechanic(List<Player> p_Player)
        {
            if (ChatGPT)
            {
                if (p_CurrentPlayerIndex == 1)
                {
                    ChatGPTMove(BoardToString(p_Board, p_Player), p_Player);
                }

            }

            base.GameMechanic(p_Player);

            int p_chosenColumn;
            if (p_FirstTurn)
            {
                PrintBoard(false, true, p_Player);
                Console.WriteLine();
                p_FirstTurn = false;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{p_Player[p_CurrentPlayerIndex].Name}, input a column number from 1 to {p_Columns}");
                SavePlayerToMatch(p_Player[p_CurrentPlayerIndex].Ident, p_MatchId);
            } while (!TryGetValidInput(out p_chosenColumn, p_Columns));

            MakeMove(p_chosenColumn, p_CurrentPlayerIndex, p_Player);
            string p_cell = p_chosenColumn.ToString();
            SaveMoveHistory(p_Player[p_CurrentPlayerIndex].Ident, p_cell, p_MatchId, p_TwistStat);

            p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Player.Count;

            PrintBoard(false, true, p_Player);
        }

        #region GameUtilities
        /// <summary>
        /// Finds the lowest available row in a specified column.
        /// </summary>
        /// <param name="p_Column">The column to check.</param>
        /// <returns>The row index of the lowest available row, or -1 if the column is full.</returns>
        public int FindLowestAvailableRow(int p_Column)
        {
            for (int Row = p_Rows - 1; Row >= 0; Row--)
            {
                if (GetCell(Row, p_Column) == '0')
                {
                    return Row;
                }
            }
            return -1;
        }
        /// <summary>
        /// Makes a move for the current player in the specified column.
        /// </summary>
        /// <param name="p_ChosenColumn">The column in which the move is made (1-based index).</param>
        /// <param name="p_CurrentPlayerIndex">The index of the current player in the player list.</param>
        /// <param name="p_Players">The list of players.</param>
        /// <returns>True if the move is successfully made, false if the column is full.</returns>
        public bool MakeMove(int p_ChosenColumn, int p_CurrentPlayerIndex, List<Player> p_Players)
        {
            int p_row = FindLowestAvailableRow(p_ChosenColumn - 1);

            if (p_row != -1)
            {
                SetCell(p_row, p_ChosenColumn - 1, p_Players[p_CurrentPlayerIndex].Icon);
                return true;
            }
            else
            {
                Console.WriteLine("Column is full. Try again.");
                return false;
            }
        }
        #endregion
        #region ChatGPT
        /// <summary>
        /// Builds a message for the Connect 4 game.
        /// </summary>
        /// <param name="p_Board">The current game board as a string.</param>
        /// <param name="p_Players">The list of players.</param>
        /// <returns>The game instructions and current board as a formatted string.</returns>
        public override string BuildMessage(string p_Board, List<Player> p_Players)
        {
            return $"Objective: Win the Connect 4 game by connecting four of your '{p_Players[1].Icon}' vertically, horizontally, or diagonally.\n" +
                   $"The board is 7 columns by 6 rows.\n" +
                   $"Current board:\n{p_Board}\n" +
                   $"Your turn:\n" +
                   $"- You are '{p_Players[1].Icon}'.\n" +
                   $"- Drop your '{p_Players[1].Icon}' into any of the columns. You cannot choose a column that is already full.\n" +
                   $"Choose a column (1-7) and return just this one number!:";
        }
        /// <summary>
        /// Executes ChatGPT's move in the Connect 4 game.
        /// </summary>
        /// <param name="p_Board">The current game board as a string.</param>
        /// <param name="p_Players">The list of players.</param>
        public override void ChatGPTMove(string p_Board, List<Player> p_Players)
        {
            ConsoleColor p_originalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            string apiKey = GetApiKey();
            if (apiKey != null)
            {
                int chosenColumn;
                Console.WriteLine();
                Console.WriteLine("ChatGPT is thinking...");

                string p_message = BuildMessage(p_Board, p_Players);
                string p_response = SendMessageToChatGPT(apiKey, p_message);
                chosenColumn = ValidateColumnChoice(p_response.Trim(), p_Players);
                MakeMove(chosenColumn, 1, p_Players);


                p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;

                PrintBoard(false, true, p_Players);

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }
            Console.ForegroundColor = p_originalForegroundColour;
        }
        /// <summary>
        /// Validates the column choice obtained from ChatGPT's response.
        /// </summary>
        /// <param name="p_Response">The column choice as a string.</param>
        /// <param name="p_Players">The list of players.</param>
        /// <returns>The validated column choice (1 to p_Columns) or -1 if invalid.</returns>
        public int ValidateColumnChoice(string p_Response, List<Player> p_Players)
        {
            if (int.TryParse(p_Response, out int chosenColumn))
            {
                if (chosenColumn >= 1 && chosenColumn <= p_Columns)
                {
                    return chosenColumn;
                }
            }

            return -1;
        }
        #endregion
    }
}