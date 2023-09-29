namespace GameFactory.Model
{
    internal class Options
    {
        public static bool GameOptions()
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
    }
}
