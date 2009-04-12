using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class Plane
    {
        public float D;
        public Vector3 Normal;

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
        }

        public Plane(float a, float b, float c, float d)
        {

        }

        public PlaneIntersectionType Intersects(BoundingBox box)
        {
            return PlaneIntersectionType.Intersecting;
        }

        public PlaneIntersectionType Intersects(BoundingFrustum frustum)
        {
            return PlaneIntersectionType.Intersecting;

        }

        public PlaneIntersectionType Intersects(BoundingSphere sphere)
        {
            return PlaneIntersectionType.Intersecting;
        }
    }
}
