using System;
using TicTacToe;

namespace TicTacToe
{
    internal class PlayGame
    {
        private GameBoard gameBoard;
        static string game;

        public PlayGame(GameBoard board)
        {
            gameBoard = board;
        }

        public (int, int, int, int) playGame(string player1, string player2, int scorePlayer1, int scorePlayer2, int draw)
        {

            Console.Write("Which Game shall we play? TTT or 4W?");
            bool validChoice = false;

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
            int startingPlayer = rand.Next(1, 3);

            Console.WriteLine(startingPlayer == 1 ? $"{player1} starts!" : $"{player2} starts!");

            while (CheckForWinner.CheckWinner(gameBoard, 3) == 0)
            {
                Console.WriteLine($"{(startingPlayer == 1 ? player1 : player2)}, input a number from 0 to {gameBoard.Rows * gameBoard.Columns - 1}");

                int currentPlayer = startingPlayer;

                if (!int.TryParse(Console.ReadLine(), out int chosenCell) || chosenCell < 0 || chosenCell >= gameBoard.Rows * gameBoard.Columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int row = chosenCell / gameBoard.Columns;
                int col = chosenCell % gameBoard.Columns;

                if (gameBoard.GetCell(row, col) == 0)
                {
                    gameBoard.SetCell(row, col, currentPlayer);
                    startingPlayer = (startingPlayer == 1) ? 2 : 1;
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
                    Console.WriteLine($"{player1} won the game! {player2} should feel ashamed!");
                    scorePlayer1++;
                    break;
                case 2:
                    Console.WriteLine($"{player2} won the game! {player1} is a utterly bad player!");
                    scorePlayer2++;
                    break;
                case -1:
                    Console.WriteLine("It's a draw, Idiots!");
                    draw++;
                    break;
            }

            return (winnerNumber, scorePlayer1, scorePlayer2, draw);
        }
    }
}
