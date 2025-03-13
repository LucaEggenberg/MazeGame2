namespace MazeGame.GameAssets
{
    public interface IGame
    {
        IPlayer Player { get; }

        void Run();
    }
}
