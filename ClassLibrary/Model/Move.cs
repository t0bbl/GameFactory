namespace CoreGameFactory.Model
{
    public class Move
    {
        public int Player { get; set; }
        public int Match { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Twist { get; set; }
        public string PlayerName { get; set; }
    }
}
