using System.Collections.Generic;
using System.Text;

namespace GameFactory.Model
{
    internal class TTTChatGPT : TTT
    {
        private int gameMechanicCallCount = 0;
        public TTTChatGPT()
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
        public void ChatGPTMove(string board, List<Player> p_Players)
        {
            string apiKey = Environment.GetEnvironmentVariable("CHATGPT_API_KEY", EnvironmentVariableTarget.Machine);
            if (apiKey != null)
            {
                ConsoleColor OriginalForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ChatGPT is thinking...");
                var chatGPTClient = new ChatGPTClient(apiKey);
                string message = ($"Here is the current Tic-Tac-Toe board: {board}Your turn. Make a move by changing one empty '.' to '{p_Players[1].Icon}' and return the new board. Note: You cannot override cells already occupied by '{p_Players[0].Icon}' or '{p_Players[1].Icon}'.");
                string response = chatGPTClient.SendMessage(message);
                Console.WriteLine("ChatGPT´s Move: ");
                Console.WriteLine();
                int dotCount = StringToBoard(response, p_Players);
                if (dotCount != 8)
                {
                    PrintBoard(false, false, p_Players);
                }
                p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;
                Console.ForegroundColor = OriginalForegroundColor;

            }
            else
            {
                Console.WriteLine("Please set the environment variable CHATGPT_API_KEY to your ChatGPT API key.");
                Environment.Exit(0);
            }

        }
        public string BoardToString(List<Player> p_Players)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < p_rows; row++)
            {
                for (int col = 0; col < p_Columns; col++)
                {
                    int cellValue = GetCell(row, col);
                    switch (cellValue)
                    {
                        case 0:
                            sb.Append(" . ");
                            break;
                        case 1:
                            sb.Append($" {p_Players[0].Icon} ");
                            break;
                        case 2:
                            sb.Append($" {p_Players[1].Icon} ");
                            break;
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
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
