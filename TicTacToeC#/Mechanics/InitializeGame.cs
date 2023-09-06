using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class InitializeGame
    {
        public enum ValidGames
        {
            TTT = 1,
            FourW = 2
        }

        public static GamesAvailable Initialize()
        {
            List<string> gameOptions = new List<string>(Enum.GetNames(typeof(ValidGames)));
            string game = null;

            while (true)
            {
                if (game == null)
                {
                    game = Menu.ShowMenu(gameOptions);
                }
                else
                {
                    switch (game)
                    {
                        case "TTT":
                            return new TTT();
                        case "FourW":
                            return new FourW(); 
                        default:
                            throw new Exception("Invalid game type.");
                    }
                }
            }
        }
    }

    public interface GamesAvailable
    {
        (Player[], int) Start(Player[] players, int draw, int startingPlayerIndex);
    }
}
