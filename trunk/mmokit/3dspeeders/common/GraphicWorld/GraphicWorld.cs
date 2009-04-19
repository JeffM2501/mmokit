using System;
using System.Collections.Generic;
using System.Drawing;

using Drawables;
using Drawables.Materials;
using Drawables.Models;
using Drawables.DisplayLists;
using World;
using Math3D;

namespace GraphicWorlds
{
    public class GraphicWorld
    {
        public Dictionary<string, Material> materials = null;
        public Dictionary<string, Model> models = null;
        public ObjectWorld world = new ObjectWorld();

        GroundRenderer ground = new GroundRenderer();

        public void Flush()
        {
            ground = new GroundRenderer();
            world.Flush();
            if (materials != null)
                materials.Clear();
            if (models != null)
                models.Clear();
        }

        public void AttatchMeshes ()
        {
            foreach( WorldObject o in world.objects )
            {
                if (o.tag == null && o.objectName != string.Empty)
                {
                    if (models.ContainsKey(o.objectName))
                        o.tag = models[o.objectName];
                }
            }
        }

        public void RebuildTree()
        {
            world.Flush();
            world.BuildTree(new BoundingBox(world.size, world.size));
        }

        public void AddDrawables()
        {
            // consolidate the materials in the system
            if (materials != null)
            {
                Dictionary<string, Material> newMats = new Dictionary<string, Material>();
                foreach (KeyValuePair<string, Material> mat in materials)
                {
                    mat.Value.Invalidate();
                    newMats.Add(mat.Key, MaterialSystem.system.getMaterial(mat.Value));
                }
                materials = newMats;
            }

            ground.Setup(world);
        }

        public void ComputeVis ( VisibleFrustum frustum )
        {

        }
    }
}
