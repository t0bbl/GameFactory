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
            SQLPlayerService sqlPlayerService = new SQLPlayerService();
            SQLStats sqlStats = new SQLStats();
            Console.WriteLine("Input a Name or LoginName to check their PlayerStats");
            string p_input = Console.ReadLine();
            int p_playerIdent = sqlPlayerService.GetPlayerIdentFromName(p_input);
            var p_playerStats = sqlStats.GetPlayerStats(p_playerIdent);
            int p_winPercentage = sqlStats.GetPlayerWinPercentage(p_playerStats.Wins, p_playerStats.Losses, p_playerStats.Draws);
            Console.Clear();
            Console.WriteLine($"{p_input}:      Wins: {p_playerStats.Wins}      Losses: {p_playerStats.Losses}          Draws: {p_playerStats.Draws}        Win Percentage: {p_winPercentage}%");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
