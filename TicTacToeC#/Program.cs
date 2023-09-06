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

            do
            {
                (players, draw) = match.StartGame(game, players, draw);

                Console.WriteLine("Want a rematch? Y/N?");
            } while (Console.ReadLine().ToLower() == "y");

            foreach (var player in players)
            {
                Console.WriteLine($"{player.Name} has won {player.Score} times!");
            }
            Console.WriteLine($"There were {draw} draws.");
            Console.ReadKey();
        }
    }
}
