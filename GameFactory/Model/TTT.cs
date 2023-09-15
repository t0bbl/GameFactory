namespace GameFactory.Model
{
    internal class TTT : Match
    {
        private bool FirstTurn = true;
        private bool p_gpt;
        public TTT(bool p_gpt) : base(3, 3, 3)
        {
            this.p_gpt = true;
        }
        public override void GameMechanic(List<Player> p_players, bool p_gpt)
        {
            Console.WriteLine($"Debug: this.p_gpt = {this.p_gpt}");
Console.WriteLine($"Debug: p_CurrentPlayerIndex = {p_CurrentPlayerIndex}");

            if (this.p_gpt && p_CurrentPlayerIndex == 1)
            {
                ChatGPTMove(BoardToString(p_players), p_players);
            }
            int chosenCell;
            bool validInput = false;
            if (FirstTurn)
            {
                PrintBoard(false, false, p_players);
                Console.WriteLine();
                FirstTurn = false;
            }
            while (!validInput)
            {
                Console.WriteLine($"{p_players[p_CurrentPlayerIndex].Name}, input a number from 1 to {p_rows * p_Columns}");

                if (TryGetValidInput(out chosenCell, p_rows * p_Columns))
                {
                    int row = (chosenCell - 1) / p_Columns;
                    int col = (chosenCell - 1) % p_Columns;

                    if (GetCell(row, col) == 0)
                    {
                        SetCell(row, col, p_CurrentPlayerIndex + 1);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_players.Count;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
            }
            PrintBoard(false, false, p_players);
        }
        public override void ResetFirstTurn()
        {
            FirstTurn = true;
        }
        public override void ChatGPTMove(string board, List<Player> p_Players)
        {
            ConsoleColor OriginalForegroundColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            string apiKey = GetApiKey();
            if (apiKey != null)
            {
                Console.WriteLine("ChatGPT is thinking...");

                string message = BuildMessage(board, p_Players);
                string response = SendMessageToChatGPT(apiKey, message);

                Console.WriteLine("ChatGPT´s Move: ");
                Console.WriteLine();
                int dotCount = StringToBoard(response, p_Players);
                if (dotCount != 8)
                {
                    PrintBoard(false, false, p_Players);
                }

                p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }
            Console.ForegroundColor = OriginalForegroundColour;
        }
        public int StringToBoard(string boardString, List<Player> p_Players)
        {
            int dotCount = 0;
            string[] p_rows = boardString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int row = 0; row < p_rows.Length; row++)
            {
                string[] cells = p_rows[row].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < p_Columns; col++)
                {
                    if (cells[col] == ".")
                    {
                        SetCell(row, col, 0);
                        dotCount++;
                    }
                    else if (cells[col] == p_Players[0].Icon)
                    {
                        SetCell(row, col, 1);
                    }
                    else if (cells[col] == p_Players[1].Icon)
                    {
                        SetCell(row, col, 2);
                    }
                }
            }
            return dotCount;
        }


    }
}
