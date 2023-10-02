namespace ClassLibrary
{
    internal class FourW : Match
    {
        #region Variables
        internal bool p_chatGPT { get; set; }
        #endregion
        public FourW() : base(6, 7, 4)
        {
            p_gameType = p_chatGPT ? "FourWChatGPT" : "FourW";
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

            int p_chosenColumn;
            if (p_firstTurn)
            {
                PrintBoard(false, true, p_player);
                Console.WriteLine();
                p_firstTurn = false;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{p_player[p_currentPlayerIndex].Name}, input a column number from 1 to {p_columns}");
                SavePlayerToMatch(p_player[p_currentPlayerIndex].Ident, p_matchId);
            } while (!TryGetValidInput(out p_chosenColumn, p_columns));

            MakeMove(p_chosenColumn, p_currentPlayerIndex, p_player);
            string p_cell = p_chosenColumn.ToString();
            SaveMoveHistory(p_player[p_currentPlayerIndex].Ident, p_cell, p_matchId, p_twistStat);

            p_currentPlayerIndex = (p_currentPlayerIndex + 1) % p_player.Count;

            PrintBoard(false, true, p_player);
        }

        #region GameUtilities
        public int FindLowestAvailableRow(int p_column)
        {
            for (int row = p_rows - 1; row >= 0; row--)
            {
                if (GetCell(row, p_column) == '0')
                {
                    return row;
                }
            }
            return -1;
        }
        public bool MakeMove(int p_chosenColumn, int p_currentPlayerIndex, List<Player> p_players)
        {
            int p_row = FindLowestAvailableRow(p_chosenColumn - 1);

            if (p_row != -1)
            {
                SetCell(p_row, p_chosenColumn - 1, p_players[p_currentPlayerIndex].Icon);
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
        public override string BuildMessage(string p_board, List<Player> p_players)
        {
            return $"Objective: Win the Connect 4 game by connecting four of your '{p_players[1].Icon}' vertically, horizontally, or diagonally.\n" +
                   $"The board is 7 columns by 6 rows.\n" +
                   $"Current board:\n{p_board}\n" +
                   $"Your turn:\n" +
                   $"- You are '{p_players[1].Icon}'.\n" +
                   $"- Drop your '{p_players[1].Icon}' into any of the columns. You cannot choose a column that is already full.\n" +
                   $"Choose a column (1-7) and return just this one number!:";
        }
        public override void ChatGPTMove(string p_board, List<Player> p_players)
        {
            ConsoleColor p_originalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            string apiKey = GetApiKey();
            if (apiKey != null)
            {
                int chosenColumn;
                Console.WriteLine();
                Console.WriteLine("ChatGPT is thinking...");

                string p_message = BuildMessage(p_board, p_players);
                string p_response = SendMessageToChatGPT(apiKey, p_message);
                chosenColumn = ValidateColumnChoice(p_response.Trim(), p_players);
                MakeMove(chosenColumn, 1, p_players);


                p_currentPlayerIndex = (p_currentPlayerIndex + 1) % p_players.Count;

                PrintBoard(false, true, p_players);

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }
            Console.ForegroundColor = p_originalForegroundColour;
        }
        public int ValidateColumnChoice(string p_response, List<Player> p_players)
        {
            if (int.TryParse(p_response, out int chosenColumn))
            {
                if (chosenColumn >= 1 && chosenColumn <= p_columns)
                {
                    return chosenColumn;
                }
            }

            return -1;
        }
        #endregion
    }
}