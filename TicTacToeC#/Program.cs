using System;
using System.Collections.Generic;
using TicTacToeC;
using TicTacToeC.Model;

namespace TicTacToeC
{
    class Program
    {
        static void Main(string[] args)
        {
            int draw = 0;
             ValidGames validGames;


            InitializeGameMenu();
            Player[] players = InitializePlayer();
            InitializeGame(players);

        }
        public enum StartMenuOptions
        {
            NewGame = 1,
            Quit = 2
        }

        static void InitializeGameMenu()
        {
            List<string> menuPoints = new List<string>(Enum.GetNames(typeof(StartMenuOptions)));
            string choosing = null;

            while (true)
            {
                if (choosing == null)
                {
                    choosing = ShowMenu(menuPoints);
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
        static string ShowMenu(List<string> menuItems)
        {
            Console.WriteLine("Please make a Choice:");

            for (int i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i]}");
            }

            while (true)
            {
                Console.Write("Enter the number of your choice: ");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                char keyChar = keyInfo.KeyChar;
                string input = keyChar.ToString();
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
            }
        }
        static Player[] InitializePlayer()
        {
            Console.WriteLine("Enter the number of players: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int numberOfPlayers = Convert.ToInt32(keyInfo.KeyChar.ToString());
            Player[] players = new Player[numberOfPlayers];
            Console.WriteLine();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i] = new Player();
                players[i].Number = i + 1;
                Console.WriteLine($"Enter the name of player {players[i].Number}: ");
                players[i].Name = Console.ReadLine();
                players[i].Score = 0;
            }

            return players;
        }

        static string InitializeGame(Player[] players)
        {
            List<string> gameOptions = new List<string>(Enum.GetNames(typeof(ValidGames)));
            string game = null;

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
                            var tttGame = new TTT(players);
                            tttGame.StartGame(players); 
                            break;
                        case "FourW":
                            var fourWGame = new FourW(players);
                            fourWGame.StartGame(players);
                            break;
                        default:
                            throw new Exception("Invalid game type.");
                    }
                }
            }
        }
    }

}


