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
                Player Player = new();

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
                    if (Player.PlayerSignIn() != 0)
                    {
                        Player.SavePlayerVariables();
                        continue;
                    }
                }
                if (GameMode == "PlayerStats")
                {
                   
                   PlayerStats.ShowPlayerStats();
                        continue;
                   
                }
                if (GameMode == "Leaderboard")
                {
                    DataProvider.DisplayRankedPlayers();
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
