using ClassLibrary;
using System;
using System.Collections.Generic;

namespace GameFactoryWPF.Utilities
{
    public interface IGameCellControlContainer
    {
        event EventHandler<Match.GameCellClickedEventArgs> GameCellClicked;

        IEnumerable<GameCell> GetAllGameCellControls();

    }
}
