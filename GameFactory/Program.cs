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
                var GameMode = CurrentGame.InitializeGameMenu();
                if (GameMode == "Options")
                {
                    var Options = new Options();
                    if (!Options.StartOptions())
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
