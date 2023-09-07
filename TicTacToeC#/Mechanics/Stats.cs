namespace TicTacToe
{
    internal class Stats
    {
        public static (Player[], int) UpdateDrawTTT(Player[] players, int draw)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].DrawsTTT += 1;
            }
            draw++;
            Console.WriteLine("It's a draw!");
            return (players, draw);
        }

        public static Player[] UpdateTTT(Player[] players, int winnerNumber)
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
            return players;

        }

        public static (Player[], int) UpdateDrawFourWins(Player[] players, int draw)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].DrawsFourWins += 1;
            }
            draw++;
            Console.WriteLine("It's a draw!");
            return (players, draw);
        }

        public static Player[] UpdateFourWins(Player[] players, int winnerNumber)
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
            return players;

        }

        public static void EndGameStats(Player[] players, int draw)
        {
            Console.WriteLine("Game over!");
            Console.WriteLine("Final scores:");
            foreach (var player in players)
            {
                Console.WriteLine($"{player.Name}: {player.Score}");
            };
            if (draw > 0)
            {
                Console.WriteLine($"Draws:{draw}");
            }
            Console.ReadLine();
    
        }


        //public static void PrintAllPlayerInfo(Player[] players)
        //{
        //    foreach (var player in players)
        //    {
        //        Console.WriteLine($"--- Player {player.Id} ---");
        //        foreach (var prop in player.GetType().GetProperties())
        //        {
        //            Console.WriteLine($"{prop.Name}: {prop.GetValue(player)}");
        //        }
        //    }
        //}

    }

}