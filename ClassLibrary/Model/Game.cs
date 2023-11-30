
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
}



