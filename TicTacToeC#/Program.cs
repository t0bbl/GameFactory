namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            int draw = 0;
            Match match = new();


            Player[] players = InitializePlayer.Initialize();
            string game = InitializeGame.Initialize();
            (players, draw) = match.StartGame(game, players, draw);

            foreach (var player in players)
            {
                Console.WriteLine($"{player.Name} has won {player.Score} times!");
            }
            Console.WriteLine($"There were {draw} draws.");
            Console.ReadKey();
        }
    }
}
