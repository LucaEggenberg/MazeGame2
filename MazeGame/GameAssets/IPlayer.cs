using MazeGame.Model;

namespace MazeGame.GameAssets
{
    public interface IPlayer
    {
        void Step();
        void TurnRight();
        void TurnLeft();

        bool IsOnGoal();

        bool IsFrontWall();
        bool IsBackWall();
        bool IsLeftWall();
        bool IsRightWall();

        #region advanced
        Maze Maze { get; }
        Orientation Orientation { get; }
        int X { get; }
        int Y { get; }
        #endregion
    }
}
