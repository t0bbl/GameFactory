//using System;
//using System.Security.AccessControl;
//using System.Text.RegularExpressions;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;


//namespace TicTacToe
//{
//    public class DatabaseHandler
//    {
//        private string connectionString;

//        public DatabaseHandler(string connectionString)
//        {
//            this.connectionString = connectionString;
//        }

//        public void Connect()
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                // Your code here: you can run SQL queries, etc.
//                connection.Close();
//            }
//        }

//        public void PrintChallengers()
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Challengers", connection))
//                {
//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            // Replace these with actual column names from your Challengers table
//                            string column1 = reader["Column1"].ToString();
//                            string column2 = reader["Column2"].ToString();

//                            Console.WriteLine($"Column1: {column1}, Column2: {column2}");
//                        }
//                    }
//                }

//                connection.Close();
//            }
//        }
//    }
//}