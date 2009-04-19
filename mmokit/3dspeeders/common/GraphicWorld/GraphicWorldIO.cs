using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using World;
using Drawables.Materials;
using Drawables.Models;

namespace GraphicWorlds
{
    public class GraphicWorldIO
    {
        public static bool read(GraphicWorld world, FileInfo file)
        {
            if (!file.Exists)
                return false;

            WorldFile worldFile;
            if (!WorldFile.read(out worldFile, file, false))
                return false;
            return read(world,worldFile);
        }

        public static bool read(GraphicWorld world, WorldFile file)
        {
            world.world = file.world;

            string meshes = file.findSection("meshes");

            if (meshes != string.Empty)
            {
                FileInfo meshTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = meshTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(meshes);
                sw.Close();
                fs.Close();

                fs = meshTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<Model>));
                List < Model > models = (List<Model>)xml.Deserialize(sr);
                sr.Close();
                fs.Close();

                world.models = new Dictionary<string, Model>();
                foreach (Model m in models)
                    world.models[m.name] = m;
            }

            string materials = file.findSection("materials");

            if (materials != string.Empty)
            {
                FileInfo matTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = matTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(materials);
                sw.Close();
                fs.Close();

                fs = matTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<Material>));
                List<Material> mats = (List<Material>)xml.Deserialize(sr);
                sr.Close();
                fs.Close();

                world.materials = new Dictionary<string, Material>();
                foreach (Material m in mats)
                    world.materials[m.name] = m;
            }

            return true;
        }

        public static bool write(GraphicWorld world, FileInfo file)
        {
            WorldFile worldFile = new WorldFile();
            if (!write(world, worldFile))
                return false;

            return WorldFile.write(worldFile, file, false);
        }

        public static bool write(GraphicWorld world, WorldFile file)
        {
            file.world = world.world;

            if (world.models != null && world.models.Count > 0)
            {
                FileInfo meshTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = meshTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<Model>));
                List<Model> models = new List<Model>();
                
                foreach (KeyValuePair<string,Model> m in world.models)
                    models.Add(m.Value);

                xml.Serialize(sw,models);
                sw.Close();
                fs.Close();

                WorldFileExtras extra = new WorldFileExtras();
                extra.name = "meshes";

                fs = meshTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                extra.data = sr.ReadToEnd();
                sr.Close();
                fs.Close();

                file.extras.Add(extra);
            }

            if (world.materials != null && world.materials.Count > 0)
            {
                FileInfo matTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = matTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<Material>));
                List<Material> mats = new List<Material>();

                foreach (KeyValuePair<string, Material> m in world.materials)
                    mats.Add(m.Value);

                xml.Serialize(sw, mats);
                sw.Close();
                fs.Close();

                WorldFileExtras extra = new WorldFileExtras();
                extra.name = "materials";

                fs = matTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                extra.data = sr.ReadToEnd();
                sr.Close();
                fs.Close();

                file.extras.Add(extra);
            }

            return true;
        }
    }
}
