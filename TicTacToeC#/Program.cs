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

            foreach (var player in players)
            {
                Console.WriteLine($"{player.Name} has won {player.Score} times!");
            }
            Console.WriteLine($"There were {draw} draws.");
            Console.ReadKey();
        }
    }
}
