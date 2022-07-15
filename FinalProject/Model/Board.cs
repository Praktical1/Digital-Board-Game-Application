using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectRedesigned.Model
{
    
    internal class Board
    {
        public Cell[,] grid { get; set; }

        public Board()
        {
            PopulateGrid();

        }
        public Cell[,] PopulateGrid()
        {
            grid = new Cell[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0 && j % 2 == 1 || i % 2 == 1 && j % 2 == 0)
                    {
                        if (i < 3)
                        {
                            grid[i, j] = new Cell(i, j, "B", "1");
                        }
                        else if (i > 4)
                        {
                            grid[i, j] = new Cell(i, j, "R", "1");
                        }
                        else
                        {
                            grid[i, j] = new Cell(i, j, " ", " ");
                        }
                    }
                    else
                    {
                        grid[i, j] = new Cell(i, j, " ", " ");
                    }
                }
            }
            return grid;
        }
    }
}
