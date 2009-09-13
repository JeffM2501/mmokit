using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

namespace World
{
    public class WorldFileExtras
    {
        public string name;
        public string data;
    }

    public class OctreeWorldFile
    {
        public ObjectWorld world = null;
        public List<WorldFileExtras> extras = new List<WorldFileExtras>();

        public string findSection ( string name )
        {
            if (extras == null)
                return string.Empty;

            foreach( WorldFileExtras e in extras)
            {
                if (e.name == name)
                    return e.data;
            }

            return string.Empty;
        }

        public static bool read ( out OctreeWorldFile worldFile, FileInfo file)
        {
            return read(out worldFile,file,true);
        }

        public static bool read ( out OctreeWorldFile worldFile, FileInfo file, bool compress )
        {
            worldFile = new OctreeWorldFile();

            if (!file.Exists)
                return false;
           
            FileStream fs = file.OpenRead();
            if (fs == null)
                return false;

            GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, false);
            StreamReader sr = new StreamReader(zip);
            if (!compress)
                sr = new StreamReader(fs);

            XmlSerializer xml = new XmlSerializer(typeof(OctreeWorldFile));
            worldFile = (OctreeWorldFile)xml.Deserialize(sr);
            sr.Close();
            zip.Close();
            fs.Close();

            return true;
        }

        public static bool write(OctreeWorldFile worldFile, FileInfo file)
        {
            return write(worldFile, file, true);
        }

        public static bool write(OctreeWorldFile worldFile, FileInfo file, bool compress)
        {
            FileStream fs = file.OpenWrite();
            if (fs == null)
                return false;
            GZipStream zip = new GZipStream(fs, CompressionMode.Compress, false);
            StreamWriter sw = new StreamWriter(zip);
            if (!compress)
                sw = new StreamWriter(fs);

            XmlSerializer xml = new XmlSerializer(typeof(OctreeWorldFile));
            xml.Serialize(sw, worldFile);
            sw.Close();
            zip.Close();
            fs.Close();

            return true;
        }
    }
}
