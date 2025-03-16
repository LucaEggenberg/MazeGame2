using MazeGame.GameAssets;
using Microsoft.Windows.Themes;

namespace MazeGame.Game
{
    public class Level5 (IPlayer player) : IGame
    {
        public IPlayer Player { get; } = player;

        public void Run()
        {
            while (Player.IsOnGoal() == false)
            {
                if (Player.IsLeftWall() && Player.IsFrontWall())
                {
                    Player.TurnRight();
                }
                if (Player.IsLeftWall() && Player.IsFrontWall() == false)
                {
                    Player.Step();
                }
                if (Player.IsFrontWall() && Player.IsLeftWall() == false)
                {
                    Player.TurnLeft();
                    Player.Step();
                }
                if (Player.IsFrontWall() == false && Player.IsLeftWall() == false)
                {
                    Player.TurnLeft();
                    Player.Step();

                }    }
            }
    }
}
