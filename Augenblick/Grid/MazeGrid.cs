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

        public int SideLength
        {
            get { return Grid.GetLength(0); }
        }

        public MazeGrid(int sideLength)
        {
            Grid = new GridCell[sideLength, sideLength];
            IsVisible = true;
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
                    Grid[i, j].Draw(batch, new Vector2(topLeftX + i * cellSideLength, topLeftY + j * cellSideLength), new Vector2(topLeftX + (i + 1) * cellSideLength, topLeftY + (j + 1) * cellSideLength));
                }
            }

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

        public static MazeGrid Generate(int sideLength)
        {
            MazeGrid grid = new MazeGrid(sideLength);

            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j < sideLength; j++)
                {
                    grid.Grid[i, j] = new GridCell(CellType.Empty);
                }
            }

            grid.Divide(0, 0, sideLength, sideLength);

            // todo generaatio
            return grid;
        }

        /// <summary>
        /// Shitty maze generator
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        private void Divide(int startX, int startY, int endX, int endY)
        {
            if (startX >= endX) return;
            if (startY >= endY) return;

            if (RandomGen.NextBool()) // pysty
            {
                int x = RandomGen.NextInt(startX, endX);
                int hole = RandomGen.NextInt(startY, endY);

                for (int i = 0; i < endY - startY; i++)
                {
                    if (i == hole) continue;
                    Grid[x, startY + i].Type = CellType.Wall;
                }

                Divide(startX, startY, x - 1, endY);
                Divide(x + 1, startY, endX, endY);
            }

            else // vaaka
            {
                int y = RandomGen.NextInt(startY, endY);
                int hole = RandomGen.NextInt(startX, endX);

                for (int i = 0; i < endX - startX; i++)
                {
                    if (i == hole) continue;
                    Grid[startX + i, y].Type = CellType.Wall;
                }

                Divide(startX, startY, endX, y - 1);
                Divide(startX, y + 1, endX, endY);
            }
        }
    }
}