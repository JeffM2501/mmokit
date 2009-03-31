using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

namespace modeler
{
    public partial class ModelerDialog : Form
    {
        Camera camera = new Camera();
        Grid grid = new Grid();

        public ModelerDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            glControl1_Resize(this, EventArgs.Empty);   // Ensure the Viewport is set up correctly
            GL.ClearColor(Color.Black);

            camera.move(new Vector3(0, 2, 0));
           // camera.dolly(15);
          //  camera.pan(1, 0);

            Invalidate();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (glControl1.ClientSize.Height == 0)
                glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);
            Glu.Perspective(120, glControl1.AspectRatio, 0.5f, 1000.0f);
            Invalidate();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);
            camera.Execute();

            grid.Execute();

            glControl1.SwapBuffers();

        }
    }
}
