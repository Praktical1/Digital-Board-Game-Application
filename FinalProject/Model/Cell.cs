using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectRedesigned.Model
{
    internal class Cell
    {
        public String colour { get; set; }
        public String type { get; set; }


        public String[] holds { get; set; }

        public Boolean Occupied { get; set; }

        public Cell(string colour, string type)
        {
            holds = new string[2] { colour, type };
        }

        public void SetHolds(String[] x)
        {
            holds = x;
        }
        public String[] GetHolds()
        {
            return holds;
        }
    }
}
