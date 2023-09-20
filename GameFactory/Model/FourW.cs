namespace GameFactory.Model
{
    internal class FourW : Match
    {

        public FourW()
        {
            p_rows = 6;
            p_columns = 7;
            p_winningLength = 4;
            p_board = new char[p_rows, p_columns];
        }
        public override void GameMechanic(List<Player> p_player)
        {
            base.GameMechanic(p_player);

            int p_chosenColumn;
            if (p_firstTurn)
            {
                PrintBoard(false, true, p_player);
                Console.WriteLine();
                p_firstTurn = false;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{p_player[p_currentPlayerIndex].Name}, input a column number from 1 to {p_columns}");
            } while (!TryGetValidInput(out p_chosenColumn, p_columns));

            MakeMove(p_chosenColumn, p_currentPlayerIndex, p_player);
            p_currentPlayerIndex = (p_currentPlayerIndex + 1) % p_player.Count;

            PrintBoard(false, true, p_player);
        }
        public int FindLowestAvailableRow(int p_column)
        {
            for (int row = p_rows - 1; row >= 0; row--)
            {
                if (GetCell(row, p_column) == '0')
                {
                    return row;
                }
            }
            return -1;
        }
        public bool MakeMove(int p_chosenColumn, int p_currentPlayerIndex, List<Player> p_players)
        {
            int p_row = FindLowestAvailableRow(p_chosenColumn - 1);

            if (p_row != -1)
            {
                SetCell(p_row, p_chosenColumn - 1, p_players[p_currentPlayerIndex].Icon);
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