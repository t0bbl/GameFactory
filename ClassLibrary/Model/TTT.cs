namespace ClassLibrary
{
    public class TTT : Match
    {
        /// <summary>
        /// Constructor for the TTT (Tic Tac Toe) class, setting up a 3x3 game board with a winning length of 3.
        /// </summary>
        public TTT() : base(3, 3, 3)
        {
            GameType = "TTT";
        }
    }
}
