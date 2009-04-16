﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class VizableFrustum : BoundingFrustum
    {
        #region Properties
        public Matrix4 view = new Matrix4();
        public Matrix4 ViewMatrix
        {
            get { return view; }
        }
        public Matrix4 projection = new Matrix4();
        public Matrix4 ProjectionMatrix
        {
            get { return projection; }
        }

        Matrix4 billboard = new Matrix4();
        public Matrix4 BillboardMatrix
        {
            get { return billboard; }
        }
        #endregion

        public Vector3 RightVec = new Vector3();
        public Vector3 Up = new Vector3();
        public Vector3 ViewDir = new Vector3();
        public Vector3 EyePoint = new Vector3();

        public float nearClip = 0;
        public float farClip = 0;

        public Vector3[] edge;

        public VizableFrustum() : base (Matrix4.Identity)
        {
            BuildFrustum();
        }

        public VizableFrustum( VizableFrustum value )
            : base(value.matrix)
        {
            projection = new Matrix4(value.projection.Row0,value.projection.Row1,value.projection.Row2,value.projection.Row3);
            view = new Matrix4(value.view.Row0, value.view.Row1, value.view.Row2, value.view.Row3);
            BuildFrustum();
        }

        public void SetProjection(float fov, float aspect, float hither, float yon, int width, int height  )
        {
            nearClip = hither;
            farClip = yon;

            bool useMatrixMath = true;

            if(useMatrixMath)
                projection = Matrix4.Perspective(Trig.DegreeToRadian(fov), aspect, hither, yon);
            else
            {
                // compute projectionMatrix
                float s = 1.0f / (float)Math.Tan(fov / 2.0f);
                float fracHeight = 1.0f - (float)height / (float)height;
                MatrixHelper4.M11(ref projection,s);
                MatrixHelper4.M22(ref projection,(1.0f - fracHeight) * s * (float)width / (float)height);
                MatrixHelper4.M31(ref projection,0.0f);
                MatrixHelper4.M32(ref projection,-fracHeight);
                MatrixHelper4.M33(ref projection,-(yon + hither) / (yon - hither));
                MatrixHelper4.M34(ref projection,-1.0f);
                MatrixHelper4.M41(ref projection,0.0f);
                MatrixHelper4.M43(ref projection,-2.0f * yon * hither / (yon - hither));
                MatrixHelper4.M44(ref projection,0.0f);

                projection.Transpose();
            }

            BuildFrustum();
        }

        public void SetView(Matrix4 mat)
        {
            view = mat;
            BuildFrustum();
        }

        public void LookAt ( Vector3 eye, Vector3 target)
        {
            EyePoint = new Vector3(eye);
            bool useMatrixMath = false;

            // compute forward vector and normalize
            ViewDir = VectorHelper3.Subtract(target, eye);
            ViewDir.Normalize();

            // compute left vector (by crossing forward with
            // world-up [0 0 1]T and normalizing)
            RightVec.X = ViewDir.Y;
            RightVec.Y = -ViewDir.X;
            float rd = 1.0f / Trig.Hypot(RightVec.X, RightVec.Y);
            RightVec.X *= rd;
            RightVec.Y *= rd;
            RightVec.Z = 0.0f;

            // compute local up vector (by crossing right and forward,
            // normalization unnecessary)
            Up.X = RightVec.Y * ViewDir.Z;
            Up.Y = -RightVec.X * ViewDir.Z;
            Up.Z = (RightVec.X * ViewDir.Y) - (RightVec.Y * ViewDir.X);

            if (useMatrixMath)
                view = Matrix4.LookAt(eye, target, Up);
            else
            {
                // build view matrix, including a transformation bringing
                // world up [0 0 1 0]T to eye up [0 1 0 0]T, world north
                // [0 1 0 0]T to eye forward [0 0 -1 0]T.
                MatrixHelper4.m0(ref view,RightVec.X);
                MatrixHelper4.m4(ref view,RightVec.Y);
                MatrixHelper4.m8(ref view,0.0f);

                MatrixHelper4.m1(ref view,Up.X);
                MatrixHelper4.m5(ref view,Up.Y);
                MatrixHelper4.m9(ref view,Up.Z);

                MatrixHelper4.m2(ref view, -ViewDir.X);
                MatrixHelper4.m6(ref view, -ViewDir.Y);
                MatrixHelper4.m10(ref view, -ViewDir.Z);

                MatrixHelper4.m12(ref view,-(MatrixHelper4.m0(view) * eye.X +
                                   MatrixHelper4.m4(view) * eye.Y +
                                   MatrixHelper4.m8(view) * eye.Z));
                MatrixHelper4.m13(ref view,-(MatrixHelper4.m1(view) * eye.X +
                                   MatrixHelper4.m5(view) * eye.Y +
                                   MatrixHelper4.m9(view) * eye.Z));
                MatrixHelper4.m14(ref view,-(MatrixHelper4.m2(view) * eye.X +
                                   MatrixHelper4.m6(view) * eye.Y +
                                   MatrixHelper4.m10(view) * eye.Z));

                MatrixHelper4.m15(ref view, 1.0f);
            }

            MatrixHelper4.M11(ref billboard, MatrixHelper4.M11(view));
            MatrixHelper4.M12(ref billboard, MatrixHelper4.M21(view));
            MatrixHelper4.M13(ref billboard, MatrixHelper4.M31(view));
            MatrixHelper4.M21(ref billboard, MatrixHelper4.M12(view));
            MatrixHelper4.M22(ref billboard, MatrixHelper4.M22(view));
            MatrixHelper4.M23(ref billboard, MatrixHelper4.M32(view));
            MatrixHelper4.M31(ref billboard, MatrixHelper4.M13(view));
            MatrixHelper4.M32(ref billboard, MatrixHelper4.M23(view));
            MatrixHelper4.M33(ref billboard, MatrixHelper4.M33(view));

            BuildFrustum();
            
        }

        #region Protected Methods

        protected void BuildFrustum()
        {
            // save off the composite matrix to the base
            base.matrix = Matrix4.Mult(view,projection);

            // compute vectors of frustum edges
            float xs = (float)Math.Abs(1.0f / projection.Column0.X);
            float ys = (float)Math.Abs(1.0f / projection.Column1.Y);
            edge = new Vector3[4];

            edge[0] = ViewDir - (xs * RightVec) - (ys * Up);
            edge[1] = ViewDir + (xs * RightVec) - (ys * Up);
            edge[2] = ViewDir + (xs * RightVec) + (ys * Up);
            edge[3] = ViewDir - (xs * RightVec) + (ys * Up);

            // make frustum planes
            this.near.Normal = ViewDir;
            this.near.D = -Vector3.Dot(EyePoint, ViewDir);

            makePlane(edge[0], edge[3], EyePoint, ref this.left);
            makePlane(edge[2], edge[1], EyePoint, ref this.right);
            makePlane(edge[1], edge[0], EyePoint, ref this.bottom);
            makePlane(edge[3], edge[2], EyePoint, ref this.top);

            this.far.Normal = -near.Normal;
            this.far.D = near.D + farClip;

            CreateCorners();
        }

        void makePlane(Vector3 v1, Vector3 v2, Vector3 eye, ref Plane plane)
        {
            if (plane == null)
                plane = new Plane();

            // get normal by crossing v1 and v2 and normalizing
            plane.Normal = Vector3.Cross(v1, v2);
            plane.Normal.Normalize();
            plane.D = -Vector3.Dot(eye, plane.Normal);
        }
        #endregion
   }
}
