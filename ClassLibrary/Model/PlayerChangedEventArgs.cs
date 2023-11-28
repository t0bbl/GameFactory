namespace CoreGameFactory.Model
{
    public class PlayerChangedEventArgs
    {
        public string CurrentPlayer { get; private set; }


        public PlayerChangedEventArgs(string p_CurrentPlayer)
        {
            CurrentPlayer = p_CurrentPlayer;
        }
    }
}
