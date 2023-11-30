using ClassLibrary;
using System;
using System.Collections.Generic;

namespace GameFactoryWPF
{
    public interface IGameCellControlContainer
    {
        event EventHandler<CellClickedEventArgs> GameCellClicked;

        IEnumerable<GameCell> GetAllGameCellControls();

    }
}
