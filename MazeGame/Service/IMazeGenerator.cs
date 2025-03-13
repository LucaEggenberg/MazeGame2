using MazeGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Service
{
    public interface IMazeGenerator
    {
        Cell[,] Generate(int width, int height);
    }
}
