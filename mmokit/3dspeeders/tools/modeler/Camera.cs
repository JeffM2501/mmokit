using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

namespace modeler
{
    public class Camera : GLListable
    {
        Vector3 position = new Vector3();
        float tilt = 0, spin = 0, pullback = 0;

        public bool ZIsUp = true;

        public void move (Vector3 pos)
        {
            position += pos;
            Invalidate();
        }

        public void pushpull( float dist )
        {
            pullback += dist;
        }

        public void pan( float _tilt, float _spin )
        {
            tilt += _tilt;
            spin += _spin;
            Invalidate();
        }

        public void set(Vector3 pos, float _tilt, float _spin, float _pullback)
        {
            position = pos;
            tilt = _tilt;
            spin = _spin;
            pullback = _pullback;
            Invalidate();
        }

        protected override void GenerateList()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(0, 0, -pullback);				// pull back on allong the zoom vector
			GL.Rotate(tilt, 1.0f, 0.0f, 0.0f);			// pops us to the tilt
            GL.Rotate(-spin, 0.0f, 1.0f, 0.0f);			// gets us on our rot
            GL.Translate(-position.X, -position.Z, position.Y);

            GL.Rotate(-90,1,0,0);
        }
    }
}
