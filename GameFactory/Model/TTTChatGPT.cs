using System.Text;

namespace GameFactory.Model
{
    internal class TTTChatGPT : Match
    {
        private int gameMechanicCallCount = 0;

        public TTTChatGPT() : base(3, 3, 3)
        {

        }
        public override void GameMechanic(List<Player> Players)
        {

            int chosenCell;
            bool validInput = false;
            gameMechanicCallCount++;

            if (gameMechanicCallCount % 2 == 0)
            {
                ChatGPTMove(BoardToString());
            }
            else
            {
                do 
                {
                    Console.WriteLine($"{Players[0].p_name}, input a number from 0 to {p_rows * p_columns - 1}");

                    if (TryGetValidInput(out chosenCell, p_rows * p_columns))
                    {
                        int row = chosenCell / p_columns;
                        int col = chosenCell % p_columns;

                        if (GetCell(row, col) == 0)
                        {
                            SetCell(row, col, p_currentPlayerIndex + 1);
                            p_currentPlayerIndex = (p_currentPlayerIndex + 1) % Players.Count;
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine("Cell already occupied. Try again.");
                        }
                    }
                    PrintBoard();
                }
                while (!validInput);
            }



        }
        public void ChatGPTMove(string board)
        {
            string apiKey = Environment.GetEnvironmentVariable("CHATGPT_API_KEY", EnvironmentVariableTarget.Machine);
            if (apiKey != null)
            {
                Console.WriteLine("ChatGPT is thinking...");
                var chatGPTClient = new ChatGPTClient(apiKey);
                string message = ($"Here is the current Tic-Tac-Toe board: {board}Your turn. Make a move by changing one empty '.' to 'O' and return the new board. Note: You cannot override cells already occupied by 'X' or 'O'.");
                string response = chatGPTClient.SendMessage(message);
                Console.WriteLine("ChatGPT´s Move: ");
                Console.WriteLine();
                Console.WriteLine($" {response}");
                StringToBoard(response);
            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }

        }
        public string BoardToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < p_rows; row++)
            {
                for (int col = 0; col < p_columns; col++)
                {
                    int cellValue = GetCell(row, col);
                    switch (cellValue)
                    {
                        case 0:
                            sb.Append(" . ");
                            break;
                        case 1:
                            sb.Append(" X ");
                            break;
                        case 2:
                            sb.Append(" O ");
                            break;
                    }
                }
                sb.AppendLine();  // Adds a new line at the end of each row
            }
            return sb.ToString();
        }

        public void StringToBoard(string boardString)
        {
            string[] rows = boardString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int row = 0; row < p_rows; row++)
            {
                string[] cells = rows[row].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < p_columns; col++)
                {
                    switch (cells[col])
                    {
                        case ".":
                            SetCell(row, col, 0);
                            break;
                        case "X":
                            SetCell(row, col, 1);
                            break;
                        case "O":
                            SetCell(row, col, 2);
                            break;

                    }
                }
            }
        }

    }
}
