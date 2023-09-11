using System;
using System.Collections.Generic;
using TicTacToeC;
using TicTacToeC.Model;

namespace TicTacToeC
{
    class Program
    {
        static void Main()
        {

            InitializeGameMenu();
            Player[] players = InitializePlayer();
            InitializeGame(players);

        }

        static void InitializeGameMenu()
        {
            List<string> menuPoints = new List<string>(Enum.GetNames(typeof(StartMenuOptions)));
            string choosing = null;
            //TODO: Do , kopfgesteuert
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
        //TODO: p_Paramaeter
        //TODO: alles Pascal
        static string ShowMenu(List<string> p_menuItems)
        {
            Console.WriteLine("Please make a Choice:");
            p_menuItems.ForEach(CurrentItem => Console.WriteLine($"{p_menuItems.IndexOf(CurrentItem) + 1}. {p_menuItems}"));

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
        static Player[] InitializePlayer()
        {
            //TODO: generische liste
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
            Player[] players = new Player[numberOfPlayers];
            Console.WriteLine();
            //TODO: i ersetzen
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i] = new Player();
                players[i].number = i + 1;
                Console.WriteLine($"Enter the name of player {players[i].number}: ");
                players[i].Name = Console.ReadLine();
                players[i].score = 0;
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


