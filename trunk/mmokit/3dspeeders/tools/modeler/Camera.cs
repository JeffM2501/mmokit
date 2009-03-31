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
        Vector3 target = new Vector3();
        Vector3 up = new Vector3(0,0,1);

        public bool ZIsUp = true;

        public void move (Vector3 pos)
        {
            position += pos;
            Invalidate();
        }

        public void moveTarget(Vector3 tar )
        {
            target += tar;
            Invalidate();
        }

        public void set ( Vector3 pos, Vector3 tar )
        {
            position = pos;
            target = tar;
            Invalidate();
        }

        protected override void GenerateList()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
           // GL.Translate(-position.X,-position.Z,position.Y);
            Glu.LookAt(position, target, up);

            GL.Rotate(-90,1,0,0);
        }
    }
}
