using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using Drawables.DisplayLists;
using Drawables.Materials;

namespace Grids
{
    class Grid : ListableItem
    {
        public float majorSpacing = 5;
        public float minorSpacing = 1;
        public float gridSize = 25;
        public float axisSize = 3;

        public Color majorColor = Color.Wheat;
        public Color minorColor = Color.Peru;
        public Color xColor = Color.Red;
        public Color yColor = Color.Green;
        public Color zColor = Color.Blue;
        public float alpha = 1.0f;

        public float gridZ = 0.01f;

        protected override void GenerateList()
        {
            // do the majors
            GLColor mc = new GLColor(majorColor, alpha);
            mc.glColor();

            GL.Begin(BeginMode.Lines);

            for (float i = -gridSize; i <= gridSize; i+= majorSpacing )
            {
                if (i != 0)
                {
                    GL.Vertex3(i, -gridSize, gridZ);
                    GL.Vertex3(i, gridSize, gridZ);
                    GL.Vertex3(-gridSize, i, gridZ);
                    GL.Vertex3(gridSize, i, gridZ);
                }
            }

            mc = new GLColor(minorColor, alpha);
            mc.glColor();

            for (float i = -gridSize; i < gridSize; i += majorSpacing)
            {
                for (float j = i + minorSpacing; j < i + majorSpacing; j += minorSpacing)
                {
                    GL.Vertex3(j, -gridSize, gridZ);
                    GL.Vertex3(j, gridSize, gridZ);
                    GL.Vertex3(-gridSize, j, gridZ);
                    GL.Vertex3(gridSize, j, gridZ);
                }
            }

            // draw the major axes
            // X
            mc = new GLColor(xColor, alpha);
            mc.glColor();
            GL.Vertex3(-gridSize, 0, gridZ);
            GL.Vertex3(gridSize, 0, gridZ);
            GL.Vertex3(gridSize, 0, gridZ);
            GL.Vertex3(gridSize - axisSize, 0, axisSize);

            GL.Vertex3(gridSize + axisSize, axisSize, gridZ);
            GL.Vertex3(gridSize + axisSize + axisSize, -axisSize, gridZ);

            GL.Vertex3(gridSize + axisSize + axisSize, axisSize, gridZ);
            GL.Vertex3(gridSize + axisSize, -axisSize, gridZ);

            // Y
            mc = new GLColor(yColor, alpha);
            mc.glColor();
            GL.Vertex3(0, -gridSize, gridZ);
            GL.Vertex3(0, gridSize, gridZ);
            GL.Vertex3(0, gridSize, gridZ);
            GL.Vertex3(0, gridSize - axisSize, axisSize);

            GL.Vertex3(0, gridSize + axisSize, gridZ);
            GL.Vertex3(0, gridSize + axisSize + axisSize, gridZ);

            GL.Vertex3(0, gridSize + axisSize + axisSize, gridZ);
            GL.Vertex3(axisSize / 2.0f, gridSize + axisSize + axisSize + axisSize, gridZ);
            GL.Vertex3(0, gridSize + axisSize + axisSize, gridZ);
            GL.Vertex3(-axisSize / 2.0f, gridSize + axisSize + axisSize + axisSize, gridZ);

            // Z
            mc = new GLColor(zColor, alpha);
            mc.glColor();
            GL.Vertex3(0, 0, -gridSize/2);
            GL.Vertex3(0, 0, gridSize);
            GL.Vertex3(0, 0, gridSize);
            GL.Vertex3(0, axisSize ,gridSize - axisSize);
         
            GL.End();
        }        
    }
}
