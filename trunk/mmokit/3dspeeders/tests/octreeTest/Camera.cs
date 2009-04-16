using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;
using Math3D;

namespace Cameras
{
    public class Camera
    {
        Vector3 position = new Vector3();
        public Vector3 EyePoint
        {
            get { return position; }
        }

        float tilt = 0, spin = 0;

        Vector3 up = new Vector3(0, 0, 1f);
        public bool ZIsUp = true;

        float aspect = 1;
        float fov = 45f;

        int width = 1;
        int height = 1;

        public float FOV
        {
            get { return fov; }
            set { fov = FOV; updatePerspective(); }
        }

        float hither = 1f;
        public float NearPlane
        {
            get { return hither; }
            set { hither = NearPlane; updatePerspective(); }
        }

        float yon = 1000.0f;
        public float FarPlane
        {
            get { return yon; }
            set { yon = FarPlane; updatePerspective(); }
        }

        VizableFrustum frustum = new VizableFrustum();
        public VizableFrustum ViewFrustum
        {
            get { return frustum; }
        }

        public void move (Vector3 pos)
        {
            position += pos;
        }

        public void move(float x, float y, float z)
        {
            position.X += x;
            position.Y += y;
            position.Z += z;
        }

        public void turn( float _tilt, float _spin )
        {
            tilt += _tilt;
            spin += _spin;
        }

        public void set(Vector3 pos, float _tilt, float _spin)
        {
            position = pos;
            tilt = _tilt;
            spin = _spin;
        }

        public float HeadingAngle ()
        {
            return spin;
        }

        public Vector2 Heading ()
        {
            return new Vector2((float)Math.Cos(Trig.DegreeToRadian(spin)), (float)Math.Sin(Trig.DegreeToRadian(spin)));
        }

        public Vector3 Forward()
        {
            Vector3 forward = new Vector3(Heading());
            forward.Z = (float)Math.Tan(Trig.DegreeToRadian(tilt));
            forward.Normalize();
            return forward;
        }

        void updatePerspective ()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            frustum.SetProjection(fov, aspect, hither, yon,width, height);
            GL.MultTransposeMatrix(ref frustum.projection);
        }

        public void Resize(Int32 _width, Int32 _height)
        {
            width = _width;
            height = _height;

            aspect = width/(float)height;
            updatePerspective();
        }

        public void Execute()
        {
            bool useFrustum = true;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            frustum.LookAt(position, position + Forward());
            if (useFrustum)
                GL.MultTransposeMatrix(ref frustum.view);
            else
            {
                Glu.LookAt(position, position + Forward(), up);
                Matrix4 matrix = new Matrix4();
                GL.GetFloat(GetPName.TransposeModelviewMatrix, out matrix.Row0.X);
                frustum.SetView(matrix);
            }
        }

        public VizableFrustum SnapshotFrusum ( )
        {
            VizableFrustum f = new VizableFrustum();

            f.SetProjection(fov, aspect, hither, 25, width, height);
            f.LookAt(position, position + Forward());

            return f;
        }
    }
}
