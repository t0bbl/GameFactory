using System;

namespace TicTacToeC.Model
{
    internal class TTT : Game
    {
         public TTT()
        {
            WinningLength = 3;
            rows = 3;
            columns = 3;
            board = new int[rows, columns];


        }
    }
}