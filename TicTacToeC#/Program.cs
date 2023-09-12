using System;
using System.Collections.Generic;
using GameFactory;
using GameFactory.Model;

namespace GameFactory
{
    class Program
    {
        static void Main()
        {

            InitializeGameMenu();
            var Players = InitializePlayer();
            InitializeGame(Players);

        }

        static void InitializeGameMenu()
        {
            List<string> menuPoints = new List<string>(Enum.GetNames(typeof(StartMenuOptions)));
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
                        case "NewGame":
                            return;
                        case "Quit":
                            Environment.Exit(0);
                            return;
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
                //char keyChar = keyInfo.KeyChar;
                //string input = keyChar.ToString();
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
        static List<Player> InitializePlayer()
        {
            var Players = new List<Player>();

            bool isValidNumber;
            int numberOfPlayers;

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
            for (int Gamer = 0; Gamer < numberOfPlayers; Gamer++)
            {
                Console.WriteLine($"Enter the name of player {Gamer + 1}: ");
                string playerName = Console.ReadLine();
                Player newPlayer = new Player { p_name = playerName };
                Players.Add(newPlayer);
            }
            return Players;

        }
        static string InitializeGame(List<Player> Players)
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
                            var tttGame = new TTT();
                            tttGame.StartGame(Players); 
                            break;
                        case "FourW":
                            var fourWGame = new FourW();
                            fourWGame.StartGame(Players);
                            break;
                        default:
                            throw new Exception("Invalid game type.");
                    }
                }
            }
        }
    }

}


