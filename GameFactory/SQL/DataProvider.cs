using System.Data.SqlClient;
using System.Text;


namespace GameFactory
{
    internal static class DataProvider
    {
        internal static List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)> GetPlayerStats(int p_ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)> statsList = new List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM dbo.GetPlayerStats(@p_ident)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_ident", p_ident));

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var stats = (Wins: reader.GetInt32(reader.GetOrdinal("Wins")),
                                           Losses: reader.GetInt32(reader.GetOrdinal("Losses")),
                                           Draws: reader.GetInt32(reader.GetOrdinal("Draws")),
                                           TotalGames: reader.GetInt32(reader.GetOrdinal("TotalGames")),
                                           WinPercentage: (float)reader.GetDouble(reader.GetOrdinal("WinPercentage")));
                                statsList.Add(stats);
                            }
                        }
                    }
                }
            }
            return statsList;
        }
        internal static int GetPlayerIdentFromName(string p_name = null)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            int p_ident = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                StringBuilder sqlQuery = new StringBuilder("SELECT Ident FROM Player WHERE 1=1");
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(p_name))
                {
                    sqlQuery.Append(" AND (Name = @p_name OR LoginName = @p_name)");
                    parameters.Add(new SqlParameter("@p_name", p_name));
                }

                using (SqlCommand cmd = new SqlCommand(sqlQuery.ToString(), conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        p_ident = Convert.ToInt32(result);
                    }
                }
            }
            return p_ident;
        }
        internal static bool CheckLoginName(string p_loginName)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            bool p_result = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT 1 FROM [Player] WHERE LoginName = @p_loginName";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));

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
        internal static Player GetPlayerVariables(int p_ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            Player player = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT Name, Icon, Color FROM Player WHERE Ident = @p_ident";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_ident", p_ident));

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
                                    Colour = p_color,
                                    Ident = p_ident
                                };
                            }
                        }
                    }
                }
            }

            return player;
        }
        internal static void DisplayRankedPlayers()
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM RankedPlayers";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.Clear();
                            Console.WriteLine("{0,-5} | {1,-15} | {2,-5} | {3,-7} | {4,-5} | {5,-12}",
                                              "Rank", "Player Name", "Wins", "Losses", "Draws", "Win Percentage");
                            Console.WriteLine(new string('-', 66));

                            while (reader.Read())
                            {
                                long rank = reader.GetInt64(reader.GetOrdinal("Rank"));
                                string playerName = reader.GetString(reader.GetOrdinal("Name"));
                                int wins = reader.GetInt32(reader.GetOrdinal("Wins"));
                                int losses = reader.GetInt32(reader.GetOrdinal("Losses"));  // Read losses
                                int draws = reader.GetInt32(reader.GetOrdinal("Draws"));  // Read draws
                                float winPercentage = (float)reader.GetDouble(reader.GetOrdinal("WinPercentage"));

                                Console.WriteLine("{0,-5} | {1,-15} | {2,-5} | {3,-7} | {4,-5} | {5,-12:F2}",
                                          rank, playerName, wins, losses, draws, winPercentage);

                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found.");
                        }
                        Console.WriteLine(new string('-', 66));
                        Console.WriteLine("Press any Key to return");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }
        internal static void ShowPlayerStats()
        {
            Console.Clear();
            Console.WriteLine("Input a Name or LoginName to check their PlayerStats");
            string p_input = Console.ReadLine();
            int p_playerIdent = GetPlayerIdentFromName(p_input);
            List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)> statsList = GetPlayerStats(p_playerIdent);
            foreach (var stats in statsList)
            {
                Console.WriteLine($"Wins: {stats.Wins}, Losses: {stats.Losses}, Draws: {stats.Draws}, Total Games: {stats.TotalGames}, Win Percentage: {stats.WinPercentage}");
            }
            //DataProvider.DisplayPlayerStats(p_playerIdent);
            //float winpercentage = DataProvider.GetPlayerWinPercentage(p_playerIdent);
            //Console.WriteLine($"Win Percentage: {winpercentage}");

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        #region NotNeededButWanted

        internal static void DisplayPlayerStats(int p_ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM PlayerStats WHERE PlayerIdent = @p_ident";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_ident", p_ident));

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int playerIdent = reader.GetInt32(reader.GetOrdinal("PlayerIdent"));
                                string statType = reader.GetString(reader.GetOrdinal("StatType"));
                                int statCount = reader.GetInt32(reader.GetOrdinal("StatCount"));

                                Console.WriteLine($"Player ID: {playerIdent}, Stat Type: {statType}, Stat Count: {statCount}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found.");
                        }
                    }
                }
            }
        }
        internal static float GetPlayerWinPercentage(int ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString(); // Replace with your connection string utility

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT dbo.GetPlayerWinPercentage(@p_ident)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_ident", ident));

                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && float.TryParse(result.ToString(), out float winPercentage))
                    {
                        return winPercentage;
                    }
                    return 0.0f;
                }
            }
        }

        #endregion

    }
}
