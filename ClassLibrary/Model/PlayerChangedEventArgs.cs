using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreGameFactory.Model
{
    public class PlayerChangedEventArgs
    {
        public string CurrentPlayer { get; private set; }


        public PlayerChangedEventArgs(string p_CurrentPlayer)
        {
            CurrentPlayer = p_CurrentPlayer;
        }
    }
}
