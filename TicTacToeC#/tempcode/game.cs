using TicTacToe;

internal class Game
{
    public List<Player> Players = new List<Player>();
    public List<TicTacToe> History = new List<TicTacToe>();
    private TicTacToe CurrentMatch = null;


    public Game()
    {
        this.ShowWelcomeMessage();
    }
    public void ShowWelcomeMessage()
    {
        Console.WriteLine("Willkommen bei TicTacToe!");
    }
    public void InitPlayers()
    {
        do
        {
            Console.Write($"Spieler {Players.Count + 1}: ");
            Players.Add(new Player() { Name = Console.ReadLine() });

        } while (Players.Count < 2);

    }
    public TicTacToe CreateMatch()
    {
        if (CurrentMatch != null)
            History.Add(CurrentMatch);

        CurrentMatch = new TicTacToe(Players);

        return CurrentMatch;
    }

    public int GetDrawMatches()
    {
        return History.Count(SingleMatch => SingleMatch.IsDraw);
    }

    public int GetWonMatches(Player p_Player)
    {
        return History.Count(SingleMatch => SingleMatch.Winner == p_Player);
    }

    public void ShowStats()
    {
        foreach (Player ToShow in Players)
            Console.WriteLine($"Gewonnene Spiele: {GetWonMatches(ToShow)}");

        Console.WriteLine($"Unentschiedene Spiele: {GetDrawMatches()}");
    }


}