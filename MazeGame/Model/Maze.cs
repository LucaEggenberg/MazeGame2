using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Model
{
    public class Maze
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int GoalX { get; set; }

        public int GoalY { get; set; }

        public Cell[,]? Cells { get; set; }
    }
}
