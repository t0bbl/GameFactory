using System;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TicTacToe
{
    class Program
    {
        static string game;
        static int draw, winnerNumber;
        static Dictionary<string, string> playerNames = new Dictionary<string, string>();
        static Dictionary<string, int> playerScores = new Dictionary<string, int>();


        static void Main(string[] args)
        {

            string[] names = Player.PlayerNames(args);

            for (int i = 0; i < names.Length; i++)
            {
                playerNames[$"player{i + 1}"] = names[i];
                playerScores[names[i]] = 0;
            }

            do
            {
                PlayGame play = new PlayGame();
                string[] players = playerNames.Values.ToArray();
                playerScores = PlayGame.StartGame(players, playerScores, draw);


                Console.WriteLine("Want a rematch Noob ? Y / N ?");


            } while (Console.ReadLine().ToLower() == "y");


            foreach (var name in names)
            {
                Console.WriteLine($"{name} has won {playerScores[name]} times!");
            }
            Console.WriteLine($"There were {draw} draws.");
            Console.ReadKey();
        }


    }
}