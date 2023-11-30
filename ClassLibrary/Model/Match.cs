using CoreGameFactory.Model;
using System.Data;
using System.Data.SqlClient;

namespace ClassLibrary
{
    public class Match : Game
    {
        #region Variables
        internal string[,] p_Board { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int WinningLength { get; set; }
        public int CurrentPlayerIndex { get; set; }
        internal bool FirstTurn { get; set; }
        public int? Winner { get; set; }
        public int? Loser { get; set; }
        public int Draw { get; set; }
        public int GameTypeIdent { get; set; }
        public int MatchId { get; set; }
        public bool TwistStat { get; set; }
        public string GameType { get; set; }
        public string WinnerName { get; set; }
        public string LoserName { get; set; }
        public string Result { get; set; }
        public string Opponent { get; set; }
        public string CurrentPlayer { get; set; }

        public event EventHandler<GameStateChangedEventArgs> GameStateChanged;

        public event EventHandler<PlayerChangedEventArgs> PlayerChanged;

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
            this.Rows = p_Rows;
            this.Columns = p_Columns;
            this.WinningLength = p_WinningLength;
            p_Board = new string[Rows, p_Columns];
        }
        public Match()
        {

        }

        /// <summary>
        /// Executes the core game mechanics, including saving game and match details.
        /// </summary>
        /// <param name="p_Player">The list of players participating in the game.</param>
        public virtual void StartGameMechanic(List<Player> p_Player)
        {
            CurrentPlayerIndex = 0;

            ShufflePlayers(PlayerList);

            ResetBoard();
            GameTypeIdent = SaveGame(Rows, Columns, WinningLength, GameType);

            MatchId = SaveMatch(Winner, Loser, Draw, GameTypeIdent, MatchId);
        }

        public void HandleClick(int p_Row, int p_Col)
        {
            SetCell(p_Row, p_Col, PlayerList[CurrentPlayerIndex].Icon);
            string Cell = $"{p_Row * Columns + p_Col + 1}";

            EndTurn(Cell);
        }


        #region BoardSetup
        /// <summary>
        /// Resets the game board by setting all cell values to '0'.
        /// </summary>
        public void ResetBoard()
        {
            for (int Row = 0; Row < Rows; Row++)
            {
                for (int Col = 0; Col < Columns; Col++)
                {
                    SetCell(Row, Col, "0");
                }
            }
        }
        /// <summary>
        /// Retrieves the value of a cell on the game board at the specified coordinates.
        /// </summary>
        /// <param name="p_Row">The row index of the cell.</param>
        /// <param name="p_Col">The column index of the cell.</param>
        /// <returns>The value of the cell at the specified coordinates.</returns>
        public string GetCell(int p_Row, int p_Col)
        {
            return p_Board[p_Row, p_Col];
        }
        /// <summary>
        /// Sets the value of a cell on the game board at the specified coordinates, provided they are within bounds.
        /// </summary>
        /// <param name="p_Row">The row index of the cell.</param>
        /// <param name="p_Col">The column index of the cell.</param>
        /// <param name="p_Icon">The value to be set at the specified cell.</param>
        public void SetCell(int p_Row, int p_Col, string p_Icon)
        {
            if (p_Row >= 0 && p_Row < Rows && p_Col >= 0 && p_Col < Columns)
            {
                p_Board[p_Row, p_Col] = p_Icon;
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
            for (int Row = 0; Row < Rows; Row++)
            {
                for (int Col = 0; Col < Columns; Col++)
                {
                    string CellValue = GetCell(Row, Col);
                    if (CellValue == "0") continue;

                    int[][] directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var Dir in directions)
                    {
                        int Count = 1;
                        for (int PlayerRow = 1; PlayerRow < WinningLength; PlayerRow++)
                        {
                            int newRow = Row + Dir[0] * PlayerRow;
                            int newCol = Col + Dir[1] * PlayerRow;
                            if (newRow < 0 || newRow >= Rows || newCol < 0 || newCol >= Columns) break;

                            if (GetCell(newRow, newCol) == CellValue)
                            {
                                Count++;
                            }
                            else break;
                        }

                        if (Count >= WinningLength)
                        {
                            Player p_winner = p_Player.FirstOrDefault(p => p.Icon == CellValue);
                            return p_winner.Ident;
                        }
                    }
                }
            }

            bool IsDraw = !Enumerable.Range(0, Rows).Any(p_Row => Enumerable.Range(0, Columns).Any(p_Col => GetCell(p_Row, p_Col) == "0"));

            if (IsDraw)
            {
                Draw = 1;
                return 0;
            }

            return null;
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
        public void EndTurn(string p_ChosenCell)
        {
            SavePlayerToMatch(PlayerList[CurrentPlayerIndex].Ident, MatchId);
            CurrentPlayer = PlayerList[CurrentPlayerIndex].Name;
            OnPlayerChanged(new PlayerChangedEventArgs(CurrentPlayer));
            SaveMoveHistory(PlayerList[CurrentPlayerIndex].Ident, p_ChosenCell, MatchId, TwistStat);

            Winner = CheckWinner(PlayerList);

            if (Winner != null)
            {
                EndGame(PlayerList);
            }
        }
        private void EndGame(List<Player> p_PlayerList)
        {
            UpdateStats(p_PlayerList);

            SaveMatch(Winner, Loser, Draw, GameTypeIdent, MatchId);

            MatchId = 0;

            OnGameStateChanged(new GameStateChangedEventArgs(Winner, Draw));
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
            if (Winner != null)
            {
                if (Draw == 0)
                {
                    foreach (var Player in p_Players)
                    {
                        if (Player.Ident == Winner)
                        {
                            Winner = Player.Ident;
                        }

                        else
                        {
                            Loser = Player.Ident;
                        }
                    }
                }
                else
                {
                    Winner = p_Players[0].Ident;
                    Loser = p_Players[1].Ident;
                }
            }
            return p_Players;
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

                    cmd.Parameters.AddWithValue("@p_Rows", Rows);
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
        #region EventArgs
        protected virtual void OnGameStateChanged(GameStateChangedEventArgs e)
        {
            GameStateChanged?.Invoke(this, e);
        }

        protected virtual void OnPlayerChanged(PlayerChangedEventArgs e)
        {
            PlayerChanged?.Invoke(this, e);
        }
        public virtual void GameCellClicked(object sender, GameCellClickedEventArgs e)
        {
            HandleClick(e.Row, e.Column);
        }
        #endregion
    }
}
