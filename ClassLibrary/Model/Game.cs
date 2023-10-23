
namespace ClassLibrary
{
    public class Game
    {
        #region Variables
        public List<Player> p_Player { get; set; } = new();
        internal string p_GameType { get; set; }
        internal string p_GameMode { get; set; }
        internal Match CurrentMatch { get; set; }
        internal int GuestCount { get; set; } = 0;

        #endregion
        /// <summary>
        /// Creates a new match based on the selected game mode and game type, adding a virtual player named ChatGPT for single-player modes when necessary.
        /// </summary>
        /// <returns>The created match, with players and any specific configurations set.</returns>
        public Match CreateMatch()
        {
            if (p_GameMode == "SinglePlayer")
            {
                if (!p_Player.Any(x => x.Name == "ChatGPT"))
                {
                    var p_GPTVariables = DataProvider.GetPlayerVariables(10);
                    p_Player.Add(p_GPTVariables);
                }
            }
            CurrentMatch = p_GameType switch
            {
                "TTT" => new TTT() { p_Player = p_Player },
                "FourW" => new FourW() { p_Player = p_Player },
                "TTTChatGPT" => new TTT() { p_Player = p_Player, p_ChatGPT = true },
                "FourWChatGPT" => new FourW() { p_Player = p_Player, p_ChatGPT = true },
                "TwistFourW" => new CustomTTT(true) { p_Player = p_Player },
                "CustomTTT" => new CustomTTT(false) { p_Player = p_Player },
            };

            return CurrentMatch;
        }

        #region initializePlayer
        /// <summary>
        /// Initializes players for a game session by either logging in existing players, signing up new players, or adding guest players.
        /// </summary>
        public void InitializePlayer()
        {
            var Player = new Player();
            Console.Clear();
            int NumberOfPlayers = DetermineNumberOfPlayers();

            for (int Gamer = 0; Gamer < NumberOfPlayers; Gamer++)
            {
                bool ValidInput = false;
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
                        p_Player.Add(playerVariables);
                        ValidInput = true;
                    }
                    else if (p_input == "u")
                    {
                        Player.PlayerSignup();
                        ValidInput = false;
                    }
                    else if (p_input == "g")
                    {
                        Console.WriteLine("Playing as a guest.");
                        var GuestVariables = DataProvider.GetPlayerVariables(GuestCount + 1);
                        p_Player.Add(GuestVariables);
                        GuestCount++;
                        ValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid input.");
                    }
                } while (!ValidInput);

                Console.Clear();
            }
        }
        /// <summary>
        /// Determines the number of players for the game based on the game mode or user input.
        /// </summary>
        /// <returns>The number of players that will participate in the game.</returns>
        internal int DetermineNumberOfPlayers()
        {
            if (p_GameMode == "SinglePlayer") return 1;

            int NumberOfPlayers;
            do
            {
                Console.WriteLine("Enter the number of players: ");
                string Input = Console.ReadLine();
                if (int.TryParse(Input, out NumberOfPlayers) && NumberOfPlayers > 0)
                {
                    return NumberOfPlayers;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            } while (true);
        }
        /// <summary>
        /// Prompts the user to enter a name for in-game use, ensuring the name is not empty.
        /// </summary>
        /// <returns>The name entered by the user.</returns>
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
        /// <summary>
        /// Prompts the user to enter a character to be used as their icon in-game, ensuring the icon is not empty or whitespace.
        /// </summary>
        /// <returns>The icon character entered by the user.</returns>
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
        /// <summary>
        /// Prompts the user to choose a color from a predefined list of valid colors to be used in-game.
        /// </summary>
        /// <returns>The color chosen by the user.</returns>
        internal static string InitializePlayerColor()
        {
            Console.WriteLine($"\n Choose a Color you want to use InGame: \n");
            string Colour = ShowMenu(typeof(ValidColours));
            return Colour;
        }
        #endregion

        #region initializeGame
        /// <summary>
        /// Displays the game menu to the user and returns the selected game mode.
        /// </summary>
        /// <returns>The game mode selected by the user.</returns>
        public string InitializeGameMenu()
        {
            do
            {
                return p_GameMode = ShowMenu(typeof(StartMenuOptions));
            } while (p_GameMode == null);
        }
        /// <summary>
        /// Displays the game selection menu based on the game mode (SinglePlayer/MultiPlayer) and returns the selected game type.
        /// </summary>
        /// <returns>The game type selected by the user.</returns>
        public string InitializeGame()
        {
            Console.Clear();
            do
            {
                return p_GameType = ShowMenu(p_GameMode == "SinglePlayer" ? typeof(SinglePlayerGames) : typeof(MultiPlayerGames));
            } while (p_GameType == null);
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Displays a menu with options enumerated in the specified type, and returns the user's choice.
        /// </summary>
        /// <param name="p_EnumType">The type of the enum which values are to be displayed as menu options.</param>
        /// <returns>The string representation of the selected enum value.</returns>
        internal static string ShowMenu(Type p_EnumType)
        {
            var EnumValues = Enum.GetValues(p_EnumType);
            var MenuItems = new List<string>();

            Console.WriteLine("");
            foreach (var value in EnumValues)
            {
                MenuItems.Add(value.ToString());
                Console.WriteLine($"{MenuItems.Count}. {value}");
            }
            do
            {
                Console.Write("Enter the number of your choice: ");
                string p_input = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();

                if (int.TryParse(p_input, out int p_choice))
                {
                    if (p_choice >= 1 && p_choice <= MenuItems.Count)
                    {
                        return MenuItems[p_choice - 1];
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



