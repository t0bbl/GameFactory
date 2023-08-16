using System;

namespace TicTacToe
{
    class Program
    {
        static int[] board = new int[9];
        static void Main(string[] args)
        {
            for (int i = 0; i < 9; i++)
            {
                board[i] = 0;
            }
            Console.WriteLine("Input name of first Challenger");
            string player1 = Console.ReadLine();
            Console.WriteLine("Input name of second Challenger");
            string player2 = Console.ReadLine();


            int player1turn = -1;
            int player2turn = -1;
            Random rand = new Random();

            int startingPlayer = rand.Next(1, 3);

            Console.WriteLine(startingPlayer == 1 ? player1 + " starts!" : player2 + " starts!");


            while (checkForWinner() == 0)
            {

                while (player1turn == -1 ||  board[player1turn] != 0)
                {
                    Console.WriteLine(player1 + " input a number from 0 to 8");
                    player1turn = int.Parse(Console.ReadLine());
                    Console.WriteLine(player1 + " typed " + player1turn);
                }
                board[player1turn] = 1;

                printBoard();

                while (player2turn == -1 || board[player2turn] != 0)
                // // for the lonely days
                //                {
                //                    player2turn = rand.Next(8);
                //                    Console.WriteLine(player2 + " typed " + player2turn);
                //                }
                {
                    Console.WriteLine(player2 + " input a number from 0 to 8");
                    player2turn = int.Parse(Console.ReadLine());
                    Console.WriteLine(player2 + " typed " + player2turn);
                }
                board[player2turn] = 2;

                printBoard();
            }
            int winnerNumber = checkForWinner();

            if (winnerNumber == 1)
            {
                Console.WriteLine(player1 + " won the game! " + player2 + " should feel ashamed!");
            }
            else if (winnerNumber == 2)
            {
                Console.WriteLine(player2 + " won the game! " + player1 + " is a utterly bad player");
            }
            else
            {
                Console.WriteLine("It's a draw, Idiots!");
            }


        }


        private static int checkForWinner()
        {
            // row 1-3
            if (board[0] == board[1] && board[0] == board[2])
            {
             //   Console.WriteLine("exit1");
                return board[0];
            }
            if (board[3] == board[4] && board[3] == board[5])
            {
             //   Console.WriteLine("exit2");
                return board[3];
            }
            if (board[6] == board[7] && board[6] == board[8])
            {
             //   Console.WriteLine("exit3");
                return board[6];
            }
            // column 1-3
            if (board[0] == board[3] && board[0] == board[6])
            {
             //   Console.WriteLine("exit4");
                return board[0];
            }
            if (board[1] == board[4] && board[4] == board[7])
            {
             //   Console.WriteLine("exit5");
                return board[1];
            }
            if (board[2] == board[5] && board[2] == board[8])
            {
             //   Console.WriteLine("exit6");
                return board[2];
            }
            // diagonal
            if (board[0] == board[4] && board[0] == board[8])
            {
             //   Console.WriteLine("exit7");
                return board[0];
            }
            if (board[2] == board[4] && board[4] == board[6])
            {
             //   Console.WriteLine("exit8");
                return board[2];
            }
            // draw
            {
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 0)
                    {
            //            Console.WriteLine("exitdraw");

                        return -1;
                    }
                }
            }
            return 0;

        }
        private static void printBoard()
        {

            for (int i = 0; i < 9; i++)
            {
                // print the board
                // print x or o
                // 0 nothing, 1 is x, 2 is o
                if (board[i] == 0)
                {
                    Console.Write(".");
                }
                if (board[i] == 1)
                {
                    Console.Write("X");
                }
                if (board[i] == 2)
                {
                    Console.Write("O");
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