using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreGameFactory.Model
{
    public class Move
    {
        public int Player { get; set; }
        public int Match { get; set; }
        public string Input { get; set; }
        public bool Twist { get; set; }
        public string PlayerName { get; set; }
    }
}
