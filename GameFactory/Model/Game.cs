
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
        public Match CreateMatch()
        {
            if (p_gameMode == "SP")
            {
                if (!p_player.Any(x => x.Name == "ChatGPT"))
                {
                    Player GPT = new() { Name = "ChatGPT", Icon = 'C', Colour = "Green", IsHuman = false };
                    p_player.Add(GPT);
                }

                if (p_gameType == "TTTChatGPT")
                {
                    CurrentMatch = new TTTChatGPT() { p_player = p_player };
                }
                else if (p_gameType == "FourWChatGPT")
                {
                    CurrentMatch = new FourWChatGPT() { p_player = p_player };
                }
                else
                {
                    throw new Exception("Invalid single-player game type.");
                }
                p_history.Add(CurrentMatch);

                return CurrentMatch;
            }
            else
            {

                if (p_gameType == "TTT")
                {
                    CurrentMatch = new TTT() { p_player = p_player };
                }
                else if (p_gameType == "FourW")
                {
                    CurrentMatch = new FourW() { p_player = p_player };
                }
                else if (p_gameType == "TwistFourW")
                {
                    CurrentMatch = new CustomTTT(true) { p_player = p_player };
                }
                else if (p_gameType == "CustomTTT")
                {
                    CurrentMatch = new CustomTTT(false) { p_player = p_player };
                }
                else
                {
                    throw new Exception("Invalid multiplayer game type.");
                }
                p_history.Add(CurrentMatch);

                return CurrentMatch;
            }
        }


        #region initializePlayer
        public void InitializePlayer()
        {
            p_player = new List<Player>();
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
                    Id = Guid.NewGuid(),
                    IsHuman = true
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
            char Icon;
            do
            {
                Console.WriteLine($"\n Enter the icon of player {p_gamer + 1}: \n");
                Icon = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (Icon == '\r' || Icon == ' ')
                {
                    Console.WriteLine("Error: Icon cannot be empty or whitespace.");
                }
            } while (Icon == '\r' || Icon == ' ');
            return Icon;
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



