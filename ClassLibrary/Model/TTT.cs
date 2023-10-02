namespace ClassLibrary
{
    internal class TTT : Match
    {
        #region Variables
        internal bool p_chatGPT { get; set; }
        #endregion

        public TTT() : base(3, 3, 3)
        {
            p_gameType = p_chatGPT ? "TTTChatGPT" : "TTT";
        }
        public override void GameMechanic(List<Player> p_player)
        {
            if (p_chatGPT)
            {
                if (p_currentPlayerIndex == 1)
                {
                    ChatGPTMove(BoardToString(p_board, p_player), p_player);
                }
            }
            base.GameMechanic(p_player);

            int p_chosenCell = 0;
            bool p_validInput = false;
            if (p_firstTurn)
            {
                PrintBoard(false, false, p_player);
                Console.WriteLine();
                p_firstTurn = false;
            }
            while (!p_validInput)
            {
                Console.WriteLine($"{p_player[p_currentPlayerIndex].Name}, input a number from 1 to {p_rows * p_columns}");

                if (TryGetValidInput(out p_chosenCell, p_rows * p_columns))
                {
                    int p_row = (p_chosenCell - 1) / p_columns;
                    int p_col = (p_chosenCell - 1) % p_columns;

                    if (GetCell(p_row, p_col) == '0')
                    {
                        SetCell(p_row, p_col, p_player[p_currentPlayerIndex].Icon);
                        SavePlayerToMatch(p_player[p_currentPlayerIndex].Ident, p_matchId);
                        p_currentPlayerIndex = (p_currentPlayerIndex + 1) % p_player.Count;
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
            SaveMoveHistory(p_player[p_currentPlayerIndex].Ident, p_cell, p_matchId, p_twistStat);
        }
        #region ChatGPT
        protected override string BuildMessage(string p_board, List<Player> p_players)
        {
            return $"Objective: Win the Tic-Tac-Toe game.\n" +
                   $"Current board:\n{p_board}\n" +
                   $"Your turn:\n" +
                   $"- You are '{p_players[1].Icon}'.\n" +
                   $"- Change one empty '.' to '{p_players[1].Icon}' and return the new board.\n" +
                   $"- You cannot override cells already occupied by '{p_players[0].Icon}' or '{p_players[1].Icon}'.\n" +
                   $"- You are only allowed to change 1 cell at a time.\n" +
                   $"- Dont Change the Icons of the players.\n" +
                   $"Make your move:";
        }
        public override void ChatGPTMove(string p_board, List<Player> p_players)
        {
            ConsoleColor p_originalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            string apiKey = GetApiKey();
            if (apiKey != null)
            {
                Console.WriteLine("ChatGPT is thinking...");

                string message = BuildMessage(p_board, p_players);
                string response = SendMessageToChatGPT(apiKey, message);
                StringToBoard(response, p_players);

                Console.WriteLine("ChatGPT´s Move: ");
                PrintBoard(false, false, p_players);


                p_currentPlayerIndex = (p_currentPlayerIndex + 1) % p_players.Count;

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }
            Console.ForegroundColor = p_originalForegroundColour;
        }
        public void StringToBoard(string p_boardString, List<Player> p_players)
        {
            string[] p_rowsString = p_boardString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int row = 0; row < Math.Min(p_rowsString.Length, p_rows); row++)
            {
                string[] cells = p_rowsString[row].Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int col = 0; col < Math.Min(cells.Length, p_columns); col++)
                {
                    if (cells[col] == ".")
                    {
                        SetCell(row, col, '0');
                    }
                    else if (cells[col] == p_players[0].Icon.ToString())
                    {
                        SetCell(row, col, p_players[0].Icon);
                    }
                    else if (cells[col] == p_players[1].Icon.ToString())
                    {
                        SetCell(row, col, p_players[1].Icon);
                    }
                }
            }
        }
        #endregion

    }
}
