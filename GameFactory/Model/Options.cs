

namespace GameFactory.Model
{
    internal class Options
    {
        public bool StartOptions()
        {
            Console.Clear();
            List<string> menuPoints = new(Enum.GetNames(typeof(OptionsMenu)));
            string p_choosing = null;
            bool p_exitMenu = false;
            do
            {
                if (p_choosing == null)
                {
                    p_choosing = Program.ShowMenu(menuPoints);
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
    }
}
