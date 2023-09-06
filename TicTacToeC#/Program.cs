namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            int draw = 0;
            Match match = new();

            Player[] players = InitializePlayer.Initialize();
            GamesAvailable gameInstance = InitializeGame.Initialize(); // Changed type here
            (players, draw) = match.StartGame(gameInstance, players, draw); // Changed argument here

            foreach (var player in players)
            {

                Console.WriteLine($"{player.Name} has won {player.Score} times!");
            }
            Console.WriteLine($"There were {draw} draws.");
            Console.ReadKey();
        }
    }
}
