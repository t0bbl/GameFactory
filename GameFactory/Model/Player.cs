namespace GameFactory
{
    internal class Player
    {
        public string Name;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public string Icon;
        public Guid Id { get; private set; }
        public Player()
        {
            Id = Guid.NewGuid();
        }
        public static List<Player> UpdateStats(List<Player> p_players, int p_winnerNumber)
        {
            if (p_winnerNumber > 0)
            {
                for (int p_gamer = 0; p_gamer < p_players.Count; p_gamer++)
                {
                    if (p_gamer == p_winnerNumber - 1)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{p_players[p_gamer].Name} won the game!");
                        p_players[p_gamer].Wins += 1;
                    }
                    else
                    {
                        p_players[p_gamer].Losses += 1;
                    }
                }
                return p_players;
            }
            else
            {
                for (int p_gamer = 0; p_gamer < p_players.Count; p_gamer++)
                {
                    p_players[p_gamer].Draws += 1;
                }
                Console.WriteLine("It's a draw!");
                return p_players;
            }
        }
        public static void EndGameStats(List<Player> p_players)
        {
            Console.WriteLine("Game over!");
            Console.WriteLine("Final scores:");
            foreach (var p_player in p_players)
            {
                Console.WriteLine($"{p_player.Name}: {p_player.Wins} Wins");
                Console.WriteLine($"{p_player.Name}: {p_player.Losses} Losses");
                Console.WriteLine($"Draws: {p_player.Draws}");
            };
        }
    }
}
