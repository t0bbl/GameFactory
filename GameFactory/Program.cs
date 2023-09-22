using GameFactory.Model;
using System.Collections.Generic;

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
                string GameMode = CurrentGame.InitializeGameMenu();
                if (GameMode == "Options")
                {
                    var Options = new Options();
                    if (!Options.StartOptions())
                    {
                        continue;
                    }
                }
                if (GameMode == "PlayerOptions")
                {
                    var Options = new Options();
                    if (!Options.PlayerOptions())
                    {
                        continue;
                    }
                }

                CurrentGame.InitializePlayer();
                CurrentGame.InitializeGame();
                do
                {
                    CurrentMatch = CurrentGame.CreateMatch();
                    CurrentMatch.StartMatch();
                } while (CurrentMatch.ReMatch());
                Game.EndGameStats(CurrentGame.p_player, CurrentGame.p_history);
            }
        }

    }
}
