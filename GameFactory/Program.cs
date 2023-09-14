using GameFactory;
using GameFactory.Model;

namespace GameFactory
{
    class Program
    {
        static void Main()
        {

            var gameMode = InitializeGameMenu();
            var players = InitializePlayer(gameMode);
            InitializeGame(players, gameMode);

        }
        static string InitializeGameMenu()
        {
            List<string> menuPoints = new(Enum.GetNames(typeof(StartMenuOptions)));
            string choosing = null;
            do
            {
                if (choosing == null)
                {
                    choosing = ShowMenu(menuPoints);
                }
                else
                {
                    switch (choosing)
                    {
                        case "Singleplayer":
                            return "SP";
                        case "Multiplayer":
                            return "MP";
                        case "Quit":
                            Environment.Exit(0);
                            return "quit";
                        default:
                            throw new Exception("Invalid Input.");
                    }
                }
            } while (true);
        }
        static string ShowMenu(List<string> p_menuItems)
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

            bool isValidNumber;
            int numberOfPlayers;
            if (p_gameMode == "SP")
            {
                numberOfPlayers = 1;
            }
            else
            {
                do
                {
                    Console.WriteLine("Enter the number of players: ");
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    isValidNumber = int.TryParse(keyInfo.KeyChar.ToString(), out numberOfPlayers);

                    if (!isValidNumber)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a number.");
                    }

                } while (!isValidNumber);
                Console.WriteLine();
            }

            for (int Gamer = 0; Gamer < numberOfPlayers; Gamer++)
            {
                Console.WriteLine($"\n Enter the name of player {Gamer + 1}: \n");
                string playerName = Console.ReadLine();
                Console.WriteLine($"\n Enter the icon of player {Gamer + 1}: \n");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                string playerIcon = keyInfo.KeyChar.ToString();
                Console.WriteLine($"\n Choose your Colour: \n");
                List<string> colours = new(Enum.GetNames(typeof(ValidColours)));
                string playerColour = ShowMenu(colours);
                Player newPlayer = new() { Name = playerName, Icon = playerIcon, Colour = playerColour };
                players.Add(newPlayer);
            }
            return players;

        }
        static void SinglePlayerGames(List<Player> Players, string game)
        {
            List<string> gameOptions = new(Enum.GetNames(typeof(SinglePlayerGames)));
            Player GPT = new() { Name = "chatGPT", Icon = "C", Colour = "green", IsHuman = false };
            Players.Add(GPT);
            while (true)
            {
                if (game == null)
                {
                    game = ShowMenu(gameOptions);
                }
                else
                {
                    switch (game)
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
        static void MultiPlayerGames(List<Player> Players, string game)
        {
            List<string> gameOptions = new(Enum.GetNames(typeof(MultiPlayerGames)));

            while (true)
            {
                if (game == null)
                {
                    game = ShowMenu(gameOptions);
                }
                else
                {
                    switch (game)
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
            } while (true);
        }
    }
}
