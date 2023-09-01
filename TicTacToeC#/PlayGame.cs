using System;
using TicTacToe;

namespace TicTacToe
{
    internal class PlayGame
    {

        public static GameBoard gameBoard;
        static string game;
        static bool validChoice = false;

        public static (Dictionary<string, int> scores, int draw) StartGame(string[] players, Dictionary<string, int> scores, int draw)
        {
            int currentPlayerIndex = 0;


            while (!validChoice)
            {
                Console.Write("Which Game shall we play? TTT or 4W?");
                game = Console.ReadLine();

                if (game == "TTT")
                {
                    validChoice = true;
                }
                else if (game == "4W")
                {
                    gameBoard = new GameBoard(7, 6);
                    validChoice = true;
                }
                else
                {
                    Console.WriteLine("Try again, idiot.");
                }
            }

            int player1turn = -1;
            int player2turn = -1;

            Random rand = new Random();
            currentPlayerIndex = rand.Next(0, players.Length);

            Console.WriteLine($"{players[currentPlayerIndex]} starts!");
            TTT tttGame = new TTT(gameBoard, players, scores, currentPlayerIndex, draw);
            var (updatedScores, updatedDraw) = tttGame.startTTT();
            scores = updatedScores;
            draw = updatedDraw;

            return (scores, draw);
        }
    }
}
