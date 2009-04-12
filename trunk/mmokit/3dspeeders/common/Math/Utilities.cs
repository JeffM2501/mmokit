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
}
