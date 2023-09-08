namespace TicTacToeC
{
    internal class Player
    {
        public string Name;
        public int Score { get; set; }
        public int WinsTTT { get; set; }
        public int LossesTTT { get; set; }
        public int DrawsTTT { get; set; }
        public int WinsFourWins { get; set; }
        public int LossesFourWins { get; set; }
        public int DrawsFourWins { get; set; }
        public int Number { get; set; }

        public Guid Id { get; private set; }

        public Player()
        {
            Id = Guid.NewGuid();
        }

        public static (Player[], int) UpdateTTT(Player[] players, int winnerNumber, int draw)
        {

            if (winnerNumber > 0)
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
                return (players, draw);
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].DrawsTTT += 1;
                }
                draw++;
                Console.WriteLine("It's a draw!");
                return (players, draw);
            }



        }

        public static (Player[], int) UpdateFourW(Player[] players, int winnerNumber, int draw)
        {

            if (winnerNumber > 0)
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
                return (players, draw);
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].DrawsFourWins += 1;
                }
                draw++;
                Console.WriteLine("It's a draw!");
                return (players, draw);
            }



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
    }
}