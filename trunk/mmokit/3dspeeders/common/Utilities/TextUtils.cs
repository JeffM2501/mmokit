using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utilities.Text
{
    public class PathUtils
    {

        public static string MakePathRelitive(string rootpath, string outpath)
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
    }
}
