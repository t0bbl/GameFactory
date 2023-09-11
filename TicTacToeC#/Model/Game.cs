

namespace TicTacToeC
{
    internal class Game
    {
        public int[,] board;
        public int rows { get; set; }
        public int columns { get; set; }
        public int winningLength { get; set; }
        public int draw { get; set; }

        public int currentPlayerIndex { get; set; }

        Random random = new Random();


        public (List<Player> Players, int) StartGame(List<Player> Players)
        {
            ShufflePlayers(Players);
            do
            {
                GameMechanic(Players);
            } while (CheckWinner() == 0);
            //TODO: checkwinner = checkgamestate (draw, win, lose)
            int winnerNumber = CheckWinner();
            (Players, draw) = Player.UpdateStats(Players, winnerNumber, draw);

            ReMatch(Players);

            ResetBoard();
            return (Players, draw);
        }

        public int CheckWinner()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int cellValue = GetCell(row, col);
                    if (cellValue == 0) continue;

                    int[][] directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var dir in directions)
                    {
                        int count = 1;
                        for (int playerRow = 1; playerRow < winningLength; playerRow++)
                        {
                            int newRow = row + dir[0] * playerRow;
                            int newCol = col + dir[1] * playerRow;
                            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= columns) break;
                            if (GetCell(newRow, newCol) == cellValue) count++;
                            else break;
                        }
                        if (count >= winningLength) return cellValue;
                    }
                }
            }

            bool isDraw = !Enumerable.Range(0, rows).Any(row => Enumerable.Range(0, columns).Any(col => GetCell(row, col) == 0));
            return isDraw ? -1 : 0;
        }
        public void ResetBoard()
        {
            for (int playedRow = 0; playedRow < rows; playedRow++)
                for (int playedColumn = 0; playedColumn < columns; playedColumn++)
                    board[playedRow, playedColumn] = 0;
        }
        public int GetCell(int row, int col)
        {
            return board[row, col];
        }
        public void SetCell(int row, int col, int value)
        {
            if (row >= 0 && row < rows && col >= 0 && col < columns)
                board[row, col] = value;
        }
        public void PrintBoard()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
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
        public void ReMatch(List<Player> Players)
        {
            Console.WriteLine("Do you want to rematch? (y/n)");
            string rematch = Console.ReadLine();
            if (rematch == "y")
            {
                ResetBoard();
                StartGame(Players);
            }
            else if (rematch == "n")
            {
                Player.EndGameStats(Players, draw);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
                ReMatch(Players);
            }
        }
        public virtual void GameMechanic(List<Player> Players)
        {
        }
        public void ShufflePlayers(List<Player> Players)
        {
            int n = Players.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Swap Players[i] and Players[j]
                Player temp = Players[i];
                Players[i] = Players[j];
                Players[j] = temp;
            }
        }

    }
}
