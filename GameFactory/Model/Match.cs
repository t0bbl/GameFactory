using System.Text;

namespace GameFactory
{
    internal class Match
    {
        public Match(int rows, int columns, int winningLength)
        {
            p_rows = rows;
            p_Columns = columns;
            p_WinningLength = winningLength;
            p_Board = new int[rows, columns];
        }
        public int[,] p_Board;
        public int p_rows { get; set; }
        public int p_Columns { get; set; }
        public int p_WinningLength { get; set; }
        public int p_CurrentPlayerIndex { get; set; }
        readonly Random p_Random = new();

        public List<Player> StartMatch(List<Player> p_Players)
        {
            ResetFirstTurn();
            if (p_Players.All(player => player.IsHuman))
            { ShufflePlayers(p_Players); }
            do
            {
                GameMechanic(p_Players);
            } while (CheckWinner() == 0);
            int p_winnerNumber = CheckWinner();
            if (p_winnerNumber != 0)
            {
                (p_Players) = Player.UpdateStats(p_Players, p_winnerNumber);
            }


            ReMatch(p_Players);

            ResetBoard();
            return (p_Players);
        }

        public int CheckWinner()
        {
            for (int p_row = 0; p_row < p_rows; p_row++)
            {
                for (int col = 0; col < p_Columns; col++)
                {
                    int cellValue = GetCell(p_row, col);
                    if (cellValue == 0) continue;

                    int[][] directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var dir in directions)
                    {
                        int count = 1;
                        for (int playerRow = 1; playerRow < p_WinningLength; playerRow++)
                        {
                            int newRow = p_row + dir[0] * playerRow;
                            int newCol = col + dir[1] * playerRow;
                            if (newRow < 0 || newRow >= p_rows || newCol < 0 || newCol >= p_Columns) break;
                            if (GetCell(newRow, newCol) == cellValue) count++;
                            else break;
                        }
                        if (count >= p_WinningLength) return cellValue;
                    }
                }
            }

            bool isDraw = !Enumerable.Range(0, p_rows).Any(p_row => Enumerable.Range(0, p_Columns).Any(col => GetCell(p_row, col) == 0));
            return isDraw ? -1 : 0;
        }
        public void ResetBoard()
        {
            for (int playedRow = 0; playedRow < p_rows; playedRow++)
                for (int playedColumn = 0; playedColumn < p_Columns; playedColumn++)
                    p_Board[playedRow, playedColumn] = 0;
        }
        public int GetCell(int p_row, int col)
        {
            return p_Board[p_row, col];
        }
        public void SetCell(int p_row, int col, int value)
        {
            if (p_row >= 0 && p_row < p_rows && col >= 0 && col < p_Columns)
                p_Board[p_row, col] = value;
        }
        public void PrintBoard(bool p_showRow, bool p_showCol, List<Player> p_Players)
        {
            for (int p_row = 0; p_row < p_rows; p_row++)
            {
                if (p_showRow)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write(p_row + 1 + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.Write("  ");
                }


                for (int col = 0; col < p_Columns; col++)
                {
                    int cellValue = GetCell(p_row, col);
                    if (cellValue == 0)
                    {
                        Console.Write(" . ");
                    }
                    else
                    {
                        for (int p_player = 0; p_player < p_Players.Count; p_player++)
                        {
                            if (cellValue == p_player + 1)
                            {
                                ConsoleColor OriginalForegroundColor = Console.ForegroundColor;
                                if (Enum.TryParse(p_Players[p_player].Colour, out ConsoleColor parsedColor))
                                {
                                    Console.ForegroundColor = parsedColor;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                Console.Write($" {p_Players[p_player].Icon} ");
                                Console.ForegroundColor = OriginalForegroundColor;
                                break;
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
            if (p_showCol)
            {
                Console.Write("  ");

                for (int col = 0; col < p_Columns; col++)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write($" {col + 1} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }

        }
        public void ReMatch(List<Player> Players)
        {
            Console.WriteLine("Do you want to rematch? (y/n)");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            string rematch = keyInfo.KeyChar.ToString();
            if (rematch == "y")
            {
                Console.Clear();
                ResetBoard();
                StartMatch(Players);
            }
            else if (rematch == "n")
            {
                Console.Clear();
                Player.EndGameStats(Players);
                Console.WriteLine("Thanks for playing!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
                ReMatch(Players);
            }
        }
        public virtual void GameMechanic(List<Player> Players)
        {
        }
        public void ShufflePlayers(List<Player> Players)
        {
            int n = Players.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = p_Random.Next(i + 1);
                Player temp = Players[i];
                Players[i] = Players[j];
                Players[j] = temp;
            }
        }
        protected bool TryGetValidInput(out int chosenValue, int maxValue)
        {
            if (int.TryParse(Console.ReadLine(), out chosenValue) && chosenValue >= 0 && chosenValue <= maxValue)
            {
                return true;
            }
            Console.WriteLine("Invalid input. Try again.");
            return false;
        }
        public virtual void ResetFirstTurn()
        {
        }
        protected string SendMessageToChatGPT(string apiKey, string message)
        {
            var chatGPTClient = new ChatGPTClient(apiKey);
            return chatGPTClient.SendMessage(message);
        }
        protected virtual string BuildMessage(string board, List<Player> p_Players)
        {
            return "error";
        }
        protected string GetApiKey()
        {
            return Environment.GetEnvironmentVariable("CHATGPT_API_KEY", EnvironmentVariableTarget.Machine);
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
    }
}
