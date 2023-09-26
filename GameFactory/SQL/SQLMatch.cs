using System.Data;
using System.Data.SqlClient;

namespace GameFactory.SQL
{
    internal class SQLMatch
    {
        internal static int SaveMatch(int? p_winner, int? p_loser, int p_draw, int p_gameType, int p_matchId)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMatch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_winner", p_winner));
                    cmd.Parameters.Add(new SqlParameter("@p_loser", p_loser));
                    cmd.Parameters.Add(new SqlParameter("@p_draw", p_draw));
                    cmd.Parameters.Add(new SqlParameter("@p_gameType", p_gameType));

                    SqlParameter resultParam = new SqlParameter("@p_matchId", SqlDbType.Int);
                    resultParam.Direction = ParameterDirection.InputOutput;
                    resultParam.Value = p_matchId;
                    cmd.Parameters.Add(resultParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    p_matchId = (int)resultParam.Value;
                    return p_matchId;
                }
            }
        }
    }
}
