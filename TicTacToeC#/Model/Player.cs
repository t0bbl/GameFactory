namespace TicTacToeC
{
    internal class Player
    {
        public string Name;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int Score { get; set; }
        public int Number { get; set; }

        public Guid id { get; private set; }

        public Player()
        {
            id = Guid.NewGuid();
        }

        public static (List<Player> Players, int) UpdateStats(List<Player> Players, int winnerNumber, int draw)
        {

            if (winnerNumber > 0)
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    if (i == winnerNumber - 1)
                    {
                        Console.WriteLine($"{Players[i].Name} won the game!");
                        Players[i].Score += 1;
                        Players[i].Wins += 1;
                    }
                    else
                    {
                        Players[i].Losses += 1;
                    }
                }
                return (Players, draw);
            }
            else
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].Draws += 1;
                }
                draw++;
                Console.WriteLine("It's a draw!");
                return (Players, draw);
            }



        }

        public static void EndGameStats(List<Player> Players, int draw)
        {
            Console.WriteLine("Game over!");
            Console.WriteLine("Final scores:");
            foreach (var player in Players)
            {
                Console.WriteLine($"{player.Name}: {player.Score}");
            };
            if (draw > 0)
            {
                Console.WriteLine($"Draws:{draw}");
            }
            Console.ReadLine();

        }
    }
}