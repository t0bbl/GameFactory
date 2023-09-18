using GameFactory.Model;

namespace GameFactory
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Game CurrentGame = new();
                Match CurrentMatch;
                var gameMode = CurrentGame.InitializeGameMenu();
                if (gameMode == "Options")
                {
                    var options = new Options();
                    if (!options.StartOptions())
                    {
                        continue;
                    }
                }

                CurrentGame.InitializePlayer();
                CurrentGame.InitializeGame();

                CurrentMatch = CurrentGame.CreateMatch();
                CurrentMatch.StartMatch();

            }
        }

    }
}
