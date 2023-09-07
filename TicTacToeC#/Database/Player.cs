namespace TicTacToe
{
    public class Player
    {
        public string Name;
        public int Score;
        public int Number;

        public Guid Id { get; private set; }

        public Player()
        {
            Id = Guid.NewGuid();
        }
    }
}