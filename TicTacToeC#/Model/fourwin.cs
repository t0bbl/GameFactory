//using TicTacToeC;

//namespace TicTacToe
//{
//    internal class FourW : Game
//    {
//        private List<int> stack;

//        private const int WinningLength = 4;
//        private int columns = 7;
//        private int rows = 6;
//        public FourW(int rows, int columns) : base(rows, columns)
//        {
//            stack = new List<int>();
//        }


//        public (Player[], int) Start(Player[] players, int draw, int startingPlayerIndex)
//        {
//            int currentPlayerIndex = startingPlayerIndex;

//            for (int i = 0; i < columns; i++)
//            {
//                stack.Add(Rows - 1);
//            }

//            while (CheckWinner( WinningLength) == 0)
//            {
//                Console.WriteLine($"{players[currentPlayerIndex].Name}, input a number from 0 to {columns - 1}");

//                if (!int.TryParse(Console.ReadLine(), out int chosenColumn) || chosenColumn < 0 || chosenColumn >= columns)
//                {
//                    Console.WriteLine("Invalid number. Try again.");
//                    continue;
//                }

//                if (stack[chosenColumn] >= 0)
//                {
//                    int row = stack[chosenColumn];
//                    stack[chosenColumn]--;
//                    SetCell(row, chosenColumn, currentPlayerIndex + 1);
//                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
//                }
//                else
//                {
//                    Console.WriteLine("Invalid move, the column is full. Try again.");
//                    continue;
//                }

//                PrintBoard();
//            }

//            int winnerNumber = CheckWinner(WinningLength);
//            (players, draw) = Player.UpdateTTT(players, winnerNumber, draw);

//            return (players, draw);
//        }
//    }
//}
