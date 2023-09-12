namespace GameFactory.Model
{
    internal class TTT : Match
    {

        public TTT()
        {
            p_winningLength = 3;
            p_rows = 3;
            p_columns = 3;
            p_board = new int[p_rows, p_columns];
        }

        public override void GameMechanic(List<Player> Players)
        {
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine($"{Players[0].p_name}, input a number from 0 to {p_rows * p_columns - 1}");

                bool isValidInput = int.TryParse(Console.ReadLine(), out int chosenCell);

                if (!isValidInput || chosenCell < 0 || chosenCell >= p_rows * p_columns)
                {
                    Console.WriteLine("Invalid number. Try again.");
                }
                else
                {
                    int row = chosenCell / p_columns;
                    int col = chosenCell % p_columns;

                    if (GetCell(row, col) == 0)
                    {
                        SetCell(row, col, p_currentPlayerIndex + 1);
                        p_currentPlayerIndex = (p_currentPlayerIndex + 1) % Players.Count;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Try again.");
                    }
                }
            }

            PrintBoard();
        }

    }
}