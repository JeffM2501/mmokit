﻿using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class BoundingFrustum
    {
        public const int CornerCount = 8;
        float[] matrix = identity;
        float[] viewMatrix = identity;
        float[] projectionMatrix = identity;

        static float[] identity = new float[16]{1,0,0,0, 0,1,0,0, 1,0,0,0, 0,0,0,0};

        public Plane Near = new Plane(0,0,-1,0);
        public Plane Far = new Plane(0, 0, 1, 0);
        public Plane Left = new Plane(1, 0, 0, 0);
        public Plane Right = new Plane(-1, 0, 0, 0);
        public Plane Top = new Plane(0, -1, 0, 0);
        public Plane Bottom = new Plane(0, 1, 0, 0);

        Plane GetPlane( int index )
        {
            if (index < 1)
                return Near;

            switch(index)
            {
                case 1:
                    return Far;
                case 2:
                    return Left;
                case 3:
                    return Right;
                case 4:
                    return Top;
                case 5:
                    return Bottom;
            }
             return Far;
        }

        void ExtractPlane ( Plane plane, int row )
        {
            int scale = (row < 0) ? -1 : 1;
            row = Math.Abs(row) - 1;

            plane.Set(matrix[3] + scale * matrix[row],matrix[7] + scale * matrix[row + 4],matrix[11] + scale * matrix[row + 8],matrix[15] + scale * matrix[row + 12]);
        }

        public BoundingFrustum()
        {
        }

        public BoundingFrustum(float[] value)
        {
            if (value.Length >= 16)
                viewMatrix = value;

            computeMatrix();
        }

        public BoundingFrustum(float[] view, float[] proj )
        {
            if (view.Length >= 16)
                viewMatrix = view;
            if (proj.Length >= 16)
                projectionMatrix = proj;
            computeMatrix();
        }

        public ContainmentType Contains(BoundingSphere sphere)
        {
            ContainmentType result = ContainmentType.Contains;
            for (int i = 0; i < 6; i++)
            {
                Plane plane = GetPlane(i);
                PlaneIntersectionType planeIntersect = plane.Intersects(sphere);
                if (planeIntersect == PlaneIntersectionType.Back)
                    return ContainmentType.Disjoint;
                else if (planeIntersect == PlaneIntersectionType.Intersecting)
                    result = ContainmentType.Intersects;
            }

            return result;
        }

        public ContainmentType Contains(ref Vector3 point)
        {
            ContainmentType result = ContainmentType.Contains;
            for (int i = 0; i < 6; i++)
            {
                Plane plane = GetPlane(i);
                PlaneIntersectionType planeIntersect = plane.Intersects(point);
                if (planeIntersect == PlaneIntersectionType.Back)
                    return ContainmentType.Disjoint;
                else if (planeIntersect == PlaneIntersectionType.Intersecting)
                    result = ContainmentType.Intersects;
            }

            return result;
        }

        public ContainmentType Contains(BoundingBox box)
        {
            Vector3 inside = new Vector3();  // inside point  (assuming partial)
            Vector3 outside = new Vector3(); // outside point (assuming partial)
            float len = 0.0f;

            ContainmentType result = ContainmentType.Contains;

            for (int i = 0; i < 6; i++)
            {
                Plane plane = GetPlane(i);

                // setup the inside/outside corners
                // this can be determined easily based
                // on the normal vector for the plane
                if (plane.Normal.X > 0.0f)
                {
                    inside.X = box.Max.X;
                    outside.X = box.Min.X;
                }
                else
                {
                    inside.X = box.Min.X;
                    outside.X = box.Max.X;
                }
                if (plane.Normal.Y > 0.0f)
                {
                    inside.Y = box.Max.Y;
                    outside.Y = box.Min.X;
                }
                else
                {
                    inside.Y = box.Min.Y;
                    outside.Y = box.Max.Y;
                }

                if (plane.Normal.Z > 0.0f)
                {
                    inside.Z = box.Max.Z;
                    outside.Z = box.Min.Z;
                }
                else
                {
                    inside.Z = box.Min.Z;
                    outside.Z = box.Max.Z;
                }

                // check the inside length
                len = Vector3.Dot(inside, plane.Normal) + plane.D;
                if (len < -1.0f)
                    return ContainmentType.Disjoint; // box is fully outside the frustum

                // check the outside length
                len = Vector3.Dot(outside, plane.Normal) + plane.D;
                if (len < -1.0f)
                    result = ContainmentType.Intersects; // partial containment at best

            }

            return result;
        }

        public void updateProjection(float[] proj)
        {
            if (proj.Length >= 16)
                projectionMatrix = proj;

            computeMatrix();
        }

        public void update(float[] view)
        {
            if (view.Length >= 16)
                viewMatrix = view;
            computeMatrix();
        }

        void computeMatrix ()
        {
            // 0, 4, 8, 12
            // 1, 5, 9, 13
            // 2, 6, 10, 14
            // 3, 7, 11, 15

            matrix[0] = viewMatrix[0] * viewMatrix[4] * viewMatrix[8] * viewMatrix[12] + projectionMatrix[0] * projectionMatrix[1] * projectionMatrix[2] * projectionMatrix[3];
            matrix[4] = viewMatrix[0] * viewMatrix[4] * viewMatrix[8] * viewMatrix[12] + projectionMatrix[4] * projectionMatrix[5] * projectionMatrix[6] * projectionMatrix[7];
            matrix[8] = viewMatrix[0] * viewMatrix[4] * viewMatrix[8] * viewMatrix[12] + projectionMatrix[8] * projectionMatrix[9] * projectionMatrix[10] * projectionMatrix[11];
            matrix[12] = viewMatrix[0] * viewMatrix[4] * viewMatrix[8] * viewMatrix[12] + projectionMatrix[12] * projectionMatrix[13] * projectionMatrix[14] * projectionMatrix[15];

            matrix[1] = viewMatrix[1] * viewMatrix[5] * viewMatrix[9] * viewMatrix[13] + projectionMatrix[0] * projectionMatrix[1] * projectionMatrix[2] * projectionMatrix[3];
            matrix[5] = viewMatrix[1] * viewMatrix[5] * viewMatrix[9] * viewMatrix[13] + projectionMatrix[4] * projectionMatrix[5] * projectionMatrix[6] * projectionMatrix[7];
            matrix[9] = viewMatrix[1] * viewMatrix[5] * viewMatrix[9] * viewMatrix[13] + projectionMatrix[8] * projectionMatrix[9] * projectionMatrix[10] * projectionMatrix[11];
            matrix[13] = viewMatrix[1] * viewMatrix[5] * viewMatrix[9] * viewMatrix[13] + projectionMatrix[12] * projectionMatrix[13] * projectionMatrix[14] * projectionMatrix[15];

            matrix[2] = viewMatrix[2] * viewMatrix[6] * viewMatrix[10] * viewMatrix[14] + projectionMatrix[0] * projectionMatrix[1] * projectionMatrix[2] * projectionMatrix[3];
            matrix[6] = viewMatrix[2] * viewMatrix[6] * viewMatrix[10] * viewMatrix[14] + projectionMatrix[4] * projectionMatrix[5] * projectionMatrix[6] * projectionMatrix[7];
            matrix[10] = viewMatrix[2] * viewMatrix[6] * viewMatrix[10] * viewMatrix[14] + projectionMatrix[8] * projectionMatrix[9] * projectionMatrix[10] * projectionMatrix[11];
            matrix[14] = viewMatrix[2] * viewMatrix[6] * viewMatrix[10] * viewMatrix[14] + projectionMatrix[12] * projectionMatrix[13] * projectionMatrix[14] * projectionMatrix[15];

            matrix[3] = viewMatrix[3] * viewMatrix[7] * viewMatrix[11] * viewMatrix[15] + projectionMatrix[0] * projectionMatrix[1] * projectionMatrix[2] * projectionMatrix[3];
            matrix[7] = viewMatrix[3] * viewMatrix[7] * viewMatrix[11] * viewMatrix[15] + projectionMatrix[4] * projectionMatrix[5] * projectionMatrix[6] * projectionMatrix[7];
            matrix[11] = viewMatrix[3] * viewMatrix[7] * viewMatrix[11] * viewMatrix[15] + projectionMatrix[8] * projectionMatrix[9] * projectionMatrix[10] * projectionMatrix[11];
            matrix[15] = viewMatrix[3] * viewMatrix[7] * viewMatrix[11] * viewMatrix[15] + projectionMatrix[12] * projectionMatrix[13] * projectionMatrix[14] * projectionMatrix[15];

            ExtractPlane(Left, 1);
            ExtractPlane(Right, -1);
            ExtractPlane(Bottom, 2);
            ExtractPlane(Top, -2);
            ExtractPlane(Near, 3);
            ExtractPlane(Far, -3);
        }

        static bool matrixEqual ( float[] m1, float[] m2)
        {
            if (m1.Length != m2.Length)
                return false;

            for (int i = 0; i < m1.Length; i++)
            {
                if (m1[i] != m2[i])
                    return false;
            }
            return true;
        }

        public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
        {
            return !matrixEqual(a.matrix, b.matrix);
        }

        public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
        {
            return matrixEqual(a.matrix, b.matrix);
        }

        public bool Equals(BoundingFrustum other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            BoundingFrustum other = obj as BoundingFrustum;
            if (other == null)
                return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < matrix.Length; i++)
                hash ^= matrix[i].GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            string s = string.Empty;
            for (int i = 0; i < matrix.Length; i++)
                s += matrix[i].ToString();

            return s;
        }
    }
}