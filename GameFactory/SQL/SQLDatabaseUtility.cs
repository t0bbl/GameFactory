

using System.Data.SqlClient;

public class SQLDatabaseUtility
{

    public SqlConnection GetConnection()
    {
        string connString = GetSQLConnectionString();
        return new SqlConnection(connString);
    }

    public string GetSQLConnectionString()
    {
        string connString = Environment.GetEnvironmentVariable("sql_Connection_String", EnvironmentVariableTarget.Machine);
        return connString;
    }
}