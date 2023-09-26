using GameFactory;
using System.Data;
using System.Data.SqlClient;

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

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("LoginPlayer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                cmd.Parameters.Add(new SqlParameter("@p_password", p_password));

                SqlParameter resultParam = new SqlParameter("@p_ident", SqlDbType.Int);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resultParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                int result = (int)resultParam.Value;
                return result;
            }
        }
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

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("GetPlayerVariables", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter nameParam = new SqlParameter("@p_name", SqlDbType.NVarChar, 50);
                SqlParameter iconParam = new SqlParameter("@p_icon", SqlDbType.NVarChar, 1);
                SqlParameter colorParam = new SqlParameter("@p_color", SqlDbType.NVarChar, 20);
                SqlParameter identParam = new SqlParameter("@p_ident", SqlDbType.Int);



                nameParam.Direction = ParameterDirection.Output;
                iconParam.Direction = ParameterDirection.Output;
                colorParam.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(nameParam);
                cmd.Parameters.Add(iconParam);
                cmd.Parameters.Add(colorParam);
                cmd.Parameters.Add(new SqlParameter("@p_ident", p_ident));  // Add this line


                conn.Open();
                cmd.ExecuteNonQuery();

                string p_name = nameParam.Value as string;
                char p_icon = Convert.ToChar(iconParam.Value);
                string p_color = colorParam.Value as string;

                Console.Write($"{p_name}, {p_icon}, {p_color}");

                if (p_name != null && p_icon != null && p_color != null)
                {
                    return new Player
                    {
                        Name = p_name,
                        Icon = p_icon,
                        Colour = p_color,
                        Ident = p_ident
                    };
                }
                else
                {
                    return null;
                }
            }
        }
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

    public (int, int, int) GetPlayerStats(int ident)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("GetPlayerStats", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Input parameter
                cmd.Parameters.Add(new SqlParameter("@p_ident", ident));

                // Output parameters
                SqlParameter winsParam = new SqlParameter("@p_wins", SqlDbType.Int);
                SqlParameter lossesParam = new SqlParameter("@p_losses", SqlDbType.Int);
                SqlParameter drawsParam = new SqlParameter("@p_draws", SqlDbType.Int);

                winsParam.Direction = ParameterDirection.Output;
                lossesParam.Direction = ParameterDirection.Output;
                drawsParam.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(winsParam);
                cmd.Parameters.Add(lossesParam);
                cmd.Parameters.Add(drawsParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                int Wins = (int)winsParam.Value;
                int Losses = (int)lossesParam.Value;
                int Draws = (int)drawsParam.Value;

                return (Wins, Losses, Draws);
            }
        }
    }
    internal bool CheckLoginName(string p_loginName)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("CheckLoginName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));

                SqlParameter resultParam = new SqlParameter("@p_result", SqlDbType.Bit);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resultParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                bool result = (bool)resultParam.Value;
                if (!result)
                {
                    Console.WriteLine("This login name is already taken. Please try again.");
                }
                return result;
            }
        }

    }
}

