using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class FourW
    {
        private GameBoard gameBoard;
        private string[] players;
        private Dictionary<string, (int PlayerNumber, int Score)> playerInfo;
        private int currentPlayerIndex;
        private int draw;
        private Dictionary<int, int> stack;

        public FourW(GameBoard gameBoard, string[] players, Dictionary<string, (int PlayerNumber, int Score)> playerInfo, int currentPlayerIndex, int draw)
        {
            this.gameBoard = gameBoard;
            this.players = players;
            this.playerInfo = playerInfo;
            this.currentPlayerIndex = currentPlayerIndex;
            this.draw = draw;
        }

        public (Dictionary<string, (int PlayerNumber, int Score)> PlayerInfo, int Draw) Start4W()
        {
            gameBoard = new GameBoard(6, 7);
            stack = new Dictionary<int, int>();

            for (int i = 0; i < 8; i++)
            {
                stack[i] = 5;
            }

            while (CheckForWinner.CheckWinner(gameBoard, 4) == 0)
            {
                Console.WriteLine($"{players[currentPlayerIndex]}, input a number from 0 to 7");

                if (!int.TryParse(Console.ReadLine(), out int chosenColumn) || chosenColumn < 0 || chosenColumn >= 8)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                if (stack[chosenColumn] >= 0)
                {
                    int row = stack[chosenColumn];
                    stack[chosenColumn]--;
                    gameBoard.SetCell(row, chosenColumn, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                }
                else
                {
                    Console.WriteLine("Invalid move, the column is full. Try again.");
                    continue;
                }

                gameBoard.PrintBoard();
            }

            int winnerNumber = CheckForWinner.CheckWinner(gameBoard, 4);
            string winner = players[winnerNumber - 1];

            if (winnerNumber > 0)
            {
                Console.WriteLine($"{winner} won the game!");
                var (playerNumber, score) = playerInfo[winner];
                playerInfo[winner] = (playerNumber, score + 1);
            }
            else
            {
                Console.WriteLine("It's a draw!");
                draw++;
            }

            return (playerInfo, draw);
        }
    }
}
