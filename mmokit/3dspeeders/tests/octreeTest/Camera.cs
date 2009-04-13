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
        float tilt = 0, spin = 0;

        public bool ZIsUp = true;

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

        public void Execute()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

			GL.Rotate(tilt, 1.0f, 0.0f, 0.0f);			// pops us to the tilt
            GL.Rotate(-spin+90.0f, 0.0f, 1.0f, 0.0f);			// gets us on our rot
            GL.Translate(-position.X, -position.Z, position.Y);

            GL.Rotate(-90,1,0,0);
        }
    }
}
