using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ClassLibrary
{
    public class Match : Game
    {
        #region Variables
        internal char[,] p_Board { get; set; }
        internal int p_Rows { get; set; }
        internal int p_Columns { get; set; }
        internal int p_WinningLength { get; set; }
        internal int p_CurrentPlayerIndex { get; set; }
        internal string p_BoardString { get; set; }
        internal bool p_FirstTurn { get; set; }
        internal int? p_Winner { get; set; }
        internal int? p_Loser { get; set; }
        internal int p_Draw { get; set; }
        internal int p_GameTypeIdent { get; set; }
        internal int p_MatchId { get; set; }
        internal bool p_TwistStat { get; set; }




        Random p_Random = new();

        #endregion
        /// <summary>
        /// Constructs a new match with specified dimensions and winning condition.
        /// </summary>
        /// <param name="p_Rows">The number of rows on the game board.</param>
        /// <param name="p_Columns">The number of columns on the game board.</param>
        /// <param name="p_WinningLength">The length of consecutive symbols required to win the game.</param>
        internal Match(int p_Rows, int p_Columns, int p_WinningLength)
        {
            this.p_Rows = p_Rows;
            this.p_Columns = p_Columns;
            this.p_WinningLength = p_WinningLength;
            p_Board = new char[p_Rows, p_Columns];
        }
        /// <summary>
        /// Initiates a game match, handles player turns, checks for a winner, updates stats, and saves match results.
        /// </summary>
        public void StartMatch()
        {
            p_FirstTurn = true;
            if (p_Player.All(player => player.Name != "CHATGPT"))
            { ShufflePlayers(p_Player); }

            p_CurrentPlayerIndex = 0;
            ResetBoard();
            do
            {
                GameMechanic(p_Player);
                p_Winner = CheckWinner(p_Player);
            } while (p_Winner == null);
            UpdateStats(p_Player);
            SaveMatch(p_Winner, p_Loser, p_Draw, p_GameTypeIdent, p_MatchId);
            p_MatchId = 0;
        }
        /// <summary>
        /// Executes the core game mechanics, including saving game and match details.
        /// </summary>
        /// <param name="p_Player">The list of players participating in the game.</param>
        public virtual void GameMechanic(List<Player> p_Player)
        {

            p_GameTypeIdent = SaveGame(p_Rows, p_Columns, p_WinningLength, p_GameType);

            p_MatchId = SaveMatch(p_Winner, p_Loser, p_Draw, p_GameTypeIdent, p_MatchId);

        }

        #region BoardSetup
        /// <summary>
        /// Resets the game board by setting all cell values to '0'.
        /// </summary>
        public void ResetBoard()
        {
            for (int Row = 0; Row < p_Rows; Row++)
            {
                for (int Col = 0; Col < p_Columns; Col++)
                {
                    SetCell(Row, Col, '0');
                }
            }
        }
        /// <summary>
        /// Retrieves the value of a cell on the game board at the specified coordinates.
        /// </summary>
        /// <param name="p_Row">The row index of the cell.</param>
        /// <param name="p_Col">The column index of the cell.</param>
        /// <returns>The value of the cell at the specified coordinates.</returns>
        public char GetCell(int p_Row, int p_Col)
        {
            return p_Board[p_Row, p_Col];
        }
        /// <summary>
        /// Sets the value of a cell on the game board at the specified coordinates, provided they are within bounds.
        /// </summary>
        /// <param name="p_Row">The row index of the cell.</param>
        /// <param name="p_Col">The column index of the cell.</param>
        /// <param name="p_Icon">The value to be set at the specified cell.</param>
        public void SetCell(int p_Row, int p_Col, char p_Icon)
        {
            if (p_Row >= 0 && p_Row < p_Rows && p_Col >= 0 && p_Col < p_Columns)
            {
                p_Board[p_Row, p_Col] = p_Icon;
            }
        }
        /// <summary>
        /// Prints the game board to the console, with options to display row and column numbers, and colorizes player icons.
        /// </summary>
        /// <param name="p_ShowRow">Indicates whether to display row numbers.</param>
        /// <param name="p_ShowCol">Indicates whether to display column numbers.</param>
        /// <param name="p_Player">The list of players participating in the game for icon colorization.</param>
        public void PrintBoard(bool p_ShowRow, bool p_ShowCol, List<Player> p_Player)
        {
            for (int Row = 0; Row < p_Rows; Row++)
            {
                if (p_ShowRow)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{Row + 1} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.Write("  ");
                }

                for (int Col = 0; Col < p_Columns; Col++)
                {
                    char CellValue = GetCell(Row, Col);

                    if (CellValue == '0')
                    {
                        Console.Write(" . ");
                    }
                    else
                    {
                        Player CurrentPlayer = p_Player.FirstOrDefault(p => p.Icon == CellValue);

                        if (CurrentPlayer != null)
                        {
                            ConsoleColor p_originalForegroundColor = Console.ForegroundColor;

                            if (Enum.TryParse(CurrentPlayer.Colour, out ConsoleColor ParsedColor))
                            {
                                Console.ForegroundColor = ParsedColor;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                            }

                            Console.Write($" {CellValue} ");
                            Console.ForegroundColor = p_originalForegroundColor;
                        }
                    }
                }
                Console.WriteLine();
            }

            if (p_ShowCol)
            {
                Console.Write("  ");
                for (int Col = 0; Col < p_Columns; Col++)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write($" {Col + 1} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
        }
        #endregion
        #region GameMechanics
        /// <summary>
        /// Checks the game board for a winner or a draw based on the game rules.
        /// </summary>
        /// <param name="p_Player">The list of players participating in the game.</param>
        /// <returns>The identifier of the winning player, 0 for a draw, or null if the game is ongoing.</returns>
        public int? CheckWinner(List<Player> p_Player)
        {
            for (int Row = 0; Row < p_Rows; Row++)
            {
                for (int Col = 0; Col < p_Columns; Col++)
                {
                    char CellValue = GetCell(Row, Col);
                    if (CellValue == '0') continue;

                    int[][] directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var Dir in directions)
                    {
                        int Count = 1;
                        for (int PlayerRow = 1; PlayerRow < p_WinningLength; PlayerRow++)
                        {
                            int newRow = Row + Dir[0] * PlayerRow;
                            int newCol = Col + Dir[1] * PlayerRow;
                            if (newRow < 0 || newRow >= p_Rows || newCol < 0 || newCol >= p_Columns) break;

                            if (GetCell(newRow, newCol) == CellValue)
                            {
                                Count++;
                            }
                            else break;
                        }

                        if (Count >= p_WinningLength)
                        {
                            Player p_winner = p_Player.FirstOrDefault(p => p.Icon == CellValue);
                            return p_winner.Ident;
                        }
                    }
                }
            }

            bool IsDraw = !Enumerable.Range(0, p_Rows).Any(p_Row => Enumerable.Range(0, p_Columns).Any(p_Col => GetCell(p_Row, p_Col) == '0'));

            if (IsDraw)
            {
                p_Draw = 1;
                return 0;
            }

            return null;
        }
        /// <summary>
        /// Prompts the user for a rematch and returns their decision.
        /// </summary>
        /// <returns>True if the user wants a rematch, false otherwise.</returns>
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
        /// <summary>
        /// Shuffles the order of players in the provided list using the Fisher-Yates algorithm.
        /// </summary>
        /// <param name="p_Player">The list of players to be shuffled.</param>
        public void ShufflePlayers(List<Player> p_Player)
        {

            int n = p_Player.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + p_Random.Next(n - i);
                Player temp = p_Player[r];
                p_Player[r] = p_Player[i];
                p_Player[i] = temp;
            }
        }
        /// <summary>
        /// Tries to get a valid integer input from the user within a specified range.
        /// </summary>
        /// <param name="p_ChosenValue">The parsed integer value from the user input.</param>
        /// <param name="p_MaxValue">The maximum valid value for the user input.</param>
        /// <returns>True if a valid integer input is received, false otherwise.</returns>
        protected bool TryGetValidInput(out int p_ChosenValue, int p_MaxValue)
        {
            if (int.TryParse(Console.ReadLine(), out p_ChosenValue) && p_ChosenValue >= 0 && p_ChosenValue <= p_MaxValue)
            {
                return true;
            }
            Console.WriteLine("Invalid input. Try again.");
            return false;
        }
        #endregion
        #region Stats
        /// <summary>
        /// Updates the match statistics, identifies the winner or detects a draw, and displays the result.
        /// </summary>
        /// <param name="p_Players">The list of players participating in the game.</param>
        /// <returns>The list of players with updated match statistics.</returns>
        internal List<Player> UpdateStats(List<Player> p_Players)
        {
            if (p_Winner != null)
            {
                if (p_Draw == 0)
                {
                    foreach (var Player in p_Players)
                    {
                        if (Player.Ident == p_Winner)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{Player.Name} won the game!");
                            p_Winner = Player.Ident;
                        }

                        else
                        {
                            Console.WriteLine();
                            p_Loser = Player.Ident;
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("It's a draw!");
                    p_Winner = p_Players[0].Ident;
                    p_Loser = p_Players[1].Ident;
                }
            }
            return p_Players;
        }
        /// <summary>
        /// Displays the final scores and statistics of the players at the end of the game, and terminates the application.
        /// </summary>
        /// <param name="p_Player">The list of players participating in the game.</param>
        public void EndGameStats(List<Player> p_Player)
        {
            Console.WriteLine("Game over!");
            Console.WriteLine("Final scores:");

            foreach (var Player in p_Player)
            {
                DataProvider.DisplayPlayerStats(Player.Ident, true);
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        #endregion
        #region SQL
        /// <summary>
        /// Saves the match result to the database using a stored procedure.
        /// </summary>
        /// <param name="p_Winner">The identifier of the winning player.</param>
        /// <param name="p_Loser">The identifier of the losing player.</param>
        /// <param name="p_Draw">Indicates whether the match was a draw.</param>
        /// <param name="p_GameType">The identifier of the game type.</param>
        /// <param name="p_MatchId">The identifier of the match, used for updating an existing record.</param>
        /// <returns>The identifier of the saved match record.</returns>
        internal int SaveMatch(int? p_Winner, int? p_Loser, int p_Draw, int p_GameType, int p_MatchId)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMatch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Winner", p_Winner.HasValue ? (object)p_Winner.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Loser", p_Loser.HasValue ? (object)p_Loser.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_Draw", p_Draw);
                    cmd.Parameters.AddWithValue("@p_GameType", p_GameType);

                    SqlParameter resultParam = new SqlParameter("@p_matchId", SqlDbType.Int);
                    resultParam.Direction = ParameterDirection.InputOutput;
                    resultParam.Value = p_MatchId;
                    cmd.Parameters.Add(resultParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    p_MatchId = (int)resultParam.Value;
                    return p_MatchId;
                }
            }
        }
        /// <summary>
        /// Saves the game configuration to the database using a stored procedure.
        /// </summary>
        /// <param name="p_Rows">The number of rows on the game board.</param>
        /// <param name="p_Columns">The number of columns on the game board.</param>
        /// <param name="p_WinningLength">The length of consecutive symbols required to win the game.</param>
        /// <param name="p_GameType">The type of game being played.</param>
        /// <returns>The identifier of the saved game configuration record.</returns>
        internal int SaveGame(int p_Rows, int p_Columns, int p_WinningLength, string p_GameType)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveGame", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Rows", p_Rows);
                    cmd.Parameters.AddWithValue("@p_Cols", p_Columns);
                    cmd.Parameters.AddWithValue("@p_WinningLength", p_WinningLength);
                    cmd.Parameters.AddWithValue("@p_GameType", p_GameType);

                    SqlParameter identParam = new SqlParameter("@p_Ident", SqlDbType.Int);
                    identParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(identParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                    int ident = (int)identParam.Value;

                    return ident;
                }
            }
        }
        /// <summary>
        /// Saves the move history to the database using a stored procedure.
        /// </summary>
        /// <param name="p_Player">The identifier of the player making the move.</param>
        /// <param name="p_Input">The input representing the move.</param>
        /// <param name="p_MatchId">The identifier of the match in which the move is made.</param>
        /// <param name="p_Twist">Indicates whether a special condition or rule was applied to the move.</param>
        internal void SaveMoveHistory(int p_Player, string p_Input, int p_MatchId, bool p_Twist)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMoveHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Player", p_Player);
                    cmd.Parameters.AddWithValue("@p_Input", p_Input);
                    cmd.Parameters.AddWithValue("@p_MatchId", p_MatchId);
                    cmd.Parameters.AddWithValue("@p_Twist", p_Twist);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        /// <summary>
        /// Associates a player with a match in the database using a stored procedure.
        /// </summary>
        /// <param name="p_PlayerId">The identifier of the player.</param>
        /// <param name="p_MatchId">The identifier of the match.</param>
        internal void SavePlayerToMatch(int p_PlayerId, int p_MatchId)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SavePlayerToMatch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_PlayerId", p_PlayerId);
                    cmd.Parameters.AddWithValue("@p_MatchId", p_MatchId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region ChatGPT
        /// <summary>
        /// Sends a message to ChatGPT service using the provided API key and retrieves the response.
        /// </summary>
        /// <param name="p_ApiKey">The API key for authentication with the ChatGPT service.</param>
        /// <param name="p_Message">The message to be sent to ChatGPT.</param>
        /// <returns>The response from the ChatGPT service.</returns>
        protected string SendMessageToChatGPT(string p_ApiKey, string p_Message)
        {
            var chatGPTClient = new ChatGPTClient(p_ApiKey);
            return chatGPTClient.SendMessage(p_Message);
        }
        /// <summary>
        /// Constructs a message based on the game board and player information. To be implemented by derived classes.
        /// </summary>
        /// <param name="p_Board">The representation of the game board.</param>
        /// <param name="p_Players">The list of players participating in the game.</param>
        /// <returns>The constructed message or "error" if not implemented.</returns>
        public virtual string BuildMessage(string p_Board, List<Player> p_Players)
        {
            return "error";
        }
        /// <summary>
        /// Retrieves the ChatGPT API key from the machine-level environment variables.
        /// </summary>
        /// <returns>The ChatGPT API key.</returns>
        protected string GetApiKey()
        {
            return Environment.GetEnvironmentVariable("CHATGPT_API_KEY", EnvironmentVariableTarget.Machine);
        }
        /// <summary>
        /// Converts the game board array to a string representation, using the player icons for occupied cells.
        /// </summary>
        /// <param name="p_Board">The game board array.</param>
        /// <param name="p_Players">The list of players participating in the game.</param>
        /// <returns>The string representation of the game board.</returns>
        protected string BoardToString(char[,] p_Board, List<Player> p_Players)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < p_Rows; row++)
            {
                for (int col = 0; col < p_Columns; col++)
                {
                    char cellValue = p_Board[row, col];
                    if (cellValue == '0')
                    {
                        sb.Append(" . ");
                    }
                    else if (cellValue == p_Players[0].Icon)
                    {
                        sb.Append($" {p_Players[0].Icon} ");
                    }
                    else if (cellValue == p_Players[1].Icon)
                    {
                        sb.Append($" {p_Players[1].Icon} ");
                    }
                    else
                    {
                        sb.Append(" ? ");
                    }
                }
                sb.AppendLine();
            }

            return p_BoardString = sb.ToString();
        }
        /// <summary>
        /// Defines the method to process a move using ChatGPT based on the game board and player information. To be implemented by derived classes.
        /// </summary>
        /// <param name="p_Board">The representation of the game board.</param>
        /// <param name="p_Players">The list of players participating in the game.</param>
        public virtual void ChatGPTMove(string p_Board, List<Player> p_Players)
        {
        }
        #endregion
    }
}
