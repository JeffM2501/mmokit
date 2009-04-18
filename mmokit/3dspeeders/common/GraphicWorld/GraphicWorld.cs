using System;
using System.Collections.Generic;
using System.Drawing;

using Drawables.Materials;
using Drawables.Models;
using World;
using Drawables;
using Math3D;

namespace GraphicWorlds
{
    public class GraphicWorld
    {
        public Dictionary<string, Material> materials;
        public Dictionary<string, Model> models;
        public ObjectWorld world = new ObjectWorld();

        GroundRenderer ground = new GroundRenderer();

        public void AttatchMeshes ()
        {
            foreach( WorldObject o in world.objects )
            {
                if (o.Tag == null && o.ObjectName != string.Empty)
                {
                    if (models.ContainsKey(o.ObjectName))
                        o.Tag = models[o.ObjectName];
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
                foreach(KeyValuePair<string,Material> mat in materials)
                    materials[mat.Key] = MaterialSystem.system.getMaterial(mat.Value);
            }

            ground.Setup(world);
        }

        public void ComputeVis ( VisibleFrustum frustum )
        {

        }
    }
}
