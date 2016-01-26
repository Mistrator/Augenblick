using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;
using System.Timers;

namespace Augenblick
{
    public class GridCell
    {
        public Color CellColor { get; private set; }

        private CellType type;
        public CellType Type 
        {
            get { return type; }
            set
            {
                type = value;
                switch (value)
                {
                    case CellType.Empty:
                        this.CellColor = GameConstants.EmptyColor;
                        break;
                    case CellType.Wall:
                        this.CellColor = GameConstants.WallColor;
                        break;
                    case CellType.Start:
                        this.CellColor = GameConstants.StartColor;
                        break;
                    case CellType.End:
                        this.CellColor = GameConstants.EndColor;
                        break;
                }
            }
        }
        private bool selected;

        public bool Selected 
        {
            get { return selected; }
            set
            {
                if (this.Type != CellType.Start && this.Type != CellType.End)
                    selected = value;
            }
        }

        private Color ownColor;
        private Color blinkingColor;
        private Timer blinkingTimer;

        public GridCell(CellType type)
        {
            this.Type = type;
        }

        public void Draw(SpriteBatch batch, Vector2 topLeft, Vector2 bottomRight, bool wallsVisible)
        {
            if (CellColor != Color.Transparent)
            {
                if (Type == CellType.Wall && !wallsVisible) 
                    return;
                Primitives2D.FillRectangle(batch, topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y, CellColor);
            }
        }

        /// <summary>
        /// Ei muuta ruudun tehtävää, esim seinää tyhjäksi.
        /// Tarkoitettu vain esim värin välkyttämiseen.
        /// </summary>
        /// <param name="c"></param>
        public void SetCellColor(Color c)
        {
            CellColor = c;
        }

        public void Blink(Color c, float interval)
        {
            ownColor = this.CellColor;
            blinkingColor = c;

            blinkingTimer = new Timer(interval * 1000);
            blinkingTimer.Elapsed += delegate
            {
                if (this.CellColor == ownColor)
                    this.CellColor = blinkingColor;
                else this.CellColor = ownColor;
            };
            blinkingTimer.Start();
        }

        public void StopBlinking()
        {
            blinkingTimer.Stop();
            this.CellColor = this.ownColor;
        }
    }
}