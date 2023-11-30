using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary
{
    public class Player
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int Ident { get; set; }
        internal string LoginName { get; set; }
        internal string Password { get; set; }
        public int Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int PlayedGames { get; set; }
        public double WinPercentage { get; set; }


        #region SQL
        /// <summary>
        /// Signs up a new player by inserting their credentials into the database.
        /// </summary>
        /// <param name="p_LoginName">The login name of the player.</param>
        /// <param name="p_Password">The password of the player.</param>
        /// <returns>The identifier of the newly created player record.</returns>
        public int SQLSignUpPlayer(string p_LoginName, string p_Password)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SignUpPlayer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_LoginName", p_LoginName);
                    cmd.Parameters.AddWithValue("@p_Password", p_Password);
                    cmd.Parameters.AddWithValue("@p_IsHuman", 1);
                    cmd.Parameters.AddWithValue("@p_Icon", "X");
                    cmd.Parameters.AddWithValue("@p_Color", "Gray");

                    SqlParameter identParam = new SqlParameter("@p_Ident", SqlDbType.Int);
                    identParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(identParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int p_Ident = (int)identParam.Value;

                    return p_Ident;
                }
            }
        }
        /// <summary>
        /// Logs in a player by verifying their credentials against the database.
        /// </summary>
        /// <param name="p_LoginName">The login name of the player.</param>
        /// <param name="p_Password">The password of the player.</param>
        /// <returns>The identifier of the player if the login is successful, 0 otherwise.</returns>
        public static int SQLLoginPlayer(string p_LoginName, string p_Password)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            int p_ident = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT Ident FROM Player WHERE LoginName = @p_LoginName AND Password = @p_Password";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@p_LoginName", p_LoginName);
                    cmd.Parameters.AddWithValue("@p_Password", p_Password);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        p_ident = Convert.ToInt32(result);
                    }
                }
            }

            return p_ident;
        }
        /// <summary>
        /// Saves the player variables to the database using a stored procedure.
        /// </summary>
        /// <param name="p_Ident">The identifier of the player.</param>
        /// <param name="p_Name">The name of the player.</param>
        /// <param name="p_Icon">The icon of the player represented as a character.</param>
        /// <param name="p_Color">The color preference of the player represented as a string.</param>
        /// <returns>True if the operation succeeded, false otherwise.</returns>
        public bool SQLSavePlayerVariables(int p_Ident, string p_Name, string p_Icon, string p_Color)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SavePlayerVariables", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Ident", p_Ident);
                    cmd.Parameters.AddWithValue("@p_Name", p_Name);
                    cmd.Parameters.AddWithValue("@p_Icon", p_Icon);
                    cmd.Parameters.AddWithValue("@p_Color", p_Color);

                    SqlParameter resultParam = new SqlParameter("@p_Result", SqlDbType.Bit);
                    resultParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    bool result = (bool)resultParam.Value;
                    return result;
                }
            }
        }
        #endregion
        #region Utility Methods
        /// <summary>
        /// Hashes a password using the SHA-256 algorithm.
        /// </summary>
        /// <param name="p_Password">The password to be hashed.</param>
        /// <returns>The hashed password as a hexadecimal string.</returns>
        public static string HashPassword(string p_Password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(p_Password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        /// <summary>
        /// Validates a login name based on length and character constraints.
        /// </summary>
        /// <param name="p_LoginName">The login name to be validated.</param>
        /// <returns>True if the login name is valid, false otherwise.</returns>
        public static bool ValidateLoginName(string p_LoginName)
        {
            if (p_LoginName.Length < 3 || p_LoginName.Length > 16)
            {
                Console.WriteLine("Your login name must be between 3 and 16 characters long.");
                return false;
            }

            Regex alphanumericRegex = new Regex("^[a-zA-Z0-9]+$");
            if (!alphanumericRegex.IsMatch(p_LoginName))
            {
                Console.WriteLine("Your login name must only contain alphanumeric characters.");
                return false;
            }

            return true;
        }
        /// <summary>
        /// Validates a password based on length constraints.
        /// </summary>
        /// <param name="p_Password">The password to be validated.</param>
        /// <returns>True if the password is valid, false otherwise.</returns>
        public static bool ValidatePassword(string p_Password)
        {
            if (p_Password.Length < 8 || p_Password.Length > 16)
            {
                Console.WriteLine("Your password must be between 8 and 16 characters long.");
                return false;
            }
            return true;
        }
        #endregion
        #region EventArgs
        public class PlayerChangedEventArgs
        {
            public string CurrentPlayer { get; private set; }


            public PlayerChangedEventArgs(string p_CurrentPlayer)
            {
                CurrentPlayer = p_CurrentPlayer;
            }
        }
        #endregion
    }

}

