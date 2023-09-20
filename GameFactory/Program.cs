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
