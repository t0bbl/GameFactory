

namespace GameFactory.Model
{
    internal class Options
    {
        public bool StartOptions()
        {
            Console.Clear();
            string p_choosing = null;
            bool p_exitMenu = false;
            do
            {
                if (p_choosing == null)
                {
                    p_choosing = Game.ShowMenu(typeof(OptionsMenu));
                }
                else
                {
                    switch (p_choosing)
                    {
                        case "NightMode":
                            Console.BackgroundColor = (Console.BackgroundColor == ConsoleColor.Black) ? ConsoleColor.White : ConsoleColor.Black;
                            Console.ForegroundColor = (Console.ForegroundColor == ConsoleColor.Black) ? ConsoleColor.White : ConsoleColor.Black;
                            Console.Clear();
                            p_choosing = null;
                            break;
                        case "Back":
                            Console.Clear();
                            return false;
                        default:
                            throw new Exception("Invalid Input.");
                    }
                }
            } while (!p_exitMenu);
            return false;
        }
        public bool PlayerOptions()
        {
            Console.Clear();
            string p_choosing = null;
            bool p_exitMenu = false;
            PlayerAuth PlayerAuth = new();
            do
            {
                if (p_choosing == null)
                {
                    p_choosing = Game.ShowMenu(typeof(PlayerOptionsMenu));
                }
                else
                {
                    switch (p_choosing)
                    {
                        case "LogIn":
                            PlayerAuth.PlayerSignIn();
                            break;
                        case "SignUp":
                            PlayerAuth.PlayerSignup();
                            return false;
                        case "Back":
                            Console.Clear();
                            return false;

                        default:
                            throw new Exception("Invalid Input.");
                    }
                }
            } while (!p_exitMenu);
            return false;
        }
    }
}
