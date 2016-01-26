using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Augenblick
{
    public static class MathHelper
    {
        /// <summary>
        /// Luo vektorin pituuden ja kulman perusteella.
        /// </summary>
        /// <param name="length">Vektorin pituus</param>
        /// <param name="angle">Kulma radiaaneina</param>
        /// <returns></returns>
        public static Vector2 FromLengthAndAngle(float length, double angle)
        {
            Vector2 result;
            result.X = (float)(length * Math.Cos(angle));
            result.Y = (float)(length * Math.Sin(angle));
            return result;
        }

        /// <summary>
        /// Muuntaa kulman asteista radiaaneiksi.
        /// </summary>
        /// <param name="degrees">Kulma asteina</param>
        /// <returns></returns>
        public static float DegreesToRadians(double degrees)
        {
            return (float)(Math.PI * degrees / 180.0);
        }

        /// <summary>
        /// Muuntaa kulman radiaaneista asteiksi.
        /// </summary>
        /// <param name="radians">Kulma radiaaneina</param>
        /// <returns></returns>
        public static float RadiansToDegrees(double radians)
        {
            return (float)(radians * (180.0 / Math.PI));
        }

        /// <summary>
        /// Ovatko kaksi pistettä tiettyä etäisyyttä lähempänä toisiaan.
        /// </summary>
        /// <param name="first">Ensimmäinen piste</param>
        /// <param name="second">Toinen piste</param>
        /// <param name="maxDistance">Tutkittava etäisyys (inclusive)</param>
        /// <returns></returns>
        public static bool AreVectorsClose(Vector2 first, Vector2 second, float maxDistance)
        {
            if (Vector2.Distance(first, second) <= maxDistance)
                return true;
            return false;
        }

        /// <summary>
        /// Ovatko kaksi lukua lähes yhtä suuret.
        /// </summary>
        /// <param name="first">Ensimmäinen luku</param>
        /// <param name="second">Toinen luku</param>
        /// <param name="inaccuracy">Paljonko luvuilla saa olla heittoa.</param>
        /// <returns></returns>
        public static bool AreApproximatelyEqual(float first, float second, float inaccuracy)
        {
            return Math.Abs(first - second) < inaccuracy;
        }

        /// <summary>
        /// Kiertää pistettä (x, y) toisen pisteen ympäri.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angleDeg">Kulma asteina</param>
        /// <param name="pivot">Piste, jonka ympäri kierretään.</param>
        /// <returns></returns>
        public static Vector2 RotatePoint(float x, float y, float angleDeg, Vector2 pivot)
        {
            float a = MathHelper.DegreesToRadians(angleDeg);

            float s = (float)Math.Sin(a);
            float c = (float)Math.Cos(a);

            float tX = x - pivot.X;
            float tY = y - pivot.Y;

            float rotX = tX * c - tY * s;
            float rotY = tX * s + tY * c;

            return new Vector2(rotX + pivot.X, rotY + pivot.Y);
        }
    }
}
