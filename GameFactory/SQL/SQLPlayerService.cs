using GameFactory;
using System.Data;
using System.Data.SqlClient;
using System.Text;

internal class SQLPlayerService
{
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
  
}

