using MazeGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Service.Mazes
{
    public class ImperfectRecursiveBacktracker(int width, int height) : RecursiveBacktracker(width, height), IMaze
    {
        private enum Orientation
        {
            Left, Right, Top, Bottom
        }

        public override Cell[,] Generate()
        {
            _maze = base.Generate();
            AddImperfections();
            return _maze;
        }

        private void AddImperfections(int level = 0)
        {
            // Solve Maze with wallhugger
            var solve = GetMazeSolution();

            // delete random walls
            var randomCoordinate = solve.steps.OrderBy(_ => _random.Next(0, solve.steps.Count)).First();
            var cellToMerge = _maze[randomCoordinate.Item1, randomCoordinate.Item2];
            var adjacent = GetAdjacentCells(cellToMerge);
            if (adjacent.Any())
            {
                Connect(cellToMerge, adjacent.OrderBy(_ => _random.Next(0, 10)).First());
            }

            // Check if still solvable
            if (GetMazeSolution((int)(solve.steps.Count * 1.5)).isSolved)
            {
                if (level > 100)
                {
                    return;
                }

                AddImperfections(level + 1);
            }
        }

        private (bool isSolved, List<Tuple<int, int>> steps) GetMazeSolution(int? maxSteps = null)
        {
            var steps = new List<Tuple<int, int>>();

            var startingCell = _maze[0, 0];
            steps.Add(new(startingCell.X, startingCell.Y));

            Cell current = startingCell;

            var player = Orientation.Top;
            var isSolved = false;
            while (!isSolved)
            {
                if (IsRightWall(player, current) && !IsFrontWall(player, current))
                {
                    current = GetForwardCell(current, player, ref steps);
                }
                else if (IsRightWall(player, current) && IsFrontWall(player, current))
                {
                    TurnLeft(ref player);
                }
                else if (!IsRightWall(player, current))
                {
                    TurnRight(ref player);
                    current = GetForwardCell(current, player, ref steps);
                }

                if (maxSteps.HasValue && steps.Count >= maxSteps.Value)
                {
                    break;
                }

                if (current.X == GoalX && current.Y == GoalY)
                {
                    isSolved = true;
                }
            }

            return (isSolved, steps);
        }

        private Cell GetForwardCell(Cell current, Orientation orientation, ref List<Tuple<int, int>> steps)
        {
            var x = current.X;
            var y = current.Y;

            switch (orientation)
            {
                case Orientation.Left:
                    x--;
                    break;
                case Orientation.Right:
                    x++;
                    break;
                case Orientation.Top:
                    y--;
                    break;
                default:
                    y++;
                    break;
            }

            if (x >= 0 && y >= 0 && _maze.GetLength(0) > x && _maze.GetLength(1) > y)
            {
                steps.Add(new(x, y));
                return _maze[x, y];
            }

            throw new Exception("Invalid Step");
        }

        private void TurnRight(ref Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Left:
                    orientation = Orientation.Top;
                    break;
                case Orientation.Right:
                    orientation = Orientation.Bottom;
                    break;
                case Orientation.Top:
                    orientation = Orientation.Right;
                    break;
                case Orientation.Bottom:
                    orientation = Orientation.Left;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void TurnLeft (ref Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Left:
                    orientation = Orientation.Bottom;
                    break;
                case Orientation.Right:
                    orientation = Orientation.Top;
                    break;
                case Orientation.Top:
                    orientation = Orientation.Left;
                    break;
                case Orientation.Bottom:
                    orientation = Orientation.Right;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private bool IsRightWall(Orientation orientation, Cell cell)
        {
            return orientation switch
            {
                Orientation.Left => cell.TopWall,
                Orientation.Right => cell.BottomWall,
                Orientation.Top => cell.RightWall,
                Orientation.Bottom => cell.LeftWall,
                _ => throw new NotImplementedException()
            };
        }

        private bool IsFrontWall(Orientation orientation, Cell cell)
        {
            return orientation switch
            {
                Orientation.Left => cell.LeftWall,
                Orientation.Right => cell.RightWall,
                Orientation.Top => cell.TopWall,
                Orientation.Bottom => cell.BottomWall,
                _ => throw new NotImplementedException()
            };
        }
    }
}
