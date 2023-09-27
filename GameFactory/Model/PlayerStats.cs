using GameFactory.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFactory.Model
{
    internal class PlayerStats
    {
        public static void ShowPlayerStats() 
        {
            Console.Clear();
            Console.WriteLine("Input a Name or LoginName to check their PlayerStats");
            string p_input = Console.ReadLine();
            int p_playerIdent = DataProvider.GetPlayerIdentFromName(p_input);
            List<(int Wins, int Losses, int Draws, int TotalGames, float WinPercentage)> statsList = DataProvider.GetPlayerStats(p_playerIdent);
            foreach (var stats in statsList)
            {
                Console.WriteLine($"Wins: {stats.Wins}, Losses: {stats.Losses}, Draws: {stats.Draws}, Total Games: {stats.TotalGames}, Win Percentage: {stats.WinPercentage}");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
