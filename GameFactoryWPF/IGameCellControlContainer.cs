using ClassLibrary;
using System;
using System.Collections.Generic;

namespace GameFactoryWPF
{
    public interface IGameCellControlContainer
    {
        event EventHandler<GameCellClickedEventArgs> GameCellClicked;

        IEnumerable<GameCell> GetAllGameCellControls();

    }
}
