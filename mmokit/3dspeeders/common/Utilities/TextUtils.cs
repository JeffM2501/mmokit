using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utilities.Paths
{
    public class PathUtils
    {
        public static string MakePathRelitive(string rootpath, string outpath)
        {
            if (rootpath == string.Empty)
                return outpath;

            string[] rootChunks = rootpath.Split(Path.DirectorySeparatorChar.ToString().ToCharArray());

            string[] outchunks = outpath.Split(Path.DirectorySeparatorChar.ToString().ToCharArray());

            string relPath = string.Empty;

            int i = 0;
            for (i = 0; i < rootChunks.Length; i++)
            {
                if (rootChunks[i] != outchunks[i])
                    break;
            }

            for (int j = i; j < rootChunks.Length; j++ )
                relPath += ".." + Path.DirectorySeparatorChar.ToString();

            for (; i < outchunks.Length; i++)
            {
                relPath += outchunks[i];
                if (i != outchunks.Length - 1)
                    relPath += Path.DirectorySeparatorChar.ToString();
            }
            return relPath;
        }
    }
}
