namespace GameFactory.Model
{
    internal class TTT : Match
    {
        private bool FirstTurn = true;
        private bool p_gpt;
        public TTT(bool p_gpt) : base(3, 3, 3)
        {
        }
        public override void GameMechanic(List<Player> p_players, bool p_gpt)
        {
            if (this.p_gpt && p_CurrentPlayerIndex == 1)
            {
                ChatGPTMove(BoardToString(p_players), p_players);
            }
            int chosenCell;
            bool validInput = false;
            if (FirstTurn)
            {
                PrintBoard(false, false, p_players);
                Console.WriteLine();
                FirstTurn = false;
            }
            while (!validInput)
            {
                Console.WriteLine($"{p_players[p_CurrentPlayerIndex].Name}, input a number from 1 to {p_rows * p_Columns}");

                if (TryGetValidInput(out chosenCell, p_rows * p_Columns))
                {
                    int row = (chosenCell - 1) / p_Columns;
                    int col = (chosenCell - 1) % p_Columns;

                    if (GetCell(row, col) == 0)
                    {
                        SetCell(row, col, p_CurrentPlayerIndex + 1);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_players.Count;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
            }
            PrintBoard(false, false, p_players);
        }
        public override void ResetFirstTurn()
        {
            FirstTurn = true;
        }

    }
}
