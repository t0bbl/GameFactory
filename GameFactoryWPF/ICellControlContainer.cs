using ClassLibrary;
using System;
using System.Collections.Generic;

namespace GameFactoryWPF
{
    public interface ICellControlContainer
    {
        event EventHandler<CellClickedEventArgs> CellClicked;

        IEnumerable<CellControl> GetAllCellControls();

    }
}
