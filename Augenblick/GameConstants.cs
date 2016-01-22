using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame;
using Microsoft.Xna.Framework;

namespace Augenblick
{
    public static class GameConstants
    {
        #region Colors

        public static Color BackgroundColor = Color.Black;
        public static Color GridColor = Color.CornflowerBlue;
        public static Color EmptyColor = Color.Transparent;
        public static Color StartColor = Color.Green;
        public static Color EndColor = Color.Red;
        public static Color WallColor = Color.Blue;

        #endregion

        public static float GridThickness = 1.0f;
        public static float SafePercentage = 0.05f;
    }
}
