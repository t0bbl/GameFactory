using System;
using System.Text.RegularExpressions;

namespace TicTacToe
{
    class Program
    {
        static GameBoard gameBoard = new GameBoard();
        static string player1, player2;
        static int scorePlayer1, scorePlayer2, draw;
        static void Main(string[] args)
        { 

            Console.WriteLine("Input name of first Challenger");
            player1 = Console.ReadLine();
            Console.WriteLine("Input name of second Challenger");
            player2 = Console.ReadLine();

            do
            {
                playGame(); 
                gameBoard.ResetBoard();

                Console.WriteLine("Want a rematch Noob ? Y / N ?");


            } while (Console.ReadLine().ToLower() == "y") ;
            Console.WriteLine("Score:  " + player1 + " : " + scorePlayer1 + " !     " + player2 + " : " + scorePlayer2 + "!     Draw: " + draw);

        }

        private class  GameBoard
        {
            private int[] board = new int[9];
            public GameBoard()
            {
                ResetBoard();
            }

            public void ResetBoard()
            {
                for (int i = 0; i < 9; i++)
                {
                    board[i] = 0;
                }
            }
            public int GetCell(int index)
            {
                return board[index];
            }

            public void SetCell(int index, int value)
            {
                if (index >= 0 && index < 9)
                {
                    board[index] = value;
                }
            }
        }
        private static int playGame()
        {
            Random rand = new Random();

            int startingPlayer = rand.Next(1, 3);

            Console.WriteLine(startingPlayer == 1 ? player1 + " starts!" : player2 + " starts!");



            while (checkForWinner() == 0)
            {
               
                    Console.WriteLine(startingPlayer == 1 ? player1 + ", input a number from 0 to 8" : player2 + ", input a number from 0 to 8");
                


                int currentPlayer = startingPlayer == 1 ? 1 : 2;

                int chosenCell = int.Parse(Console.ReadLine());

                if (gameBoard.GetCell(chosenCell) == 0)
                {
                    gameBoard.SetCell(chosenCell, currentPlayer);
                    startingPlayer = startingPlayer == 1 ? 2 : 1;
                }
                else
                {
                    Console.WriteLine("Invalid move, idiot. Try again.");
                    continue;
                }

                printBoard();


            }
            int winnerNumber = checkForWinner();

            if (winnerNumber == 1)
            {
                Console.WriteLine(player1 + " won the game! " + player2 + " should feel ashamed!");
                scorePlayer1 += 1;
            }
            else if (winnerNumber == 2)
            {
                Console.WriteLine(player2 + " won the game! " + player1 + " is a utterly bad player!");
                scorePlayer2 += 1;
            }
            else if (winnerNumber == -1)
            {
                Console.WriteLine("It's a draw, Idiots!");
                draw += 1;
            }
            return winnerNumber;


        }
        private static int checkForWinner()
        {
            int[][] winningCombinations = new int[][] {
        // Rows
        new int[] {0, 1, 2},
        new int[] {3, 4, 5},
        new int[] {6, 7, 8},
        // Columns
        new int[] {0, 3, 6},
        new int[] {1, 4, 7},
        new int[] {2, 5, 8},
        // Diagonals
        new int[] {0, 4, 8},
        new int[] {2, 4, 6}
    };

            foreach (var combination in winningCombinations)
            {
                if (gameBoard.GetCell(combination[0]) != 0 &&
                    gameBoard.GetCell(combination[0]) == gameBoard.GetCell(combination[1]) &&
                    gameBoard.GetCell(combination[0]) == gameBoard.GetCell(combination[2]))
                {
                    return gameBoard.GetCell(combination[0]);
                }
            }

            //  draw
            bool isDraw = true;
            for (int i = 0; i < 9; i++)
            {
                if (gameBoard.GetCell(i) == 0)
                {
                    isDraw = false;
                    break;
                }
            }

            return isDraw ? -1 : 0;
        }


        private static void printBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                int cellValue = gameBoard.GetCell(i);
                // print x or o
                // 0 nothing, 1 is x, 2 is o
                switch (cellValue)
                {
                    case 0:
                        Console.Write(".");
                        break;
                    case 1:
                        Console.Write("X");
                        break;
                    case 2:
                        Console.Write("O");
                        break;
                }

                // lines
                if (i == 2 || i == 5 || i == 8)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}