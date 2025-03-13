using MazeGame.Model;

namespace MazeGame.GameAssets
{
    public class Player (Maze maze) : IPlayer
    {
        public Maze Maze { get; } = maze;

        public Orientation Orientation { get; set; } = Orientation.Top;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public int MoveDelay { get; set; } = 300;

        public delegate void PlayerMoved(object sender);

        public event PlayerMoved? OnPlayerMoved;

        public bool IsOnGoal()
        {
            return X == Maze.GoalX && Y == Maze.GoalY;
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            Orientation = Orientation.Top;
        }

        public void Step()
        {
            if (IsFrontWall())
            {
                throw new Exception("You lost");
            }

            switch (Orientation)
            {
                case Orientation.Left:
                    X--;
                    break;
                case Orientation.Right:
                    X++;
                    break;
                case Orientation.Top:
                    Y--;
                    break;
                default:
                    Y++;
                    break;
            }

            Thread.Sleep(MoveDelay);
            OnPlayerMoved?.Invoke(this);
        }

        public bool IsFrontWall()
        {
            var cell = Maze.Cells[X, Y];
            return Orientation switch
            {
                Orientation.Left => cell.LeftWall,
                Orientation.Right => cell.RightWall,
                Orientation.Top => cell.TopWall,
                Orientation.Bottom => cell.BottomWall,
                _ => throw new NotImplementedException()
            };
        }

        public bool IsBackWall()
        {
            var cell = Maze.Cells[X, Y];
            return Orientation switch
            {
                Orientation.Left => cell.RightWall,
                Orientation.Right => cell.LeftWall,
                Orientation.Top => cell.BottomWall,
                Orientation.Bottom => cell.TopWall,
                _ => throw new NotImplementedException()
            };
        }

        public bool IsLeftWall()
        {
            var cell = Maze.Cells[X, Y];
            return Orientation switch
            {
                Orientation.Left => cell.BottomWall,
                Orientation.Right => cell.TopWall,
                Orientation.Top => cell.LeftWall,
                Orientation.Bottom => cell.RightWall,
                _ => throw new NotImplementedException()
            };
        }

        public bool IsRightWall()
        {
            var cell = Maze.Cells[X, Y];
            return Orientation switch
            {
                Orientation.Left => cell.TopWall,
                Orientation.Right => cell.BottomWall,
                Orientation.Top => cell.RightWall,
                Orientation.Bottom => cell.LeftWall,
                _ => throw new NotImplementedException()
            };
        }

        public void TurnLeft()
        {
            switch (Orientation)
            {
                case Orientation.Left:
                    Orientation = Orientation.Bottom;
                    break;
                case Orientation.Right:
                    Orientation = Orientation.Top;
                    break;
                case Orientation.Top:
                    Orientation = Orientation.Left;
                    break;
                case Orientation.Bottom:
                    Orientation = Orientation.Right;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void TurnRight()
        {
            switch (Orientation)
            {
                case Orientation.Left:
                    Orientation = Orientation.Top;
                    break;
                case Orientation.Right:
                    Orientation = Orientation.Bottom;
                    break;
                case Orientation.Top:
                    Orientation = Orientation.Right;
                    break;
                case Orientation.Bottom:
                    Orientation = Orientation.Left;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
