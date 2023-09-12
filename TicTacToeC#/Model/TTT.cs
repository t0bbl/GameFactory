namespace GameFactory.Model
{
    internal class TTT : Match
    {
        public TTT() : base(3, 3, 3)
        { }

        public override void GameMechanic(List<Player> Players)
        {
            int chosenCell;
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine($"{Players[0].p_name}, input a number from 0 to {p_rows * p_columns - 1}");

                if (TryGetValidInput(out chosenCell, p_rows * p_columns))
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
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
            }

            PrintBoard();
        }
    }
}
