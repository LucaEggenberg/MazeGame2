using MazeGame.Game;
using MazeGame.GameAssets;
using MazeGame.Model;
using MazeGame.Service;
using Newtonsoft.Json;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WallThickness = 2;
        private const int WindowSize = 1000;
        private MazeService _mazeService;
        
        private Player? _player;
        private IGame? _game;
        private Maze? _maze;
        private int _cellSize = 10;
        private int _level = 1;
        private ICommand? runGameCommand;

        public ICommand RunGameCommand => runGameCommand ??= new DelegateCommand(async () => await RunGame());

        public MainWindow()
        {
            InitializeComponent();
            _mazeService = new MazeService();
            ChangeLevel();
            UpdatePlayerPosition(_player);
        }

        public void OnChangeLevelClicked(object? sender, RoutedEventArgs? e)
        {
            if (sender is not Button button || !int.TryParse(button.Name.Replace("level", string.Empty), out var i))
            {
                return;
            }

            _level = i;
            ChangeLevel();
        }

        public void OnResetClicked(object? sender, RoutedEventArgs? e)
        {
            _player.Reset();
            UpdatePlayerPosition(_player);
        }

        private void ChangeLevel()
        {
            if (_level < 4)
            {
                var levelsPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "GameAssets", "StaticLevels");
                var filePath = System.IO.Path.Combine(levelsPath, $"level_{_level}.json");
                _maze = JsonConvert.DeserializeObject<Maze>(File.ReadAllText(filePath));
            }
            else if (_level == 4)
            {
                _maze = _mazeService.GenerateMaze(61, 61);
            }
            else if (_level == 5)
            {
                _maze = _mazeService.GenerateMaze(61, 61, MazeAlgorithm.ImperfectRecursiveBacktracking);
            }
            else
            {
                throw new NotImplementedException();
            }

            _player = new Player(_maze);
            _game = _level switch
            {
                1 => new Level1(_player),
                2 => new Level2(_player),
                3 => new Level3(_player),
                4 => new Level4(_player),
                5 => new Level5(_player),
                _ => throw new NotImplementedException()
            };

            if (_level < 3)
            {
                _player.MoveDelay = 50;
                _player.OnPlayerMoved += UpdatePlayerPosition;
            }
            else
            {
                _player.MoveDelay = 5;
                _player.OnPlayerMoved += UpdatePlayerPositionBigLevel;
            }

            DrawMaze();
            UpdatePlayerPosition(_player);
        }

        public int ChildSize => _cellSize - WallThickness * 2;

        private async Task RunGame()
        {
            if (_player == null || _game == null)
            {
                throw new ArgumentException("No level selected");
            }
           
            await Task.Run(() =>
            {
                _game.Run();
            });
        }

        private void DrawMaze()
        {
            if (_maze == null)
            {
                throw new ArgumentException("Maze was not defined");
            }

            _cellSize = WindowSize / _maze.Height;

            MazeCanvas.Children.Clear();

            for (var i = 0; i < _maze.Width; i++)
            {
                for (var j = 0; j < _maze.Height; j++)
                {
                    var cell = _maze.Cells[i, j];
                    double x = i * _cellSize;
                    double y = j * _cellSize;

                    if (i == _maze.GoalX && j == _maze.GoalY)
                    {
                        Canvas.SetLeft(Goal, i * _cellSize + WallThickness);
                        Canvas.SetTop(Goal, j * _cellSize + WallThickness);
                    }

                    if (cell.TopWall)
                    {
                        DrawLine(x, y, x + _cellSize, y);
                    }
                    if (cell.RightWall)
                    {
                        DrawLine(x + _cellSize, y, x + _cellSize, y + _cellSize);
                    }
                    if (cell.BottomWall)
                    {
                        DrawLine(x, y + _cellSize, x + _cellSize, y + _cellSize);
                    }
                    if (cell.LeftWall)
                    {
                        DrawLine(x, y, x, y + _cellSize);
                    }
                }
            }

            MazeCanvas.Children.Add(Goal);
            MazeCanvas.Children.Add(Player);
        }

        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            var line = new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = WallThickness
            };
            MazeCanvas.Children.Add(line);
        }

        private void UpdatePlayerPosition(object? obj)
        {
            if (obj == null || obj is not Player player)
            {
                return;
            }

            var triangleWidth = ChildSize * 0.5;
            var triangleHeight = ChildSize * 0.75;
            var sideMargin = (ChildSize - triangleWidth) / 2;
            var topMargin = (ChildSize - triangleHeight) / 2;

            var bottomLeft = new Point(sideMargin, triangleHeight + topMargin);
            var bottomRight = new Point(sideMargin + triangleWidth, triangleHeight + topMargin);
            var top = new Point(triangleWidth / 2 + sideMargin, topMargin);

            var centerX = triangleWidth * 0.5 + sideMargin;
            var centerY = triangleHeight * 0.5 + topMargin;

            Player.Dispatcher.BeginInvoke(() =>
            {
                Player.Points = new PointCollection(new List<Point> { bottomLeft, bottomRight, top });
                Player.RenderTransform = new RotateTransform(GetAngle(player.Orientation), centerX, centerY);
                Canvas.SetLeft(Player, player.X * _cellSize);
                Canvas.SetTop(Player, player.Y * _cellSize);
            });
        }

        private void UpdatePlayerPositionBigLevel(object? obj)
        {
            if (obj == null || obj is not Player player)
            {
                return;
            }

            // Draw Traceline
            MazeCanvas.Dispatcher.BeginInvoke(() =>
            {
                var x = player.X * _cellSize;
                var y = player.Y * _cellSize;

                var c = new Rectangle
                {
                    Width = _cellSize - WallThickness,
                    Height = _cellSize - WallThickness,
                    Fill = Brushes.Red
                };
                MazeCanvas.Children.Add(c);
                Canvas.SetLeft(c, x + WallThickness);
                Canvas.SetTop(c, y + WallThickness);
            });
        }

        private static int GetAngle(GameAssets.Orientation o)
        {
            return o switch
            {
                GameAssets.Orientation.Top => 0,
                GameAssets.Orientation.Right => 90,
                GameAssets.Orientation.Bottom => 180,
                GameAssets.Orientation.Left => 270,
                _ => throw new NotImplementedException()
            };
        }

        //private void SaveMazeAsJson(int levelNo)
        //{
        //    var json = JsonConvert.SerializeObject(_maze);
        //    var fileName = $"level_{levelNo}.json";

        //    var di = new DirectoryInfo("C:\\temp\\mazelevels\\");
        //    if (!di.Exists)
        //    {
        //        di.Create();
        //    }

        //    File.WriteAllText(System.IO.Path.Combine(di.FullName, fileName), json);
        //}
    }
}