namespace GameFactory.Model
{
    internal class FourW : Match
    {

        public FourW() : base(6, 7, 4)
        { }
        public override void GameMechanic(List<Player> Players)
        {
            int chosenColumn;
            do
            {
                Console.WriteLine($"{Players[0].p_name}, input a column number from 0 to {p_columns - 1}");
            } while (!TryGetValidInput(out chosenColumn, p_columns));

            int row = FindLowestAvailableRow(chosenColumn);

            if (row != -1)
            {
                SetCell(row, chosenColumn, p_currentPlayerIndex + 1);
                p_currentPlayerIndex = (p_currentPlayerIndex + 1) % Players.Count;
            }
            else
            {
                Console.WriteLine("Column is full. Try again.");
            }
            PrintBoard();
        }
        public int FindLowestAvailableRow(int column)
        {
            for (int row = p_rows - 1; row >= 0; row--)
            {
                if (GetCell(row, column) == 0)
                {
                    return row;
                }
            }
            return -1;
        }

    }

}