
using System.Data;
using System.Data.SqlClient;

namespace GameFactory.SQL
{
    internal class SQLMoveHistory
    {
        internal static bool SaveMoveHistory(int p_player, string p_input, int p_matchId)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveMoveHistory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@p_player", p_player));
                    cmd.Parameters.Add(new SqlParameter("@p_input", p_input));
                    cmd.Parameters.Add(new SqlParameter("@p_matchId", p_matchId));




                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
        }
    }
}
