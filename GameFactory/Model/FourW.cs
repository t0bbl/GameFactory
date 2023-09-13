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
                Console.WriteLine($"{Players[p_currentPlayerIndex].p_name}, input a column number from 1 to {p_columns}");
            } while (!TryGetValidInput(out chosenColumn, p_columns));

            int row = FindLowestAvailableRow(chosenColumn - 1);

            if (row != -1)
            {
                SetCell(row, chosenColumn - 1, p_currentPlayerIndex + 1);
                p_currentPlayerIndex = (p_currentPlayerIndex + 1) % Players.Count;
            }
            else
            {
                Console.WriteLine("Column is full. Try again.");
            }
            PrintBoard(false, true);
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