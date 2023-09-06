namespace TicTacToe
{
    internal class Match
    {
        public string game;


        public (Player[], int) StartGame(string game, Player[] players, int draw)
        {
            do
            {

                Random random = new Random();
                int startingPlayerIndex = random.Next(0, players.Length);
                // if there are several methods for determening a starting player refactor to Mechanics

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
                Console.WriteLine("Want a rematch? Y/N?");
            } while (Console.ReadLine().ToLower() == "y");
            return (players, draw);

        }
    }
}
