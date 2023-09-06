namespace TicTacToe
{
    class CheckForWinner
    {
        public static int CheckWinner(GameBoard gameBoard, int winningLength)
        {
            for (int row = 0; row < gameBoard.Rows; row++)
            {
                for (int col = 0; col < gameBoard.Columns; col++)
                {
                    int cellValue = gameBoard.GetCell(row, col);
                    if (cellValue == 0) continue;

                    int[][] directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var dir in directions)
                    {
                        int count = 1;
                        for (int i = 1; i < winningLength; i++)
                        {
                            int newRow = row + dir[0] * i;
                            int newCol = col + dir[1] * i;
                            if (newRow < 0 || newRow >= gameBoard.Rows || newCol < 0 || newCol >= gameBoard.Columns) break;
                            if (gameBoard.GetCell(newRow, newCol) == cellValue) count++;
                            else break;
                        }
                        if (count >= winningLength) return cellValue;
                    }
                }
            }

            bool isDraw = !Enumerable.Range(0, gameBoard.Rows).Any(row => Enumerable.Range(0, gameBoard.Columns).Any(col => gameBoard.GetCell(row, col) == 0));
            return isDraw ? -1 : 0;
        }
    }
}
