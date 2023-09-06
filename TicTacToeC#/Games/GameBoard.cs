namespace TicTacToe
{
    public class GameBoard
    {
        private int[,] board;
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public GameBoard(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            board = new int[rows, columns];
            ResetBoard();
        }
        public void ResetBoard()
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    board[i, j] = 0;
        }

        public int GetCell(int row, int col)
        {
            return board[row, col];
        }

        public void SetCell(int row, int col, int value)
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
                board[row, col] = value;
        }
        public void PrintBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    int cellValue = GetCell(row, col);

                    switch (cellValue)
                    {
                        case 0:
                            Console.Write(" . ");
                            break;
                        case 1:
                            Console.Write(" X ");
                            break;
                        case 2:
                            Console.Write(" O ");
                            break;
                    }
                }
                Console.WriteLine();
            }


        }
    }
}

