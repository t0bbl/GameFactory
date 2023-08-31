using System;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace TicTacToe
{
    class Program
    {
        static string player1, player2, game;
        static int scorePlayer1, scorePlayer2, draw, winnerNumber;
        static GameBoard gameBoard;

        static void Main(string[] args)
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
                    gameBoard = new GameBoard(7,6);
                    validChoice = true;
                }
                else
                {
                    Console.WriteLine("Try again, idiot.");
                }
            }

            Console.WriteLine("Input name of first Challenger");
            player1 = Console.ReadLine();
            Console.WriteLine("Input name of second Challenger");
            player2 = Console.ReadLine();

            do
            {
                var play = new PlayGame(gameBoard);

                (winnerNumber, scorePlayer1, scorePlayer2, draw) =  play.playGame(game, player1, player2, scorePlayer1, scorePlayer2, draw);

                gameBoard.ResetBoard();

                Console.WriteLine("Want a rematch Noob ? Y / N ?");


            } while (Console.ReadLine().ToLower() == "y");
            Console.WriteLine("Score:  " + player1 + " : " + scorePlayer1 + " !     " + player2 + " : " + scorePlayer2 + "!     Draw: " + draw);

        }


    }
}