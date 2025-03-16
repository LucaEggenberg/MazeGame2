using MazeGame.GameAssets;

namespace MazeGame.Game
{
    public class Level1 (IPlayer player) : IGame
    {
        public IPlayer Player { get; } = player;

        public void Run()
        {

        }
    }
}
