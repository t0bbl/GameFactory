using System;
using TicTacToe;

namespace TicTacToe
{
    public class TTT
    {

        private GameBoard gameBoard;
        private string[] players;
        private Dictionary<string, int> scores;
        private int currentPlayerIndex;
        private int draw;

        public TTT(GameBoard gameBoard, string[] players, Dictionary<string, int> scores, int currentPlayerIndex, int draw)
        {
            this.gameBoard = gameBoard;
            this.players = players;
            this.scores = scores;
            this.currentPlayerIndex = currentPlayerIndex;
            this.draw = draw;
        }


        public (Dictionary<string, int> scores, int draw) StartTTT()
        {
            gameBoard = new GameBoard(3, 3);

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
                case 2:
                    Console.WriteLine($"{players[winnerNumber - 1]} won the game!");
                    scores[players[winnerNumber - 1]]++;
                    gameBoard.ResetBoard();
                    break;
                case -1:
                    Console.WriteLine("It's a draw, Idiots!");
                    draw++;
                    gameBoard.ResetBoard();
                    break;
            }
            return (scores, draw);

        }
    }
    }
