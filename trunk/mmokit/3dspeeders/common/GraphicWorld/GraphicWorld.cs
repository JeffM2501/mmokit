using System;
using System.Collections.Generic;
using System.Drawing;

using Drawables;
using Drawables.Materials;
using Drawables.Models;
using Drawables.DisplayLists;
using World;
using Math3D;

using OpenTK;
using OpenTK.Math;

namespace GraphicWorlds
{
    public class GraphicWorld
    {
        public Dictionary<string, Material> materials = new Dictionary<string,Material>();
        public Dictionary<string, Model> models = new Dictionary<string,Model>();
        public ObjectWorld world = new ObjectWorld();

        GroundRenderer ground = new GroundRenderer();
        ObjectRenderer objRender;

        public bool drawAll = false;

        public GraphicWorld ()
        {
            objRender = new ObjectRenderer(this);
        }

        public bool ObjcetVis ( WorldObject obj )
        {
            if (drawAll)
                return true;

            return world.visList.Contains(obj);
        }

        public void Flush()
        {
            ground = new GroundRenderer();
            world.Flush();
            if (materials != null)
                materials.Clear();
            if (models != null)
                models.Clear();
        }

        public void AttachMesh ( WorldObject o )
        {
            if (o.tag == null && o.objectName != string.Empty)
            {
                if (models.ContainsKey(o.objectName))
                    o.tag = models[o.objectName];
            }
        }

        public void AttatchMeshes ()
        {
            foreach (WorldObject o in world.objects)
                AttachMesh(o);
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

            AttatchMeshes();
            foreach (WorldObject o in world.objects)
                objRender.AddCallbacks(o);

            ground.Setup(world);
        }

        public void SetBounds ( WorldObject obj )
        {
            Model model = obj.tag as Model;
            if (model == null)
                return;

            obj.bounds = BoundingBox.Empty;

            Matrix4 mat = objRender.GetTransformMatrix(obj);

            foreach(Mesh m in model.meshes)
            {
                List<Vector3> l = new List<Vector3>();
                foreach(Vector3 v in m.verts)
                    l.Add(new Vector3(Vector3.Transform(v, mat)));
                if (obj.bounds == BoundingBox.Empty)
                    obj.bounds = BoundingBox.CreateFromPoints(l);
                else
                    obj.bounds = BoundingBox.CreateMerged(BoundingBox.CreateFromPoints(l),obj.bounds);
            }
        }

        public void SetBounds()
        {
            foreach (WorldObject o in world.objects)
                SetBounds(o);
        }

        public void AddObject( WorldObject obj )
        {
            AttachMesh(obj);
            SetBounds(obj);
            objRender.AddCallbacks(obj);
            world.objects.Add(obj);
        }

        public void ComputeVis ( VisibleFrustum frustum )
        {

        }
    }
}
