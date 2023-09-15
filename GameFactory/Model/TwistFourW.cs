namespace GameFactory.Model
{
    internal class TwistFourW : Match
    {
        private bool FirstTurn = true;

        public TwistFourW() : base(8, 5, 4)
        { }
        public override void GameMechanic(List<Player> p_Players)
        {
            if (FirstTurn)
            {
                Console.WriteLine("Twist FourW!");
                Console.WriteLine("Twist FourW is a variation of FourW where there is a 50/50 chance ");
                Console.WriteLine("that the column you choose will be twisted, reversing the order of the cells in that column.");
                PrintBoard(true, true, p_Players);
                FirstTurn = false;
            }
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine();
                Console.WriteLine($"{p_Players[p_CurrentPlayerIndex].Name}, input a coordinate X/Y");

                string input = Console.ReadLine();
                string[] parts = input.Split('/');

                if (parts.Length == 2
                    && int.TryParse(parts[0], out int row)
                    && int.TryParse(parts[1], out int col)
                    && row >= 1 && row <= p_rows
                    && col >= 1 && col <= p_Columns)
                {
                    row--;
                    col--;

                    if (GetCell(row, col) == 0)
                    {
                        SetCell(row, col, p_CurrentPlayerIndex + 1);
                        p_CurrentPlayerIndex = (p_CurrentPlayerIndex + 1) % p_Players.Count;
                        validInput = true;
                        TwistColumn(col);

                    }
                    else
                    {
                        Console.WriteLine("Cell already occupied. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
            PrintBoard(true, true, p_Players);
        }
        public void TwistColumn(int chosenColumn) 
        {
            Random random = new Random();
            bool TwistOrNot = random.Next(0, 2) == 0;
            if (TwistOrNot)
            {
                Console.WriteLine("Twist!");

                List<int> tempColumn = new List<int>();
                for (int i = 0; i < p_rows; i++)
                {
                    tempColumn.Add(GetCell(i, chosenColumn));
                }

                tempColumn.Reverse();

                for (int i = 0; i < p_rows; i++)
                {
                    SetCell(i, chosenColumn, tempColumn[i]);
                }
            }
        }
    }

}