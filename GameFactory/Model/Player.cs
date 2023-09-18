namespace GameFactory
{
    public class Player
    {
        public string Name { get; set; }
        public char Icon { get; set; }
        public string Colour { get; set; }
        public bool IsHuman { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public Guid Id { get; private set; }
        public Player()
        {
            Id = Guid.NewGuid();
        }

    }
}
