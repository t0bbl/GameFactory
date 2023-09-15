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
                        continue;
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
            Console.WriteLine("");
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

            int p_numberOfPlayers = DetermineNumberOfPlayers(p_gameMode);

            for (int p_gamer = 0; p_gamer < p_numberOfPlayers; p_gamer++)
            {
                Console.Clear();

                Player newPlayer = new Player();
                InitializePlayerName(newPlayer, p_gamer);
                InitializePlayerIcon(newPlayer, p_gamer);
                InitializePlayerColor(newPlayer, p_gamer);

                players.Add(newPlayer);
            }
            return players;
        }

        static int DetermineNumberOfPlayers(string p_gameMode)
        {
            if (p_gameMode == "SP") return 1;

            int p_numberOfPlayers;
            do
            {
                Console.WriteLine("Enter the number of players: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out p_numberOfPlayers) && p_numberOfPlayers > 0)
                {
                    return p_numberOfPlayers;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            } while (true);
        }

        static void InitializePlayerName(Player p_player, int p_gamerIndex)
        {
            do
            {
                Console.WriteLine($"\n Enter the name of player {p_gamerIndex + 1}: \n");
                p_player.Name = Console.ReadLine();
                if (string.IsNullOrEmpty(p_player.Name))
                {
                    Console.WriteLine("Error: Name cannot be empty.");
                }
            } while (string.IsNullOrEmpty(p_player.Name));
        }

        static void InitializePlayerIcon(Player p_player, int p_gamerIndex)
        {
            do
            {
                Console.WriteLine($"\n Enter the icon of player {p_gamerIndex + 1}: \n");
                p_player.Icon = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                if (string.IsNullOrEmpty(p_player.Icon) || p_player.Icon == "\r" || p_player.Icon == " ")
                {
                    Console.WriteLine("Error: Icon cannot be empty or whitespace.");
                }
            } while (string.IsNullOrEmpty(p_player.Icon) || p_player.Icon == "\r" || p_player.Icon == " ");
        }

        static void InitializePlayerColor(Player p_player, int p_gamerIndex)
        {
            Console.WriteLine($"\n Choose a Colour for player {p_gamerIndex + 1}: ");
            List<string> colours = new(Enum.GetNames(typeof(ValidColours)));
            p_player.Colour = ShowMenu(colours);
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
