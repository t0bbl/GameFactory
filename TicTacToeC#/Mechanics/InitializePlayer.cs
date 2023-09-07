namespace TicTacToe
{
    public class InitializePlayer
    {
        public static Player[] Initialize()
        {
            Console.WriteLine("Enter the number of players: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int numberOfPlayers = Convert.ToInt32(keyInfo.KeyChar.ToString());
            Player[] players = new Player[numberOfPlayers];
            Console.WriteLine();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i] = new Player();
                players[i].Number = i + 1;
                Console.WriteLine($"Enter the name of player {players[i].Number}: ");
                players[i].Name = Console.ReadLine();
                players[i].Score = 0;

            }

            return players;
        }
    }


}
