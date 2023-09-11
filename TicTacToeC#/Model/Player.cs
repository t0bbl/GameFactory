namespace TicTacToeC
{
    internal class Player
    {
        public string Name;
        public int score { get; set; }
        public int winsTTT { get; set; }
        public int losesTTT { get; set; }
        public int drawsTTT { get; set; }
        public int winsFourWins { get; set; }
        public int lossesFourWins { get; set; }
        public int drawsFourWins { get; set; }
        public int number { get; set; }

        public Guid id { get; private set; }

        public Player()
        {
            id = Guid.NewGuid();
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
                        players[i].score += 1;
                        players[i].winsTTT += 1;
                    }
                    else
                    {
                        players[i].losesTTT += 1;
                    }
                }
                return (players, draw);
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].drawsTTT += 1;
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
                        players[i].score += 1;
                        players[i].winsFourWins += 1;
                    }
                    else
                    {
                        players[i].lossesFourWins += 1;
                    }
                }
                return (players, draw);
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].drawsFourWins += 1;
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
                Console.WriteLine($"{player.Name}: {player.score}");
            };
            if (draw > 0)
            {
                Console.WriteLine($"Draws:{draw}");
            }
            Console.ReadLine();

        }
    }
}