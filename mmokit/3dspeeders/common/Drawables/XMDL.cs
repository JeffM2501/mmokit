using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

using Drawables.Models;
using Drawables.Materials;

namespace Drawables.Models.XMDL
{
    public class XMDLFile
    {
        public Model read(FileInfo file)
        {
            Model model = new Model();

            XmlSerializer xml = new XmlSerializer(typeof(Model));
            GZipStream zip = new GZipStream(file.OpenRead(), CompressionMode.Decompress, false);
            StreamReader sr = new StreamReader(zip);

            model = (Model)xml.Deserialize(sr);
            sr.Close();
            zip.Close();

            // make the texture paths relative to the file.
            foreach (Mesh m in model.meshes)
                m.material.textureName = Path.Combine(Path.GetDirectoryName(file.FullName), m.material.textureName);

            foreach (MaterialOverride m in model.skins)
            {
                foreach (MeshOverride mesh in m.materials)
                    mesh.newMaterial.textureName = Path.Combine(Path.GetDirectoryName(file.FullName), mesh.newMaterial.textureName);
            }

            // setup the new model to draw
            model.Invalidate();

            return model;
        }

        private string makePathRelitive(string rootpath, string outpath)
        {
            string[] rootChunks = Path.GetDirectoryName(rootpath).Split(Path.DirectorySeparatorChar.ToString().ToCharArray());
            string[] outchunks = outpath.Split(Path.DirectorySeparatorChar.ToString().ToCharArray());

            string relPath = string.Empty;

            int i = 0;
            for (i = 0; i < rootChunks.Length; i++)
            {
                if (i >= outchunks.Length)
                    return outpath;

                if (rootChunks[i] != outchunks[i])
                {
                    relPath += ".." + Path.DirectorySeparatorChar.ToString();
                }
            }

            for (; i < outchunks.Length; i++)
            {
                relPath += outchunks[i];
                if (i != outchunks.Length - 1)
                    relPath += Path.DirectorySeparatorChar.ToString();

            }

            return relPath;
        }


        public bool write(FileInfo file, Model model)
        {
            if (model == null || !model.valid())
                return false;

            foreach (Mesh m in model.meshes)
                m.material.textureName = makePathRelitive(file.FullName, m.material.textureName);

            foreach (MaterialOverride m in model.skins)
            {
                foreach (MeshOverride mesh in m.materials)
                    mesh.newMaterial.textureName = makePathRelitive(file.FullName, mesh.newMaterial.textureName);
            }

            XmlSerializer xml = new XmlSerializer(typeof(Model));
            GZipStream zip = new GZipStream(file.OpenWrite(), CompressionMode.Compress, true);
            StreamWriter sr = new StreamWriter(zip);
            xml.Serialize(sr, model);
            sr.Close();
            zip.Close();

            // make the texture paths relative to the file.
            foreach (Mesh m in model.meshes)
                m.material.textureName = Path.Combine(Path.GetDirectoryName(file.FullName), m.material.textureName);

            foreach (MaterialOverride m in model.skins)
            {
                foreach (MeshOverride mesh in m.materials)
                    mesh.newMaterial.textureName = Path.Combine(Path.GetDirectoryName(file.FullName), mesh.newMaterial.textureName);
            }
            return true;
        }
    }
}
