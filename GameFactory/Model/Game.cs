
using System.Collections.Generic;

namespace GameFactory.Model
{
    internal class Game
    {
        public List<Player> p_player { get; set; } = new List<Player>();
        internal string p_gameType { get; set; }
        internal string p_gameMode { get; set; }

        public Match CreateMatch()
        {
            if (p_gameMode == "SP")
            {
                Player GPT = new() { Name = "ChatGPT", Icon = 'C', Colour = "Green", IsHuman = false };
                p_player.Add(GPT);
                switch (p_gameType)
                {
                    case "TTTChatGPT":
                        return new TTTChatGPT() { p_player = p_player };
                    case "FourWChatGPT":
                        return new FourWChatGPT() { p_player = p_player };
                    default:
                        throw new Exception("Invalid single-player game type.");
                }
            }
            else
            {
                switch (p_gameType)
                {
                    case "TTT":
                        return new TTT() { p_player = p_player} ;
                    case "FourW":
                        return new FourW() { p_player = p_player };
                    case "TwistFourW":
                        return new CustomTTT(true) { p_player = p_player };
                    case "CustomTTT":
                        return new CustomTTT(false) { p_player = p_player };
                    default:
                        throw new Exception("Invalid multiplayer game type.");
                }
            }
        }


        #region initializePlayer
        public void InitializePlayer()
        {
            Console.Clear();
            int p_numberOfPlayers = DetermineNumberOfPlayers();

            for (int p_gamer = 0; p_gamer < p_numberOfPlayers; p_gamer++)
            {
                Console.Clear();

                Player newPlayer = new Player
                {
                    Name = InitializePlayerName(p_gamer),
                    Icon = InitializePlayerIcon(p_gamer),
                    Colour = InitializePlayerColor(p_gamer),
                };
                p_player.Add(newPlayer);

            }
        }
        public int DetermineNumberOfPlayers()
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
        static string InitializePlayerName(int p_gamer)
        {
            string Name;
            do
            {
                Console.WriteLine($"\n Enter the name of player {p_gamer + 1}: \n");
                Name = Console.ReadLine();
                if (string.IsNullOrEmpty(Name))
                {
                    Console.WriteLine("Error: Name cannot be empty.");
                }
            } while (string.IsNullOrEmpty(Name));
            return Name;
        }
        static char InitializePlayerIcon(int p_gamer)
        {
            char icon;
            do
            {
                Console.WriteLine($"\n Enter the icon of player {p_gamer + 1}: \n");
                icon = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (icon == '\r' || icon == ' ')
                {
                    Console.WriteLine("Error: Icon cannot be empty or whitespace.");
                }
            } while (icon == '\r' || icon == ' ');
            return icon;
        }
        static string InitializePlayerColor(int p_gamer)
        {
            Console.WriteLine($"\n Choose a Colour for player {p_gamer + 1}: ");
            string Colour = ShowMenu(typeof(ValidColours));
            return Colour;
        }
        #endregion

        #region initializeGame
        public string InitializeGameMenu()
        {
            string p_choosing = null;
            do
            {
                if (p_choosing == null)
                {
                    p_choosing = ShowMenu(typeof(StartMenuOptions));
                }
                else
                {
                    switch (p_choosing)
                    {
                        case "SinglePlayer":
                            return p_gameMode = "SP";
                        case "MultiPlayer":
                            return p_gameMode = "MP";
                        case "Options":
                            return p_gameMode = "Options";
                        case "Quit":
                            Environment.Exit(0);
                            return "quit";
                        default:
                            throw new Exception("Invalid Input.");
                    }
                }
            } while (true);
        }
        public void InitializeGame()
        {
            Console.Clear();

            do
            {

                if (p_gameType == null)
                {
                    p_gameType = ShowMenu(p_gameMode == "SP" ? typeof(SinglePlayerGames) : typeof(MultiPlayerGames));
                }
                else
                {
                    Console.Clear();

                }
            } while (p_gameType == null);
        }
        #endregion



        public static string ShowMenu(Type enumType)
        {
            var enumValues = Enum.GetNames(enumType);
            List<string> menuItems = new List<string>(enumValues);

            Console.WriteLine("");
            menuItems.ForEach(currentItem => Console.WriteLine($"{menuItems.IndexOf(currentItem) + 1}. {currentItem}"));
            do
            {
                Console.Write("Enter the number of your choice: ");
                string input = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();

                if (int.TryParse(input, out int choice))
                {
                    if (choice >= 1 && choice <= menuItems.Count)
                    {
                        return menuItems[choice - 1];
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

    }


}



