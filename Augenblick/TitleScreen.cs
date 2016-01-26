using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Augenblick
{
    public class TitleScreen : IDrawable, IUpdatable
    {
        private float screenWidth;
        private float screenHeight;

        public Texture2D title;

        public TitleScreen(float screenWidth, float screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            Augenblick.GameInstance.Content.Load<Texture2D>("title");
        }

        public void Draw(SpriteBatch batch)
        {
        }

        public void Update(GameTime time)
        {

        }
    }
}