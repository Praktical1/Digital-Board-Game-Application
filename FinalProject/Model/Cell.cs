using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectRedesigned.Model
{
    internal class Cell
    {
        public int[] coords { get; set; }

        public int col { get; set; }
        public int row { get; set; }
        public String colour { get; set; }
        public String type { get; set; }


        public String[] holds { get; set; }

        public Boolean Occupied { get; set; }

        public Cell(int col, int row, string colour, string type)
        {
            this.col = col;
            this.row = row;
            holds = new string[2] { colour, type };
            Console.WriteLine("Created a cell");
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
