using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class TTT
    {
        private GameBoard gameBoard;
        private string[] players;
        private Dictionary<string, (int PlayerNumber, int Score)> playerInfo;
        private int currentPlayerIndex;
        private int draw;

        public TTT(GameBoard gameBoard, string[] players, Dictionary<string, (int PlayerNumber, int Score)> playerInfo, int currentPlayerIndex, int draw)
        {
            this.gameBoard = gameBoard;
            this.players = players;
            this.playerInfo = playerInfo;
            this.currentPlayerIndex = currentPlayerIndex;
            this.draw = draw;
        }

        public (Dictionary<string, (int PlayerNumber, int Score)> PlayerInfo, int Draw) StartTTT()
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
            string winner = players[winnerNumber - 1];

            if (winnerNumber > 0)
            {
                Console.WriteLine($"{winner} won the game!");
                var (playerNumber, score) = playerInfo[winner];
                playerInfo[winner] = (playerNumber, score + 1);
            }
            else
            {
                Console.WriteLine("It's a draw, Idiots!");
                draw++;
            }

            gameBoard.ResetBoard();
            return (playerInfo, draw);
        }
    }
}
