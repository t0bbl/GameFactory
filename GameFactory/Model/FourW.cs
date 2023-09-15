namespace GameFactory.Model
{
    internal class FourW : Match
    {
        private bool FirstTurn = true;
        private bool p_gpt;
        public FourW(bool p_gpt) : base(6, 7, 4)
        { }
        public override void GameMechanic(List<Player> p_Players, bool p_gpt)
        {
            if (p_gpt)
            {
                ChatGPTMove(BoardToString(p_Players), p_Players);
            }
            int chosenColumn;
            if (FirstTurn)
            {
                PrintBoard(false, true, p_Players);
                Console.WriteLine();
                FirstTurn = false;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{p_Players[p_CurrentPlayerIndex].Name}, input a column number from 1 to {p_Columns}");
            } while (!TryGetValidInput(out chosenColumn, p_Columns));

            MakeMove(chosenColumn, p_CurrentPlayerIndex, p_Players);
            p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;

            PrintBoard(false, true, p_Players);
        }
        public int FindLowestAvailableRow(int column)
        {
            for (int row = p_rows - 1; row >= 0; row--)
            {
                if (GetCell(row, column) == 0)
                {
                    return row;
                }
            }
            return -1;
        }
        public override void ResetFirstTurn()
        {
            FirstTurn = true;
        }

        public bool MakeMove(int chosenColumn, int p_CurrentPlayerIndex, List<Player> p_Players)
        {
            int row = FindLowestAvailableRow(chosenColumn - 1);

            if (row != -1)
            {
                SetCell(row, chosenColumn - 1, p_CurrentPlayerIndex + 1);
                return true;
            }
            else
            {
                Console.WriteLine("Column is full. Try again.");
                return false;
            }
        }

        public override void ChatGPTMove(string board, List<Player> p_Players)
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
            if (int.TryParse(response, out int chosenColumn))
            {
                if (chosenColumn >= 1 && chosenColumn <= p_Columns)
                {
                    return chosenColumn;
                }
            }

            return -1;
        }
    }

}