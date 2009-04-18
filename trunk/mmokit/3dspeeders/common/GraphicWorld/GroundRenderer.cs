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
    class GroundRenderer : ListableItem
    {
        Material groundMaterial = null;
        Material wallMaterial = null;

        Vector3 size;

        float uvScale = 1.0f;

        public void Setup(ObjectWorld world )
        {
            if (groundMaterial != null)
                groundMaterial.Invalidate();

            if (world.groundMaterialName != string.Empty)
                groundMaterial = MaterialSystem.system.getMaterial(world.groundMaterialName);

            if (groundMaterial == null)
                groundMaterial = MaterialSystem.system.getMaterial(new Material(Color.ForestGreen));

            if (world.groundUVSize > 0)
                uvScale = world.groundUVSize;

            size = world.size;

            DrawablesSystem.system.addItem(groundMaterial, new ExecuteCallback(DrawGround),DrawablesSystem.FirstPass);
        }

        protected override void GenerateList()
        {
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, 1f);

            GL.TexCoord2(size.X * uvScale, size.Y * uvScale);
            GL.Vertex3(size.X, size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, size.Y * uvScale);
            GL.Vertex3(-size.X, size.Y, 0);

            GL.TexCoord2(-size.X * uvScale, -size.Y * uvScale);
            GL.Vertex3(-size.X, -size.Y, 0);

            GL.TexCoord2(size.X * uvScale,- size.Y * uvScale);
            GL.Vertex3(size.X, -size.Y, 0);
            GL.End();
        }

        protected bool DrawGround(Material mat, object tag)
        {
            base.Execute();
            return true;
        }
    }
}
