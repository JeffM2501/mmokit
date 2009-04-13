using System;

namespace Math3D
{
    public enum ContainmentType
    {
        Disjoint,
        Contains,
        Intersects,
    }

    public enum PlaneIntersectionType
    {
        Front,
        Back,
        Intersecting,
    }

    public class Trig
    {
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI); 
        }
    }

    public class FloatRand
    {
        static Random rand = new Random();

        public static float RandInRange(float min, float max)
        {
            return (float)(rand.NextDouble() * (max - min) + min);
        }

        public static float RandPlusMinus()
        {
            return RandInRange(-1, 1);
        }
    }
}
