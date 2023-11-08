using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ClassLibrary
{
    public class Player
    {
        public string Name { get; set; }
        public char Icon { get; set; }
        internal string Colour { get; set; }
        internal bool IsHuman { get; set; }
        internal int Ident { get; set; }
        internal string LoginName { get; set; }
        internal string Password { get; set; }
        public int Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int PlayedGames { get; set; }
        public double WinPercentage { get; set; }



        #region variables
        string p_LoginName { get; set; }
        string p_Password { get; set; }
        string p_Name { get; set; }
        char p_Icon { get; set; }
        string p_Colour { get; set; }

        #endregion

        /// <summary>
        /// Guides the user through the sign-up process, validates their inputs, and saves their credentials and initial variables to the database.
        /// </summary>
        /// <returns>The identifier of the newly created player record, or 0 if the sign-up process fails.</returns>
        internal int PlayerSignup()
        {
            int p_Ident = 0;
            Console.Clear();

            do
            {
                do
                {
                    Console.WriteLine("Please enter the name you want to signup with:");
                    p_LoginName = Console.ReadLine();
                } while (!ValidateLoginName(p_LoginName));

            } while (!DataProvider.CheckLoginNameAvailability(p_LoginName));

            do
            {
                Console.WriteLine("Please enter your password:");
                p_Password = ReadPassword();
            } while (!ValidatePassword(p_Password));

            string p_passwordSave = HashPassword(p_Password);

            p_Ident = SQLSignUpPlayer(p_LoginName, p_passwordSave);

            if (p_Ident > 0)
            {
                Console.WriteLine("You have successfully signed up! Hit any key to continue");
                SetPlayerVariables(p_Ident);
                Console.Clear();
                return p_Ident;
            }
            else
            {
                Console.WriteLine("An error occurred while signing you up.");
                Console.ReadLine();
                return 0;
            }
        }
        /// <summary>
        /// Guides the user through the sign-in process, validates their inputs, and if necessary, redirects them to the sign-up process.
        /// </summary>
        /// <returns>The identifier of the authenticated player record, or 0 if the sign-in process fails.</returns>
        public int PlayerSignIn()
        {
            int p_Ident = 0;
            string p_LoginName;
            while (true)
            {
                Console.Clear();
                do
                {
                    Console.WriteLine("Please enter your login name:");
                    p_LoginName = Console.ReadLine();
                } while (!DataProvider.ValidateLoginName(p_LoginName));
                Console.WriteLine("Please enter your password:");
                string p_Password = ReadPassword();
                string p_PasswordSave = HashPassword(p_Password);

                p_Ident = SQLLoginPlayer(p_LoginName, p_PasswordSave);

                if (p_Ident != 0)
                {
                    Console.WriteLine("You have successfully logged in! Hit any key to continue");
                    Console.ReadLine();
                    Console.Clear();
                    return p_Ident;
                }
                else
                {
                    Console.WriteLine("An error occurred while you logged in.");
                    Console.WriteLine("You want to try (a)gain or sign(U)p?");
                    string loginAgainOrSignUp = Console.ReadKey().KeyChar.ToString().ToLower();

                    if (loginAgainOrSignUp == "a")
                    {
                        continue;
                    }
                    else if (loginAgainOrSignUp == "u")
                    {
                        p_Ident = PlayerSignup();
                        return p_Ident;
                    }
                    else
                    {
                        Console.WriteLine("You have entered an invalid input. Please try again.");
                        continue;
                    }
                }
            }
        }
        /// <summary>
        /// Prompts the user for a name or login name, retrieves and displays the corresponding player statistics.
        /// </summary>
        public static void ShowPlayerStats()
        {
            Console.Clear();
            Console.WriteLine("Input a Name or LoginName to check their PlayerStats");
            string p_Input = Console.ReadLine();
            List<int> p_PlayerIdent = DataProvider.GetPlayerIdentsFromName(p_Input);
            foreach (var player in p_PlayerIdent)
            {
                DataProvider.DisplayPlayerStats(player, true);
            }


            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

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
        internal bool SQLSavePlayerVariables(int p_Ident, string p_Name, char p_Icon, string p_Color)
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
        /// <summary>
        /// Initializes and sets the player variables by prompting the user and saves them to the database.
        /// </summary>
        /// <param name="p_Ident">The identifier of the player.</param>
        /// <returns>True upon successful execution.</returns>
        public bool SetPlayerVariables(int p_Ident)
        {
            Console.Clear();
            p_Name = Game.InitializePlayerName();
            p_Icon = Game.InitializePlayerIcon();
            p_Colour = Game.InitializePlayerColor();

            SQLSavePlayerVariables(p_Ident, p_Name, p_Icon, p_Colour);
            Console.Clear();
            return true;
        }
        /// <summary>
        /// Reads a password from the console input, masking each character with an asterisk for privacy.
        /// </summary>
        /// <returns>The password as a string.</returns>
        static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return password.ToString();
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b"); 
                    }
                }
                else
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
        }

        #endregion
    }

}

