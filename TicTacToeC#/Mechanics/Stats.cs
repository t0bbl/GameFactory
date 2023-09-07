namespace TicTacToe
{
    internal class Stats
    {
        public static void UpdateDrawTTT(Player[] players)
        {
            foreach (var player in players)
            {
                Console.WriteLine("It's a draw!");
                player.DrawsTTT += 1;
            }
        }

        public static void UpdateTTT(Player[] players, int winnerNumber)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (i == winnerNumber - 1)
                {
                    Console.WriteLine($"{players[i].Name} won the game!");
                    players[i].Score += 1;
                    players[i].WinsTTT += 1;
                }
                else
                {
                    players[i].LossesTTT += 1;
                }
            }
        }

        public static void UpdateDrawFourWins(Player[] players)
        {
            foreach (var player in players)
            {
                Console.WriteLine("It's a draw!");
                player.DrawsFourWins += 1;
            }
        }

        public static void UpdateFourWins(Player[] players, int winnerNumber)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (i == winnerNumber - 1)
                {
                    Console.WriteLine($"{players[i].Name} won the game!");
                    players[i].Score += 1;
                    players[i].WinsFourWins += 1;
                }
                else
                {
                    players[i].LossesFourWins += 1;
                }
            }
        }
    }
}