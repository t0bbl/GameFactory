using CoreGameFactory.Model;
using System.Data.SqlClient;



namespace ClassLibrary
{
    public static class DataProvider
    {

        /// <summary>
        /// Displays the player's statistics based on the provided identifier, p_Ident.
        /// Fetches data including Name, Wins, Losses, Draws, PlayedGames, and WinPercentage
        /// from the PlayerStatsView in the database. If p_DisplayWithName is true, the output includes the player's name.
        /// </summary>
        /// <param name="p_Ident">The unique identifier of the player.</param>
        /// <param name="p_DisplayWithName">A flag to determine if the player's name should be included in the display.</param>
        public static Player DisplayPlayerStats(int p_Ident, bool? p_DisplayWithName)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            Player Player = null;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sqlQuery = "SELECT * FROM PlayerStats_V WHERE Ident = @p_ident";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@p_ident", p_Ident);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string Name = reader.GetString(reader.GetOrdinal("PlayerName"));
                                int Wins = reader.GetInt32(reader.GetOrdinal("Wins"));
                                int Losses = reader.GetInt32(reader.GetOrdinal("Losses"));
                                int Draws = reader.GetInt32(reader.GetOrdinal("Draws"));
                                int PlayedGames = reader.GetInt32(reader.GetOrdinal("PlayedGames"));
                                double WinPercentage = reader.GetDouble(reader.GetOrdinal("WinPercentage"));

                                Player = new Player
                                {
                                    Name = Name,
                                    Wins = Wins,
                                    Losses = Losses,
                                    Draws = Draws,
                                    PlayedGames = PlayedGames,
                                    WinPercentage = WinPercentage
                                };
                                //if (p_DisplayWithName.HasValue)
                                //{
                                //    Console.WriteLine($"{Name}: Wins: {Wins}, Losses: {Losses}, Draws: {Draws}, PlayedGames: {PlayedGames}, Win Percentage: {WinPercentage}");
                                //}
                                //else
                                //{
                                //    Console.WriteLine($"Wins: {Wins}, Losses: {Losses}, Draws: {Draws}, PlayedGames: {PlayedGames}, Win Percentage: {WinPercentage}");
                                //}
                            }
                        }
                    }
                }
            }
            return Player;
        }
        /// <summary>
        /// Validates whether a given login name exists in the Player database. Returns true if the login name is found, and false otherwise.
        /// </summary>
        public static bool ValidateLoginNameForSignup(string p_Loginname)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            bool p_result = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT 1 FROM [Player] WHERE LoginName = @p_loginName";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_Loginname));

                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        p_result = true;
                    }
                    else
                    {
                        p_result = false;
                    }
                }
            }

            return p_result;
        }
        /// <summary>
        /// Retrieves the details of a player from the Player database based on the provided identifier. The details include the player's name, icon, and color. Returns a Player object populated with these details, or null if no matching record is found.
        /// </summary>
        public static Player GetPlayerVariables(int p_Ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            Player player = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT Name, Icon, Color FROM Player WHERE Ident = @p_ident";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_ident", p_Ident));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string p_name = reader["Name"] as string;
                            string p_icon = reader["Icon"] as string;
                            string p_color = reader["Color"] as string;
                            if (p_name != null && p_icon != null && p_color != null)
                            {
                                player = new Player
                                {
                                    Name = p_name,
                                    Icon = p_icon,
                                    Color = p_color,
                                    Ident = p_Ident
                                };
                            }
                        }
                    }
                }
            }

            return player;
        }
        /// <summary>
        /// Retrieves and combines player statistics with additional player variables such as name, icon, and color. This method first calls GetPlayerVariables to obtain the player's basic information based on the provided identifier. It then retrieves the player's statistics using DisplayPlayerStats. The method returns a Player object populated with both sets of information, or null if no matching record is found.
        /// </summary>
        public static Player GetStatsAndVariables(int p_Ident)
        {
            Player Variables = GetPlayerVariables(p_Ident);

            Player Player = DisplayPlayerStats(p_Ident, true);
            Player.Icon = Variables.Icon;
            Player.Color = Variables.Color;
            Player.Name = Variables.Name;
            Player.Ident = p_Ident;
            return Player;
        }
        /// <summary>
        /// Displays the leaderboard from the LeaderBoard database table, listing players by their rank. The displayed details include rank, player name, number of wins, losses, draws, and win percentage. The leaderboard is presented in a tabulated format and prompts the user to press any key to return once displayed.
        /// </summary>
        public static List<Player> DisplayLeaderBoard()
        {
            List<Player> Leaderboard = new List<Player>();
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM LeaderBoard_V";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Player player = new Player
                                {
                                    Rank = reader.GetInt32(reader.GetOrdinal("Rank")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Wins = reader.GetInt32(reader.GetOrdinal("Wins")),
                                    Losses = reader.GetInt32(reader.GetOrdinal("Losses")),
                                    Draws = reader.GetInt32(reader.GetOrdinal("Draws")),
                                    WinPercentage = (float)reader.GetDouble(reader.GetOrdinal("WinPercentage"))
                                };
                                Leaderboard.Add(player);
                            }
                        }

                    }
                }
                return Leaderboard;
            }

        }
        /// <summary>
        /// Retrieves the move history for a specific match identified by p_Ident. Each move is represented as a Move object, containing details such as row, column, twist, and player information. This method queries the MoveHistory_V view and populates a list of Move objects, which is then returned. If no moves are found for the given match, an empty list is returned.
        /// </summary>
        public static List<Move> DisplayMoveHistory(int p_Ident)
        {
            List<Move> MoveHistory = new List<Move>();
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM MoveHistory_V WHERE Match = @p_ident";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@p_ident", p_Ident);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Row = 0;
                                int MatchId = reader.GetInt32(reader.GetOrdinal("Match"));
                                int Player = reader.GetInt32(reader.GetOrdinal("PlayerIdent"));
                                if (!reader.IsDBNull(reader.GetOrdinal("Row")))
                                    Row = reader.GetInt32(reader.GetOrdinal("Row"));
                                int Column = reader.GetInt32(reader.GetOrdinal("Column"));
                                bool Twist = reader.GetBoolean(reader.GetOrdinal("Twist"));
                                string PlayerName = reader.GetString(reader.GetOrdinal("PlayerName"));
                                MoveHistory.Add(new Move
                                {
                                    Match = MatchId,
                                    Player = Player,
                                    Row = Row,
                                    Column = Column,
                                    Twist = Twist,
                                    PlayerName = PlayerName

                                });

                            }
                        }
                    }
                }
                return MoveHistory;
            }
        }
        /// <summary>
        /// Fetches the match history for a player specified by p_Ident. This method queries the History_V view to obtain matches where the player was either a winner or a loser. Each match is represented as a Match object, encompassing details such as game type, winner, loser, and match dimensions. The method returns a list of Match objects in descending order of match identity. If no matches are found, an empty list is returned.
        /// </summary>
        public static List<Match> DisplayHistory(int p_Ident)
        {
            List<Match> History = new List<Match>();
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM History_V WHERE Winner = @p_ident OR Loser = @p_ident ORDER BY MatchIdent DESC";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@p_ident", p_Ident);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int MatchId = reader.GetInt32(reader.GetOrdinal("MatchIdent"));
                                int GameTypeIdent = reader.GetInt32(reader.GetOrdinal("GameTypeIdent"));
                                int Winner = reader.GetInt32(reader.GetOrdinal("Winner"));
                                int Loser = reader.GetInt32(reader.GetOrdinal("Loser"));
                                int? Draw = reader.IsDBNull(reader.GetOrdinal("Draw")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Draw"));
                                int Rows = reader.GetInt32(reader.GetOrdinal("Rows"));
                                int Columns = reader.GetInt32(reader.GetOrdinal("Cols"));
                                int WinningLength = reader.GetInt32(reader.GetOrdinal("WinningLength"));
                                string GameType = reader.GetString(reader.GetOrdinal("GameType"));
                                string WinnerName = reader.GetString(reader.GetOrdinal("WinnerName"));
                                string LoserName = reader.GetString(reader.GetOrdinal("LoserName"));

                                Match match = new Match()
                                {
                                    GameTypeIdent = GameTypeIdent,
                                    Winner = Winner,
                                    Loser = Loser,
                                    Draw = Draw.HasValue ? Draw.Value : 0,
                                    MatchId = MatchId,
                                    Rows = Rows,
                                    Columns = Columns,
                                    WinningLength = WinningLength,
                                    GameType = GameType,
                                    WinnerName = reader.GetString(reader.GetOrdinal("WinnerName")),
                                    LoserName = reader.GetString(reader.GetOrdinal("LoserName")),
                                };
                                History.Add(match);
                            }
                        }

                    }
                }
                return History;
            }
        }
    }
}
