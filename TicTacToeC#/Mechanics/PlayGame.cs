using System;
using System.Collections.Generic;
using TicTacToe;

namespace TicTacToe
{
    internal class PlayGame
    {
        public static GameBoard gameBoard;
        public static string game;
        public static List<string> validGames = new List<string> { "TTT", "4W" };

        public static (Dictionary<string, int>, int) StartGame(string[] players, Dictionary<string, int> scores, int draw)
        {
            int currentPlayerIndex; 

            while (true)
            {
                Console.Write("Which Game shall we play? ");
                for (int i = 0; i < validGames.Count; i++)
                {
                    Console.Write(validGames[i]);
                    if (i < validGames.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine("?");
                game = Console.ReadLine();

                if (validGames.Contains(game))
                {
                    Random rand = new();
                    currentPlayerIndex = rand.Next(0, players.Length);
                    Console.WriteLine($"{players[currentPlayerIndex]} starts!");

                    if (game == "TTT")
                    {
                        TTT tttGame = new TTT(gameBoard, players, scores, currentPlayerIndex, draw);
                        var (updatedScores, updatedDraw) = tttGame.StartTTT();
                        scores = updatedScores;
                        draw = updatedDraw;
                    }
                    else if (game == "4W")
                    {
                        FourW fourwGame = new FourW(gameBoard, players, scores, currentPlayerIndex, draw);
                        var (updatedScores, updatedDraw) = fourwGame.Start4W();
                        scores = updatedScores;
                        draw = updatedDraw;
                    }

                    return (scores, draw);
                }
                else
                {
                    Console.WriteLine("Invalid game. Try again.");
                }
            }
        }
    }
}
