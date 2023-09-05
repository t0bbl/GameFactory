using System;
using System.Collections.Generic;

namespace TicTacToe
{
    internal class PlayGame
    {
        public static GameBoard gameBoard;
        public static string game;
        public static List<string> validGames = new List<string> { "TTT", "4W" };
        public static string choosenGame = null;

        public static (Dictionary<string, (int PlayerNumber, int Score)>, int) StartGame(
     string[] players, Dictionary<string, (int PlayerNumber, int Score)> playerInfo, int draw)
        {
            int currentPlayerIndex;

            while (true)
            {
                if (choosenGame == null)
                {
                    //Console.Write("Which Game shall we play? ");
                    //for (int i = 0; i < validGames.Count; i++)
                    //{
                    //    Console.Write(validGames[i]);
                    //    if (i < validGames.Count - 1)
                    //    {
                    //        Console.Write(", ");
                    //    }
                    //}
                    //Console.WriteLine("?");
                    //game = Console.ReadLine();
                    //if (validGames.Contains(game))
                    //{
                    //    choosenGame = game;
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Invalid game. Try again.");
                    //    continue;
                    //}
                    game = Menu.ShowMenu(validGames);
                    choosenGame = game;
                }
                else
                {
                    game = choosenGame;
                }

                Random rand = new Random();
                currentPlayerIndex = rand.Next(0, players.Length);
                Console.WriteLine($"{players[currentPlayerIndex]} starts!");

                if (game == "TTT")
                {
                    TTT tttGame = new TTT(gameBoard, players, playerInfo, currentPlayerIndex, draw);
                    var (updatedPlayerInfo, updatedDraw) = tttGame.StartTTT();
                    playerInfo = updatedPlayerInfo;
                    draw = updatedDraw;
                }
                else if (game == "4W")
                {
                    FourW fourwGame = new FourW(gameBoard, players, playerInfo, currentPlayerIndex, draw);
                    var (updatedPlayerInfo, updatedDraw) = fourwGame.Start4W();
                    playerInfo = updatedPlayerInfo;
                    draw = updatedDraw;
                }

                return (playerInfo, draw);
            }
        }
    }
}
