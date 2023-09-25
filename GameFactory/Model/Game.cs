
using System.Collections.Generic;
using System.Linq;

namespace GameFactory.Model
{
    internal class Game
    {
        #region Variables
        internal List<Player> p_player { get; set; }
        internal string p_gameType { get; set; }
        internal string p_gameMode { get; set; }

        internal List<Match> p_history = new();
        internal Match CurrentMatch { get; set; }

        #endregion
        internal Match CreateMatch()
        {
            if (p_gameMode == "SinglePlayer")
            {
                if (!p_player.Any(x => x.Name == "ChatGPT"))
                {
                    Player GPT = new() { Name = "ChatGPT", Icon = 'C', Colour = "Green", IsHuman = false };
                    p_player.Add(GPT);
                }
            }
            CurrentMatch = CurrentMatch = p_gameType switch
            {
                "TTT" => new TTT() { p_player = p_player },
                "FourW" => new FourW() { p_player = p_player },
                "TTTChatGPT" => new TTTChatGPT() { p_player = p_player },
                "FourWChatGPT" => new FourWChatGPT() { p_player = p_player },
                "TwistFourW" => new CustomTTT(true) { p_player = p_player },
                "CustomTTT" => new CustomTTT(false) { p_player = p_player },
            };
            p_history.Add(CurrentMatch);

            return CurrentMatch;
        }



        #region initializePlayer
        public void InitializePlayer()
        {
            var playerAuth = new PlayerAuth();
            p_player = new List<Player>();
            Console.Clear();
            int p_numberOfPlayers = DetermineNumberOfPlayers();

            for (int p_gamer = 0; p_gamer < p_numberOfPlayers; p_gamer++)
            {
                do {
                    Console.WriteLine($"Do you want to Log(i)n, Sign(U)p or play as a (G)uest?");
                    string p_input = Console.ReadKey().KeyChar.ToString();
                    if (p_input == "i")
                    {
                        playerAuth.PlayerSignIn();
                    }
                    else if (p_input == "u")
                    {
                        playerAuth.PlayerSignup();
                    }
                    else if (p_input == "g")
                    {
                        Console.WriteLine("Playing as a guest.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    Console.Clear();
                } while (p_gameMode == "PlayerSignup" && !playerAuth.PlayerSignup());


                Player newPlayer = new Player
                {
                    Name = InitializePlayerName(),
                    Icon = InitializePlayerIcon(),
                    Colour = InitializePlayerColor(),
                    IsHuman = true
                };
                p_player.Add(newPlayer);

            }
        }
        public int DetermineNumberOfPlayers()
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
        public static string InitializePlayerName()
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
        public static char InitializePlayerIcon()
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
        public static string InitializePlayerColor()
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

        internal static void EndGameStats(List<Player> p_player, List<Match> p_history)
        {

            Console.WriteLine("Game over!");
            Console.WriteLine("Final scores:");

            foreach (var Player in p_player)
            {
                int Wins = p_history.Count(Match => Match.Winner == Player);
                int Losses = p_history.Count(Match => Match.Loser == Player);
                Console.WriteLine($"{Player.Name}:  Wins: {Wins}   Losses: {Losses} \n");
            }
            int Draws = p_history.Count(Match => Match.Draw != null);

            Console.WriteLine($"Draws: {Draws}");

            Console.ReadKey();
            Environment.Exit(0);

        }


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



