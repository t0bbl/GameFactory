namespace ClassLibrary
{
    public class FourW : Match
    {
        #region Variables
        internal bool ChatGPT { get; set; }
        #endregion
        /// <summary>
        /// Initializes a new instance of the FourW class with specified board dimensions and winning length.
        /// </summary>
        public FourW() : base(6, 7, 4)
        {
            p_GameType = ChatGPT ? "FourWChatGPT" : "FourW";
        }
        /// <summary>
        /// Handles the game mechanics for FourW.
        /// </summary>
        /// <param name="p_Player">The list of players.</param>
        public override void GameMechanic(List<Player> p_Player)
        {
    

            base.GameMechanic(p_Player);

            int p_chosenColumn;
            if (FirstTurn)
            {
                Console.WriteLine();
                FirstTurn = false;
            }
            do
            {
                Console.WriteLine();
                Console.WriteLine($"{p_Player[CurrentPlayerIndex].Name}, input a column number from 1 to {Columns}");
                SavePlayerToMatch(p_Player[CurrentPlayerIndex].Ident, MatchId);
            } while (!TryGetValidInput(out p_chosenColumn, Columns));

            MakeMove(p_chosenColumn, CurrentPlayerIndex, p_Player);
            string p_cell = p_chosenColumn.ToString();
            SaveMoveHistory(p_Player[CurrentPlayerIndex].Ident, p_cell, MatchId, TwistStat);

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % p_Player.Count;

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
                if (GetCell(Row, p_Column) == '0')
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

            if (p_row != -1)
            {
                SetCell(p_row, p_ChosenColumn - 1, p_Players[p_CurrentPlayerIndex].Icon);
                return true;
            }
            else
            {
                Console.WriteLine("Column is full. Try again.");
                return false;
            }
        }
        #endregion
    }
}