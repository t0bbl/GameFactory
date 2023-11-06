public class SQLDatabaseUtility
{
    public string GetSQLConnectionString()
    {
        string connString = "Server=ZUB-PC147\\SQLEXPRESS;Database=GameFactory;Trusted_Connection=True";
        //string connString = Environment.GetEnvironmentVariable("sql_Connection_String", EnvironmentVariableTarget.Machine);
        //Console.Write(connString);
        return connString;
    }

}