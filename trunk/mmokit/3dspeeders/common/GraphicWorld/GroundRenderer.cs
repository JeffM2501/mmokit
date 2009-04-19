using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Drawables.DisplayLists;
using Drawables.Materials;
using Drawables;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using World;

namespace GraphicWorlds
{
    class GroundRenderer
    {
        DisplayList groundList = new DisplayList();
        DisplayList wallList = new DisplayList();

        Material groundMaterial = null;
        Material wallMaterial = null;

        Vector3 size;

        float wallHeight = 0;

        float uvScale = 1.0f;

        public void Setup(ObjectWorld world )
        {
            groundList.Invalidate();
            wallList.Invalidate();

            size = world.size;
            
            if (groundMaterial != null)
                groundMaterial.Invalidate();

            if (world.groundMaterialName != string.Empty)
                groundMaterial = MaterialSystem.system.getMaterial(world.groundMaterialName);

            if (groundMaterial == null)
                groundMaterial = MaterialSystem.system.getMaterial(new Material(Color.ForestGreen));

            if (world.groundUVSize > 0)
                uvScale = 1f/world.groundUVSize;

            DrawablesSystem.system.addItem(groundMaterial, new ExecuteCallback(DrawGround),DrawablesSystem.FirstPass, groundList);

            wallHeight = world.wallHeight;
            if (wallHeight > 0)
            {
                if (world.wallMaterialName != string.Empty)
                    wallMaterial = MaterialSystem.system.getMaterial(world.wallMaterialName);
                if (wallMaterial == null)
                    wallMaterial = MaterialSystem.system.getMaterial(new Material(Color.Brown));

                DrawablesSystem.system.addItem(wallMaterial, new ExecuteCallback(DrawGround), DrawablesSystem.FirstPass, wallList);
            }
        }

        void BuildGroundList()
        {
            groundList.Start();
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, 1f);

            GL.TexCoord2(size.X * uvScale, -size.Y * uvScale);
            GL.Vertex3(size.X, size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, -size.Y * uvScale);
            GL.Vertex3(-size.X, size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, size.Y * uvScale);
            GL.Vertex3(-size.X, -size.Y, 0);

            GL.TexCoord2(size.X * uvScale,size.Y * uvScale);
            GL.Vertex3(size.X, -size.Y, 0);
            GL.End();
            groundList.End();
        }

        void BuildWallList()
        {
            wallList.Start();

            GL.Begin(BeginMode.Quads);

            // y+
            GL.Normal3(0, -1f, 0);
            GL.TexCoord2(size.X * uvScale, 0);
            GL.Vertex3(size.X, size.Y, 0);

            GL.TexCoord2(size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, size.Y, wallHeight);
          
            GL.TexCoord2(-size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, size.Y, wallHeight);

            GL.TexCoord2(-size.X * uvScale, 0);
            GL.Vertex3(-size.X, size.Y, 0);

            // y-
            GL.Normal3(0, 1f, 0);
            GL.TexCoord2(-size.X * uvScale, 0);
            GL.Vertex3(-size.X, -size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, -size.Y, wallHeight);

            GL.TexCoord2(size.X * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, -size.Y, wallHeight);
 
            GL.TexCoord2(size.X * uvScale, 0);
            GL.Vertex3(size.X, -size.Y, 0);


            // X+
            GL.Normal3(-1f, 0, 0);
            GL.TexCoord2(-size.Y * uvScale, 0);
            GL.Vertex3(size.X, -size.Y, 0);

            GL.TexCoord2(-size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, -size.Y, wallHeight);

            GL.TexCoord2(size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(size.X, size.Y, wallHeight);
          
            GL.TexCoord2(size.Y * uvScale, 0);
            GL.Vertex3(size.X, size.Y, 0);

            // X-
            GL.Normal3(1f, 0, 0);
            GL.TexCoord2(size.Y * uvScale, 0);
            GL.Vertex3(-size.X, size.Y, 0);

            GL.TexCoord2(size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, size.Y, wallHeight);

            GL.TexCoord2(-size.Y * uvScale, wallHeight * uvScale);
            GL.Vertex3(-size.X, -size.Y, wallHeight);

            GL.TexCoord2(-size.Y * uvScale, 0);
            GL.Vertex3(-size.X, -size.Y, 0);

            GL.End();

            wallList.End();
        }

        protected bool DrawGround(Material mat, object tag)
        {
            DisplayList list = tag as DisplayList;
            if (list == null)
                return false;

            if (list == groundList && !groundList.Valid())
                BuildGroundList();
            else if (list == wallList && !wallList.Valid())
                BuildWallList();

            list.Call();

            return true;
        }
    }
}
