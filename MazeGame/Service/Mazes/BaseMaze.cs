using MazeGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeGame.Service.Mazes
{
    public class BaseMaze (int width, int height)
    {
        protected Cell[,] _maze = Create(width, height);

        private static Cell[,] Create(int width, int height)
        {
            var grid = new Cell[width, height];
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Cell
                    {
                        X = i,
                        Y = j,
                        TopWall = true,
                        RightWall = true,
                        BottomWall = true,
                        LeftWall = true,
                    };
                }
            }

            return grid;
        }

        protected static void Connect(Cell? cell, Cell? other)
        {
            if (cell == null || other == null)
            {
                return;
            }

            if (cell.Y == other.Y)
            {
                if (cell.X + 1 == other.X)
                {
                    cell.RightWall = false;
                    other.LeftWall = false;
                    return;
                }

                if (cell.X - 1 == other.X)
                {
                    cell.LeftWall = false;
                    other.RightWall = false;
                    return;
                }
            }
            else if (cell.X == other.X)
            {
                if (cell.Y + 1 == other.Y)
                {
                    cell.BottomWall = false;
                    other.TopWall = false;
                    return;
                }

                if (cell.Y - 1 == other.Y)
                {
                    cell.TopWall = false;
                    other.BottomWall = false;
                    return;
                }
            }

            throw new ArgumentException("Invalid Step");
        }
    }
}
