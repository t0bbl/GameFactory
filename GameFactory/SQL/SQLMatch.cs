
using System.Data;

using System.Data.SqlClient;

namespace GameFactory.SQL
{
    internal class SQLMatch
    {

        internal static bool SaveMatch(string p_winner, string p_loser, bool p_draw, int p_gameType)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMatch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    Console.WriteLine($"{p_winner} winner, {p_loser} loser, {p_draw} draw, {p_gameType} gametype");
                    Console.ReadLine();
                    cmd.Parameters.Add(new SqlParameter("@Loser", p_loser));
                    cmd.Parameters.Add(new SqlParameter("@Winner", p_winner));
                    cmd.Parameters.Add(new SqlParameter("@Draw", p_draw));
                    cmd.Parameters.Add(new SqlParameter("@GameType", p_gameType));


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
    }
}
