using System;
using TicTacToe;

namespace TicTacToe
{
    public class FourW
    {
        private GameBoard gameBoard;
        private string[] players;
        private Dictionary<string, int> scores;
        private int currentPlayerIndex;
        private int draw;
        private Dictionary<int, int> stack;

        // Constructor that takes 5 arguments
        public FourW(GameBoard gameBoard, string[] players, Dictionary<string, int> scores, int currentPlayerIndex, int draw)
        {
            this.gameBoard = gameBoard;
            this.players = players;
            this.scores = scores;
            this.currentPlayerIndex = currentPlayerIndex;
            this.draw = draw;
        }
        public (Dictionary<string, int> scores, int draw) start4W()
        {
            gameBoard = new GameBoard(6, 7);
            stack = new Dictionary<int, int>();

            // Initialize each column's stack height
            for (int i = 0; i < 8; i++)
            {
                stack[i] = 0;
            }

            while (CheckForWinner.CheckWinner(gameBoard, 4) == 0)
            {
                Console.WriteLine($"{players[currentPlayerIndex]}, input a number from 0 to 7");

                if (!int.TryParse(Console.ReadLine(), out int chosenColumn) || chosenColumn < 0 || chosenColumn >= 8)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                if (stack[chosenColumn] < 6)
                {
                    int row = stack[chosenColumn];
                    stack[chosenColumn]++;
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

            switch (winnerNumber)
            {
                case 1:
                case 2:
                    Console.WriteLine($"{players[winnerNumber - 1]} won the game!");
                    scores[players[winnerNumber - 1]]++;
                    break;
                case -1:
                    Console.WriteLine("It's a draw!");
                    draw++;
                    break;
            }

            return (scores, draw);
        }
    }
}