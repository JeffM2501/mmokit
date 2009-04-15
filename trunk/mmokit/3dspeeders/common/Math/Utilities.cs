using System;

using OpenTK.Math;

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

    public class MatrixHelper4
    {
        public static float M11(Matrix4 m) { return m.Row0.X; }
        public static void M11(Matrix4 m, float value) { m.Row0.X = value; }

        public static float M12(Matrix4 m) { return m.Row0.Y; }
        public static void M12(Matrix4 m, float value) { m.Row0.Y = value; }

        public static float M13(Matrix4 m) { return m.Row0.Z; }
        public static void M13(Matrix4 m, float value) { m.Row0.Z = value; }

        public static float M14(Matrix4 m) { return m.Row0.W; }
        public static void M14(Matrix4 m, float value) { m.Row0.W = value; }

        public static float M21(Matrix4 m) { return m.Row1.X; }
        public static void M21(Matrix4 m, float value) { m.Row1.X = value; }

        public static float M22(Matrix4 m) { return m.Row1.Y; }
        public static void M22(Matrix4 m, float value) { m.Row1.Y = value; }

        public static float M23(Matrix4 m) { return m.Row1.Z; }
        public static void M23(Matrix4 m, float value) { m.Row1.Z = value; }

        public static float M24(Matrix4 m) { return m.Row1.W; }
        public static void M24(Matrix4 m, float value) { m.Row1.W = value; }

        public static float M31(Matrix4 m) { return m.Row2.X; }
        public static void M31(Matrix4 m, float value) { m.Row2.X = value; }

        public static float M32(Matrix4 m) { return m.Row2.Y; }
        public static void M32(Matrix4 m, float value) { m.Row2.Y = value; }

        public static float M33(Matrix4 m) { return m.Row2.Z; }
        public static void M33(Matrix4 m, float value) { m.Row2.Z = value; }

        public static float M34(Matrix4 m) { return m.Row2.W; }
        public static void M34(Matrix4 m, float value) { m.Row2.W = value; }

        public static float M41(Matrix4 m) { return m.Row3.X; }
        public static void M41(Matrix4 m, float value) { m.Row3.X = value; }

        public static float M42(Matrix4 m) { return m.Row3.Y; }
        public static void M42(Matrix4 m, float value) { m.Row3.Y = value; }

        public static float M43(Matrix4 m) { return m.Row3.Z; }
        public static void M43(Matrix4 m, float value) { m.Row3.Z = value; }

        public static float M44(Matrix4 m) { return m.Row3.W; }
        public static void M44(Matrix4 m, float value) { m.Row3.W = value; }
    }

    public class VectorHelper3
    {
        public static float Distance ( Vector3 v1, Vector3 v2)
        {
            return (float)Math.Sqrt((v2.X - v1.X) * (v2.X - v1.X) + (v2.Y - v1.Y) * (v2.Y - v1.Y) + (v2.Z - v1.Z) * (v2.Z - v1.Z));
        }

        public static float DistanceSquared(Vector3 v1, Vector3 v2)
        {
            return (v2.X - v1.X) * (v2.X - v1.X) + (v2.Y - v1.Y) * (v2.Y - v1.Y) + (v2.Z - v1.Z) * (v2.Z - v1.Z);
        }

        public static Vector3 Subtract(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X-v2.X,v1.Y-v2.Y,v1.Z-v2.Z);
        }
    }
}
