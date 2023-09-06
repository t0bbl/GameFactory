namespace TicTacToe
{
    internal class Match
    {
        public string game;
        public string choosenGame = null;


        public (Player[], int) StartGame(string game, Player[] players, int draw)
        {
            Random random = new Random();
            int startingPlayerIndex = random.Next(0, players.Length);
            Console.WriteLine($"{players[startingPlayerIndex].Name} starts!");
            switch (game)
            {
                case "TTT":
                    TTT tttGame = new();
                    (players, draw) = tttGame.Start(players, draw, startingPlayerIndex);
                    break;
                case "FourW":
                    FourW fourwGame = new();
                    (players, draw) = fourwGame.Start(players, draw, startingPlayerIndex);
                    break;
            }
            return (players, draw);
        }
    }
}
