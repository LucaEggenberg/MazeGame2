using MazeGame.Model;
using MazeGame.Service.Mazes;

namespace MazeGame.Service
{
    public class MazeService
    {
        public Maze GenerateMaze(int width, int height, MazeAlgorithm algorithm = MazeAlgorithm.RecursiveBacktracking)
        {
            IMaze maze = algorithm switch
            {
                MazeAlgorithm.RecursiveBacktracking => new RecursiveBacktracker(width, height),
                MazeAlgorithm.BinaryTree => new BinaryTree(width, height),
                MazeAlgorithm.ImperfectRecursiveBacktracking => new ImperfectRecursiveBacktracker(width, height),
                _ => throw new NotImplementedException()
            };

            maze.GoalX = width / 2;
            maze.GoalY = height / 2;

            return new()
            {
                Height = height,
                Width = width,
                GoalX = maze.GoalX,
                GoalY = maze.GoalY,
                Cells = maze.Generate()
            }; 
        }
    }
}
