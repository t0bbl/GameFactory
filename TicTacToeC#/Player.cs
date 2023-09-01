using System;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace TicTacToe
{
    class Player
    {
        static string player1, player2;

        static public string[]  PlayerNames(string[] args)
        {
            Console.WriteLine("How many Challengers are there?");
            int playerNumber = int.Parse(Console.ReadLine());

            string[] players = new string[playerNumber];


            for (int i = 0; i < playerNumber; i++)
            {   Console.WriteLine($"Input name of Challenger {i + 1}:");
                players[i] = Console.ReadLine();
             }


            return players;

        }
    }
}