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

                switch (GameMode)
                {
                    case "Options":
                        if (!Options.GameOptions())
                        {
                            continue;
                        }
                        break;

                    case "PlayerOptions":
                        int p_ident = Player.PlayerSignIn();
                        Player.SetPlayerVariables(p_ident);
                        continue;

                    case "PlayerStats":
                        Player.ShowPlayerStats();
                        continue;

                    case "Leaderboard":
                        DataProvider.DisplayLeaderBoard();
                        continue;

                    case "Quit":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
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
