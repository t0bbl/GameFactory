using System.ComponentModel.Design;
using System.Net.NetworkInformation;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            int draw = 0;
            Match match = new();

            StartMenu.InitializeGameMenu();
            Player[] players = InitializePlayer.Initialize();
            GamesAvailable gameInstance = InitializeGame.Initialize();
            (players, draw) = match.StartGame(gameInstance, players, draw);

            Stats.EndGameStats(players, draw);
        }
    }
}
