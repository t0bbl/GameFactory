namespace TicTacToe
{
    internal class Match
    {
        public string game;
        public string choosenGame = null;

        public (Player[], int) StartGame(string game, Player[] players, int draw)
        {
                Random rand = new();
                int startingPlayer = rand.Next(0, players.Length);
                Console.WriteLine($"{players[startingPlayer].Name} starts!");
                game match = new();
                (players, draw) = match.Start(players, draw);

                return (players, draw);
            }
        }
    }

