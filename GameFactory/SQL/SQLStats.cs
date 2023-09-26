using GameFactory;
using System.Data;
using System.Data.SqlClient;
using System.Text;



internal class SQLStats
{
    internal (int Wins, int Losses, int Draws) GetPlayerStats(int ident)
    {
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();
        int wins = 0, losses = 0, draws = 0;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Match WHERE Winner = @p_ident", conn))
            {
                cmd.Parameters.AddWithValue("@p_ident", ident);
                wins = (int)cmd.ExecuteScalar();
            }

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Match WHERE Loser = @p_ident", conn))
            {
                cmd.Parameters.AddWithValue("@p_ident", ident);
                losses = (int)cmd.ExecuteScalar();
            }

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Match WHERE Draw = 1 AND (Winner = @p_ident OR Loser = @p_ident)", conn))
            {
                cmd.Parameters.AddWithValue("@p_ident", ident);
                draws = (int)cmd.ExecuteScalar();
            }
        }

        return (Wins: wins, Losses: losses, Draws: draws);
    }
    internal int GetPlayerWinPercentage(int wins, int losses, int draws)
    {
        int winperc = 0;
        string connString = new SQLDatabaseUtility().GetSQLConnectionString();
        string sql = "SELECT dbo.playerWinPerc(@wins, @losses, @draws)";


        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@wins", SqlDbType.Int);
                cmd.Parameters.Add("@losses", SqlDbType.Int);
                cmd.Parameters.Add("@draws", SqlDbType.Int);

                cmd.Parameters["@wins"].Value = wins;
                cmd.Parameters["@losses"].Value = losses;
                cmd.Parameters["@draws"].Value = draws;

                winperc = (int)cmd.ExecuteScalar();
                
            }
        }
        return winperc;
    }


}

