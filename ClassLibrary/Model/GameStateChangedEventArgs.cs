
namespace ClassLibrary
{
    public class GameStateChangedEventArgs : EventArgs
    {
        public int? Winner { get; private set; }

        public GameStateChangedEventArgs(int? winner)
        {
            Winner = winner;
        }
    }
}