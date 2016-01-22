using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Augenblick
{
    public interface IDrawable
    {
        void Draw(SpriteBatch batch);
    }
}
