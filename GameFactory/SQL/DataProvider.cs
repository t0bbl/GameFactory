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
                                int losses = reader.GetInt32(reader.GetOrdinal("Losses"));
                                int draws = reader.GetInt32(reader.GetOrdinal("Draws"));
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
            foreach (var item in statsList)
            {

                Console.WriteLine($"Wins: {item.Wins}, Losses: {item.Losses}, Draws: {item.Draws}, Total Games: {item.TotalGames}, Win Percentage: {item.WinPercentage}");

            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
        internal static void GetMatchOutcomeForPlayer1(int p_playerIdent1, int p_playerIdent2, out float p_matchOutcome)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            float localMatchOutcome = 0.0f;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.GetMatchOutcomeForPlayer1(@player1Id, @player2Id)", conn))
                {
                    cmd.Parameters.AddWithValue("@player1Id", p_playerIdent1);
                    cmd.Parameters.AddWithValue("@player2Id", p_playerIdent2);

                    object result = cmd.ExecuteScalar();
                    Console.WriteLine(result + " result");
                    localMatchOutcome = (float)Convert.ToDouble(result);
                }
            }
            p_matchOutcome = localMatchOutcome;
            Console.WriteLine(p_matchOutcome + " p_matchOutcome");

        }
        internal static void GetPlayerEloFromIdent(int p_playerIdent1, int p_playerIdent2, out int p_eloPlayer1, out int p_eloPlayer2)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            p_eloPlayer1 = 0;
            p_eloPlayer2 = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Elo FROM Player WHERE Ident = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", p_playerIdent1);
                    p_eloPlayer1 = (int)cmd.ExecuteScalar();

                    cmd.Parameters["@id"].Value = p_playerIdent2;
                    p_eloPlayer2 = (int)cmd.ExecuteScalar();
                }
                Console.WriteLine(p_eloPlayer1 + "eloplayer1");
                Console.WriteLine(p_eloPlayer2 + "eloplayer2");
            }
        }
        internal static void CalculateExpectedScore(int p_eloPlayer1, int p_eloPlayer2, out float p_expectedScorePlayer1, out float p_expectedScorePlayer2)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.CalculateExpectedScore(@RatingPlayerA, @RatingPlayerB)", conn))
                {
                    cmd.Parameters.AddWithValue("@RatingPlayerA", p_eloPlayer1);
                    cmd.Parameters.AddWithValue("@RatingPlayerB", p_eloPlayer2);
                    p_expectedScorePlayer1 = Convert.ToSingle(cmd.ExecuteScalar());

                    cmd.Parameters["@RatingPlayerA"].Value = p_eloPlayer2;
                    cmd.Parameters["@RatingPlayerB"].Value = p_eloPlayer1;
                    p_expectedScorePlayer2 = Convert.ToSingle(cmd.ExecuteScalar());
                }
                Console.WriteLine(p_expectedScorePlayer1 + "expectedscoreplayer1");
                Console.WriteLine(p_expectedScorePlayer2 + "expectedscoreplayer2");
            }

        }
        internal static void UpdateRating(int p_eloPlayer1, int p_eloPlayer2, float p_expectedScorePlayer1, float p_expectedScorePlayer2, float p_matchOutcome, out int p_newEloPlayer1, out int p_newEloPlayer2)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT dbo.UpdateRating(@oldRating, @expectedScore, @actualScore, @k)", conn))
                {
                    cmd.Parameters.AddWithValue("@oldRating", p_eloPlayer1);
                    cmd.Parameters.AddWithValue("@expectedScore", p_expectedScorePlayer1);
                    cmd.Parameters.AddWithValue("@actualScore", p_matchOutcome);
                    cmd.Parameters.AddWithValue("@k", 32);  // K-factor
                    p_newEloPlayer1 = (int)cmd.ExecuteScalar();

                    cmd.Parameters["@oldRating"].Value = p_eloPlayer2;
                    cmd.Parameters["@expectedScore"].Value = p_expectedScorePlayer2;
                    cmd.Parameters["@actualScore"].Value = 1 - p_matchOutcome;  // If player1 scored 1, player2 scored 0
                    p_newEloPlayer2 = (int)cmd.ExecuteScalar();
                }
                Console.WriteLine(p_newEloPlayer1 + "neweloplayer1");
                Console.WriteLine(p_newEloPlayer2 + "neweloplayer2");

            }
        }

        // private static T ConvertField<T>(object field) => (field == null || field == DBNull.Value) ? default : (T)Convert.ChangeType(field, typeof(T));

        #region NotNeededButWanted

        public class IndividualPlayerStat
        {
            public int PlayerIdent { get; set; }
            public string StatType { get; set; }
            public int StatCount { get; set; }
        }
        internal static List<IndividualPlayerStat> DisplayPlayerStats(int p_ident)
        {
            List<IndividualPlayerStat> statsList = new List<IndividualPlayerStat>();

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
                                IndividualPlayerStat stat = new IndividualPlayerStat
                                {
                                    PlayerIdent = reader.GetInt32(reader.GetOrdinal("PlayerIdent")),
                                    StatType = reader.GetString(reader.GetOrdinal("StatType")),
                                    StatCount = reader.GetInt32(reader.GetOrdinal("StatCount"))
                                };

                                statsList.Add(stat);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found.");
                        }
                    }
                }
            }

            return statsList;
        }

        internal static float GetPlayerWinPercentage(int ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

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
