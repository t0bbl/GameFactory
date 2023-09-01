using System;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace TicTacToe
{
    class Program
    {
        static string player1, player2, game;
        static int scorePlayer1, scorePlayer2, draw, winnerNumber;

        static void Main(string[] args)
        {

            string[] names = Player.PlayerNames(args);
            player1 = names[0];
            player2 = names[1]; 

            do
            {
                PlayGame play = new PlayGame();
                (winnerNumber, scorePlayer1, scorePlayer2, draw) =  PlayGame.StartGame( player1, player2, scorePlayer1, scorePlayer2, draw);


                Console.WriteLine("Want a rematch Noob ? Y / N ?");


            } while (Console.ReadLine().ToLower() == "y");

            Console.WriteLine("Score:  " + player1 + " : " + scorePlayer1 + " !     " + player2 + " : " + scorePlayer2 + "!     Draw: " + draw);

        }


    }
}