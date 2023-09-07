namespace TicTacToe
{
    public class TTT : GamesAvailable
    {
        private GameBoard gameBoard;

        public (Player[], int) Start(Player[] players, int draw, int startingPlayerIndex)
        {

            int currentPlayerIndex = startingPlayerIndex;
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
                Stats.UpdateTTT(players, winnerNumber);
            }
            else
            {
                Stats.UpdateDrawTTT(players);
            }

            gameBoard.ResetBoard();
            return (players, draw);
        }
    }
}
