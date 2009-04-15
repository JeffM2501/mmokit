using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class BoundingFrustum
    {
        public const int CornerCount = 8;
        Matrix4 matrix = Matrix4.Identity;
        public Matrix4 CompositeMatrix
        {
            get { return matrix; }
        }

        public Matrix4 viewMatrix = Matrix4.Identity;
        public Matrix4 projectionMatrix = Matrix4.Identity;

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

        // gl format
        // 0, 4, 8, 12
        // 1, 5, 9, 13
        // 2, 6, 10, 14
        // 3, 7, 11, 15

        float getMatValRow(int index)
        {
            switch(index)
            {
                case 1:
                    return matrix.Row0.Y;
                case 2:
                    return matrix.Row0.Z;
                case 3:
                    return matrix.Row0.W;

                case 4:
                    return matrix.Row1.X;
                case 5:
                    return matrix.Row1.Y;
                case 6:
                    return matrix.Row1.Z;
                case 7:
                    return matrix.Row1.W;
              
                case 8:
                    return matrix.Row2.X;
                case 9:
                    return matrix.Row2.Y;
                case 10:
                    return matrix.Row2.Z;
                case 11:
                    return matrix.Row2.W;
               
                case 12:
                    return matrix.Row3.X;
                case 13:
                    return matrix.Row3.Y;
                case 14:
                    return matrix.Row3.Z;
                case 15:
                    return matrix.Row3.W;
            }
            return matrix.Row0.X;
        }

        float getMatValCol(int index)
        {
            switch (index)
            {
                case 1:
                    return matrix.Column0.Y;
                case 2:
                    return matrix.Column0.Z;
                case 3:
                    return matrix.Column0.W;

                case 4:
                    return matrix.Column1.X;
                case 5:
                    return matrix.Column1.Y;
                case 6:
                    return matrix.Column1.Z;
                case 7:
                    return matrix.Column1.W;

                case 8:
                    return matrix.Column2.X;
                case 9:
                    return matrix.Column2.Y;
                case 10:
                    return matrix.Column2.Z;
                case 11:
                    return matrix.Column2.W;

                case 12:
                    return matrix.Column3.X;
                case 13:
                    return matrix.Column3.Y;
                case 14:
                    return matrix.Column3.Z;
                case 15:
                    return matrix.Column3.W;
            }
            return matrix.Column0.X;
        }

        float getMatVal(int index)
        {
            return getMatValRow(index);
        }

        void ExtractPlane ( Plane plane, int row )
        {
            int scale = (row < 0) ? -1 : 1;
            row = Math.Abs(row) - 1;

            plane.Set(getMatVal(3) + scale * getMatVal(row), getMatVal(7) + scale * getMatVal(row + 4), getMatVal(11) + scale * getMatVal(row + 8), getMatVal(15) + scale * getMatVal(row + 12));
        }

        public BoundingFrustum()
        {
        }

        public BoundingFrustum(BoundingFrustum value)
        {
            matrix = new Matrix4(value.matrix.Row0, value.matrix.Row1, value.matrix.Row2, value.matrix.Row3);
            viewMatrix = new Matrix4(value.viewMatrix.Row0, value.viewMatrix.Row1, value.viewMatrix.Row2, value.viewMatrix.Row3);
            projectionMatrix = new Matrix4(value.projectionMatrix.Row0, value.projectionMatrix.Row1, value.projectionMatrix.Row2, value.matrix.Row3);
        }

        public BoundingFrustum(Matrix4 value)
        {
            matrix = new Matrix4(value.Row0, value.Row1, value.Row2, value.Row3);
            viewMatrix = new Matrix4(value.Row0, value.Row1, value.Row2, value.Row3); // cheap but it's so we can comptue it if we need to compute it
        }

        public BoundingFrustum(Matrix4 view, Matrix4 proj)
        {
            viewMatrix = view;
            projectionMatrix = proj;
            computeMatrix();
        }

        public Vector3 Corner ( int index )
        {
            // hither left/top left/bottom right/bottom right/top
            // yon right/top right/bottom left/bottom left/top
            switch (index)
            {
                case 0:
                    return Plane.Intersection(Near, Left, Top);
                case 1:
                    return Plane.Intersection(Near, Left, Bottom);
                case 2:
                    return Plane.Intersection(Near, Right, Bottom);
                case 3:
                    return Plane.Intersection(Near, Right, Top);

                case 4:
                    return Plane.Intersection(Far, Right, Top);
                case 5:
                    return Plane.Intersection(Far, Right, Bottom);
                case 6:
                    return Plane.Intersection(Far, Left, Bottom);
            }
            return Plane.Intersection(Far, Left, Top);
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

        Matrix4 fillFromFloatsRow(Matrix4 mat, float[] vals)
        {
            if (vals.Length < 16)
                return mat;
            mat.Row0.X = vals[0];
            mat.Row0.Y = vals[1];
            mat.Row0.Z = vals[2];
            mat.Row0.W = vals[3];
            mat.Row1.X = vals[4];
            mat.Row1.Y = vals[5];
            mat.Row1.Z = vals[6];
            mat.Row1.W = vals[7];
            mat.Row2.X = vals[8];
            mat.Row2.Y = vals[9];
            mat.Row2.Z = vals[10];
            mat.Row2.W = vals[11];
            mat.Row3.X = vals[12];
            mat.Row3.Y = vals[13];
            mat.Row3.Z = vals[14];
            mat.Row3.W = vals[15];
            return mat;
        }

        Matrix4 fillFromFloats(Matrix4 mat, float[] vals)
        {
            if (vals.Length < 16)
                return mat;
            mat.Row0.X = vals[0];
            mat.Row0.Y = vals[4];
            mat.Row0.Z = vals[8];
            mat.Row0.W = vals[12];
            mat.Row1.X = vals[1];
            mat.Row1.Y = vals[5];
            mat.Row1.Z = vals[9];
            mat.Row1.W = vals[13];
            mat.Row2.X = vals[2];
            mat.Row2.Y = vals[6];
            mat.Row2.Z = vals[10];
            mat.Row2.W = vals[14];
            mat.Row3.X = vals[3];
            mat.Row3.Y = vals[7];
            mat.Row3.Z = vals[11];
            mat.Row3.W = vals[15];
            return mat;
        }

        public void updateProjection(Matrix4 proj)
        {
            projectionMatrix = proj;
            computeMatrix();
        }

        public void update(Matrix4 view)
        {
            viewMatrix = view;
            computeMatrix();
        }

        void computeMatrix ()
        {
            matrix = Matrix4.Mult(viewMatrix, projectionMatrix);

            ExtractPlane(Left, 1);
            ExtractPlane(Right, -1);
            ExtractPlane(Bottom, 2);
            ExtractPlane(Top, -2);
            ExtractPlane(Near, 3);
            ExtractPlane(Far, -3);
        }

        public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
        {
            return a.matrix != b.matrix;
        }

        public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
        {
            return a.matrix == b.matrix;
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
            return matrix.GetHashCode();
        }

        public override string ToString()
        {
            return matrix.ToString();
        }
    }
}
