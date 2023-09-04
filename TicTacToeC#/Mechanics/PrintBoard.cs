//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TicTacToe;

//namespace TicTacToe
//{
//    class PrintBoard
//    {
//        public static void printBoard(GameBoard gameBoard)
//        {
//            for (int i = 0; i < 9; i++)
//            {
//                int cellValue = gameBoard.GetCell(i);
//                // print x or o
//                // 0 nothing, 1 is x, 2 is o
//                switch (cellValue)
//                {
//                    case 0:
//                        Console.Write(".");
//                        break;
//                    case 1:
//                        Console.Write("X");
//                        break;
//                    case 2:
//                        Console.Write("O");
//                        break;
//                }

//                // lines
//                if (i == 2 || i == 5 || i == 8)
//                {
//                    Console.WriteLine();
//                }
//            }
//        }
//    }
//}
