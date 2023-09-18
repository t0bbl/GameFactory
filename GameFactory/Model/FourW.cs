namespace GameFactory.Model
{
    internal class FourW : Match
    {
        private bool FirstTurn = true;

        public FourW()
        {
            p_rows = 6;
            p_Columns = 7;
            p_WinningLength = 4;
            p_board = new char[p_rows, p_Columns];
        }
        public override void GameMechanic(List<Player> p_player)
        {
            int chosenColumn;
            if (FirstTurn)
            {
                PrintBoard(false, true, p_player);
                Console.WriteLine();
                FirstTurn = false;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{p_player[p_CurrentPlayerIndex].Name}, input a column number from 1 to {p_Columns}");
            } while (!TryGetValidInput(out chosenColumn, p_Columns));

            MakeMove(chosenColumn, p_CurrentPlayerIndex, p_player);
            p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_player.Count;

            PrintBoard(false, true, p_player);
        }
        public int FindLowestAvailableRow(int column)
        {
            for (int row = p_rows - 1; row >= 0; row--)
            {
                if (GetCell(row, column) == '0')
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
                SetCell(row, chosenColumn - 1, p_players[p_CurrentPlayerIndex].Icon);
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