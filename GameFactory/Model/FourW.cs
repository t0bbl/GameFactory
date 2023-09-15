namespace GameFactory.Model
{
    internal class FourW : Match
    {
        private bool FirstTurn = true;

        public FourW() : base(6, 7, 4)
        { }
        public override void GameMechanic(List<Player> p_players)
        {
            int chosenColumn;
            if (FirstTurn)
            {
                PrintBoard(false, true, p_players);
                Console.WriteLine();
                FirstTurn = false;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{p_players[p_CurrentPlayerIndex].Name}, input a column number from 1 to {p_Columns}");
            } while (!TryGetValidInput(out chosenColumn, p_Columns));

            MakeMove(chosenColumn, p_CurrentPlayerIndex, p_players);
            p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_players.Count;

            PrintBoard(false, true, p_players);
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
        public override void ResetFirstTurn()
        {
            FirstTurn = true;
        }

        public bool MakeMove(int chosenColumn, int p_CurrentPlayerIndex, List<Player> p_players)
        {
            int row = FindLowestAvailableRow(chosenColumn - 1);

            if (row != -1)
            {
                SetCell(row, chosenColumn - 1, p_CurrentPlayerIndex + 1);
                return true;
            }
            else
            {
                Console.WriteLine("Column is full. Try again.");
                return false;
            }
        }
    }

}