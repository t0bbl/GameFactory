namespace GameFactory.Model
{
    internal class Game
    {
        #region Variables
        internal List<Player> p_player { get; set; } = new();
        internal string p_gameType { get; set; }
        internal string p_gameMode { get; set; }
        internal Match CurrentMatch { get; set; }
        internal int p_guestCount { get; set; } = 0;

        #endregion
        internal Match CreateMatch()
        {
            if (p_gameMode == "SinglePlayer")
            {
                if (!p_player.Any(x => x.Name == "ChatGPT"))
                {
                    var p_GPTVariables = DataProvider.GetPlayerVariables(10);
                    p_player.Add(p_GPTVariables);
                }
            }
            CurrentMatch = p_gameType switch
            {
                "TTT" => new TTT() { p_player = p_player },
                "FourW" => new FourW() { p_player = p_player },
                "TTTChatGPT" => new TTT() { p_player = p_player, p_chatGPT = true },
                "FourWChatGPT" => new FourW() { p_player = p_player, p_chatGPT = true },
                "TwistFourW" => new CustomTTT(true) { p_player = p_player },
                "CustomTTT" => new CustomTTT(false) { p_player = p_player },
            };

            return CurrentMatch;
        }

        #region initializePlayer
        internal void InitializePlayer()
        {
            var Player = new Player();
            Console.Clear();
            int p_numberOfPlayers = DetermineNumberOfPlayers();

            for (int p_gamer = 0; p_gamer < p_numberOfPlayers; p_gamer++)
            {
                bool p_validInput = false;
                do
                {
                    Console.WriteLine($"Do you want to Log(i)n, Sign(U)p or play as a (G)uest?");
                    string p_input = Console.ReadKey().KeyChar.ToString();
                    if (p_input == "i")
                    {
                        var p_ident = 0;
                        do
                        {
                            p_ident = Player.PlayerSignIn();
                        } while (p_ident == 0);
                        var playerVariables = DataProvider.GetPlayerVariables(p_ident);
                        Console.WriteLine($"Welcome back {playerVariables.Name}!");
                        p_player.Add(playerVariables);
                        p_validInput = true;
                    }
                    else if (p_input == "u")
                    {
                        Player.PlayerSignup();
                        p_validInput = false;
                    }
                    else if (p_input == "g")
                    {
                        Console.WriteLine("Playing as a guest.");
                        var GuestVariables = DataProvider.GetPlayerVariables(p_guestCount + 1);
                        p_player.Add(GuestVariables);
                        p_guestCount++;
                        p_validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid input.");
                    }
                } while (!p_validInput);

                Console.Clear();
            }
        }
        internal int DetermineNumberOfPlayers()
        {
            if (p_gameMode == "SinglePlayer") return 1;

            int p_numberOfPlayers;
            do
            {
                Console.WriteLine("Enter the number of players: ");
                string p_input = Console.ReadLine();
                if (int.TryParse(p_input, out p_numberOfPlayers) && p_numberOfPlayers > 0)
                {
                    return p_numberOfPlayers;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            } while (true);
        }
        internal static string InitializePlayerName()
        {
            string Name;
            do
            {
                Console.WriteLine($"\n Enter the name you want to use InGame: \n");
                Name = Console.ReadLine();
                if (string.IsNullOrEmpty(Name))
                {
                    Console.WriteLine("Error: Name cannot be empty.");
                }
            } while (string.IsNullOrEmpty(Name));
            return Name;
        }
        internal static char InitializePlayerIcon()
        {
            char Icon;
            do
            {
                Console.WriteLine($"\n Enter the Icon you want to use InGame: \n");
                Icon = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (Icon == '\r' || Icon == ' ')
                {
                    Console.WriteLine("Error: Icon cannot be empty or whitespace.");
                }
            } while (Icon == '\r' || Icon == ' ');
            return Icon;
        }
        internal static string InitializePlayerColor()
        {
            Console.WriteLine($"\n Choose a Color you want to use InGame: \n");
            string Colour = ShowMenu(typeof(ValidColours));
            return Colour;
        }
        #endregion

        #region initializeGame
        internal string InitializeGameMenu()
        {
            do
            {
                return p_gameMode = ShowMenu(typeof(StartMenuOptions));
            } while (p_gameMode == null);
        }
        internal string InitializeGame()
        {
            Console.Clear();
            do
            {
                return p_gameType = ShowMenu(p_gameMode == "SinglePlayer" ? typeof(SinglePlayerGames) : typeof(MultiPlayerGames));
            } while (p_gameType == null);
        }
        #endregion

        #region Utilities
        internal static string ShowMenu(Type p_enumType)
        {
            var p_enumValues = Enum.GetValues(p_enumType);
            var p_menuItems = new List<string>();

            Console.WriteLine("");
            foreach (var value in p_enumValues)
            {
                p_menuItems.Add(value.ToString());
                Console.WriteLine($"{p_menuItems.Count}. {value}");
            }
            do
            {
                Console.Write("Enter the number of your choice: ");
                string p_input = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();

                if (int.TryParse(p_input, out int p_choice))
                {
                    if (p_choice >= 1 && p_choice <= p_menuItems.Count)
                    {
                        return p_menuItems[p_choice - 1];
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
        #endregion
    }
}



