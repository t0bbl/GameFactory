using System;
using TicTacToe;

namespace TicTacToe
{
    internal class PlayGame
    {
        public static GameBoard gameBoard;
        static string game;
        static bool validChoice = false;

        public static Dictionary<string, int> StartGame(string[] players, Dictionary<string, int> scores, int draw)
        {
            int currentPlayerIndex = 0;


            while (!validChoice)
            {
                Console.Write("Which Game shall we play? TTT or 4W?");
                game = Console.ReadLine();

                if (game == "TTT")
                {
                    gameBoard = new GameBoard(3, 3);
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

            while (CheckForWinner.CheckWinner(gameBoard, 3) == 0)
            {
                Console.WriteLine($"{players[currentPlayerIndex]}, input a number from 0 to {gameBoard.Rows * gameBoard.Columns - 1}");

                if (!int.TryParse(Console.ReadLine(), out int chosenCell) || chosenCell < 0 || chosenCell >= gameBoard.Rows * gameBoard.Columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int row = chosenCell / gameBoard.Columns;
                int col = chosenCell % gameBoard.Columns;

                if (gameBoard.GetCell(row, col) == 0)
                {
                    gameBoard.SetCell(row, col, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;  
                }
                else
                {
                    Console.WriteLine("Invalid move, idiot. Try again.");
                    continue;
                }

                gameBoard.PrintBoard();
            }

            int winnerNumber = CheckForWinner.CheckWinner(gameBoard, 3);
            switch (winnerNumber)
            {
                case 1:
                    Console.WriteLine($"{players[winnerNumber - 1]} won the game!");
                    scores[players[winnerNumber - 1]]++;  
                    gameBoard.ResetBoard();
                    break;
                default: 
                    Console.WriteLine("It's a draw, Idiots!");
                    draw++;
                    gameBoard.ResetBoard();
                    break;
            }

            return scores;
        }
    }
}
