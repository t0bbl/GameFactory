using System;
using System.Collections.Generic;

namespace TicTacToe
{
    class Player
    {
        public static Dictionary<string, (int PlayerNumber, int Score)> InitializePlayers()
        {
            Dictionary<string, (int PlayerNumber, int Score)> playerInfo = new Dictionary<string, (int, int)>();

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"Input name of Challenger {i + 1}:");
                string playerName = Console.ReadLine();
                playerInfo[playerName] = (i + 1, 0);
            }

            return playerInfo;
        }
    }
}