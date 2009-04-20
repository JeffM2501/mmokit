using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using World;

namespace gameserver
{
    public class GameWorld
    {
        public ObjectWorld world;
        public FileInfo mapFile;

        public bool loadWorldFile ( string file )
        {
            mapFile = new FileInfo(file);
            if (!mapFile.Exists)
                return false;

            WorldFile worldFile = new WorldFile();
            if (!WorldFile.read(out worldFile, mapFile))
                return false;

            // grab off the part we care about
            world = worldFile.world;
            world.BuildTree();

            return true;
        }
    }
}
