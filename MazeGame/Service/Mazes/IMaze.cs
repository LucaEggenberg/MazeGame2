using MazeGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Service.Mazes
{
    public interface IMaze
    {
        public int GoalX { get; set; }
        public int GoalY { get; set; }
        public Cell[,] Generate();
    }
}
