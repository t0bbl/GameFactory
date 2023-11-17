namespace ClassLibrary
{
    public class CellClickedEventArgs : EventArgs
    {
        public int Row { get; }
        public int Column { get; }

        public CellClickedEventArgs(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}