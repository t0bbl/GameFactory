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


        internal int PlayerSignup()
        {
            int p_ident = 0;
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

            p_ident = SQLSignUpPlayer(p_loginName, p_passwordSave, out p_ident);

            if (p_ident > 0)
            {
                Console.WriteLine("You have successfully signed up! Hit any key to continue");
                Console.ReadLine();
                Console.Clear();
                return p_ident;
            }
            else
            {
                Console.WriteLine("An error occurred while signing you up.");
                Console.ReadLine();
                return 0;
            }
        }
        internal int PlayerSignIn()
        {
            int p_ident = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please enter your login name:");
                string p_loginName = Console.ReadLine();
                Console.WriteLine("Please enter your password:");
                string p_password = Console.ReadLine();
                string p_passwordSave = HashPassword(p_password);

                p_ident = SQLLoginPlayer(p_loginName, p_passwordSave);

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
                    string loginAgainOrSignUp = Console.ReadKey().KeyChar.ToString().ToLower();

                    if (loginAgainOrSignUp == "a")
                    {
                        continue;
                    }
                    else if (loginAgainOrSignUp == "u")
                    {
                        p_ident = PlayerSignup();
                        return p_ident;
                    }
                    else
                    {
                        Console.WriteLine("You have entered an invalid input. Please try again.");
                        continue;
                    }
                }
            }
        }


        public static void ShowPlayerStats()
        {
            Console.Clear();
            Console.WriteLine("Input a Name or LoginName to check their PlayerStats");
            string p_input = Console.ReadLine();
            int p_playerIdent = DataProvider.GetPlayerIdentFromName(p_input);
            List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)> statsList = DataProvider.GetPlayerStats(p_playerIdent);
            foreach (var stats in statsList)
            {
                Console.WriteLine($"Wins: {stats.Wins}, Losses: {stats.Losses}, Draws: {stats.Draws}, Total Games: {stats.TotalGames}, Win Percentage: {stats.WinPercentage}");
            }
            //DataProvider.DisplayPlayerStats(p_playerIdent);
            //float winpercentage = DataProvider.GetPlayerWinPercentage(p_playerIdent);
            //Console.WriteLine($"Win Percentage: {winpercentage}");

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        #region SQL
        internal int SQLSignUpPlayer(string p_loginName, string p_password, out int p_ident)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();
            p_ident = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SignUpPlayer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_loginName", p_loginName));
                    cmd.Parameters.Add(new SqlParameter("@p_password", p_password));
                    cmd.Parameters.Add(new SqlParameter("@p_isHuman", 1));

                    SqlParameter identParam = new SqlParameter("@p_ident", SqlDbType.Int);
                    identParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(identParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    p_ident = (int)identParam.Value;

                    return p_ident;
                }
            }
        }
        internal int SQLLoginPlayer(string p_loginName, string p_password)
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
        internal bool SQLSavePlayerVariables(int p_ident, string p_name, char p_icon, string p_color)
        {
            string connString = new SQLDatabaseUtility().GetSQLConnectionString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SavePlayerVariables", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@p_ident", p_ident));
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
        internal bool GetPlayerVariables(int p_ident)
        {
            Console.Clear();
            p_name = Game.InitializePlayerName();
            p_icon = Game.InitializePlayerIcon();
            p_colour = Game.InitializePlayerColor();

            SQLSavePlayerVariables(p_ident, p_name, p_icon, p_colour);
            Console.Clear();
            return true;
        }

        #endregion
    }

}
