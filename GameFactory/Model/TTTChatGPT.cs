namespace GameFactory.Model
{
    internal class TTTChatGPT : TTT
    {
        public TTTChatGPT()
        {

        }
        public override void GameMechanic(List<Player> p_players)
        {
            if (p_CurrentPlayerIndex == 1)
            {
                ChatGPTMove(BoardToString(p_board, p_players), p_players);
            }
            else
            {
                base.GameMechanic(p_players);
            }
        }
        protected override string BuildMessage(string board, List<Player> p_players)
        {
            return $"Objective: Win the Tic-Tac-Toe game.\n" +
                   $"Current board:\n{board}\n" +
                   $"Your turn:\n" +
                   $"- You are '{p_players[1].Icon}'.\n" +
                   $"- Change one empty '.' to '{p_players[1].Icon}' and return the new board.\n" +
                   $"- You cannot override cells already occupied by '{p_players[0].Icon}' or '{p_players[1].Icon}'.\n" +
                   $"- You are only allowed to change 1 cell at a time.\n" +
                   $"- Dont Change the Icons of the players.\n" +
                   $"Make your move:";
        }
        public override void ChatGPTMove(string board, List<Player> p_players)
        {
            ConsoleColor OriginalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("this is the board", board);
            string apiKey = GetApiKey();
            if (apiKey != null)
            {
                Console.WriteLine("ChatGPT is thinking...");

                string message = BuildMessage(board, p_players);
                string response = SendMessageToChatGPT(apiKey, message);
                Console.WriteLine($"Response from API: {response}");
                StringToBoard(response, p_players);

                Console.WriteLine("ChatGPT´s Move: ");
                    PrintBoard(false, false, p_players);


                p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_players.Count;

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }
            Console.ForegroundColor = OriginalForegroundColour;
        }
        public void StringToBoard(string boardString, List<Player> p_players)
        {
            string[] p_rowsString = boardString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int row = 0; row < Math.Min(p_rowsString.Length, p_rows); row++)
            {
                string[] cells = p_rowsString[row].Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int col = 0; col < Math.Min(cells.Length, p_Columns); col++)
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



    }
}
