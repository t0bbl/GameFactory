namespace TicTacToe
{
    public class TTT
    {
        private GameBoard gameBoard;

        public (Player[], int) Start(Player[] players, int draw)
        {
            StartingPlayer startingPlayer = new StartingPlayer();
            int currentPlayerIndex = startingPlayer.GetStartingPlayerIndex(players);

            gameBoard = new GameBoard(3, 3);

            while (CheckForWinner.CheckWinner(gameBoard, 3) == 0)
            {
                Console.WriteLine($"{players[currentPlayerIndex].Name}, input a number from 0 to {gameBoard.Rows * gameBoard.Columns - 1}");

                if (!int.TryParse(Console.ReadLine(), out int chosenCell) || chosenCell < 0 || chosenCell >= gameBoard.Rows * gameBoard.Columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }

                int row = chosenCell / gameBoard.Columns;
                int col = chosenCell % gameBoard.Columns;

                if (gameBoard.GetCell(row, col) == 0)
                {
                    gameBoard.SetCell(row, col, currentPlayerIndex + 1);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                    continue;
                }

                gameBoard.PrintBoard();
            }

            int winnerNumber = CheckForWinner.CheckWinner(gameBoard, 3);

            if (winnerNumber > 0)
            {
                Console.WriteLine($"{players[winnerNumber - 1].Name} won the game!");
                players[winnerNumber - 1].Score += 1;
            }
            else
            {
                Console.WriteLine("It's a draw!");
                draw++;
            }

            gameBoard.ResetBoard();
            return (players, draw);
        }
    }
}
