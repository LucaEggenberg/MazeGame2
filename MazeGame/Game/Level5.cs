using MazeGame.GameAssets;
using Microsoft.Windows.Themes;

namespace MazeGame.Game
{
    public class Level5 (IPlayer player) : IGame
    {
        private class CellExt : Model.Cell
        {
            public CellExt(Model.Cell cell)
            {
                X = cell.X;
                Y = cell.Y;
                LeftWall = cell.LeftWall;
                RightWall = cell.RightWall;
                TopWall = cell.TopWall;
                BottomWall = cell.BottomWall;
            }

            public int? Weight { get; set; }
        }

        private CellExt[,] _maze;

        public IPlayer Player { get; } = player;

        public void Run()
        {
            //SolveWithWallHugger(); // will fail
            SolveWithFloodFill();
        }

        public void SolveWithWallHugger()
        {
            while (!Player.IsOnGoal())
            {
                if (Player.IsRightWall() && !Player.IsFrontWall())
                {
                    Player.Step();
                }
                else if (Player.IsRightWall() && Player.IsFrontWall())
                {
                    Player.TurnLeft();
                }
                else if (!Player.IsRightWall())
                {
                    Player.TurnRight();
                    Player.Step();
                }
            }
        }

        public void SolveWithFloodFill()
        {
            _maze = new CellExt[Player.Maze.Cells.GetLength(0), Player.Maze.Cells.GetLength(1)];
            for (var i = 0; i < _maze.GetLength(0); i++)
            {
                for (var j = 0; j < _maze.GetLength(1); j++)
                {
                    _maze[i, j] = new CellExt(Player.Maze.Cells[i, j]);
                }
            }

            var endCell = _maze[Player.Maze.GoalX, Player.Maze.GoalY];
            MarkCell(endCell, 0);

            
            while (!Player.IsOnGoal())
            {
                var currentCell = _maze[Player.X, Player.Y];
                var nextCell = GetPotentialMoves(currentCell).OrderBy(c => c.Weight).First();
                MovePlayerTo(nextCell.X, nextCell.Y);
            }
        }

        private void MovePlayerTo(int x, int y)
        {
            if (x > Player.X)
            {
                TurnPlayerTo(Orientation.Right);
            }
            else if (x < Player.X)
            {
                TurnPlayerTo(Orientation.Left);
            }
            else if (y > Player.Y)
            {
                TurnPlayerTo(Orientation.Bottom);
            }
            else if (y < Player.Y)
            {
                TurnPlayerTo(Orientation.Top);
            }

            Player.Step();
        }

        private void TurnPlayerTo(Orientation orientation)
        {
            do
            {
                Player.TurnRight();
            } while (Player.Orientation != orientation);
        }

        private void MarkCell(CellExt current, int weight)
        {
            current.Weight = weight;
            var adjacent = GetPotentialMoves(current).Where(c => !c.Weight.HasValue);

            foreach (var a in adjacent)
            {
                MarkCell(a, weight + 1);
            }
        }

        private IEnumerable<CellExt> GetPotentialMoves(CellExt current)
        {
            if (!current.TopWall && current.Y > 0) 
            {
                yield return _maze[current.X, current.Y - 1];
            }

            if (!current.BottomWall && current.Y + 1 < _maze.GetLength(1))
            {
                yield return _maze[current.X, current.Y + 1];
            }

            if (!current.LeftWall && current.X > 0)
            {
                yield return _maze[current.X - 1, current.Y];
            }

            if (!current.RightWall && current.X + 1 < _maze.GetLength(0))
            {
                yield return _maze[current.X + 1, current.Y];
            }
        }
    }
}
