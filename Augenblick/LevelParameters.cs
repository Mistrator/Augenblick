using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Augenblick
{
    /// <summary>
    /// Properties of a level on different difficulty settings.
    /// </summary>
    public class LevelParameters
    {
        public int SideLength { get; private set; }
        public bool RotationsEnabled { get; private set; }

        public float InspectionTime { get; private set; }
        public float SolveTime { get; private set; }

        public LevelParameters(int sideLength, bool rotEnabled, float insTime, float solTime)
        {
            this.SideLength = sideLength;
            this.RotationsEnabled = rotEnabled;
            this.InspectionTime = insTime;
            this.SolveTime = solTime;
        }
    }
}