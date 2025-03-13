using MazeGame.GameAssets;

namespace MazeGame.Game
{
    public class Level2 (IPlayer player) : IGame
    {
        public IPlayer Player { get; } = player;

        public void Run()
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
    }
}
