using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class BoundingSphere
    {
        public float Radius;
        protected Vector3 Center;

        public Vector3 CenterPoint
        {
            get { return Center; }
        }

        public BoundingSphere(ref Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public BoundingSphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public ContainmentType Contains(BoundingBox box)
        {
            ContainmentType first = Contains(box.Corner(0));
            for (int i = 1; i < 8; i++)
            {
                if (first != Contains(box.Corner(i)))
                    return ContainmentType.Intersects;
            }
            return first;
        }
        
        public ContainmentType Contains(BoundingSphere sphere)
        {
            Vector3 dist = Center-sphere.Center;
            float mag = dist.LengthSquared;
            if (mag + sphere.Radius * sphere.Radius < Radius * Radius)
                return ContainmentType.Contains;
            if (mag > sphere.Radius * sphere.Radius + Radius * Radius)
                return ContainmentType.Disjoint;
            return ContainmentType.Intersects;
        }
      
        public ContainmentType Contains(Vector3 point)
        {
            Vector3 dist = Center-point;
            if (dist.LengthSquared > Radius * Radius)
                return ContainmentType.Disjoint;
            return ContainmentType.Contains;
        }

        public float Distance (BoundingSphere sphere)
        {
            Vector3 dist = Center - sphere.Center;
            return dist.Length - sphere.Radius - Radius;
        }

        public float Distance(Vector3 point)
        {
            Vector3 dist = Center - point;
            return dist.Length-Radius;
        }
       
        public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
        {
            return new BoundingSphere(box.CenterPoint,box.BoxSize.Length);
        }
        
        public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum)
        {
            return new BoundingSphere(new Vector3(),0);
        }

        public static bool operator !=(BoundingSphere a, BoundingSphere b)
        {
            return a.Center != b.Center || a.Radius != b.Radius;
        }

        public static bool operator ==(BoundingSphere a, BoundingSphere b)
        {
            return a.Center == b.Center && a.Radius == b.Radius;
        }
        public bool Equals(BoundingSphere other)
        {
            return Center==other.Center && Radius == other.Radius;
        }
        
        public override bool Equals(object obj)
        {
            BoundingSphere other = obj as BoundingSphere;
            if (other == null)
                return false;
            return this == other;
        }
       
        public override int GetHashCode()
        {
            return Center.GetHashCode() ^ Radius.GetHashCode();
        }

        public override string ToString()
        {
            return Center.ToString() + Radius.ToString();
        }
    }
}

