﻿using GameFactory;
using GameFactory.Model;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace GameFactory.SQL
{
    internal static class DataProvider
    {
        internal static List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)> GetPlayerStats(int p_ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)> statsList = new List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlQuery = "SELECT * FROM dbo.GetPlayerStats(@p_ident)";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_ident", p_ident));

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var stats = (Wins: reader.GetInt32(reader.GetOrdinal("Wins")),
                                           Losses: reader.GetInt32(reader.GetOrdinal("Losses")),
                                           Draws: reader.GetInt32(reader.GetOrdinal("Draws")),
                                           TotalGames: reader.GetInt32(reader.GetOrdinal("TotalGames")),
                                           WinPercentage: (float)reader.GetDouble(reader.GetOrdinal("WinPercentage")));
                                statsList.Add(stats);
                            }
                        }
                    }
                }
            }
            return statsList;
        }
        internal static int GetPlayerIdentFromName(string p_name = null)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            int p_ident = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                StringBuilder sqlQuery = new StringBuilder("SELECT Ident FROM Player WHERE 1=1");
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(p_name))
                {
                    sqlQuery.Append(" AND (Name = @p_name OR LoginName = @p_name)");
                    parameters.Add(new SqlParameter("@p_name", p_name));
                }

                using (SqlCommand cmd = new SqlCommand(sqlQuery.ToString(), conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        p_ident = Convert.ToInt32(result);
                    }
                }
            }
            return p_ident;
        }
        internal static bool CheckLoginName(string p_loginName)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            bool p_result = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT 1 FROM [Player] WHERE LoginName = @p_loginName";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));

                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        p_result = true;
                    }
                    else
                    {
                        Console.WriteLine("LoginName already exists. Try again.");
                    }
                }
            }

            return p_result;
        }
        internal static Player GetPlayerVariables(int p_ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            Player player = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT Name, Icon, Color FROM Player WHERE Ident = @p_ident";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_ident", p_ident));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string p_name = reader["Name"] as string;
                            char p_icon = Convert.ToChar(reader["Icon"]);
                            string p_color = reader["Color"] as string;

                            Console.Write($"{p_name}, {p_icon}, {p_color}");

                            if (p_name != null && p_icon != null && p_color != null)
                            {
                                player = new Player
                                {
                                    Name = p_name,
                                    Icon = p_icon,
                                    Colour = p_color,
                                    Ident = p_ident
                                };
                            }
                        }
                    }
                }
            }

            return player;
        }
    }
}