namespace ClassLibrary
{
    public class FourW : Match
    {

        public FourW() : base(6, 7, 4)
        {
            GameType = "FourW";
        }



        private void HandleGameCellClicked(int p_ChosenColumn)
        {
            MakeMove(p_ChosenColumn, CurrentPlayerIndex, PlayerList);
            string Cell = p_ChosenColumn.ToString();
            EndTurn(Cell);
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
            int Row = FindLowestAvailableRow(p_ChosenColumn);

            SetCell(Row, p_ChosenColumn, p_Players[p_CurrentPlayerIndex].Icon);
            return true;

        }
        #endregion
        #region Events
        public override void GameCellClicked(object sender, GameCellClickedEventArgs e)
        {
            HandleGameCellClicked(e.Column);
        }
        #endregion
    }
}