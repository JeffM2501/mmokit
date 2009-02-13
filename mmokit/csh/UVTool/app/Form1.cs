using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using UVapi;
using UVapi.FileIO;

using Tao.OpenGl;
using Tao.Platform.Windows;

using GLRenderer;

namespace UVTool
{
    public partial class Form1 : Form
    {
        private Renderer renderer;

        Dictionary<string, IFileIOPlugin> fileIOClasses = new Dictionary<string, IFileIOPlugin>();

        public void checkForFileIOHandler ( Type type )
        {
            if (type.IsDefined(typeof(UVapi.FileIO.FileIOPluginAttribute), true))
            {
                IFileIOPlugin pclass = (IFileIOPlugin)Activator.CreateInstance(type);
                fileIOClasses.Add(pclass.getName(), pclass);
            }

        }
        public void loadPlugins ( )
        {
            DirectoryInfo dir = new DirectoryInfo("./plugins");

            if (!dir.Exists)
                dir.Create();

            foreach (FileInfo f in dir.GetFiles("*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(f.FullName);
                if (assembly != null)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.IsAbstract)
                            continue;

                        checkForFileIOHandler(type);
                    }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            this.CreateParams.ClassStyle = this.CreateParams.ClassStyle | User.CS_HREDRAW | User.CS_VREDRAW | User.CS_OWNDC;      // Redraw On Size, And Own DC For Window.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
            this.SetStyle(ControlStyles.DoubleBuffer, true);                    // Buffer Control
            this.SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
            this.SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
            this.SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves

            renderer = new Renderer(this);
            renderer.setClearColor(0, 0, 0);

            this.Paint += new PaintEventHandler(paint);                  // On Paint

        }

        public void paint( Object sender, PaintEventArgs e )
        {
            renderer.beginDraw();

            // do stuff
            renderer.endDraw();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // build the list of fileIO plugins
            foreach(KeyValuePair<string,IFileIOPlugin> p in fileIOClasses)
            {
                if (p.Value.carRead())
                {
                    
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}