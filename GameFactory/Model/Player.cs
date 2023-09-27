using GameFactory.Model;
using System.Data;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;

namespace GameFactory
{
    internal class Player
    {
        internal string Name;
        internal char Icon;
        internal string Colour;
        internal bool IsHuman;
        internal int Ident;
        internal string LoginName;
        internal string Password;

        #region variables
        string p_loginName;
        string p_password;
        string p_name;
        char p_icon;
        string p_colour;
        #endregion


        internal bool PlayerSignup()
        {
            Console.Clear();
            do
            {
                do
                {
                    Console.WriteLine("Please enter the name you want to signup with:");
                    p_loginName = Console.ReadLine();
                } while (!ValidateLoginName(p_loginName));

            } while (!DataProvider.CheckLoginName(p_loginName));

            do
            {
                Console.WriteLine("Please enter your password:");
                p_password = Console.ReadLine();
            } while (!ValidatePassword(p_password));

            string p_passwordSave = HashPassword(p_password);
            if (SignUpPlayer(p_loginName, p_passwordSave))
            {
                Console.WriteLine("You have successfully signed up! Hit any key to continue");

                Console.ReadLine();
                SavePlayerVariables();

                Console.Clear();
                return false;
            }
            else
            {
                Console.WriteLine("An error occurred while signing you up.");
                Console.ReadLine();
                return false;
            }



        }

        internal int PlayerSignIn()
        {

            bool loggedIn = false;
            Console.Clear();
            Console.WriteLine("Please enter your login name:");
            p_loginName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            p_password = Console.ReadLine();
            string p_passwordSave = HashPassword(p_password);
            do
            {
                int p_ident = LoginPlayer(p_loginName, p_passwordSave);
                if (p_ident != 0)
                {
                    Console.WriteLine("You have successfully logged in! Hit any key to continue");
                    Console.ReadLine();
                    Console.Clear();
                    return p_ident;
                }
                else
                {
                    Console.WriteLine("An error occurred while you logged in.");
                    Console.WriteLine("You want to try (a)gain or sign(U)p?");
                    string loginAgainOrSignUp = Console.ReadKey().KeyChar.ToString();

                    if (loginAgainOrSignUp == "a")
                    {
                        return 0;
                    }
                    else if (loginAgainOrSignUp == "u")
                    {
                        PlayerSignup();
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("You have entered an invalid input. Please try again.");
                        return 0;
                    }
                }
            } while (!loggedIn);

        }

        #region SQL
        internal bool SignUpPlayer(string p_loginName, string p_password)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SignUpPlayer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                    cmd.Parameters.Add(new SqlParameter("@p_password", p_password));
                    cmd.Parameters.Add(new SqlParameter("@p_isHuman", 1));

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
        internal int LoginPlayer(string p_loginName, string p_password)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            int p_ident = 0; // Initialize to 0

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string sqlQuery = "SELECT Ident FROM Player WHERE LoginName = @p_loginName AND Password = @p_password";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                    cmd.Parameters.Add(new SqlParameter("@p_password", p_password));

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        p_ident = Convert.ToInt32(result); // Set output parameter
                    }
                }
            }

            return p_ident;
        }
        internal bool SavePlayerVariables(string p_loginName, string p_name, char p_icon, string p_color)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SavePlayerVariables", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                    cmd.Parameters.Add(new SqlParameter("@p_name", p_name));
                    cmd.Parameters.Add(new SqlParameter("@p_icon", p_icon));
                    cmd.Parameters.Add(new SqlParameter("@p_color", p_color));

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
        #endregion

        #region Utility Methods
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public bool ValidateLoginName(string loginName)
        {
            if (loginName.Length < 3 || loginName.Length > 16)
            {
                Console.WriteLine("Your login name must be between 3 and 16 characters long.");
                return false;
            }

            Regex alphanumericRegex = new Regex("^[a-zA-Z0-9]+$");
            if (!alphanumericRegex.IsMatch(loginName))
            {
                Console.WriteLine("Your login name must only contain alphanumeric characters.");
                return false;
            }

            return true;
        }
        public static bool ValidatePassword(string password)
        {
            if (password.Length < 8 || password.Length > 16)
            {
                Console.WriteLine("Your password must be between 8 and 16 characters long.");
                return false;
            }
            return true;
        }
        internal bool SavePlayerVariables()
        {
            Console.Clear();
            if (p_loginName == null)
            {
                p_loginName = "Guest";
            }
            p_name = Game.InitializePlayerName();
            p_icon = Game.InitializePlayerIcon();
            p_colour = Game.InitializePlayerColor();

            SavePlayerVariables(p_loginName, p_name, p_icon, p_colour);
            Console.Clear();
            return true;
        }

        #endregion
    }

}
