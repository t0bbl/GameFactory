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
                var playerAuth = new PlayerAuth();
                Match CurrentMatch;

                string GameMode = CurrentGame.InitializeGameMenu();

                if (GameMode == "Options")
                {
                    if (!Options.GameOptions())
                    {
                        continue;
                    }
                }
                if (GameMode == "PlayerOptions")
                {
                    if (playerAuth.PlayerSignIn() != 0)
                    {
                        playerAuth.SavePlayerVariables();
                        continue;
                    }
                }
                if (GameMode == "PlayerStats")
                {
                   
                   PlayerStats.ShowPlayerStats();
                        continue;
                   
                }

                CurrentGame.InitializePlayer();
                CurrentGame.InitializeGame();
                CurrentMatch = CurrentGame.CreateMatch();
                do
                {
                    CurrentMatch.StartMatch();
                } while (CurrentMatch.ReMatch());
                Game.EndGameStats(CurrentMatch.p_player);
            }
        }

    }
}
