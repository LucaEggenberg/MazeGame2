using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Model
{
    public class Cell
    {
        public bool LeftWall { get; set; }
        public bool RightWall { get; set; }
        public bool TopWall { get; set; }
        public bool BottomWall { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public bool IsVisited { get; set; }
    }
}
