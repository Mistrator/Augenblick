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

        public static readonly Color BackgroundColor = Color.Black;
        public static readonly Color GridColor = Color.CornflowerBlue;
        public static readonly Color EmptyColor = Color.Transparent;
        public static readonly Color StartColor = Color.Green;
        public static readonly Color EndColor = Color.Red;
        public static readonly Color WallColor = Color.Blue;
        public static readonly Color SelectionColor = Color.Green;
        public static readonly Color BlinkingColor = Color.OrangeRed;

        #endregion

        #region difficulties

        public static readonly LevelParameters Easy = new LevelParameters(5, false, 10.0f, 10.0f); // ajat tilapäiset
        public static readonly LevelParameters Normal = new LevelParameters(5, true, 15.0f, 15.0f);
        public static readonly LevelParameters Hard = new LevelParameters(7, true, 10.0f, 10.0f);
        public static readonly LevelParameters Impossible = new LevelParameters(11, true, 10.0f, 10.0f);

        #endregion

        public const float GridThickness = 1.0f;
        public const float SafePercentage = 0.05f;

        public const float SelectionThickness = 1.0f;
        public const float SelectionEndWidthPercentage = 0.20f;

        public const float BlinkingSpeed = 0.5f; // ruudun vilkkumisnopeus epäonnistuttaessa
        public const float RotationTime = 1.0f;

        public const float MinimumInspectionTimePercentage = 0.05f;
    }
}