namespace TicTacToe
{
    internal class Match
    {
        public GameBoard gameBoard;
        public string game;
        public string choosenGame = null;

        public enum ValidGames
        {
            TTT = 1,
            Wins = 2
        }

        public (Player[], int) StartGame(Player[] players, int draw)
        {

            List<string> gameOptions = new List<string>(Enum.GetNames(typeof(ValidGames)));


            while (true)
            {
                if (choosenGame == null)
                {
                    game = Menu.ShowMenu(gameOptions);
                    choosenGame = game;
                }
                else
                {
                    game = choosenGame;
                }

                Random rand = new Random();
                int startingPlayer = rand.Next(0, players.Length);
                Console.WriteLine($"{players[startingPlayer].Name} starts!");



                if (Enum.TryParse(game, out ValidGames gameType))
                {
                    switch (gameType)
                    {
                        case ValidGames.TTT:
                            TTT tttGame = new TTT();
                            (players, draw) = tttGame.StartTTT(players, draw);
                            break;
                        case ValidGames.Wins:
                            FourW fourwGame = new FourW();
                            (players, draw) = fourwGame.Start4W(players, draw);
                            break;
                    }
                    return (players, draw);
                }
                else
                {
                    Console.WriteLine("Invalid game. Try again.");
                }
            }
        }
    }
}
