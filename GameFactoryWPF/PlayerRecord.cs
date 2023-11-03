using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFactoryWPF
{
    public class PlayerRecord
    {
        public int Place { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public double WinPercentage { get; set; }
    }
}
