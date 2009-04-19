using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using Math3D;

using Drawables;
using Drawables.Cameras;
using Drawables.Materials;
using Drawables.Textures;
using Drawables.Models;
using Drawables.Models.XMDL;
using Drawables.DisplayLists;

using World;
using GraphicWorlds;

namespace MapEdit
{
    public class Editor
    {
        public GraphicWorld world = new GraphicWorld();

        Form1 form;
        public Editor (Form1 f)
        {
            form = f;
        }

        protected void refreshGLItems()
        {
            MaterialSystem.system.Invalidate();
            DrawablesSystem.system.removeAll();
            DisplayListSystem.system.Invalidate();
        }

        public bool OpenWorldFile ( string fileName )
        {
            refreshGLItems();
            world.Flush();
            MaterialSystem.system.Flush();
            DisplayListSystem.system.Flush();

            FileInfo file = new FileInfo(fileName);
            GraphicWorldIO.read(world, new FileInfo(fileName));

            world.AddDrawables();

            return true;
        }

        public bool SaveWorldFile ( string fileName )
        {
            // build the tree
            world.RebuildTree();
            GraphicWorldIO.write(world, new FileInfo(fileName));
            return true;
        }

        public void AddMaterial ()
        {
            if (world.materials == null)
                world.materials = new Dictionary<string, Material>();

            Material mat = new Material();
            mat.name = "New Material " + world.materials.Count.ToString();

            Dialog_Boxes.MaterialEdit dlog = new MapEdit.Dialog_Boxes.MaterialEdit(mat, form.prefs.dataDir);
            if (dlog.ShowDialog() == DialogResult.OK)
                world.materials[mat.name] = MaterialSystem.system.getMaterial(mat);

        }

        public void EditMaterial(string matName)
        {
            if (!world.materials.ContainsKey(matName))
                return;

            Material mat = world.materials[matName];
            mat.Invalidate();
            Dialog_Boxes.MaterialEdit dlog = new MapEdit.Dialog_Boxes.MaterialEdit(mat, form.prefs.dataDir);
            if (dlog.ShowDialog() == DialogResult.OK)
            {
                world.materials.Remove(matName);
                world.materials[mat.name] = MaterialSystem.system.getMaterial(mat);
            }
        }

        public void RemoveMaterial (string matName)
        {
            if (!world.materials.ContainsKey(matName))
                return;

            Material mat = world.materials[matName];
            world.materials.Remove(matName);
            mat.Invalidate();
        }

        public bool EditMapInfo ()
        {
            Dialog_Boxes.MapInfoDlog dlog = new MapEdit.Dialog_Boxes.MapInfoDlog(world);
            if (dlog.ShowDialog() == DialogResult.OK)
            {
                // blow it all out since we don't know what changed.
                refreshGLItems();

                // flush the world
                world.AddDrawables();
                return true;
            }
            return false;
        }

        protected string getUniqueMatName ( string name )
        {
            string newName = name + world.materials.Count.ToString();
            while (world.materials.ContainsKey(newName))
                newName += "_copy";

            return newName;
        }

        protected void replaceMatNameInModel (Model model, string oldname, string newname)
        {
            foreach( Mesh m in model.meshes)
            {
                if (m.material.name == oldname)
                    m.material.name = newname;
            }

            foreach (MaterialOverride ovd in model.skins )
            {
                foreach( MeshOverride m in ovd.materials)
                {
                    if (m.origonalMatName == oldname)
                        m.origonalMatName = newname;

                    if (m.newMaterial.name == oldname)
                        m.newMaterial.name = newname;
                }
            }
        }

        public bool AddModel( string fileName)
        {
            XMDLFile file = new XMDLFile();

            Model model = file.read(new FileInfo(fileName));
            if (model == null)
                return false;

            string modelName = Path.GetFileNameWithoutExtension(fileName);
            if (world.models.ContainsKey(modelName))
                modelName += world.models.Count.ToString();
            if (world.models.ContainsKey(modelName))
                modelName += world.models.GetHashCode().ToString();

            // scan the sucker for materials and add em
            foreach (Mesh m in model.meshes)
            {
                // check and see if this mat's name is taken.
                string matName = m.material.name;
                if (world.materials.ContainsKey(matName))
                    replaceMatNameInModel( model, matName,getUniqueMatName(matName));

                // should be unique
                matName = m.material.name;
                m.material = MaterialSystem.system.getMaterial(m.material);

                world.materials.Add(matName, m.material);
            }

            // run through the skins and just add it's override materials to mat system, not the list
            foreach (MaterialOverride ovd in model.skins)
            {
                foreach (MeshOverride m in ovd.materials)
                    m.newMaterial = MaterialSystem.system.getMaterial(m.newMaterial);
            }

            world.models.Add(modelName, model);

            return true;
        }

        BoundingBox BoundsFromObject(WorldObject obj, Model model)
        {
            return new BoundingBox();
        }

        public void AddObject ( string meshName )
        {
            if (!world.models.ContainsKey(meshName))
                return;


            Model model = world.models[meshName];

            WorldObject obj = new WorldObject();
            obj.objectName = meshName;
            obj.skipTree = false;
            obj.tag = model;
        }
    }
}
