using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace GameFactoryWPF
{
    public interface ICellControlContainer
    {
        event EventHandler<CellClickedEventArgs> CellClicked;

        IEnumerable<CellControl> GetAllCellControls();

    }
}
