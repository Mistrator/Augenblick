using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using C3.XNA;

namespace Augenblick
{
    public class MazeGrid : IDrawable, IUpdatable
    {
        public GridCell[,] Grid { get; private set; }

        /// <summary>
        /// Master-visibility
        /// </summary>
        public bool IsVisible { get; set; }

        public bool WallsVisible { get; set; }
        public bool GridVisible { get; set; }

        public List<Point> SelectedRoute;

        public int SideLength
        {
            get { return Grid.GetLength(0); }
        }

        public MazeGrid(int sideLength)
        {
            Grid = new GridCell[sideLength, sideLength];
            IsVisible = true;
            WallsVisible = true;

            SelectedRoute = new List<Point>();
        }

        public void Draw(SpriteBatch batch)
        {
            if (!IsVisible) return;

            int screenHeight = Augenblick.graphics.PreferredBackBufferHeight;
            int screenWidth = Augenblick.graphics.PreferredBackBufferWidth;

            float heightCenter = screenHeight / 2.0f;
            float widthCenter = screenWidth / 2.0f;

            float safeHeight = screenHeight - (screenHeight * GameConstants.SafePercentage * 2);
            float cellSideLength = safeHeight / (float)SideLength;

            float topLeftY = heightCenter - safeHeight / 2;
            float topLeftX = widthCenter - safeHeight / 2; // safeHeight, koska ruudukon leveys ja korkeus samat

            float bottomRightX = widthCenter + safeHeight / 2;
            float bottomRightY = heightCenter + safeHeight / 2;

            for (int i = 0; i < SideLength; i++)
            {
                for (int j = 0; j < SideLength; j++)
                {
                    Grid[i, j].Draw(batch, new Vector2(topLeftX + i * cellSideLength, topLeftY + j * cellSideLength), new Vector2(topLeftX + (i + 1) * cellSideLength, topLeftY + (j + 1) * cellSideLength), WallsVisible);
                }
            }

            for (int i = 0; i < SelectedRoute.Count - 1; i++)
            {
                Primitives2D.DrawLine(batch, GetCoordinatesByCell(SelectedRoute[i]), GetCoordinatesByCell(SelectedRoute[i + 1]), GameConstants.SelectionColor, GameConstants.SelectionThickness);
            }
            if (SelectedRoute.Count != 0)
                Primitives2D.FillRectangle(batch, GetCoordinatesByCell(SelectedRoute[SelectedRoute.Count - 1]) - 
                    new Vector2(cellSideLength * GameConstants.SelectionEndWidthPercentage / 2, cellSideLength * GameConstants.SelectionEndWidthPercentage / 2), 
                    new Vector2(cellSideLength * GameConstants.SelectionEndWidthPercentage, cellSideLength * GameConstants.SelectionEndWidthPercentage), 
                    GameConstants.SelectionColor);

            if (GridVisible)
            {
                for (int i = 0; i <= SideLength; i++)
                {
                    Primitives2D.DrawLine(batch, new Vector2(topLeftX, topLeftY + i * cellSideLength), new Vector2(bottomRightX, topLeftY + i * cellSideLength), GameConstants.GridColor, GameConstants.GridThickness);
                    Primitives2D.DrawLine(batch, new Vector2(topLeftX + i * cellSideLength, topLeftY), new Vector2(topLeftX + i * cellSideLength, bottomRightY), GameConstants.GridColor, GameConstants.GridThickness);
                }
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Rotate(Rotation rot)
        {
            switch (rot)
            {
                case Rotation.QuarterClockwise:
                    Transpose(Grid);

                    for (int y = 0; y < SideLength; y++)
                    {
                        for (int i = 0; i < SideLength / 2; i++) // reverse each row
                        {
                            GridCell tmp = Grid[i, y];
                            Grid[i, y] = Grid[SideLength - i - 1, y];
                            Grid[SideLength - i - 1, y] = tmp;
                        }
                    }
                    break;
                case Rotation.QuarterCounterClockwise:
                    Transpose(Grid);

                    for (int x = 0; x < SideLength; x++)
                    {
                        for (int i = 0; i < SideLength / 2; i++) // reverse each column
                        {
                            GridCell tmp = Grid[x, i];
                            Grid[x, i] = Grid[x, SideLength - i - 1];
                            Grid[x, SideLength - i - 1] = tmp;
                        }
                    }
                    break;
                case Rotation.Half:
                    for (int y = 0; y < SideLength; y++)
                    {
                        for (int i = 0; i < SideLength / 2; i++)
                        {
                            GridCell tmp = Grid[i, y];
                            Grid[i, y] = Grid[SideLength - i - 1, y];
                            Grid[SideLength - i - 1, y] = tmp;
                        }
                    }

                    for (int x = 0; x < SideLength; x++)
                    {
                        for (int i = 0; i < SideLength / 2; i++)
                        {
                            GridCell tmp = Grid[x, i];
                            Grid[x, i] = Grid[x, SideLength - i - 1];
                            Grid[x, SideLength - i - 1] = tmp;
                        }
                    }

                    break;
            }
        }

        private void Transpose(GridCell[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    GridCell temp = arr[i, j];
                    arr[i, j] = arr[j, i];
                    arr[j, i] = temp;
                }
            }
        }

        public GridCell GetCellByCoordinates(Vector2 coords)
        {
            // bestest copypaste no kappa

            int screenHeight = Augenblick.graphics.PreferredBackBufferHeight;
            int screenWidth = Augenblick.graphics.PreferredBackBufferWidth;

            float heightCenter = screenHeight / 2.0f;
            float widthCenter = screenWidth / 2.0f;

            float safeHeight = screenHeight - (screenHeight * GameConstants.SafePercentage * 2);
            float cellSideLength = safeHeight / (float)SideLength;

            float topLeftY = heightCenter - safeHeight / 2;
            float topLeftX = widthCenter - safeHeight / 2; // safeHeight, koska ruudukon leveys ja korkeus samat

            // float bottomRightX = widthCenter + safeHeight / 2;
            // float bottomRightY = heightCenter + safeHeight / 2;

            int x = (int)Math.Floor((coords.X - topLeftX) / cellSideLength);
            int y = (int)Math.Floor((coords.Y - topLeftY) / cellSideLength);

            if (x < 0 || x >= SideLength) return null;
            if (y < 0 || y >= SideLength) return null;

            return Grid[x, y];
        }

        public Point? GetPointByCoordinates(Vector2 coords)
        {
            // bestest copypaste no kappa

            int screenHeight = Augenblick.graphics.PreferredBackBufferHeight;
            int screenWidth = Augenblick.graphics.PreferredBackBufferWidth;

            float heightCenter = screenHeight / 2.0f;
            float widthCenter = screenWidth / 2.0f;

            float safeHeight = screenHeight - (screenHeight * GameConstants.SafePercentage * 2);
            float cellSideLength = safeHeight / (float)SideLength;

            float topLeftY = heightCenter - safeHeight / 2;
            float topLeftX = widthCenter - safeHeight / 2; // safeHeight, koska ruudukon leveys ja korkeus samat

            // float bottomRightX = widthCenter + safeHeight / 2;
            // float bottomRightY = heightCenter + safeHeight / 2;

            int x = (int)Math.Floor((coords.X - topLeftX) / cellSideLength);
            int y = (int)Math.Floor((coords.Y - topLeftY) / cellSideLength);

            if (x < 0 || x >= SideLength) return null;
            if (y < 0 || y >= SideLength) return null;

            return new Point(x, y);
        }


        /// <summary>
        /// Palauttaa ruudun keskipisteen koordinaatit.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Vector2 GetCoordinatesByCell(Point cell)
        {
            // bestest copypaste no kappa

            int screenHeight = Augenblick.graphics.PreferredBackBufferHeight;
            int screenWidth = Augenblick.graphics.PreferredBackBufferWidth;

            float heightCenter = screenHeight / 2.0f;
            float widthCenter = screenWidth / 2.0f;

            float safeHeight = screenHeight - (screenHeight * GameConstants.SafePercentage * 2);
            float cellSideLength = safeHeight / (float)SideLength;

            float topLeftY = heightCenter - safeHeight / 2;
            float topLeftX = widthCenter - safeHeight / 2; // safeHeight, koska ruudukon leveys ja korkeus samat

            float x = topLeftX + cell.X * cellSideLength + cellSideLength / 2;
            float y = topLeftY + cell.Y * cellSideLength + cellSideLength / 2;

            return new Vector2(x, y);
        }

        public static MazeGrid Generate(int sideLength)
        {
            MazeGrid grid = new MazeGrid(sideLength);
            List<Point> walls = new List<Point>();

            int[,] labels = new int[sideLength, sideLength];
            int currentLabel = 0;

            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    labels[i, j] = -1;

                    if (i % 2 != 0 || j % 2 != 0)
                    {
                        grid.Grid[i, j] = new GridCell(CellType.Wall);
                        walls.Add(new Point(i, j));
                    }
                    else
                    {
                        grid.Grid[i, j] = new GridCell(CellType.Empty);
                        labels[i, j] = currentLabel;
                        currentLabel++;
                    }
                }
            }

            while (walls.Count > 0) 
            {
                int cWall = RandomGen.NextInt(0, walls.Count);
                Point cPoint = walls[cWall];
                walls.RemoveAt(cWall);

                Point[] nBs = GetEmptyNeighbours(grid, cPoint);

                int lowestLabel = int.MaxValue; // gg
                int lowestIndex = -1;
                bool diffLabels = false;

                for (int i = 0; i < nBs.Length; i++)
                {
                    int cLabel = labels[nBs[i].X, nBs[i].Y];

                    if (cLabel != lowestLabel && lowestLabel != int.MaxValue)
                        diffLabels = true;

                    if (cLabel < lowestLabel)
                    {
                        
                        lowestLabel = cLabel;
                        lowestIndex = i;
                    }
                }

                if (!diffLabels) 
                    continue; // ei poisteta seinää

                // grid.Grid[nBs[lowestIndex].X, nBs[lowestIndex].Y].Type = CellType.Empty;
                grid.Grid[cPoint.X, cPoint.Y].Type = CellType.Empty;
                // labels[nBs[lowestIndex].X, nBs[lowestIndex].Y] = lowestLabel;
                labels[cPoint.X, cPoint.Y] = lowestLabel;

                List<Point> connectedNodes = new List<Point>();

                connectedNodes = GetAllConnectedEmptyCells(grid, cPoint, connectedNodes);

                for (int i = 0; i < connectedNodes.Count; i++)
                {
                    labels[connectedNodes[i].X, connectedNodes[i].Y] = lowestLabel;
                }
            }
            Point start, end;

            do
                start = new Point(RandomGen.NextInt(0, (int)Math.Ceiling(sideLength / 3.0f)), RandomGen.NextInt(0, (int)Math.Ceiling(sideLength / 3.0f)));
            while (grid.Grid[start.X, start.Y].Type != CellType.Empty);
            grid.Grid[start.X, start.Y].Type = CellType.Start;

            do
                end = new Point(RandomGen.NextInt(2 * (sideLength / 3), sideLength), RandomGen.NextInt(2 * (sideLength / 3), sideLength));
            while (grid.Grid[end.X, end.Y].Type != CellType.Empty);
            grid.Grid[end.X, end.Y].Type = CellType.End;

            return grid;
        }

        public void AddStartPosition()
        {
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j].Type == CellType.Start)
                    {
                        SelectedRoute.Add(new Point(i, j));
                        return;
                    }
                }
            }
        }

        private static Point[] GetEmptyNeighbours(MazeGrid grid, Point p)
        {
            List<Point> points = new List<Point>();
            Point top = new Point(p.X, p.Y + 1);
            Point bottom = new Point(p.X, p.Y - 1);
            Point left = new Point(p.X - 1, p.Y);
            Point right = new Point(p.X + 1, p.Y);

            // dem epic design
            if (IsValidCell(grid, top))
                if (IsEmpty(grid, top))
                    points.Add(top);
            if (IsValidCell(grid, bottom))
                if (IsEmpty(grid, bottom))
                    points.Add(bottom);
            if (IsValidCell(grid, left))
                if (IsEmpty(grid, left))
                    points.Add(left);
            if (IsValidCell(grid, right))
                if (IsEmpty(grid, right))
                    points.Add(right);

            return points.ToArray();
        }

        /// <summary>
        /// Onko ruutu ruudukon sisäpuolella.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool IsValidCell(MazeGrid grid, Point p)
        {
            return p.X >= 0 && p.X < grid.SideLength && p.Y >= 0 && p.Y < grid.SideLength;               
        }

        private static bool IsEmpty(MazeGrid grid, Point p)
        {
            return grid.Grid[p.X, p.Y].Type == CellType.Empty;
        }

        public static List<Point> GetAllConnectedEmptyCells(MazeGrid grid, Point p, List<Point> connected)
        {
            connected.Add(p);

            Point[] nBs = GetEmptyNeighbours(grid, p);

            for (int i = 0; i < nBs.Length; i++)
            {
                if (!connected.Contains(nBs[i]))
                {
                    GetAllConnectedEmptyCells(grid, nBs[i], connected);
                }
                    //connected.AddRange(GetAllConnectedEmptyCells(grid, nBs[i], connected));
            }

            return connected;
        }

        /// <summary>
        /// Tarkistaa, kelpaako pelaajan valitsema reitti. Jos ei kelpaa, palauttaa pisteen, jossa meni pieleen.
        /// Jos kelpaa, piste on maali.
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, Point> VerifySelectedRoute()
        {
            if (Grid[SelectedRoute[SelectedRoute.Count - 1].X, SelectedRoute[SelectedRoute.Count - 1].Y].Type != CellType.End)
                return new Tuple<bool,Point>(false, new Point(SelectedRoute[SelectedRoute.Count - 1].X, SelectedRoute[SelectedRoute.Count - 1].Y));

            for (int i = 0; i < SelectedRoute.Count; i++)
            {
                if (Grid[SelectedRoute[i].X, SelectedRoute[i].Y].Type == CellType.Wall)
                    return new Tuple<bool, Point>(false, SelectedRoute[i]);
            }

            return new Tuple<bool, Point>(true, new Point(SelectedRoute[SelectedRoute.Count - 1].X, SelectedRoute[SelectedRoute.Count - 1].Y));
        }
    }
}