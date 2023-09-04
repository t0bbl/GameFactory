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

            for (int i = 0; i < 2; i++)
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