using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;

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

            bool threadedSound = false;
            if (threadedSound)
               new Thread(SoundSystem.instance.threadUpdate);
            else
                Application.Idle += new EventHandler(appIdle);

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
            while (true)
            {
                Form1 form = new Form1(dataDir);
                Application.Run(form);

                if (form.connectionInfo.connect)
                {
                    form.Dispose();
                    Game game = new Game(form.config, form.connectionInfo);
                    game.run();
                    if (game.quit)
                        break;
                    else if (game.disconnected)
                        MessageBox.Show("The server unexpectedly disconnected");
                    game = null;
                }
                else
                    break;
            }

            SoundSystem.instance.cleanup();
            SoundSystem.instance.exit();
            Application.Exit();
        }

        static void appIdle(Object sender, EventArgs e)
        {
            SoundSystem.instance.update();
        }
    }
}
