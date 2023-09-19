using GameFactory.Model;
using System.Text;

namespace GameFactory
{
    internal class Match : Game
    {
        #region Variables
        public char[,] p_board;
        public int p_rows { get; set; }
        public int p_columns { get; set; }
        public int p_winningLength { get; set; }
        public int p_currentPlayerIndex { get; set; }
        public string p_boardString { get; set; }
        public bool p_firstTurn { get; set; }
        public string p_winner { get; set; }
        readonly Random p_random = new();
        #endregion
        public Match()
        {
        }

        public void StartMatch()
        {
            p_currentPlayerIndex = 0;
            ResetBoard();
            ResetFirstTurn();
            ShufflePlayers(p_player);

            do
            {
                GameMechanic(p_player);
                p_winner = CheckWinner(p_player);
            } while (p_winner == null);

            if (p_winner != null)
            {
                UpdateStats(p_player, p_winner);
                ReMatch(p_player);
            }

        }
        public virtual void GameMechanic(List<Player> p_player)
        {
        }

        #region BoardSetup
        public void ResetBoard()
        {
            for (int p_row = 0; p_row < p_rows; p_row++)
            {
                for (int p_col = 0; p_col < p_columns; p_col++)
                {
                    SetCell(p_row, p_col, '0');
                }
            }
        }
        public char GetCell(int p_row, int p_col)
        {
            return p_board[p_row, p_col];
        }
        public void SetCell(int p_row, int p_col, char p_icon)
        {
            if (p_row >= 0 && p_row < p_rows && p_col >= 0 && p_col < p_columns)
            {
                p_board[p_row, p_col] = p_icon;
            }
        }
        public void PrintBoard(bool p_showRow, bool p_showCol, List<Player> p_player)
        {
            for (int p_row = 0; p_row < p_rows; p_row++)
            {
                if (p_showRow)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{p_row + 1} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.Write("  ");
                }

                for (int p_col = 0; p_col < p_columns; p_col++)
                {
                    char p_cellValue = GetCell(p_row, p_col);

                    if (p_cellValue == '0')
                    {
                        Console.Write(" . ");
                    }
                    else
                    {
                        Player p_currentPlayer = p_player.FirstOrDefault(p => p.Icon == p_cellValue);

                        if (p_currentPlayer != null)
                        {
                            ConsoleColor p_originalForegroundColor = Console.ForegroundColor;

                            if (Enum.TryParse(p_currentPlayer.Colour, out ConsoleColor p_parsedColor))
                            {
                                Console.ForegroundColor = p_parsedColor;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                            }

                            Console.Write($" {p_cellValue} ");
                            Console.ForegroundColor = p_originalForegroundColor;
                        }
                    }
                }
                Console.WriteLine();
            }

            if (p_showCol)
            {
                Console.Write("  ");
                for (int p_col = 0; p_col < p_columns; p_col++)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write($" {p_col + 1} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
        }
        #endregion
        #region GameMechanics
        public string CheckWinner(List<Player> p_player)
        {
            for (int p_row = 0; p_row < p_rows; p_row++)
            {
                for (int p_col = 0; p_col < p_columns; p_col++)
                {
                    char p_cellValue = GetCell(p_row, p_col);
                    if (p_cellValue == '0') continue;

                    int[][] p_directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var p_dir in p_directions)
                    {
                        int p_count = 1;
                        for (int p_playerRow = 1; p_playerRow < p_winningLength; p_playerRow++)
                        {
                            int p_newRow = p_row + p_dir[0] * p_playerRow;
                            int p_newCol = p_col + p_dir[1] * p_playerRow;
                            if (p_newRow < 0 || p_newRow >= p_rows || p_newCol < 0 || p_newCol >= p_columns) break;

                            if (GetCell(p_newRow, p_newCol) == p_cellValue)
                            {
                                p_count++;
                            }
                            else break;
                        }

                        if (p_count >= p_winningLength)
                        {
                            Player p_winner = p_player.FirstOrDefault(p => p.Icon == p_cellValue);
                            return p_winner?.Name ?? "Unknown"; // return the player's name or "Unknown" if not found
                        }
                    }
                }
            }

            // Check for draw
            bool p_isDraw = !Enumerable.Range(0, p_rows).Any(p_row => Enumerable.Range(0, p_columns).Any(p_col => GetCell(p_row, p_col) == '0'));

            if (p_isDraw)
            {
                return "Draw";
            }

            return null;
        }
        public void ReMatch(List<Player> p_player)
        {
            Console.WriteLine("Do you want to rematch? (y/n)");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            string rematch = keyInfo.KeyChar.ToString();
            if (rematch == "y")
            {
                Console.Clear();
                ResetBoard();
                StartMatch();
            }
            else if (rematch == "n")
            {
                Console.Clear();
                EndGameStats(p_player);
                Console.WriteLine("Thanks for playing!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
                ReMatch(p_player);
            }
        }
        public void ShufflePlayers(List<Player> p_player)
        {
            if (p_player.All(player => player.IsHuman))
            {
                int n = p_player.Count;
                for (int i = n - 1; i > 0; i--)
                {
                    int j = p_random.Next(i + 1);
                    Player temp = p_player[i];
                    p_player[i] = p_player[j];
                    p_player[j] = temp;
                }
            }
        }
        protected bool TryGetValidInput(out int p_chosenValue, int p_maxValue)
        {
            if (int.TryParse(Console.ReadLine(), out p_chosenValue) && p_chosenValue >= 0 && p_chosenValue <= p_maxValue)
            {
                return true;
            }
            Console.WriteLine("Invalid input. Try again.");
            return false;
        }
        public virtual void ResetFirstTurn()
        {
            p_firstTurn = true;
        }
        #endregion
        #region Stats
        public static List<Player> UpdateStats(List<Player> p_players, string p_winnerName)
        {
            if (p_winnerName != null && p_winnerName != "Draw")
            {
                foreach (var p_player in p_players)
                {
                    if (p_player.Name == p_winnerName)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{p_player.Name} won the game!");
                        p_player.Wins++;
                    }
                    else
                    {
                        p_player.Losses++;
                    }
                }
            }
            else if (p_winnerName == "Draw")
            {
                foreach (var p_player in p_players)
                {
                    p_player.Draws++;
                }
                Console.WriteLine("It's a draw!");
            }

            return p_players;
        }
        public static void EndGameStats(List<Player> p_player)
        {
            Console.WriteLine("Game over!");
            Console.WriteLine("Final scores:");
            foreach (var player in p_player)
            {
                Console.WriteLine($"{player.Name}: {player.Wins} Wins");
                Console.WriteLine($"{player.Name}: {player.Losses} Losses");
                Console.WriteLine($"{player.Name}: {player.Draws} Draws");
            }
        }

        #endregion

        #region ChatGPT
        protected string SendMessageToChatGPT(string apiKey, string p_message)
        {
            var chatGPTClient = new ChatGPTClient(apiKey);
            return chatGPTClient.SendMessage(p_message);
        }
        protected virtual string BuildMessage(string p_board, List<Player> p_players)
        {
            return "error";
        }
        protected string GetApiKey()
        {
            return Environment.GetEnvironmentVariable("CHATGPT_API_KEY", EnvironmentVariableTarget.Machine);
        }
        public string BoardToString(char[,] p_board, List<Player> p_players)
        {
            Console.WriteLine($"Rows: {p_rows}, Columns: {p_columns}"); 

            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < p_rows; row++)
            {
                for (int col = 0; col < p_columns; col++)
                {
                    char cellValue = p_board[row, col];
                    if (cellValue == '0') 
                    {
                        sb.Append(" . ");
                    }
                    else if (cellValue == p_players[0].Icon)
                    {
                        sb.Append($" {p_players[0].Icon} ");
                    }
                    else if (cellValue == p_players[1].Icon)
                    {
                        sb.Append($" {p_players[1].Icon} ");
                    }
                    else
                    {
                        sb.Append(" ? ");
                    }
                }
                sb.AppendLine();
            }

            Console.WriteLine("this is the string: " + sb.ToString());
            return p_boardString = sb.ToString();
        }

        public virtual void ChatGPTMove(string p_board, List<Player> p_players)
        {
        }
        #endregion
    }
}
