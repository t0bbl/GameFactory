using GameFactory.SQL;

namespace GameFactory.Model
{
    internal class TTT : Match
    {
        public TTT()
        {
            p_gameType = "TTT";
            p_rows = 3;
            p_columns = 3;
            p_winningLength = 3;
            p_board = new char[p_rows, p_columns];
        }
        public override void GameMechanic(List<Player> p_player)
        {
            base.GameMechanic(p_player);
            int p_chosenCell = 0;
            bool p_validInput = false;
            if (p_firstTurn)
            {
                PrintBoard(false, false, p_player);
                Console.WriteLine();
                p_firstTurn = false;
            }
            while (!p_validInput)
            {
                SQLPlayerService playerService = new SQLPlayerService();
                Console.WriteLine($"{p_player[p_currentPlayerIndex].Name}, input a number from 1 to {p_rows * p_columns}");

                if (TryGetValidInput(out p_chosenCell, p_rows * p_columns))
                {
                    int row = (p_chosenCell - 1) / p_columns;
                    int col = (p_chosenCell - 1) % p_columns;

                    if (GetCell(row, col) == '0')
                    {
                        SetCell(row, col, p_player[p_currentPlayerIndex].Icon);
                        Console.WriteLine($"{p_player[p_currentPlayerIndex].Name} has chosen cell {p_chosenCell}");
                        Console.WriteLine($"{p_player[p_currentPlayerIndex].Ident} has chosen cell {p_chosenCell}");

                        playerService.SavePlayerList(p_player[p_currentPlayerIndex].Ident, p_matchId);
                        p_currentPlayerIndex = (p_currentPlayerIndex + 1) % p_player.Count;
                        p_validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
            }
            PrintBoard(false, false, p_player);
            Console.WriteLine($"{p_chosenCell}this is chosencell");
            string p_cell = p_chosenCell.ToString();
            Console.WriteLine($"{p_cell}this is p_cell");
            SQLMoveHistory.SaveMoveHistory(p_player[p_currentPlayerIndex].Ident, p_cell, p_matchId);

        }
    }
}
