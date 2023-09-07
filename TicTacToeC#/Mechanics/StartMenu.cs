namespace TicTacToe 
{
    public class StartMenu
    {
        public enum StartMenuOptions
        {
            NewGame = 1,
            Quit = 2
        }

        public static void InitializeGameMenu()
        {
            List<string> menuPoints = new List<string>(Enum.GetNames(typeof(StartMenuOptions)));
            string choosing = null;

            while (true)
            {
                if (choosing == null)
                {
                    choosing = Menu.ShowMenu(menuPoints);
                }
                else
                {
                    switch (choosing)
                    {
                        case "NewGame":
                            return;
                        case "Quit":
                            Environment.Exit(0);
                            return;
                        default:
                            throw new Exception("Invalid Input.");
                    }
                }
            }
        }
    }
}
