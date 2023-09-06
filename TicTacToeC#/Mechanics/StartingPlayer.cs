namespace TicTacToe
{
    internal class StartingPlayer
    {
        public int GetStartingPlayerIndex(Player[] players)
        {
            Random random = new Random();
            int startingPlayerIndex = random.Next(0, players.Length - 1);
            Console.WriteLine($"{players[startingPlayerIndex].Name} starts!");
            return startingPlayerIndex;
        }
    }
}