
namespace ClassLibrary
{
    public class Game
    {
        #region Variables
        public List<Player> PlayerList { get; set; } = new();
        internal string GameType { get; set; }
        internal string GameMode { get; set; }
        internal Match CurrentMatch { get; set; }
        internal int GuestCount { get; set; } = 0;

        #endregion

    }

    public class GameStateChangedEventArgs : EventArgs
    {
        public int? Winner { get; private set; }
        public int? Draw { get; private set; }

        public GameStateChangedEventArgs(int? p_Winner, int? p_Draw)
        {
            Winner = p_Winner;
            Draw = p_Draw;
        }
    }
}



