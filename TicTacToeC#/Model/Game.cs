

namespace TicTacToeC
{
    internal class Game
    {
        public int[,] p_board;
        public int p_rows { get; set; }
        public int p_columns { get; set; }
        public int p_winningLength { get; set; }

        public int p_currentPlayerIndex { get; set; }

        Random p_random = new Random();



        public List<Player> StartGame(List<Player> p_Players)
        {
            ShufflePlayers(p_Players);
            do
            {
                GameMechanic(p_Players);
            } while (CheckWinner() == 0);
            //TODO: checkwinner = checkgamestate (draw, win, lose)
            int p_winnerNumber = CheckWinner();
            if (p_winnerNumber != 0)
            {
                (p_Players) = Player.UpdateStats(p_Players, p_winnerNumber);

            }


            ReMatch(p_Players);

            ResetBoard();
            return (p_Players);
        }

        public int CheckWinner()
        {
            for (int row = 0; row < p_rows; row++)
            {
                for (int col = 0; col < p_columns; col++)
                {
                    int cellValue = GetCell(row, col);
                    if (cellValue == 0) continue;

                    int[][] directions = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
                    foreach (var dir in directions)
                    {
                        int count = 1;
                        for (int playerRow = 1; playerRow < p_winningLength; playerRow++)
                        {
                            int newRow = row + dir[0] * playerRow;
                            int newCol = col + dir[1] * playerRow;
                            if (newRow < 0 || newRow >= p_rows || newCol < 0 || newCol >= p_columns) break;
                            if (GetCell(newRow, newCol) == cellValue) count++;
                            else break;
                        }
                        if (count >= p_winningLength) return cellValue;
                    }
                }
            }

            bool isDraw = !Enumerable.Range(0, p_rows).Any(row => Enumerable.Range(0, p_columns).Any(col => GetCell(row, col) == 0));
            return isDraw ? -1 : 0;
        }
        public void ResetBoard()
        {
            for (int playedRow = 0; playedRow < p_rows; playedRow++)
                for (int playedColumn = 0; playedColumn < p_columns; playedColumn++)
                    p_board[playedRow, playedColumn] = 0;
        }
        public int GetCell(int row, int col)
        {
            return p_board[row, col];
        }
        public void SetCell(int row, int col, int value)
        {
            if (row >= 0 && row < p_rows && col >= 0 && col < p_columns)
                p_board[row, col] = value;
        }
        public void PrintBoard()
        {
            for (int row = 0; row < p_rows; row++)
            {
                for (int col = 0; col < p_columns; col++)
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
                Player.EndGameStats(Players);
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
                int j = p_random.Next(i + 1);
                // Swap Players[i] and Players[j]
                Player temp = Players[i];
                Players[i] = Players[j];
                Players[j] = temp;
            }
        }

    }
}
