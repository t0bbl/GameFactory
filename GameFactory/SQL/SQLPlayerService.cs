using GameFactory;
using System.Data;
using System.Data.SqlClient;
using System.Text;

internal class SQLPlayerService
{
    private readonly SqlConnection conn = new SQLDatabaseUtility().GetConnection();
    internal bool SignUpPlayer(string p_loginName, string p_password)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("SignUpPlayer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                cmd.Parameters.Add(new SqlParameter("@p_password", p_password));
                cmd.Parameters.Add(new SqlParameter("@p_isHuman", 1));

                SqlParameter resultParam = new SqlParameter("@p_result", SqlDbType.Bit);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resultParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                bool result = (bool)resultParam.Value;
                return result;
            }
        }
    }
    internal int LoginPlayer(string p_loginName, string p_password)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();
        int p_ident = 0; // Initialize to 0

        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();

            string sqlQuery = "SELECT Ident FROM Player WHERE LoginName = @p_loginName AND Password = @p_password";

            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                cmd.Parameters.Add(new SqlParameter("@p_password", p_password));

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    p_ident = Convert.ToInt32(result); // Set output parameter
                }
            }
        }

        return p_ident;
    }

    internal bool SavePlayerVariables(string p_loginName, string p_name, char p_icon, string p_color)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("SavePlayerVariables", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                cmd.Parameters.Add(new SqlParameter("@p_name", p_name));
                cmd.Parameters.Add(new SqlParameter("@p_icon", p_icon));
                cmd.Parameters.Add(new SqlParameter("@p_color", p_color));

                SqlParameter resultParam = new SqlParameter("@p_result", SqlDbType.Bit);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resultParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                bool result = (bool)resultParam.Value;
                return result;
            }
        }
    }
    internal Player GetPlayerVariables(int p_ident)
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
    internal bool SavePlayerList(int p_playerId, int p_matchId)
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

                return true;
            }
        }
    }
    internal int GetPlayerIdentFromName(string p_name = null, string p_loginName = null)
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
                sqlQuery.Append(" AND Name = @p_name");
                parameters.Add(new SqlParameter("@p_name", p_name));
            }

            if (!string.IsNullOrEmpty(p_loginName))
            {
                sqlQuery.Append(" AND LoginName = @p_loginName");
                parameters.Add(new SqlParameter("@p_loginName", p_loginName));
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


    internal (int Wins, int Losses, int Draws) GetPlayerStats(int ident)
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

    internal bool CheckLoginName(string p_loginName)
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
}

