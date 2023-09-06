namespace TicTacToe
{
    public class FourW
    {
        private GameBoard gameBoard;
        private Dictionary<int, int> stack;

        public (Player[], int) Start(Player[] players, int draw)
        {
            gameBoard = new GameBoard(6, 7);
            stack = new Dictionary<int, int>();
            int currentPlayerIndex = 0;

            for (int i = 0; i < 8; i++)
            {
                stack[i] = 5;
            }

            while (CheckForWinner.CheckWinner(gameBoard, 4) == 0)
            {
                Console.WriteLine($"{players[currentPlayerIndex].Name}, input a number from 0 to 7");

                if (!int.TryParse(Console.ReadLine(), out int chosenColumn) || chosenColumn < 0 || chosenColumn >= 8)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                if (stack[chosenColumn] >= 0)
                {
                    int row = stack[chosenColumn];
                    stack[chosenColumn]--;
                    gameBoard.SetCell(row, chosenColumn, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                }
                else
                {
                    Console.WriteLine("Invalid move, the column is full. Try again.");
                    continue;
                }

                gameBoard.PrintBoard();
            }

            int winnerNumber = CheckForWinner.CheckWinner(gameBoard, 4);
            Player winner = players[winnerNumber - 1];

            if (winnerNumber > 0)
            {
                Console.WriteLine($"{winner.Name} won the game!");
                winner.Score++;
            }
            else
            {
                Console.WriteLine("It's a draw!");
                draw++;
            }

            return (players, draw);
        }
    }
}
