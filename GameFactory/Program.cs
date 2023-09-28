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
                    int p_ident = Player.PlayerSignIn();
                    Player.SetPlayerVariables(p_ident);
                    continue;
                }
                if (GameMode == "PlayerStats")
                {
                    Player.ShowPlayerStats();
                    continue;
                }
                if (GameMode == "Leaderboard")
                {
                    DataProvider.DisplayLeaderBoard();
                    continue;
                }
                if (GameMode == "Quit")
                {
                    Environment.Exit(0);
                }

                CurrentGame.InitializePlayer();
                CurrentGame.InitializeGame();
                CurrentMatch = CurrentGame.CreateMatch();
                do
                {
                    CurrentMatch.StartMatch();
                } while (CurrentMatch.ReMatch());
                CurrentMatch.EndGameStats(CurrentMatch.p_player);
            }
        }

    }
}
