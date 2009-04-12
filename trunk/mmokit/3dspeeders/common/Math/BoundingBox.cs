using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class BoundingBox
    {
        public const int CornerCount = 8;

        public Vector3 Max;
        public Vector3 Min;

        // to make it easier to compute things
        protected Vector3 Center;
        protected Vector3 Size;
        protected float Radius;

        public Vector3 CenterPoint
        {
            get { return Center; }
        }

        public Vector3 BoxSize
        {
            get { return Size; }
        }

        protected bool doRadTests = false;

        public BoundingBox()
        {
            Max = new Vector3();
            Min = new Vector3();
            ComputeCenter();
        }

        public BoundingBox(ref Vector3 min, ref Vector3 max)
        {
            Max = new Vector3(max);
            Min = new Vector3(min);
            ComputeCenter();
        }

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Max = new Vector3(max);
            Min = new Vector3(min);
            ComputeCenter();
        }

        protected void ComputeCenter()
        {
            Size = new Vector3((Max - Min) / 2.0f);
            Center = new Vector3(Min+Size);

            Radius = Size.Length;
        }

        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return a.Min != b.Min || a.Max != b.Max;
        }

        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Min == b.Min && a.Max == b.Max;
        }
        
        public override bool Equals(object other)
        {
            BoundingBox box = other as BoundingBox;
            if (box == null)
                return false;

            return this == box;
        }

        public bool Equals(BoundingBox box)
        {
            return this == box;
        }
       
        public override int GetHashCode()
        {
            return Max.GetHashCode() ^ Min.GetHashCode();
        }

        public ContainmentType Contains(BoundingBox box)
        {
            if(doRadTests)
            {
                Vector3 dist = Center-box.Center;
                float mag = dist.LengthSquared;
                if (mag > Radius*Radius+box.Radius*Radius)
                    return ContainmentType.Disjoint; // the rads are outside, so it MUST be Disjoint;
            }

            // check to see our axes are outside of his axes;
            bool xIn = true, yIn = true, zIn = true;

            if ((Min.X > box.Max.X) && (Max.X < box.Min.X))
                xIn = false;
            if ((Min.Y > box.Max.Y) && (Max.Y < box.Min.Y))
                yIn = false;
            if ((Min.Z > box.Max.Z) && (Max.Z < box.Min.Z))
                zIn = false;

            if (zIn && yIn && xIn) // all of them are inside of me
                return ContainmentType.Contains;
            if (!zIn && !yIn && !xIn) // all of them are inside of me
                return ContainmentType.Disjoint;

            return ContainmentType.Intersects;
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            ContainmentType[] status = new ContainmentType[3] { ContainmentType.Intersects, ContainmentType.Intersects, ContainmentType.Intersects };

            Vector3 vec = sphere.CenterPoint - CenterPoint;

            if (Math.Abs(vec.X) > Size.X + sphere.Radius)
                status[0] = ContainmentType.Disjoint;
            else if (Math.Abs(vec.X) < Size.X - sphere.Radius)
                status[0] = ContainmentType.Contains;

            if (Math.Abs(vec.Y) > Size.Y + sphere.Radius)
                status[1] = ContainmentType.Disjoint;
            else if (Math.Abs(vec.Y) < Size.Y - sphere.Radius)
                status[1] = ContainmentType.Contains;

            if (Math.Abs(vec.Z) > Size.Z + sphere.Radius)
                status[2] = ContainmentType.Disjoint;
            else if (Math.Abs(vec.Z) < Size.Z - sphere.Radius)
                status[2] = ContainmentType.Contains;

            if (status[0] == status[1] && status[0] == status[2])
                return status[0];

            return ContainmentType.Intersects;
        }

        public bool Intersects ( BoundingFrustum frustum )
        {
            return frustum.Contains(this) != ContainmentType.Disjoint;
        }

        public bool Intersects(BoundingBox box)
        {
            return box.Contains(this) != ContainmentType.Disjoint;
        }

        public bool Intersects(BoundingSphere sphere)
        {
            return sphere.Contains(this) != ContainmentType.Disjoint;
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            Vector3 inside = new Vector3();  // inside point  (assuming partial)
            Vector3 outside = new Vector3(); // outside point (assuming partial)
            float len = 0.0f;

            // setup the inside/outside corners
            // this can be determined easily based
            // on the normal vector for the plane
            if (plane.Normal.X > 0.0f)
            {
                inside.X = Max.X;
                outside.X = Min.X;
            }
            else
            {
                inside.X = Min.X;
                outside.X = Max.X;
            }
            if (plane.Normal.Y > 0.0f)
            {
                inside.Y = Max.Y;
                outside.Y = Min.X;
            }
            else
            {
                inside.Y = Min.Y;
                outside.Y = Max.Y;
            }

            if (plane.Normal.Z > 0.0f)
            {
                inside.Z = Max.Z;
                outside.Z = Min.Z;
            }
            else
            {
                inside.Z = Min.Z;
                outside.Z = Max.Z;
            }

            // check the inside length
            len = Vector3.Dot(inside, plane.Normal) + plane.D;
            if (len < -1.0f)
                return PlaneIntersectionType.Back; // box is fully outside the frustum

            // check the outside length
            len = Vector3.Dot(outside, plane.Normal) + plane.D;
            if (len < -1.0f)
                return PlaneIntersectionType.Intersecting; // partial containment at best

            return PlaneIntersectionType.Front;
        }
       
        public Vector3 Corner( int index )
        {
            if (index < 1)
                return Min;                

           switch(index)
           {
               case 1:
                   return new Vector3(Min.X,Max.Y,Min.Z);
               case 2:
                   return new Vector3(Max.X, Min.Y, Min.Z);
               case 3:
                   return new Vector3(Max.X, Max.Y, Min.Z);
               case 4:
                   return new Vector3(Min.X, Min.Y, Max.Z);
               case 5:
                   return new Vector3(Min.X, Max.Y, Max.Z);
               case 6:
                   return new Vector3(Max.X, Min.Y, Max.Z);
           }
           return Max;
        }

        public void GetCorners(Vector3[] corners)
        {
            for (int i = 0; i < corners.Length; i++)
                corners[i] = Corner(i);
        }

        public static BoundingBox CreateMerged ( BoundingBox b1, BoundingBox b2 )
        {
            BoundingBox box = new BoundingBox();

            Vector3 min = new Vector3(0, 0, 0);
            Vector3 max = new Vector3(0, 0, 0);

            for (int i = 0; i < 8; i++ )
            {
                Vector3 vec = b1.Corner(i);
                if (vec.X < min.X)
                    min.X = vec.X;
                if (vec.X > max.X)
                    max.X = vec.X;
                if (vec.Y < min.Y)
                    min.Y = vec.Y;
                if (vec.Y > max.Y)
                    max.Y = vec.Y;
                if (vec.Z < min.Z)
                    min.Z = vec.Z;
                if (vec.Z > max.Y)
                    max.Z = vec.Z;
            }

            return new BoundingBox(ref min, ref max);
        }

        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            Vector3 min = new Vector3();
            Vector3 max = new Vector3();
            foreach(Vector3 p in points)
            {
                if (p.X < min.X)
                    min.X = p.X;
                if (p.X > max.X)
                    max.X = p.X;

                if (p.Y < min.Y)
                    min.Y = p.Y;
                if (p.Y > max.Y)
                    max.Y = p.Y;

                if (p.Z < min.Z)
                    min.Z = p.Z;
                if (p.Z > max.Z)
                    max.Z = p.Z;
            }

            return new BoundingBox(min, max);
        }
       
        public static BoundingBox CreateFromSphere(BoundingSphere sphere)
        {
            Vector3 min = sphere.CenterPoint * -sphere.Radius;
            Vector3 max = sphere.CenterPoint * sphere.Radius;
            return new BoundingBox(min, max);
        }

        public override string ToString()
        {
            return Min.ToString() + Max.ToString();
        }
    }
}
