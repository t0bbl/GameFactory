using CoreGameFactory.Model;

namespace ClassLibrary
{
    public class FourW : Match
    {

        public FourW() : base(6, 7, 4)
        {
            GameType = "FourW";
        }

        public override void CellClicked(object sender, CellClickedEventArgs e)
        {
            int ChosenColumn = e.Column;


            SavePlayerToMatch(PlayerList[CurrentPlayerIndex].Ident, MatchId);
            CurrentPlayer = PlayerList[CurrentPlayerIndex].Name;
            OnPlayerChanged(new PlayerChangedEventArgs(CurrentPlayer));
            MakeMove(ChosenColumn, CurrentPlayerIndex, PlayerList);
            string p_cell = ChosenColumn.ToString();
            SaveMoveHistory(PlayerList[CurrentPlayerIndex].Ident, p_cell, MatchId, TwistStat);

            Winner = CheckWinner(PlayerList);

            if (Winner != null)
            {
                UpdateStats(PlayerList);

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
                int Row = FindLowestAvailableRow(p_ChosenColumn);

                SetCell(Row, p_ChosenColumn, p_Players[p_CurrentPlayerIndex].Icon);
                return true;

        }
        #endregion
    }
}