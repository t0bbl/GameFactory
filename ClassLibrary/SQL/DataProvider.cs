using CoreGameFactory.Model;
using System.Data.SqlClient;
using System.Text;



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
        /// Retrieves a list of player identifiers from the Player database based on an optional name parameter. The function searches both the "Name" and "LoginName" fields for a match using the provided name. If no name is given, all player identifiers are returned.
        /// </summary>
        internal static List<int> GetPlayerIdentsFromName(string p_Name = null)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            List<int> p_idents = new List<int>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                StringBuilder sqlQuery = new StringBuilder("SELECT Ident FROM Player WHERE 1=1");
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(p_Name))
                {
                    sqlQuery.Append(" AND (Name LIKE @p_name OR LoginName LIKE @p_name)");
                    parameters.Add(new SqlParameter("@p_name", p_Name));
                }

                using (SqlCommand cmd = new SqlCommand(sqlQuery.ToString(), conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Ident"] != DBNull.Value)
                            {
                                p_idents.Add(Convert.ToInt32(reader["Ident"]));
                            }
                        }
                    }
                }
            }
            return p_idents;
        }
        /// <summary>
        /// Checks the availability of a given login name in the Player database. Returns true if the login name is available (not taken), and false otherwise. If the login name is already taken, a message is displayed to the user.
        /// </summary>
        internal static bool CheckLoginNameAvailability(string p_LoginName)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            bool p_result = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT 1 FROM [Player] WHERE LoginName = @p_loginName";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_LoginName));

                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        p_result = true;
                    }
                    else
                    {
                        Console.WriteLine("LoginName already exists. Try again.");
                    }
                }
            }

            return p_result;
        }
        /// <summary>
        /// Validates whether a given login name exists in the Player database. Returns true if the login name is found, and false otherwise. If the login name doesn't exist, a message is displayed to the user.
        /// </summary>
        internal static bool ValidateLoginName(string p_LoginName)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            bool p_result = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT 1 FROM [Player] WHERE LoginName = @p_loginName";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_LoginName));

                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        Console.WriteLine("LoginName doesn't exists. Try again.");
                    }
                    else
                    {
                        p_result = true;
                    }
                }
            }

            return p_result;
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
                            char p_icon = Convert.ToChar(reader["Icon"]);
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
                                int MatchId = reader.GetInt32(reader.GetOrdinal("Match"));
                                int Player = reader.GetInt32(reader.GetOrdinal("PlayerIdent"));
                                string Input = reader.GetString(reader.GetOrdinal("Input"));
                                bool Twist = reader.GetBoolean(reader.GetOrdinal("Twist"));
                                string PlayerName = reader.GetString(reader.GetOrdinal("PlayerName"));
                                MoveHistory.Add(new Move
                                {
                                    Match = MatchId,
                                    Player = Player,
                                    Input = Input,
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


        public static List<Match> DisplayHistory(int p_Ident)
        {
            List<Match> History = new List<Match>();
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM History_V WHERE Winner = @p_ident OR Loser = @p_ident";

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
                                int Draw = reader.GetInt32(reader.GetOrdinal("Draw"));
                                int Rows = reader.GetInt32(reader.GetOrdinal("Rows"));
                                int Columns = reader.GetInt32(reader.GetOrdinal("Cols"));
                                int WinningLength = reader.GetInt32(reader.GetOrdinal("WinningLength"));
                                string GameType = reader.GetString(reader.GetOrdinal("GameType"));
                                string WinnerName = reader.GetString(reader.GetOrdinal("WinnerName"));
                                string LoserName = reader.GetString(reader.GetOrdinal("LoserName"));

                                Match match = new Match()
                                {
                                    p_GameTypeIdent = GameTypeIdent,
                                    p_Winner = Winner,
                                    p_Loser = Loser,
                                    p_Draw = Draw,
                                    p_MatchId = MatchId,
                                    p_Rows = Rows,
                                    p_Columns = Columns,
                                    p_WinningLength = WinningLength,
                                    p_GameType = GameType,
                                    p_WinnerName = reader.GetString(reader.GetOrdinal("WinnerName")),
                                    p_LoserName = reader.GetString(reader.GetOrdinal("LoserName")),
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
