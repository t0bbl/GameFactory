using System.Text;

namespace GameFactory.Model
{
    internal class FourWChatGPT : FourW
    {
        public FourWChatGPT()
        {

        }
        public override void GameMechanic(List<Player> p_Players)
        {
            if (p_CurrentPlayerIndex == 1)
            {
                ChatGPTMove(BoardToString(p_Players), p_Players);
            }
            else
            {
                base.GameMechanic(p_Players);
            }
        }
        protected override string BuildMessage(string board, List<Player> p_Players)
        {
            return $"Objective: Win the Connect 4 game by connecting four of your '{p_Players[1].Icon}' vertically, horizontally, or diagonally.\n" +
                   $"The board is 7 columns by 6 rows.\n" +
                   $"Current board:\n{board}\n" +
                   $"Your turn:\n" +
                   $"- You are '{p_Players[1].Icon}'.\n" +
                   $"- Drop your '{p_Players[1].Icon}' into any of the columns. You cannot choose a column that is already full.\n" +
                   $"Choose a column (1-7) and return just this one number!:";
        }
        public void ChatGPTMove(string board, List<Player> p_Players)
        {
            ConsoleColor OriginalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            string apiKey = GetApiKey();
            if (apiKey != null)
            {
                int chosenColumn;
                Console.WriteLine();
                Console.WriteLine("ChatGPT is thinking...");

                string message = BuildMessage(board, p_Players);
                string response = SendMessageToChatGPT(apiKey, message);
                Console.WriteLine("ChatGPT´s Move: " + response);
                chosenColumn = ValidateColumnChoice(response.Trim(), p_Players);
                MakeMove(chosenColumn, 1, p_Players);


                p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;

                PrintBoard(false, true, p_Players);

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }
            Console.ForegroundColor = OriginalForegroundColour;
        }
        public int ValidateColumnChoice(string response, List<Player> p_Players)
        {
            // Attempt to parse the column choice as an integer
            if (int.TryParse(response, out int chosenColumn))
            {
                // Validate if it is within the board range
                if (chosenColumn >= 1 && chosenColumn <= p_Columns)
                {
                    // Further checks could be added here, like checking if the column is full
                    return chosenColumn;
                }
            }

            return -1;  // Invalid column choice
        }

    }
}
