
using System.Data;

using System.Data.SqlClient;

namespace GameFactory.SQL
{
    internal class SQLMatch
    {

        internal static bool SaveMatch(int p_winner, int p_loser, int p_draw, int p_gameType)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMatch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@p_loser", p_loser));
                    cmd.Parameters.Add(new SqlParameter("@p_winner", p_winner));
                    cmd.Parameters.Add(new SqlParameter("@p_draw", p_draw));
                    cmd.Parameters.Add(new SqlParameter("@p_gameType", p_gameType));


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
    }
}
