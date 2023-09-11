

namespace TicTacToeC
{
    internal class Game
    {
        public int[,] board;
        public int rows { get; set; }
        public int columns { get; set; }
        public int winningLength { get; set; }
        public Player[] players { get; set; }
        public int draw { get; set; }

        public int currentPlayerIndex { get; set; }

        Random random = new Random();

        public Game(Player[] players)
        {
            this.players = players;
        }

        public (Player[], int) StartGame(Player[] players)
        {
            this.players = players;

            do
            {
                GameMechanic();
            } while (CheckWinner() == 0);
            //TODO: checkwinner = checkgamestate (draw, win, lose)
            int winnerNumber = CheckWinner(winningLength);
            (players, draw) = UpdateStats(players, winnerNumber, draw);

            ReMatch();

            ResetBoard();
            return (players, draw);
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
        public void ReMatch()
        {
            Console.WriteLine("Do you want to rematch? (y/n)");
            string rematch = Console.ReadLine();
            if (rematch == "y")
            {
                ResetBoard();
                StartGame(players);
            }
            else if (rematch == "n")
            {
                Player.EndGameStats(players, draw);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
                ReMatch();
            }
        }
        public virtual (Player[] players, int draw) UpdateStats(Player[] players, int winnerNumber, int draw)
        {
            return (players, draw);
        }
        public virtual void GameMechanic()
        {
            currentPlayerIndex = random.Next(0, players.Length); 
        }

    }
}
