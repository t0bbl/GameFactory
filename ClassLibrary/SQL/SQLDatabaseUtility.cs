public class SQLDatabaseUtility
{
    public string GetSQLConnectionString()
    {
        string connString = Environment.GetEnvironmentVariable("sql_Connection_String", EnvironmentVariableTarget.Machine);
        return connString;
    }

}