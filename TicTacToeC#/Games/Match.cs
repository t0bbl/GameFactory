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

                    switch (game)
                    {
                        case "TTT":
                            TTT tttGame = new();
                            (players, draw) = tttGame.Start(players, draw);
                            break;
                        case "FourW":
                            FourW fourwGame = new();
                            (players, draw) = fourwGame.Start(players, draw);
                            break;
                    }
                    return (players, draw);
            }
        }
    }

