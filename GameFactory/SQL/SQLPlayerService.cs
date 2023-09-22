using System.Data;
using System.Data.SqlClient;

public class SQLPlayerService
{
    private readonly SqlConnection conn = new SQLDatabaseUtility().GetConnection();
    public bool SignUpPlayer(string loginName, string password)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("SignUpPlayer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@LoginName", loginName));
                cmd.Parameters.Add(new SqlParameter("@Password", password));
                cmd.Parameters.Add(new SqlParameter("@IsHuman", 1));

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

    public bool LoginPlayer(string loginName, string password)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("LoginPlayer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@LoginName", loginName));
                cmd.Parameters.Add(new SqlParameter("@Password", password));

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

    public bool CheckLoginName(string loginName)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("CheckLoginName", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@LoginName", loginName));

                SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit);
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

