using MazeGame.GameAssets;

namespace MazeGame.Game
{
    public class Level1 (IPlayer player) : IGame
    {
        public IPlayer Player { get; } = player;

        public void Run()
        {
            Player.TurnRight();
            Player.TurnRight();
            Player.Step();

            Player.TurnLeft();
            Player.Step();
            Player.TurnLeft();
            Player.Step();

            Player.TurnRight();
            Player.Step();
            Player.Step();
            Player.Step();
            Player.TurnRight();
            Player.Step();
            Player.TurnRight();
            Player.Step();
            Player.Step();
            Player.TurnLeft();
            Player.Step();
        }
    }
}
