using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using UVapi.FileIO;

namespace UVapi.FileIO.OBJReader
{
    [FileIOPlugin]
    public class OBJreader : IFileIOPlugin
    {
        public string getName()
        {
            return "OBJ_FILE_IO";
        }

        public string getExtension()
        {
            return "obj";
        }

        public string getDescription()
        {
            return "Wavefront OBJ Files";
        }

        bool carRead()
        {
            return true;
        }

        bool canWrite()
        {
            return false;
        }

        public bool read(FileInfo file, Model model)
        {
            return false;
        }
    }
}
