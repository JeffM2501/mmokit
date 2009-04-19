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
using Utilities.Paths;

namespace Drawables.Models.XMDL
{
    public class XMDLFile
    {
        public Model read(FileInfo file)
        {
            Model model = new Model();

            XmlSerializer xml = new XmlSerializer(typeof(Model));
            FileStream fs = file.OpenRead();
            GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, false);
            StreamReader sr = new StreamReader(zip);

            model = (Model)xml.Deserialize(sr);
            sr.Close();
            zip.Close();
            fs.Close();

            // setup the new model to draw
            model.Invalidate();

            return model;
        }

        public bool write(FileInfo file, Model model)
        {
            if (model == null || !model.valid())
                return false;

            XmlSerializer xml = new XmlSerializer(typeof(Model));
            FileStream fs = file.OpenWrite();
            GZipStream zip = new GZipStream(fs, CompressionMode.Compress, true);
            StreamWriter sr = new StreamWriter(zip);
            xml.Serialize(sr, model);
            sr.Close();
            zip.Close();
            fs.Close();

            return true;
        }
    }
}
