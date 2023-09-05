using System;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    class Program
    {
        static int draw;


        static void Main(string[] args)
        {


            Dictionary<string, (int PlayerNumber, int Score)> playerInfo = Player.InitializePlayers();


            do
            {
                string[] players = playerInfo.Keys.ToArray();
                (playerInfo, draw) = PlayGame.StartGame(players, playerInfo, draw);


                Console.WriteLine("Want a rematch Noob ? Y / N ?");


            } while (Console.ReadLine().ToLower() == "y");


            foreach (var name in playerInfo.Keys)
            {
                Console.WriteLine($"{name} has won {playerInfo[name].Score} times!");
            }
            Console.WriteLine($"There were {draw} draws.");
            Console.ReadKey();
        }


    }
}