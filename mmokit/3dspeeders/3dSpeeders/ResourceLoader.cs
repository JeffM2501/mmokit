﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace _3dSpeeders
{
    public class ResourceLoader
    {
        void recursiveAddDir(DirectoryInfo dir)
        {
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
//                ResourceManager.AddCommonArchive(d.FullName, "Folder");
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
