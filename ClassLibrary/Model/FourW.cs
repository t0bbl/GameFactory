namespace ClassLibrary
{
    public class FourW : Match
    {

        public FourW() : base(6, 7, 4)
        {
            p_GameType = "FourW";
        }

        public override void CellClicked(object sender, CellClickedEventArgs e)
        {
            int p_chosenColumn;

            do
            {
                SavePlayerToMatch(p_Player[CurrentPlayerIndex].Ident, MatchId);
            } while (!TryGetValidInput(out p_chosenColumn, Columns));

            MakeMove(p_chosenColumn, CurrentPlayerIndex, p_Player);
            string p_cell = p_chosenColumn.ToString();
            SaveMoveHistory(p_Player[CurrentPlayerIndex].Ident, p_cell, MatchId, TwistStat);

            Winner = CheckWinner(p_Player);

            if (Winner != null)
            {
                UpdateStats(p_Player);

                SaveMatch(Winner, Loser, Draw, GameTypeIdent, MatchId);

                MatchId = 0;

                OnGameStateChanged(new GameStateChangedEventArgs(Winner, Draw));
            }
        }

        #region GameUtilities
        /// <summary>
        /// Finds the lowest available row in a specified column.
        /// </summary>
        /// <param name="p_Column">The column to check.</param>
        /// <returns>The row index of the lowest available row, or -1 if the column is full.</returns>
        public int FindLowestAvailableRow(int p_Column)
        {
            for (int Row = Rows - 1; Row >= 0; Row--)
            {
                if (GetCell(Row, p_Column) == "0")
                {
                    return Row;
                }
            }
            return -1;
        }
        /// <summary>
        /// Makes a move for the current player in the specified column.
        /// </summary>
        /// <param name="p_ChosenColumn">The column in which the move is made (1-based index).</param>
        /// <param name="p_CurrentPlayerIndex">The index of the current player in the player list.</param>
        /// <param name="p_Players">The list of players.</param>
        /// <returns>True if the move is successfully made, false if the column is full.</returns>
        public bool MakeMove(int p_ChosenColumn, int p_CurrentPlayerIndex, List<Player> p_Players)
        {
            int p_row = FindLowestAvailableRow(p_ChosenColumn - 1);

                SetCell(p_row, p_ChosenColumn - 1, p_Players[p_CurrentPlayerIndex].Icon);
                return true;

        }
        #endregion
    }
}