namespace GameFactory.Model
{
    internal class FourWChatGPT : FourW
    {
        public FourWChatGPT()
        {
            p_gameType = "FourWChatGPT";
            p_rows = 6;
            p_columns = 7;
            p_winningLength = 4;

        }
        public override void GameMechanic(List<Player> p_players)
        {
            if (p_currentPlayerIndex == 1)
            {
                ChatGPTMove(BoardToString(p_board, p_players), p_players);
            }
            else
            {
                base.GameMechanic(p_players);
            }
        }
        protected override string BuildMessage(string p_board, List<Player> p_players)
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

    }
}
