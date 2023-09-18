namespace GameFactory.Model
{
    internal class TTT : Match
    {
        private bool FirstTurn = true;
        public TTT()
        {
            p_rows = 3;
            p_Columns = 3;
            p_WinningLength = 3;
            p_board = new char[p_rows, p_Columns];
        }
        public override void GameMechanic(List<Player> p_player)
        {

            int chosenCell;
            bool validInput = false;
            if (FirstTurn)
            {
                PrintBoard(false, false, p_player);
                Console.WriteLine();
                FirstTurn = false;
            }
            while (!validInput)
            {
                Console.WriteLine($"{p_player[p_CurrentPlayerIndex].Name}, input a number from 1 to {p_rows * p_Columns}");

                if (TryGetValidInput(out chosenCell, p_rows * p_Columns))
                {
                    int row = (chosenCell - 1) / p_Columns;
                    int col = (chosenCell - 1) % p_Columns;

                    if (GetCell(row, col) == '0')
                    {
                        SetCell(row, col, p_player[p_CurrentPlayerIndex].Icon);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_player.Count;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
            }
            PrintBoard(false, false, p_player);
        }
        public override void ResetFirstTurn()
        {
            FirstTurn = true;
        }

    }
}
