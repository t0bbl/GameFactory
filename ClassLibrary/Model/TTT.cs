namespace ClassLibrary
{
    internal class TTT : Match
    {
        #region Variables
        internal bool ChatGPT { get; set; }
        #endregion

        /// <summary>
        /// Constructor for the TTT (Tic Tac Toe) class, setting up a 3x3 game board with a winning length of 3.
        /// It also sets the game type based on whether it's being played with ChatGPT or not.
        /// </summary>
        public TTT() : base(3, 3, 3)
        {
            p_GameType = ChatGPT ? "TTTChatGPT" : "TTT";
        }
        /// <summary>
        /// Overrides the base GameMechanic method to implement the game logic for Tic Tac Toe (TTT). 
        /// If playing with ChatGPT, it invokes ChatGPTMove method for the ChatGPT's turn. 
        /// Prompts the current player to input a number to choose a cell on the board. 
        /// Validates the input and if the cell is unoccupied, sets the cell with the current player's icon, 
        /// updates the match and move history, then switches the turn to the next player. 
        /// Displays the board after each move.
        /// </summary>
        public override void GameMechanic(List<Player> p_player)
        {
            if (ChatGPT)
            {
                if (p_CurrentPlayerIndex == 1)
                {
                    ChatGPTMove(BoardToString(p_Board, p_player), p_player);
                }
            }
            base.GameMechanic(p_player);

            int p_chosenCell = 0;
            bool p_validInput = false;
            if (p_FirstTurn)
            {
                PrintBoard(false, false, p_player);
                Console.WriteLine();
                p_FirstTurn = false;
            }
            while (!p_validInput)
            {
                Console.WriteLine($"{p_player[p_CurrentPlayerIndex].Name}, input a number from 1 to {p_Rows * p_Columns}");

                if (TryGetValidInput(out p_chosenCell, p_Rows * p_Columns))
                {
                    int p_row = (p_chosenCell - 1) / p_Columns;
                    int p_col = (p_chosenCell - 1) % p_Columns;

                    if (GetCell(p_row, p_col) == '0')
                    {
                        SetCell(p_row, p_col, p_player[p_CurrentPlayerIndex].Icon);
                        SavePlayerToMatch(p_player[p_CurrentPlayerIndex].Ident, p_MatchId);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_player.Count;
                        p_validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
            }
            PrintBoard(false, false, p_player);
            string p_cell = p_chosenCell.ToString();
            SaveMoveHistory(p_player[p_CurrentPlayerIndex].Ident, p_cell, p_MatchId, p_TwistStat);
        }
        #region ChatGPT
        /// <summary>
        /// Builds and returns a message with instructions and the current game state for a ChatGPT turn in Tic Tac Toe (TTT).
        /// </summary>
        /// <param name="p_Board">The current game board as a string.</param>
        /// <param name="p_Players">A list of Player objects representing the players in the game.</param>
        /// <returns>A message containing game instructions, the current board, and information about the ChatGPT's turn.</returns>
        public override string BuildMessage(string p_Board, List<Player> p_Players)
        {
            return $"Objective: Win the Tic-Tac-Toe game.\n" +
                   $"Current board:\n{p_Board}\n" +
                   $"Your turn:\n" +
                   $"- You are '{p_Players[1].Icon}'.\n" +
                   $"- Change one empty '.' to '{p_Players[1].Icon}' and return the new board.\n" +
                   $"- You cannot override cells already occupied by '{p_Players[0].Icon}' or '{p_Players[1].Icon}'.\n" +
                   $"- You are only allowed to change 1 cell at a time.\n" +
                   $"- Dont Change the Icons of the players.\n" +
                   $"Make your move:";
        }
        /// <summary>
        /// Initiates ChatGPT's move in the Tic Tac Toe game.
        /// </summary>
        /// <param name="p_Board">The current game board as a string.</param>
        /// <param name="p_Players">A list of Player objects representing the players in the game.</param>
        public override void ChatGPTMove(string p_Board, List<Player> p_Players)
        {
            ConsoleColor p_originalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            string apiKey = GetApiKey();
            if (apiKey != null)
            {
                Console.WriteLine("ChatGPT is thinking...");

                string message = BuildMessage(p_Board, p_Players);
                string response = SendMessageToChatGPT(apiKey, message);
                StringToBoard(response, p_Players);

                Console.WriteLine("ChatGPT´s Move: ");
                PrintBoard(false, false, p_Players);


                p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }
            Console.ForegroundColor = p_originalForegroundColour;
        }
        /// <summary>
        /// Parses a string representation of the game board and updates the internal game board.
        /// </summary>
        /// <param name="p_BoardString">The string representation of the game board.</param>
        /// <param name="p_Players">A list of Player objects representing the players in the game.</param>
        public void StringToBoard(string p_BoardString, List<Player> p_Players)
        {
            string[] RowsString = p_BoardString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int Row = 0; Row < Math.Min(RowsString.Length, p_Rows); Row++)
            {
                string[] Cells = RowsString[Row].Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int Col = 0; Col < Math.Min(Cells.Length, p_Columns); Col++)
                {
                    if (Cells[Col] == ".")
                    {
                        SetCell(Row, Col, '0');
                    }
                    else if (Cells[Col] == p_Players[0].Icon.ToString())
                    {
                        SetCell(Row, Col, p_Players[0].Icon);
                    }
                    else if (Cells[Col] == p_Players[1].Icon.ToString())
                    {
                        SetCell(Row, Col, p_Players[1].Icon);
                    }   
                }
            }
        }
        #endregion

    }
}
