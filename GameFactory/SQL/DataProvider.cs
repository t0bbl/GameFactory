using System.Data.SqlClient;
using System.Text;


namespace GameFactory
{
    internal static class DataProvider
    {
        internal static void DisplayPlayerStats(int p_ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sqlQuery = "SELECT * FROM PlayerStatsView WHERE Ident = @p_ident";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@p_ident", p_ident);  // Assuming p_ident is the variable containing the player identifier
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int p_wins = reader.GetInt32(reader.GetOrdinal("Wins"));
                                int p_losses = reader.GetInt32(reader.GetOrdinal("Losses"));
                                int p_draws = reader.GetInt32(reader.GetOrdinal("Draws"));
                                int p_playedGames = reader.GetInt32(reader.GetOrdinal("PlayedGames"));
                                double p_winPercentage = reader.GetDouble(reader.GetOrdinal("WinPercentage"));

                                // Do something with these values
                                Console.WriteLine($"Wins: {p_wins}, Losses: {p_losses}, Draws: {p_draws}, PlayedGames: {p_playedGames}, Win Percentage: {p_winPercentage}");
                            }
                        }
                    }
                }
            }
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
        internal static void DisplayLeaderBoard()
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM LeaderBoard";

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
    }
}
