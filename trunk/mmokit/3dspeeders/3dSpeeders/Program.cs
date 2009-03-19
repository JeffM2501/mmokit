using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using Axiom;
using Axiom.Configuration;

namespace _3dSpeeders
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form = new Form1();

            // ok find the data dir
            // check our dir
            DirectoryInfo dataDir = new DirectoryInfo("./data");
            if (!dataDir.Exists)
            {
                // check if we are coming from the bin/x86/debug dir
                dataDir = new DirectoryInfo("../../../data");
                if (!dataDir.Exists)
                {
                    // check if we are coming from the bin/debug dir
                    dataDir = new DirectoryInfo("../../data");
                    if (!dataDir.Exists)
                    {
                        // ok all Bets are off, we are screwed, bail
                        // TODO, fire up the update manager and GET the dir

                        MessageBox.Show("Data dir not found");
                        return;
                    }
                }
            }

            form.connectionInfo.dataDir = dataDir;

            Application.Run(form);

            if (form.connectionInfo.connect)
            {
                form.Dispose();
                new Game(form.config,form.connectionInfo).run();
            }

            Application.Exit();
        }
    }
}
