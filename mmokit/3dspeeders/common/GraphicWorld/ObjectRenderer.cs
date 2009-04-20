using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using World;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using Drawables;
using Drawables.Models;
using Drawables.Materials;

using Math3D;

namespace GraphicWorlds
{
    public class MeshMat
    {
        public Mesh mesh;
        public Material mat;
        public WorldObject obj;
        public MeshOverride skin;

        public MeshMat (WorldObject _obj, Mesh _mesh, Material _mat, MeshOverride _skin)
        {
            obj = _obj;
            mesh = _mesh;
            mat = _mat;
            skin = _skin;
        }
    }

    public class ObjectRenderer
    {
        public GraphicWorld world = null;

        int objectPass = DrawablesSystem.FirstPass + 10;

        public ObjectRenderer(GraphicWorld w)
        {
            world = w;
        }

        public void AddCallbacks ( WorldObject obj )
        {
            Model model = obj.tag as Model;
            if (model == null)
                return;

            MaterialOverride ovd = model.findSkin(obj.skin);
            if (ovd != null)
            {
                foreach (Mesh m in model.meshes)
                    DrawablesSystem.system.addItem(ovd.getMaterial(m.material), new ExecuteCallback(DrawObject), objectPass, new MeshMat(obj,m,m.material,ovd.findOverride(m.material.name)));
            }
            else
            {
                foreach (Mesh m in model.meshes)
                    DrawablesSystem.system.addItem(m.material, new ExecuteCallback(DrawObject), objectPass, new MeshMat(obj,m,m.material,null));
            }
        }

        protected void TransformForObject ( WorldObject obj )
        {
            GL.Translate(obj.postion);
            GL.Scale(obj.scale);
            GL.Rotate(obj.rotation.Z,0,0,1f);
            if (obj.scaleSkinToSize)
            {
                GL.MatrixMode(MatrixMode.Texture);
                GL.Scale(obj.scale);
                GL.MatrixMode(MatrixMode.Modelview);
            }
        }

        public Matrix4 GetTransformMatrix(WorldObject obj)
        {
            Matrix4 mat = Matrix4.Identity;

            mat = Matrix4.Mult(mat, Matrix4.Translation(obj.postion));
            mat = Matrix4.Mult(mat, Matrix4.Scale(obj.scale));
            mat = Matrix4.Mult(mat, Matrix4.Rotate(new Vector3(0,0,1f),Trig.DegreeToRadian(obj.rotation.Z)));

            return mat;
        }

        public bool DrawObject(Material mat, object tag)
        {
            if (world == null)
                return false;

            MeshMat meshMat = tag as MeshMat;
            if (meshMat == null)
                return false;

            Model model = meshMat.obj.tag as Model;
            if (model == null)
                return false;

            if (!world.ObjcetVis(meshMat.obj))
                return true;

            GL.PushMatrix();

            TransformForObject(meshMat.obj);

            if (meshMat.skin == null)
                model.draw(meshMat.mesh);
            else            // there is a skin
                model.draw(meshMat.mesh, meshMat.skin);

            if (meshMat.obj.scaleSkinToSize)
            {
                GL.MatrixMode(MatrixMode.Texture);
                GL.LoadIdentity();
                GL.MatrixMode(MatrixMode.Modelview);
            }

            GL.PopMatrix();
            return true;
        }
    }
}
