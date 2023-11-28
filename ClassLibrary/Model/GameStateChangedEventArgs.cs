
namespace ClassLibrary
{
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