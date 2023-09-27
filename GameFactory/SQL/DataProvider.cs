using GameFactory;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace GameFactory.SQL
{
    internal static class DataProvider
    {
        internal static (int Wins, int Losses, int Draws) GetPlayerStats(int ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            int wins = 0, losses = 0, draws = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Match WHERE Winner = @p_ident", conn))
                {
                    cmd.Parameters.AddWithValue("@p_ident", ident);
                    wins = (int)cmd.ExecuteScalar();
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Match WHERE Loser = @p_ident", conn))
                {
                    cmd.Parameters.AddWithValue("@p_ident", ident);
                    losses = (int)cmd.ExecuteScalar();
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Match WHERE Draw = 1 AND (Winner = @p_ident OR Loser = @p_ident)", conn))
                {
                    cmd.Parameters.AddWithValue("@p_ident", ident);
                    draws = (int)cmd.ExecuteScalar();
                }
            }

            return (Wins: wins, Losses: losses, Draws: draws);
        }
        internal static int GetPlayerWinPercentage(int wins, int losses, int draws)
        {
            int winperc = 0;
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            string sql = "SELECT dbo.playerWinPerc(@wins, @losses, @draws)";


            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@wins", SqlDbType.Int);
                    cmd.Parameters.Add("@losses", SqlDbType.Int);
                    cmd.Parameters.Add("@draws", SqlDbType.Int);

                    cmd.Parameters["@wins"].Value = wins;
                    cmd.Parameters["@losses"].Value = losses;
                    cmd.Parameters["@draws"].Value = draws;

                    winperc = (int)cmd.ExecuteScalar();

                }
            }
            return winperc;
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
            Console.WriteLine(p_ident);
            Console.ReadKey();
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

                            Console.Write($"{p_name}, {p_icon}, {p_color}");

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

    }
}
