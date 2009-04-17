using System;
using System.Collections.Generic;

using OpenTK.Math;

namespace Math3D
{
    class Polygon
    {
        List<Vector3> points;
        Plane plane;

        public bool Valid ()
        {
            return points.Count > 2;
        }

        public bool Clip ( Plane clipPlane)
        {
            List<Vector3> newPoints = new List<Vector3>();

            foreach (Vector3 p in points)
            {
                if (clipPlane.Intersects(p) != PlaneIntersectionType.Back)
                    newPoints.Add(p);
            }
            points = newPoints;
            return Valid();
        }

        public static Polygon Clip ( Polygon input, Plane clipPlane )
        {
            List<Vector3> newPoints = new List<Vector3>();

            foreach(Vector3 p in input.points)
            {
                if (clipPlane.Intersects(p) != PlaneIntersectionType.Back)
                    newPoints.Add(p);
            }

            Polygon outPoly = new Polygon();
            outPoly.plane = new Plane(input.plane.Normal, input.plane.D);
            outPoly.points = newPoints;

            return outPoly;
        }
    }
}
