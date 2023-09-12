namespace GameFactory
{
    internal class Player
    {
        public string p_name;
        public int p_wins { get; set; }
        public int p_losses { get; set; }
        public int p_draws { get; set; }
        public int p_number { get; set; }
        public Guid p_id { get; private set; }
        public Player()
        {
            p_id = Guid.NewGuid();
        }
        public static List<Player> UpdateStats(List<Player> p_players, int p_winnerNumber)
        {
            if (p_winnerNumber > 0)
            {
                for (int p_gamer = 0; p_gamer < p_players.Count; p_gamer++)
                {
                    if (p_gamer == p_winnerNumber - 1)
                    {
                        Console.WriteLine($"{p_players[p_gamer].p_name} won the game!");
                        p_players[p_gamer].p_wins += 1;
                    }
                    else
                    {
                        p_players[p_gamer].p_losses += 1;
                    }
                }
                return p_players;
            }
            else
            {
                for (int p_gamer = 0; p_gamer < p_players.Count; p_gamer++)
                {
                    p_players[p_gamer].p_draws += 1;
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
                Console.WriteLine($"{p_player.p_name}: {p_player.p_wins} Wins");
                Console.WriteLine($"{p_player.p_name}: {p_player.p_losses} Losses");
                Console.WriteLine($"Draws: {p_player.p_draws}");
            };
        }
    }
}
