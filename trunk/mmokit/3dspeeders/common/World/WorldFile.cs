﻿using System;
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

    public class WorldFile
    {
        public ObjectWorld world = null;
        public List<WorldFileExtras> extras = null;

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

        public static bool read ( WorldFile worldFile, FileInfo file )
        {
            if (!file.Exists)
                return false;
           
            FileStream fs = file.OpenRead();
            if (fs == null)
                return false;
            GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, false);
            StreamReader sr = new StreamReader(zip);

            XmlSerializer xml = new XmlSerializer(typeof(WorldFile));
            worldFile = (WorldFile)xml.Deserialize(sr);
            sr.Close();
            zip.Close();
            fs.Close();

            return true;
        }

        public static bool write(WorldFile worldFile, FileInfo file)
        {
            FileStream fs = file.OpenWrite();
            if (fs == null)
                return false;
            GZipStream zip = new GZipStream(fs, CompressionMode.Compress, false);
            StreamWriter sw = new StreamWriter(zip);

            XmlSerializer xml = new XmlSerializer(typeof(WorldFile));
            xml.Serialize(sw, worldFile);
            sw.Close();
            zip.Close();
            fs.Close();

            return true;
        }
    }
}