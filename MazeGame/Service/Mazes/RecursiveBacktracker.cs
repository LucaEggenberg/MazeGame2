using MazeGame.Model;
using System.Data;

namespace MazeGame.Service.Mazes
{
    public class RecursiveBacktracker(int width, int height) : BaseMaze(width, height), IMaze
    {
        internal readonly Random _random = new();

        public int GoalX { get; set; }

        public int GoalY { get; set; }  

        public virtual Cell[,] Generate()
        {
            var startingCell = _maze[GoalX, GoalY];
            DoStep(startingCell, null);
            return _maze;
        }

        protected void DoStep(Cell current, Cell? previous)
        {
            current.IsVisited = true;
            Connect(current, previous);
            
            Cell? nextCell;
            do
            {
                nextCell = GetNextCell(current);

                if (nextCell != null)
                {
                    DoStep(nextCell, current);
                }
            }
            while (nextCell != null);
        }

        protected Cell? GetNextCell(Cell current)
        {
            var adjacent = GetAdjacentCells(current);
            return adjacent.Where(a => a.IsVisited == false).OrderBy(_ => _random.Next(1, 10)).FirstOrDefault();
        }

        protected IEnumerable<Cell> GetAdjacentCells(Cell cell)
        {
            if (_maze.GetLength(0) > cell.X + 1)
            {
                yield return _maze[cell.X + 1, cell.Y];
            }

            if (cell.X > 0)
            {
                yield return _maze[cell.X - 1, cell.Y];
            }

            if (_maze.GetLength(1) > cell.Y + 1)
            {
                yield return _maze[cell.X, cell.Y + 1];
            }

            if (cell.Y > 0)
            {
                yield return _maze[cell.X, cell.Y - 1];
            }
        }
    }
}
