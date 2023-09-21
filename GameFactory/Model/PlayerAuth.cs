using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using System.Threading.Tasks;

namespace GameFactory.Model
{
    internal class PlayerAuth
    {
        SQLPlayerService PlayerService = new();
        string loginName;
        string password;

        internal bool PlayerSignup()
        {
            Console.Clear();
            Console.WriteLine("Please enter the name you want to signup with:");
            string loginName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            string p_password = HashPassword(password);
            if (PlayerService.SignUpPlayer(loginName, p_password))
            {
                Console.WriteLine("You have successfully signed up!");
                return true;
            }
            else
            {
                Console.WriteLine("An error occurred while signing you up.");
                return false;
            }


        }

        internal bool PlayerSignIn()
        {
            Console.Clear();
            Console.WriteLine("Please enter your login name:");
            loginName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            password = Console.ReadLine();
            string p_password = HashPassword(password);
            if (PlayerService.LoginPlayer(loginName, p_password))
            {
                Console.WriteLine("You have successfully logged in!");
                Console.ReadLine();
                return true;
            }
            else
            {
                Console.WriteLine("An error occurred while you logged in.");
                Console.ReadLine();

                return false;
            }

        }

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
    }


}
