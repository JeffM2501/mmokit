using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

namespace Grids
{
    class Grid
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

        public void Exectute()
        {
            // do the majors

            GL.Color4(1,1,1,alpha);
            GL.Color3(majorColor);
            GL.Begin(BeginMode.Lines);

            for (float i = -gridSize; i <= gridSize; i+= majorSpacing )
            {
                if (i != 0)
                {
                    GL.Vertex3(i, -gridSize,0);
                    GL.Vertex3(i, gridSize, 0);
                    GL.Vertex3(-gridSize, i, 0);
                    GL.Vertex3(gridSize, i, 0);
                }
            }

            GL.Color3(minorColor);

            for (float i = -gridSize; i < gridSize; i += majorSpacing)
            {
                for (float j = i + minorSpacing; j < i + majorSpacing; j += minorSpacing)
                {
                    GL.Vertex3(j, -gridSize,0);
                    GL.Vertex3(j, gridSize, 0);
                    GL.Vertex3(-gridSize, j, 0);
                    GL.Vertex3(gridSize, j, 0);
                }
            }

            // draw the major axes
            // X
            GL.Color3(xColor);
            GL.Vertex3(-gridSize, 0, 0);
            GL.Vertex3(gridSize, 0, 0);
            GL.Vertex3(gridSize, 0, 0);
            GL.Vertex3(gridSize - axisSize, 0, axisSize);

            GL.Vertex3(gridSize + axisSize, axisSize, 0);
            GL.Vertex3(gridSize + axisSize+axisSize, -axisSize, 0);

            GL.Vertex3(gridSize + axisSize+axisSize, axisSize, 0);
            GL.Vertex3(gridSize + axisSize, -axisSize, 0);

            // Y
            GL.Color3(yColor);
            GL.Vertex3(0, -gridSize, 0);
            GL.Vertex3(0, gridSize, 0);
            GL.Vertex3(0, gridSize, 0);
            GL.Vertex3(0, gridSize - axisSize, axisSize);

            GL.Vertex3(0, gridSize + axisSize, 0);
            GL.Vertex3(0, gridSize + axisSize+ axisSize, 0);

            GL.Vertex3(0, gridSize + axisSize + axisSize, 0);
            GL.Vertex3(axisSize/2.0f, gridSize + axisSize + axisSize + axisSize, 0);
            GL.Vertex3(0, gridSize + axisSize + axisSize, 0);
            GL.Vertex3(-axisSize/2.0f, gridSize + axisSize + axisSize + axisSize, 0);

            // Z
            GL.Color3(zColor);
            GL.Vertex3(0, 0, -gridSize/2);
            GL.Vertex3(0, 0, gridSize);
            GL.Vertex3(0, 0, gridSize);
            GL.Vertex3(0, axisSize ,gridSize - axisSize);
         
            GL.End();
        }        
    }
}
