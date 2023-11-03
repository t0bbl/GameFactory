namespace ClassLibrary
{
    public class Options
    {
        /// <summary>
        /// Provides a menu for the user to select various game options including toggling night mode or exiting the menu.
        /// </summary>
        /// <returns>False when the user exits the options menu.</returns>
        public static bool GameOptions()
        {
            Console.Clear();
            string p_Choosing = null;
            bool p_ExitMenu = false;
            do
            {
                if (p_Choosing == null)
                {
                    p_Choosing = Game.ShowMenu(typeof(OptionsMenu));
                }
                else
                {
                    switch (p_Choosing)
                    {
                        case "NightMode":
                            Console.BackgroundColor = (Console.BackgroundColor == ConsoleColor.Black) ? ConsoleColor.White : ConsoleColor.Black;
                            Console.ForegroundColor = (Console.ForegroundColor == ConsoleColor.Black) ? ConsoleColor.White : ConsoleColor.Black;
                            Console.Clear();
                            p_Choosing = null;
                            break;
                        case "Back":
                            Console.Clear();
                            return false;
                        default:
                            throw new Exception("Invalid Input.");
                    }
                }
            } while (!p_ExitMenu);
            return false;
        }
    }
}
