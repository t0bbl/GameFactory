using GameFactory.Model;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace GameFactory
{
    internal class Match : Game
    {
        #region Variables
        internal char[,] p_board;
        internal int p_rows { get; set; }
        internal int p_columns { get; set; }
        internal int p_winningLength { get; set; }
        internal int p_currentPlayerIndex { get; set; }
        internal string p_boardString { get; set; }
        internal bool p_firstTurn { get; set; }
        internal int? p_winner { get; set; }
        internal int? p_loser { get; set; }
        internal int p_draw { get; set; }
        internal int p_gameTypeIdent { get; set; }
        protected int p_matchId { get; set; }
        protected bool p_twistStat { get; set; }

        private static Random p_random = new();

        #endregion

        internal void StartMatch()
        {
            p_firstTurn = true;
            p_currentPlayerIndex = 0;
            ResetBoard();
            ShufflePlayers(p_player);

            do
            {
                GameMechanic(p_player);
                p_winner = CheckWinner(p_player);
            } while (p_winner == null);
            UpdateStats(p_player);
            SaveMatch(p_winner, p_loser, p_draw, p_gameTypeIdent, p_matchId);
            p_matchId = 0;
        }

        public virtual void GameMechanic(List<Player> p_player)
        {
            p_gameTypeIdent = SaveGame(p_rows, p_columns, p_winningLength, p_gameType);

            p_matchId = SaveMatch(p_winner, p_loser, p_draw, p_gameTypeIdent, p_matchId);

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
        public int? CheckWinner(List<Player> p_player)
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
                            return p_winner.Ident;
                        }
                    }
                }
            }

            // Check for draw
            bool p_isDraw = !Enumerable.Range(0, p_rows).Any(p_row => Enumerable.Range(0, p_columns).Any(p_col => GetCell(p_row, p_col) == '0'));

            if (p_isDraw)
            {
                p_draw = 1;
                return 0;
            }

            return null;
        }
        public bool ReMatch()
        {
            while (true)
            {
                Console.WriteLine("Do you want to rematch? (y/n)");
                string keyInfo = Console.ReadKey().KeyChar.ToString().ToLower();
                Console.WriteLine();

                if (keyInfo == "n")
                {
                    return false;
                }
                if (keyInfo == "y")
                {
                    Console.Clear();
                    return true;
                }

                Console.WriteLine("Invalid input. Try again.");
            }



        }
        public void ShufflePlayers(List<Player> p_player)
        {

            if (p_player.All(player => player.IsHuman))
            {
                p_currentPlayerIndex = p_random.Next(p_player.Count);

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
        #endregion
        #region Stats
        internal List<Player> UpdateStats(List<Player> p_players)
        {
            if (p_winner != null)
            {
                if (p_draw == 0)
                {
                    foreach (var p_player in p_players)
                    {
                        if (p_player.Ident == p_winner)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{p_player.Name} won the game!");
                            p_winner = p_player.Ident;
                        }

                        else
                        {
                            Console.WriteLine();
                            p_loser = p_player.Ident;
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("It's a draw!");
                    p_winner = p_players[0].Ident;
                    p_loser = p_players[1].Ident;
                }
            }
            return p_players;
        }
        internal void EndGameStats(List<Player> p_player)
        {
            Console.WriteLine("Game over!");
            Console.WriteLine("Final scores:");

            foreach (var Player in p_player)
            {
                DataProvider.DisplayPlayerStats(Player.Ident);
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        #endregion
        #region SQL
        internal  int SaveMatch(int? p_winner, int? p_loser, int p_draw, int p_gameType, int p_matchId)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMatch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_winner", p_winner));
                    cmd.Parameters.Add(new SqlParameter("@p_loser", p_loser));
                    cmd.Parameters.Add(new SqlParameter("@p_draw", p_draw));
                    cmd.Parameters.Add(new SqlParameter("@p_gameType", p_gameType));

                    SqlParameter resultParam = new SqlParameter("@p_matchId", SqlDbType.Int);
                    resultParam.Direction = ParameterDirection.InputOutput;
                    resultParam.Value = p_matchId;
                    cmd.Parameters.Add(resultParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    p_matchId = (int)resultParam.Value;
                    return p_matchId;
                }
            }
        }
        internal  int SaveGame(int p_rows, int p_columns, int p_winningLength, string p_gameType)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveGame", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_rows", p_rows));
                    cmd.Parameters.Add(new SqlParameter("@p_cols", p_columns));
                    cmd.Parameters.Add(new SqlParameter("@p_winningLength", p_winningLength));
                    cmd.Parameters.Add(new SqlParameter("@p_gameType", p_gameType));


                    SqlParameter identParam = new SqlParameter("@p_ident", SqlDbType.Int);
                    identParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(identParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                    int ident = (int)identParam.Value;

                    return  ident;
                }
            }
        }
        internal  void SaveMoveHistory(int p_player, string p_input, int p_matchId, bool p_twist)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMoveHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@p_player", p_player));
                    cmd.Parameters.Add(new SqlParameter("@p_input", p_input));
                    cmd.Parameters.Add(new SqlParameter("@p_matchId", p_matchId));
                    cmd.Parameters.Add(new SqlParameter("@p_twist", p_twist));
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        internal void SavePlayerList(int p_playerId, int p_matchId)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SavePlayerList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_playerId", p_playerId));
                    cmd.Parameters.Add(new SqlParameter("@p_matchId", p_matchId));

                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
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

            return p_boardString = sb.ToString();
        }

        public virtual void ChatGPTMove(string p_board, List<Player> p_players)
        {
        }
        #endregion
    }
}
