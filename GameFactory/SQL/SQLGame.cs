using System;
using System.Data;
using System.Data.SqlClient;

namespace GameFactory.SQL
{
    internal class SQLGame
    {
        private readonly SqlConnection conn = new SQLDatabaseUtility().GetConnection();

        internal static (bool Result, int Ident) SaveGame(int p_rows, int p_columns, int p_winningLength, string p_gameType)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveGame", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_rows", p_rows));
                    cmd.Parameters.Add(new SqlParameter("@p_cols", p_columns));
                    cmd.Parameters.Add(new SqlParameter("@p_winningLength", p_winningLength));
                    cmd.Parameters.Add(new SqlParameter("@p_gameType", p_gameType));

                    SqlParameter resultParam = new SqlParameter("@p_Result", SqlDbType.Bit);
                    resultParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultParam);

                    SqlParameter identParam = new SqlParameter("@p_Ident", SqlDbType.Int);
                    identParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(identParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    bool result = (bool)resultParam.Value;

                    int ident = (int)identParam.Value;

                    return (result, ident);
                }
            }
        }

    }
}