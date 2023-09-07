using TicTacToeC;


namespace TicTacToe
{
    internal class Game : GamesAvailable
    {
        private int[,] board;
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Game(int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
            this.board = new int[rows, columns];
            ResetBoard();
        }

        public (Player[], int) Start(Player[] players, int draw, int startingPlayerIndex)
        {

            int currentPlayerIndex = startingPlayerIndex;

            do
            {
                Console.WriteLine($"{players[currentPlayerIndex].Name}, input a number from 0 to {Rows * Columns - 1}");

                if (!int.TryParse(Console.ReadLine(), out int chosenCell) || chosenCell < 0 || chosenCell >= Rows * Columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int row = chosenCell / Columns;
                int col = chosenCell % Columns;

                if (GetCell(row, col) == 0)
                {
                    SetCell(row, col, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                    continue;
                }

                PrintBoard();
            } while (CheckWinner(3) == 0);

            int winnerNumber = CheckWinner(3);

            (players, draw) = Player.UpdateTTT(players, winnerNumber, draw);

            ResetBoard();
            return (players, draw);
        }
        public int CheckWinner(int winningLength)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    int cellValue = GetCell(row, col);
                    if (cellValue == 0) continue;

                    int[][] directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var dir in directions)
                    {
                        int count = 1;
                        for (int i = 1; i < winningLength; i++)
                        {
                            int newRow = row + dir[0] * i;
                            int newCol = col + dir[1] * i;
                            if (newRow < 0 || newRow >= Rows || newCol < 0 || newCol >= Columns) break;
                            if (GetCell(newRow, newCol) == cellValue) count++;
                            else break;
                        }
                        if (count >= winningLength) return cellValue;
                    }
                }
            }

            bool isDraw = !Enumerable.Range(0, Rows).Any(row => Enumerable.Range(0, Columns).Any(col => GetCell(row, col) == 0));
            return isDraw ? -1 : 0;
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
