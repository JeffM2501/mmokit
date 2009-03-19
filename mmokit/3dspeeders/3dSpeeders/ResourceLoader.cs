using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Axiom;
using Axiom.Core;
using Axiom.Configuration;

namespace _3dSpeeders
{
    public class ResourceLoader
    {
        void recursiveAddDir(DirectoryInfo dir)
        {
            foreach (FileInfo f in dir.GetFiles("*.zip"))
                ResourceManager.AddCommonArchive(f.FullName, "ZipFile");

            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                ResourceManager.AddCommonArchive(d.FullName, "Folder");
                recursiveAddDir(d);
            }
        }

        public bool setup(DirectoryInfo dataDir)
        {
            recursiveAddDir(dataDir);

            return true;
        }
    }
}
