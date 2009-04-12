using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Math;

namespace Math3D
{
    public class BoundingSphere
    {
        public float Radius;
        protected Vector3 Center;

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
  
        public static bool operator !=(BoundingSphere a, BoundingSphere b)
        {
            return a.Center != b.Center || a.Radius != b.Radius;
        }

        public static bool operator ==(BoundingSphere a, BoundingSphere b)
        {
            return a.Center == b.Center && a.Radius == b.Radius;
        }

        public ContainmentType Contains(BoundingBox box)
        {
            return ContainmentType.Disjoint;
        }
        
        public ContainmentType Contains(BoundingFrustum frustum)
        {
            return ContainmentType.Disjoint;
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            Vector3 dist = Center-sphere.Center;
            float mag = dist.Length;
            if ( mag+sphere.Radius < Radius)
                return ContainmentType.Contains;
            if ( mag > sphere.Radius+Radius)
                return ContainmentType.Disjoint;
            return ContainmentType.Intersects;
        }
      
        public ContainmentType Contains(Vector3 point)
        {
            Vector3 dist = Center-point;
            if (dist.Length > Radius)
                return ContainmentType.Disjoint;
            return ContainmentType.Contains;
        }
       
        public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
        {
            return new BoundingSphere(box.CenterPoint,box.BoxSize.Length);
        }
        
        public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum)
        {
            return new BoundingSphere(new Vector3(),0);
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
       
    }
}

