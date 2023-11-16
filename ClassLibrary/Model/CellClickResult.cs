using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class CellClickResult
    {
        public int Row { get; }
        public int Col { get; }

        public CellClickResult(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}
