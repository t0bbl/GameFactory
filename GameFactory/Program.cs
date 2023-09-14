using GameFactory.Model;

namespace GameFactory
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                var gameMode = InitializeGameMenu();
                if (gameMode == "Options")
                {
                    var options = new Options();
                    if (!options.StartOptions())
                    {
                        continue; // If StartOptions returns false, go back to InitializeGameMenu
                    }
                }
                else
                {
                    var players = InitializePlayer(gameMode);
                    InitializeGame(players, gameMode);
                }
            }
        }
        static string InitializeGameMenu()
        {
            List<string> menuPoints = new(Enum.GetNames(typeof(StartMenuOptions)));
            string p_choosing = null;
            do
            {
                if (p_choosing == null)
                {
                    p_choosing = ShowMenu(menuPoints);
                }
                else
                {
                    switch (p_choosing)
                    {
                        case "SinglePlayer":
                            return "SP";
                        case "MultiPlayer":
                            return "MP";
                        case "Options":
                            return "Options";
                        case "Quit":
                            Environment.Exit(0);
                            return "quit";
                        default:
                            throw new Exception("Invalid Input.");
                    }
                }
            } while (true);
        }
        internal static string ShowMenu(List<string> p_menuItems)
        {
            Console.WriteLine("Please make a Choice:");
            p_menuItems.ForEach(CurrentItem => Console.WriteLine($"{p_menuItems.IndexOf(CurrentItem) + 1}. {CurrentItem}"));
            do
            {
                Console.Write("Enter the number of your choice: ");
                string input = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();


                if (int.TryParse(input, out int choice))
                {
                    if (choice >= 1 && choice <= p_menuItems.Count)
                    {
                        return p_menuItems[choice - 1];
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice, try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            } while (true);
        }
        static List<Player> InitializePlayer(string p_gameMode)
        {
            Console.Clear();
            var players = new List<Player>();

            bool p_isValidNumber;
            int p_numberOfPlayers;
            if (p_gameMode == "SP")
            {
                p_numberOfPlayers = 1;
            }
            else
            {
                do
                {
                    Console.WriteLine("Enter the number of players: ");
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    p_isValidNumber = int.TryParse(keyInfo.KeyChar.ToString(), out p_numberOfPlayers);

                    if (!p_isValidNumber)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a number.");
                    }

                } while (!p_isValidNumber);
                Console.WriteLine();
            }

            for (int p_gamer = 0; p_gamer < p_numberOfPlayers; p_gamer++)
            {
                Console.Clear();
                string playerName;
                string playerIcon;
                do
                {
                    Console.WriteLine($"\n Enter the name of player {p_gamer + 1}: \n");
                    playerName = Console.ReadLine();
                    if (string.IsNullOrEmpty(playerName))
                    {
                        Console.WriteLine("Error: Name cannot be empty.");
                    }
                } while (string.IsNullOrEmpty(playerName));
                do
                {
                    Console.WriteLine($"\n Enter the icon of player {p_gamer + 1}: \n");
                    ConsoleKeyInfo icon = Console.ReadKey();
                    Console.WriteLine();
                    playerIcon = icon.KeyChar.ToString();

                    if (string.IsNullOrWhiteSpace(playerIcon) || playerIcon == "\r" || playerIcon == " ")
                    {
                        Console.WriteLine("Error: Icon cannot be empty or whitespace.");
                        playerIcon = "";
                    }
                } while (string.IsNullOrEmpty(playerIcon) || playerIcon == "\r" || playerIcon == " ");
                Console.WriteLine($"\n Choose your Colour: \n");
                List<string> colours = new(Enum.GetNames(typeof(ValidColours)));
                string playerColour = ShowMenu(colours);
                Player newPlayer = new() { Name = playerName, Icon = playerIcon, Colour = playerColour };
                players.Add(newPlayer);
            }
            return players;

        }
        static void SinglePlayerGames(List<Player> Players, string p_game)
        {
            List<string> gameOptions = new(Enum.GetNames(typeof(SinglePlayerGames)));
            Player GPT = new() { Name = "ChatGPT", Icon = "C", Colour = "Green", IsHuman = false };
            Players.Add(GPT);
            while (true)
            {
                if (p_game == null)
                {
                    p_game = ShowMenu(gameOptions);
                }
                else
                {
                    switch (p_game)
                    {
                        case "TTTChatGPT":
                            Console.Clear();
                            var tttChatGPTGame = new TTTChatGPT();
                            tttChatGPTGame.StartMatch(Players);
                            break;
                        case "FourWChatGPT":
                            Console.Clear();
                            var fourWChatGPTGame = new FourWChatGPT();
                            fourWChatGPTGame.StartMatch(Players);
                            break;

                        default:
                            throw new Exception("Invalid game type.");
                    }
                }
            }
        }
        static void MultiPlayerGames(List<Player> Players, string p_game)
        {
            List<string> gameOptions = new(Enum.GetNames(typeof(MultiPlayerGames)));

            while (true)
            {
                if (p_game == null)
                {
                    p_game = ShowMenu(gameOptions);
                }
                else
                {
                    switch (p_game)
                    {
                        case "TTT":
                            Console.Clear();
                            var tttGame = new TTT();
                            tttGame.StartMatch(Players);
                            break;
                        case "FourW":
                            Console.Clear();
                            var fourWGame = new FourW();
                            fourWGame.StartMatch(Players);
                            break;
                        case "CustomTTT":
                            Console.Clear();
                            var costumTTTGame = new CustomTTT();
                            costumTTTGame.StartMatch(Players);
                            break;
                        default:
                            throw new Exception("Invalid game type.");

                    }

                }
            }
        }
        static void InitializeGame(List<Player> Players, string p_gameMode)
        {
            Console.Clear();

            string game = null;

            do
            {
                if (p_gameMode == "SP")
                {
                    SinglePlayerGames(Players, game);
                }
                else if (p_gameMode == "MP")
                {
                    MultiPlayerGames(Players, game);
                }
            } while (game == null);
        }
    }
}
