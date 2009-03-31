﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

namespace modeler
{
    class Grid : GLListable
    {
        float majorSpacing = 5;
        float minorSpacing = 1;
        float gridSize = 25;
        float axisSize = 3;

        Color majorColor = Color.Wheat;
        Color minorColor = Color.Gray;
        Color xColor = Color.Red;
        Color yColor = Color.Green;
        Color zColor = Color.Blue;
        float alpha = 1.0f;

        protected override void GenerateList()
        {
            // do the majors

            GL.Color4(1,1,1,alpha);
            GL.Color3(majorColor);
            GL.Begin(BeginMode.Lines);

            for (float i = -gridSize; i <= gridSize; i+= majorSpacing )
            {
                GL.Vertex3(i, -gridSize,0);
                GL.Vertex3(i, gridSize, 0);
                GL.Vertex3(-gridSize, i, 0);
                GL.Vertex3(gridSize, i, 0);
            }

            GL.Color3(minorColor);

            for (float i = -gridSize; i <= gridSize; i += majorSpacing)
            {
                for (float j = i + minorSpacing; j < i + majorSpacing; j += minorSpacing)
                {
                    GL.Vertex3(j, -gridSize,0);
                    GL.Vertex3(j, gridSize, 0);
                    GL.Vertex3(-gridSize, j, 0);
                    GL.Vertex3(gridSize, j, 0);
                }
            }
            GL.End();
        }        
    }
}
