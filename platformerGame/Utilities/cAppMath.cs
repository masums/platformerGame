﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;

namespace platformerGame.Utilities
{
    /// <summary>
    /// Used as a callback function in the RayTrace function.
    /// </summary>
    /// <param name="x">the current X position to visit</param>
    /// <param name="y">the current Y position to visit</param>
    /// <returns>True, if the raytrace algoryhtm can be interrupted.</returns>
    public delegate bool VisitMethod(int x, int y);

    /// <summary>
    /// Contains mathematical utilies to make life easier in game programming.
    /// </summary>
    class cAppMath
    {
        public const double PI = 3.1415926535897932384626433832795;
        public const double TWO_PI = PI * 2.0;
        public const double HALF_PI = PI / 2.0;

        public const float MAX_FLOAT = float.MaxValue;
        public const float MIN_FLOAT = float.MinValue;
        public const int MAX_INT = int.MaxValue;
        public const double MAX_DOUBLE = double.MaxValue;
        public const double MIN_DOUBLE = double.MinValue;

        //==============================================================================================
        // RANDOM HELPER FUNCTIONS
        //==============================================================================================
        public static int GetRandomNumber(int min, int max)
        {
            return cAppRandom.GetRandomNumber(min, max);
        }

        public static byte GetRandomByte(byte min, byte max)
        {
            return (byte)cAppRandom.GetRandomNumber(min, max);
        }

        public static double GetRandomDouble()
        {
            return cAppRandom.GetRandomDouble();
        }

        public static double GetRandomDoubleInRange(double x, double y)
        {
            return x + GetRandomDouble() * (y - x);
        }
        //==============================================================================================

        public static T Max<T>(T val1, T val2) where T : IComparable<T>
        {
            return (Comparer<T>.Default.Compare(val1, val2) > 0 ? val1 : val2);
        }
        //==============================================================================================
        public static T Min<T>(T val1, T val2) where T : IComparable<T>
        {
            return (Comparer<T>.Default.Compare(val1, val2) < 0 ? val1 : val2);
        }

        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            return Comparer<T>.Default.Compare(val, min) < 0 ? min : Comparer<T>.Default.Compare(val, max) > 0 ? max : val;
            //return ((val < min) ? min : (val > max) ? max : val);
        }

        public static T Truncate<T>(T val, T min, T max) where T : IComparable<T>
        {
            return Comparer<T>.Default.Compare(val, min) < 0 ? min : Comparer<T>.Default.Compare(val, max) > 0 ? max : val;
            //return ((val < min) ? min : (val > max) ? max : val);
        }

        public static double GetAngleOfVector(Vector2f vec)
        {
            return (Math.Atan2(vec.X, -vec.Y) - HALF_PI);
        }

        //Getting the angle of two vectors
        public static float GetAngleBetwenVecs(Vector2f vecA, Vector2f vecB)
        {
            return (float)Math.Acos(Vec2Dot(vecA, vecB) / (Vec2Length(vecA) * Vec2Length(vecB)));
        }
        //==============================================================================================
        //only works correctly with unit vectors
        public static float GetAngleBetwenUnitVecs(Vector2f uvecA, Vector2f uvecB)
        {
            return (float)Math.Acos(Vec2Dot(uvecA, uvecB));
        }

        /// <summary>
        /// Calculate a random vector by giving a middle direction and a spread angle.
        /// </summary>
        /// <param name="dir">the "middle" direction vector</param>
        /// <param name="ang_spread">the spread angle in radian</param>
        /// <returns>A unit vector of the random direction.</returns>
        public static Vector2f GetRandomVecBySpread(Vector2f dir, double ang_spread) //in radian
        {
            Vector2f returner;
            double angOfDir = GetAngleOfVector(dir);

            double randomAngInRad = GetRandomDoubleInRange(angOfDir - ang_spread / 2.0, angOfDir + ang_spread / 2.0);
            returner.X = (float)Math.Cos(randomAngInRad);
            returner.Y = (float)Math.Sin(randomAngInRad);

            return returner;
        }

        public static Vector2f GetRandomUnitVec() //in radian
        {
            Vector2f returner;

            double randomAngInRad = GetRandomDoubleInRange(-PI, PI);
            returner.X = (float)Math.Cos(randomAngInRad);
            returner.Y = (float)Math.Sin(randomAngInRad);

            return returner;
        }

        public static void Raytrace(int x0, int y0, int x1, int y1, VisitMethod breakVisit)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int x = x0;
            int y = y0;
            int n = 1 + dx + dy;
            int x_inc = (x1 > x0) ? 1 : -1;
            int y_inc = (y1 > y0) ? 1 : -1;
            int error = dx - dy;
            dx *= 2;
            dy *= 2;
            while (n > 0 && !breakVisit(x, y))
            //for (; n > 0; --n)
            {

                if (error > 0)
                {
                    x += x_inc;
                    error -= dy;
                }
                else
                {
                    y += y_inc;
                    error += dx;
                }

                --n;
            }
        }

        public static int side(Vector2f lineA, Vector2f lineB, Vector2f point)
        {
            Vector2f diff = lineB - lineA;
            Vector2f perp = new Vector2f(-diff.Y, diff.X);
            float d = cAppMath.Vec2Dot(point - lineA, perp);
            return Math.Sign(d);
        }

        public static Vector2f Vec2Perp(Vector2f vec)
        {
            return new Vector2f(vec.Y, -vec.X);
        }

        public static Vector2f Vec2Opposite(Vector2f vec)
        {
            return new Vector2f(-vec.X, -vec.Y);
        }
        public static double Vec2Length(Vector2f vec)
        {
            return Math.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
        }

        public static double Vec2Distance(Vector2f pointA, Vector2f pointB)
        {
            Vector2f sub = pointA - pointB;
            return Math.Sqrt((sub.X * sub.X) + (sub.Y * sub.Y));
        }

        public static double Vec2DistanceSqrt(Vector2f pointA, Vector2f pointB)
        {
            Vector2f sub = pointA - pointB;
            return (sub.X * sub.X) + (sub.Y * sub.Y);
        }

        //==============================================================================================
        public static void Vec2Normalize(ref Vector2f vec)
        {
            double len = Vec2Length(vec);

            if (len > 0.0)
            {
                vec.X /= (float)len;
                vec.Y /= (float)len;
            }
        }

        public static Vector2f Vec2NormalizeReturn(Vector2f vec)
        {
            Vector2f r = new Vector2f(vec.X, vec.Y);
            double len = Vec2Length(r);

            if (len > 0.0)
            {
                r.X /= (float)len;
                r.Y /= (float)len;
            }

            return r;
        }

        public static float Vec2Dot(Vector2f a, Vector2f b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        public static bool Vec2IsZero(Vector2f vec)
        {
            return ((vec.X * vec.X + vec.Y * vec.Y) <= 0.0f);
        }

        public static Vector2f Vec2Abs(Vector2f v)
        {
            return new Vector2f(Math.Abs(v.X), Math.Abs(v.Y));
        }
        public static void Vec2Truncate(ref Vector2f vec, double max)
        {
            if (Vec2Length(vec) > max)
            {
                Vec2Normalize(ref vec);
                vec.X *= (float)max;
                vec.Y *= (float)max;
            }
        }

        public static double DegressToRadian(double deg)
        {
            return deg * (Math.PI / 180.0);
        }
        //==============================================================================================
        public static double RadianToDegress(double rad)
        {
            return rad * (180.0 / PI);
        }

        public static float Lerp(float from, float to, float value)
        {
            if (value < 0.0f)
                return from;
            else if (value > 1.0f)
                return to;
            return (to - from) * value + from;
        }
        public static float LerpUnclamped(float from, float to, float value)
        {
            return (1.0f - value) * from + value * to;
        }

        
        public static Vector2f Interpolate(Vector2f current_pos, Vector2f last_pos, float alpha)
        {
            //return current_pos;

            Vector2f ip = new Vector2f();
            ip.X = current_pos.X * alpha + last_pos.X * (1.0f - alpha);
            ip.Y = current_pos.Y * alpha + last_pos.Y * (1.0f - alpha);
            return ip;
        }
    }
}
