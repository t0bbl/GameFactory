namespace ClassLibrary
{
    public class CellClickedEventArgs : EventArgs
    {
        public int Row { get; }
        public int Column { get; }

        public CellClickedEventArgs(int p_Row, int p_Column)
        {
            Row = p_Row;
            Column = p_Column;
        }
    }
}