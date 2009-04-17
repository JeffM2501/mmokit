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

        public static float DegreeToRadian(float angle)
        {
            return (float)Math.PI * angle / 180.0f;
        }

        public static float RadianToDegree(float angle)
        {
            return angle * (180.0f / (float)Math.PI);
        }

        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public static float Hypot(float x, float y)
        {
            return (float)Math.Sqrt(x * x + y * y);
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

    public class FrustumHelper
    {
        public static Plane[] GetPlanes ( BoundingFrustum frustum )
        {
            Plane[] l = new Plane[6];
            l[0] = frustum.Near;
            l[1] = frustum.Left;
            l[2] = frustum.Right;
            l[3] = frustum.Top;
            l[4] = frustum.Bottom;
            l[5] = frustum.Far;

            return l;
        }
    }

    public class PlaneHelper
    {
        // sets a plane from 3 points with out a copy
        public static void Set ( ref Plane plane, Vector3 p1, Vector3 p2, Vector3 p3 )
        {
                        // get normal by crossing v1 and v2 and normalizing
            plane.Normal = Vector3.Cross(p1, p2);
            plane.Normal.Normalize();
            plane.D = -Vector3.Dot(p3, plane.Normal);
        }
    }

    public class MatrixHelper4
    {
        // matrix grid methods
        public static float M11(Matrix4 m) { return m.Row0.X; }
        public static void M11(ref Matrix4 m, float value) { m.Row0.X = value; }

        public static float M12(Matrix4 m) { return m.Row0.Y; }
        public static void M12(ref Matrix4 m, float value) { m.Row0.Y = value; }

        public static float M13(Matrix4 m) { return m.Row0.Z; }
        public static void M13(ref Matrix4 m, float value) { m.Row0.Z = value; }

        public static float M14(Matrix4 m) { return m.Row0.W; }
        public static void M14(ref Matrix4 m, float value) { m.Row0.W = value; }

        public static float M21(Matrix4 m) { return m.Row1.X; }
        public static void M21(ref Matrix4 m, float value) { m.Row1.X = value; }

        public static float M22(Matrix4 m) { return m.Row1.Y; }
        public static void M22(ref Matrix4 m, float value) { m.Row1.Y = value; }

        public static float M23(Matrix4 m) { return m.Row1.Z; }
        public static void M23(ref Matrix4 m, float value) { m.Row1.Z = value; }

        public static float M24(Matrix4 m) { return m.Row1.W; }
        public static void M24(ref Matrix4 m, float value) { m.Row1.W = value; }

        public static float M31(Matrix4 m) { return m.Row2.X; }
        public static void M31(ref Matrix4 m, float value) { m.Row2.X = value; }

        public static float M32(Matrix4 m) { return m.Row2.Y; }
        public static void M32(ref Matrix4 m, float value) { m.Row2.Y = value; }

        public static float M33(Matrix4 m) { return m.Row2.Z; }
        public static void M33(ref Matrix4 m, float value) { m.Row2.Z = value; }

        public static float M34(Matrix4 m) { return m.Row2.W; }
        public static void M34(ref Matrix4 m, float value) { m.Row2.W = value; }

        public static float M41(Matrix4 m) { return m.Row3.X; }
        public static void M41(ref Matrix4 m, float value) { m.Row3.X = value; }

        public static float M42(Matrix4 m) { return m.Row3.Y; }
        public static void M42(ref Matrix4 m, float value) { m.Row3.Y = value; }

        public static float M43(Matrix4 m) { return m.Row3.Z; }
        public static void M43(ref Matrix4 m, float value) { m.Row3.Z = value; }

        public static float M44(Matrix4 m) { return m.Row3.W; }
        public static void M44(ref Matrix4 m, float value) { m.Row3.W = value; }

        // matrix index methods
//         public static float m0(Matrix4 m) { return m.Row0.X; }
//         public static void m0(ref Matrix4 m, float value) { m.Row0.X = value; }
// 
//         public static float m1(Matrix4 m) { return m.Row0.Y; }
//         public static void m1(ref Matrix4 m, float value) { m.Row0.Y = value; }
// 
//         public static float m2(Matrix4 m) { return m.Row0.Z; }
//         public static void m2(ref Matrix4 m, float value) { m.Row0.Z = value; }
//      
//         public static float m3(Matrix4 m) { return m.Row0.W; }
//         public static void m3(ref Matrix4 m, float value) { m.Row0.W = value; }
// 
//         public static float m4(Matrix4 m) { return m.Row1.X; }
//         public static void m4(ref Matrix4 m, float value) { m.Row1.X = value; }
// 
//         public static float m5(Matrix4 m) { return m.Row1.Y; }
//         public static void m5(ref Matrix4 m, float value) { m.Row1.Y = value; }
// 
//         public static float m6(Matrix4 m) { return m.Row1.Z; }
//         public static void m6(ref Matrix4 m, float value) { m.Row1.Z = value; }
// 
//         public static float m7(Matrix4 m) { return m.Row1.W; }
//         public static void m7(ref Matrix4 m, float value) { m.Row1.W = value; }
// 
//         public static float m8(Matrix4 m) { return m.Row2.X; }
//         public static void m8(ref Matrix4 m, float value) { m.Row2.X = value; }
// 
//         public static float m9(Matrix4 m) { return m.Row2.Y; }
//         public static void m9(ref Matrix4 m, float value) { m.Row1.Y = value; }
// 
//         public static float m10(Matrix4 m) { return m.Row2.Z; }
//         public static void m10(ref Matrix4 m, float value) { m.Row2.Z = value; }
// 
//         public static float m11(Matrix4 m) { return m.Row2.W; }
//         public static void m11(ref Matrix4 m, float value) { m.Row2.W = value; }
// 
//         public static float m12(Matrix4 m) { return m.Row3.X; }
//         public static void m12(ref Matrix4 m, float value) { m.Row3.X = value; }
// 
//         public static float m13(Matrix4 m) { return m.Row3.Y; }
//         public static void m13(ref Matrix4 m, float value) { m.Row3.Y = value; }
// 
//         public static float m14(Matrix4 m) { return m.Row3.Z; }
//         public static void m14(ref Matrix4 m, float value) { m.Row3.Z = value; }
//      
//         public static float m15(Matrix4 m) { return m.Row3.W; }
//         public static void m15(ref Matrix4 m, float value) { m.Row3.W = value; }

// col major
        public static float m0(Matrix4 m) { return m.Row0.X; }
        public static void m0(ref Matrix4 m, float value) { m.Row0.X = value; }

        public static float m1(Matrix4 m) { return m.Row1.X; }
        public static void m1(ref Matrix4 m, float value) { m.Row1.X = value; }

        public static float m2(Matrix4 m) { return m.Row2.X; }
        public static void m2(ref Matrix4 m, float value) { m.Row2.X = value; }

        public static float m3(Matrix4 m) { return m.Row3.X; }
        public static void m3(ref Matrix4 m, float value) { m.Row3.X = value; }

        public static float m4(Matrix4 m) { return m.Row0.Y; }
        public static void m4(ref Matrix4 m, float value) { m.Row0.Y = value; }

        public static float m5(Matrix4 m) { return m.Row1.Y; }
        public static void m5(ref Matrix4 m, float value) { m.Row1.Y = value; }

        public static float m6(Matrix4 m) { return m.Row2.Y; }
        public static void m6(ref Matrix4 m, float value) { m.Row2.Y = value; }

        public static float m7(Matrix4 m) { return m.Row3.Y; }
        public static void m7(ref Matrix4 m, float value) { m.Row2.Y = value; }

        public static float m8(Matrix4 m) { return m.Row0.Z; }
        public static void m8(ref Matrix4 m, float value) { m.Row0.Z = value; }

        public static float m9(Matrix4 m) { return m.Row1.Z; }
        public static void m9(ref Matrix4 m, float value) { m.Row1.Z = value; }

        public static float m10(Matrix4 m) { return m.Row2.Z; }
        public static void m10(ref Matrix4 m, float value) { m.Row2.Z = value; }

        public static float m11(Matrix4 m) { return m.Row3.Z; }
        public static void m11(ref Matrix4 m, float value) { m.Row3.Z = value; }

        public static float m12(Matrix4 m) { return m.Row0.W; }
        public static void m12(ref Matrix4 m, float value) { m.Row0.W = value; }

        public static float m13(Matrix4 m) { return m.Row1.W; }
        public static void m13(ref Matrix4 m, float value) { m.Row1.W = value; }

        public static float m14(Matrix4 m) { return m.Row2.W; }
        public static void m14(ref Matrix4 m, float value) { m.Row2.W = value; }

        public static float m15(Matrix4 m) { return m.Row3.W; }
        public static void m15(ref Matrix4 m, float value) { m.Row3.W = value; }

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
