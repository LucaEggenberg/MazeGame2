using MazeGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Service.Mazes
{
    public class BinaryTree(int width, int height) : BaseMaze(width, height), IMaze
    {
        private readonly Random _random = new();

        public int GoalX { get; set; }
        public int GoalY { get; set; }

        public Cell[,] Generate()
        {
            for (var i = 0; i < _maze.GetLength(0); i++)
            {
                for (var j = 0; j < _maze.GetLength(1); j++)
                {
                    DoStep(_maze[i, j]);
                }
            }

            return _maze;
        }

        private void DoStep(Cell current)
        {
            var next = GetPotentialCells(current).OrderBy(_ => _random.Next(0, 10)).FirstOrDefault();

            if (next != null)
            {
                Connect(current, next);
            }
        }

        private IEnumerable<Cell> GetPotentialCells(Cell current)
        {
            if (current.X + 1 < _maze.GetLength(0))
            {
                yield return _maze[current.X + 1, current.Y];
            }

            if (current.Y + 1 < _maze.GetLength(1))
            {
                yield return _maze[current.X, current.Y + 1];
            }
        }
    }
}
