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
            Console.WriteLine("Input a Name or LoginName to check theier PlayerStats");
            string p_input = Console.ReadLine();
            int p_playerIdent = sqlPlayerService.GetPlayerIdentFromName(p_input, p_input);
            var p_playerStats = sqlPlayerService.GetPlayerStats(p_playerIdent);
            Console.Clear();
            Console.WriteLine($"{p_input}:      Wins: {p_playerStats.Wins}      Losses: {p_playerStats.Losses}          Draws: {p_playerStats.Draws}");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
