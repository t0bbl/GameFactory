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
    internal bool LoginPlayer(string p_loginName, string p_password)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("LoginPlayer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@LoginName", p_loginName));
                cmd.Parameters.Add(new SqlParameter("@Password", p_password));

                SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resultParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                bool result = (bool)resultParam.Value; 
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

