using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace fileswap
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] args)
        {
            string source;
            string dest;
            bool deleteSource = false;

            // always wait 1 second before we do anything
            Thread.Sleep(1000);

            if (args.Length > 1)
            {
                source = args[0];
                dest = args[1];

                if (args.Length > 2 && args[2] == "delete")
                    deleteSource = true;

                FileInfo sourceInfo = new FileInfo(source);
                if (!sourceInfo.Exists)
                    return;

                FileInfo destInfo = new FileInfo(dest);
                if (destInfo.Exists)
                {
                    destInfo.Delete();
                    int deleteAttempts = 0;
                    while (destInfo.Exists)
                    {
                        deleteAttempts++;
                        if (deleteAttempts > 3)
                            return;

                        destInfo.Delete();
                        Thread.Sleep(1000);
                    }
                } // delete it

                sourceInfo.CopyTo(dest, true);
                if ( deleteSource)
                    sourceInfo.Delete();

                string runCommand = string.Empty;
                if (deleteSource && args.Length > 3)
                    runCommand = args[3];
                else if (args.Length > 2)
                    runCommand = args[2];

                if (runCommand.Length > 0)
                    Process.Start(runCommand);
            }
        }
    }
}