using TicTacToe;

internal class TicTacToe
{
    public int BoardColumns { get; set; }
    public int BoardRows { get; set; }
    public int WinCondition { get; set; }
    public List<Player> Players { get; set; }
    public Player[,] Board;

    public Player CurrentPlayer { get; set; }
    public TicTacToe(List<Player> p_PlayerList)
    {
        BoardColumns = 3;
        BoardRows = 3;
        WinCondition = 3;
        Board = new Player[BoardColumns, BoardRows];

        Players = p_PlayerList;
        CurrentPlayer = null;
    }

    public Player Winner = null;
    public bool IsDraw = false;



    public void NextPlayer()
    {
        if (CurrentPlayer == null)
            CurrentPlayer = GetRandomPlayer();
        else
        {
            int NextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;

            if (NextPlayerIndex > Players.Count - 1)
                NextPlayerIndex = 0;

            CurrentPlayer = Players[NextPlayerIndex];
        }

    }
    private Player GetRandomPlayer()
    {
        Random PlayerRandomizer = new Random();
        return Players[PlayerRandomizer.Next(Players.Count)];
    }
    public void DrawBoard()
    {
        int BoardCounter = 1;

        for (int ColumnCounter = 0; ColumnCounter < BoardColumns; ColumnCounter++)
        {
            for (int RowCounter = 0; RowCounter < BoardRows; RowCounter++)
            {
                Player FieldValue = Board[ColumnCounter, RowCounter];

                if (FieldValue == null)
                    Console.Write(BoardCounter);
                else
                    Console.Write(FieldValue);


                BoardCounter++;

                if (RowCounter + 1 < BoardRows)
                    Console.Write("\t|");
            }

            Console.WriteLine();
        }
    }

}