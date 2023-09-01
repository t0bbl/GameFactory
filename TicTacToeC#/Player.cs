using System;
using System.Collections.Generic;

namespace TicTacToe
{
    class Player
    {
        public static Dictionary<string, string> playerNames = new Dictionary<string, string>();
        public static Dictionary<string, int> playerScores = new Dictionary<string, int>();

        public static Dictionary<string, string> PlayerNames(string[] args)
        {
            Console.WriteLine("How many Challengers are there?");
            int playerNumber = int.Parse(Console.ReadLine());

            for (int i = 0; i < playerNumber; i++)
            {
                Console.WriteLine($"Input name of Challenger {i + 1}:");
                string playerName = Console.ReadLine();
                playerNames[$"player{i + 1}"] = playerName;
                playerScores[playerName] = 0;
            }

            return playerNames;
        }
    }
}