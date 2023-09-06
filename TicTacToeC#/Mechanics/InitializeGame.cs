using System;

namespace TicTacToe
{
    public class InitializeGame
    {

        public enum ValidGames
        {
            TTT = 1,
            FourW = 2
        }


        public static string Initialize()
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
                    return game;
                }

            }
        }
    }
}