using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using OpenTK;
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
                float mag = dist.Length;
                if (mag > Radius+box.Radius)
                    return ContainmentType.Disjoint; // the rads are outside, so it MUST be Disjoint;
            }

            // check to see our axes are outside of his axes;
            bool xIn = true, yIn = true, zIn = true;

            if ((Center.X - Size.X > box.Center.X + Size.X) && (Center.X + Size.X < box.Center.X - Size.X))
                xIn = false;
            if ((Center.Y - Size.Y > box.Center.Y + Size.Y) && (Center.Y + Size.Y < box.Center.Y - Size.Y))
                yIn = false;
            if ((Center.Z - Size.Z > box.Center.Z + Size.Z) && (Center.Z + Size.Z < box.Center.Z - Size.Z))
                zIn = false;

            if (zIn && yIn && xIn) // all of them are inside of me
                return ContainmentType.Contains;
            if (!zIn && !yIn && !xIn) // all of them are inside of me
                return ContainmentType.Disjoint;

            return ContainmentType.Intersects;
        }

        public bool Intersects ( BoundingFrustum frustum )
        {
            return true;
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
    }
}
