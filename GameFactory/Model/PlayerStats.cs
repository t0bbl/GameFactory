using GameFactory.SQL;
using System;
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
            var p_playerStats = DataProvider.GetPlayerStats(p_playerIdent);
            int p_winPercentage = DataProvider.GetPlayerWinPercentage(p_playerStats.Wins, p_playerStats.Losses, p_playerStats.Draws);
            Console.Clear();
            Console.WriteLine($"{p_input}:      Wins: {p_playerStats.Wins}      Losses: {p_playerStats.Losses}          Draws: {p_playerStats.Draws}        Win Percentage: {p_winPercentage}%");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
