using System;
using TicTacToe;

namespace TicTacToe
{
    internal class PlayGame
    {

        public static GameBoard gameBoard;
        public static string game = null;

        public static (Dictionary<string, int> scores, int draw) StartGame(string[] players, Dictionary<string, int> scores, int draw)
        {
            int currentPlayerIndex = 0;


            while (string.IsNullOrEmpty(game))
            {
                Console.Write("Which Game shall we play? TTT or 4W?");
                game = Console.ReadLine();
            }

            int player1turn = -1;
            int player2turn = -1;

            Random rand = new Random();
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

            else
            {
                Console.WriteLine("Invalid game. Try again.");
            }
            
            return (scores, draw);
        }
    }
}
