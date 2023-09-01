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

            Console.WriteLine("Input name of first Challenger");
            player1 = Console.ReadLine();
            Console.WriteLine("Input name of second Challenger");
            player2 = Console.ReadLine();

            return new string[] { player1, player2 };

        }
    }
}