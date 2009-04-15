using System;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class Plane
    {
        public float D;
        public Vector3 Normal;

        static float SmallNumber = 0.00001f;

        public Plane(Vector3 norm, float d )
        {
            D = d;
            Normal = norm;
        }

        public Plane (ref Vector3 p1, ref Vector3 p2, ref Vector3 p3 )
        {
            Vector3 v1 = p2 - p1;
            Vector3 v2 = p3 - p1;
            Normal = Vector3.Cross(v1, v2);
            Normal.Normalize();

            D = Vector3.Dot(Normal * -1.0f, p1);
        }

        public Plane(float a, float b, float c, float d)
        {
            Normal = new Vector3(a, b, c);
            D = d;
        }

        public void Set (float a, float b, float c, float d)
        {
            Normal.X = a;
            Normal.Y = b;
            Normal.Z = c;
            Normal.Normalize();
            D = d;
        }

        public PlaneIntersectionType Intersects(BoundingBox box)
        {
            PlaneIntersectionType first = Intersects(box.Corner(0));
            for (int i = 1; i < 8; i++)
            {
                if (first != Intersects(box.Corner(i)))
                    return PlaneIntersectionType.Intersecting;
            }
            return first;
        }

        public PlaneIntersectionType Intersects(BoundingFrustum frustum)
        {
            return PlaneIntersectionType.Intersecting;
        }

        public PlaneIntersectionType Intersects(BoundingSphere sphere)
        {
            float d = Distance(sphere.CenterPoint);
            if (Math.Abs(d) < sphere.Radius)
                return PlaneIntersectionType.Intersecting;
           
            if (d < 0)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Front;
        }

        public PlaneIntersectionType Intersects(Vector3 point)
        {
            float d = Distance(point);
            if (Math.Abs(d) < Plane.SmallNumber)
                return PlaneIntersectionType.Intersecting;
            if (d < 0)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Front;
        }

        public float Distance(Vector3 point)
        {
            return (Normal.X * point.X + Normal.Y * point.Y + Normal.Z * point.Z + D);
        }

        public float Distance(BoundingSphere sphere)
        {
            return (Normal.X * sphere.CenterPoint.X + Normal.Y * sphere.CenterPoint.Y + Normal.Z * sphere.CenterPoint.Z + D) - sphere.Radius;
        }

        public static bool operator !=(Plane a, Plane b)
        {
            return a.Normal != b.Normal || a.D != b.D;
        }

        public static bool operator ==(Plane a, Plane b)
        {
            return a.Normal == b.Normal && a.D == b.D;
        }

        public bool Equals(Plane other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            Plane other = obj as Plane;
            if (other == null)
                return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }

        public override string ToString()
        {
            return Normal.ToString() + D.ToString(); ;
        }

        public static Vector3 Intersection (Plane p1, Plane p2, Plane p3)
        {
            Vector3 point = new Vector3();

            if (p1.Normal == p2.Normal || p1.Normal == p3.Normal || p2.Normal == p3.Normal)
                return point;

            Matrix4 matrix = new Matrix4(new Vector4(p1.Normal),new Vector4(p2.Normal),new Vector4(p3.Normal),new Vector4(0,0,0,1f));
            matrix.Invert();

            Matrix4 dMatrix = new Matrix4(new Vector4(-p1.D,0,0,0),new Vector4(-p2.D,0,0,1f),new Vector4(-p3.D,0,1f,0),new Vector4(0,0,0,1f));

            Matrix4 result = matrix * dMatrix;

            point.X = result.Column0.X;
            point.Y = result.Column0.Y;
            point.Z = result.Column0.Z;

            return point;
        }
    }
}
