namespace ClassLibrary
{
    public class TTT : Match
    {
        #region Variables



        #endregion

        /// <summary>
        /// Constructor for the TTT (Tic Tac Toe) class, setting up a 3x3 game board with a winning length of 3.
        /// </summary>
        public TTT() : base(3, 3, 3)
        {
            p_GameType = "TTT";

        }
        /// <summary>
        /// Overrides the base GameMechanic method to implement the game logic for Tic Tac Toe (TTT). 
        /// If playing with ChatGPT, it invokes ChatGPTMove method for the ChatGPT's turn. 
        /// Prompts the current player to input a number to choose a cell on the board. 
        /// Validates the input and if the cell is unoccupied, sets the cell with the current player's icon, 
        /// updates the match and move history, then switches the turn to the next player. 
        /// Displays the board after each move.
        /// </summary>

    }
}
