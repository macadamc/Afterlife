using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadyPixel
{
    public static class ShadyMath
    {
        /// <summary>
        /// Snaps a <paramref name="direction"/> vector to an angle increment <paramref name="angleSnap"/> (eg 45)
        /// </summary>
        /// <param name="direction">The direction vector to snap.</param>
        /// <param name="angleSnap">The angle to snap to.</param>
        public static Vector3 AngleSnap(Vector2 direction, int angleSnap)
        {
            //  gets the angle from the look direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //  rotates object to face the new angle
            Vector3 vec = Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles;

            vec.x = Mathf.Round(vec.x / angleSnap) * angleSnap;
            vec.y = Mathf.Round(vec.y / angleSnap) * angleSnap;
            vec.z = Mathf.Round(vec.z / angleSnap) * angleSnap;

            return Quaternion.Euler(vec) * Vector3.right;
        }

        /// <summary>
        /// Test if two <see cref="Vector2"/> are parallel to one another.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public static bool IsParallel(Vector2 v1, Vector2 v2)
        {
            //2 vectors are parallel if the angle between the vectors are 0 or 180 degrees
            if (IsWithin(Vector2.Angle(v1, v2), -.2f, .2f) || IsWithin(Vector2.Angle(v1, v2), 179.8f, 180.2f))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simple helper Min/Max check.
        /// </summary>
        /// <param name="value"> the float to check.</param>
        /// <param name="min">min value</param>
        /// <param name="max">max value</param>
        public static bool IsWithin(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        //linePnt - point the line passes through
        //lineDir - unit vector in direction of line, either direction works
        //pnt - the point to find nearest on line for
        /// <summary>
        /// Finds the nearest point from <paramref name="pnt"/> on an infinite line with <paramref name="linePnt"/> as tthe center expanding along a direction. (<paramref name="lineDir"/>)
        /// </summary>
        /// <param name="linePnt">Line center point</param>
        /// <param name="lineDir">the direction the line moves along.</param>
        /// <param name="pnt"> the point used to find the nearest point on the line.</param>
        public static Vector3 NearestPointOnLine(Vector2 linePnt, Vector2 lineDir, Vector2 pnt)
        {
            lineDir.Normalize();//this needs to be a unit vector
            var v = pnt - linePnt;
            var d = Vector2.Dot(v, lineDir);
            return linePnt + lineDir * d;
        }
        /// <summary>
        /// Finds the nearest point from <paramref name="pnt"/> on an line starting at <paramref name="start"/> and ending at <paramref name="end"/> 
        /// </summary>
        /// <param name="linePnt">Line center point</param>
        /// <param name="lineDir">the direction the line moves along.</param>
        /// <param name="pnt"> the point used to find the nearest point on the line.</param>
        public static Vector3 NearestPointOnFiniteLine(Vector3 start, Vector3 end, Vector3 pnt)
        {
            var line = (end - start);
            var len = line.magnitude;
            line.Normalize();

            var v = pnt - start;
            var d = Vector3.Dot(v, line);
            d = Mathf.Clamp(d, 0f, len);
            return start + line * d;
        }
    }
}