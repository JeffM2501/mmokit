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

namespace UVTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Dictionary<string,IFileIOPlugin> fileIOClasses = new Dictionary<string,IFileIOPlugin>();

            DirectoryInfo dir = new DirectoryInfo("./plugins");

            if (!dir.Exists)
                dir.Create();

            foreach (FileInfo f in dir.GetFiles("*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(f.FullName);
                if (assembly != null)
                {
                    foreach(Type type in assembly.GetTypes())
                    {
                        if (type.IsAbstract)
                            continue;

                        if (type.IsDefined(typeof(UVapi.FileIO.FileIOPluginAttribute), true))
                        {
                            IFileIOPlugin pclass = (IFileIOPlugin)Activator.CreateInstance(type);
                            fileIOClasses.Add(pclass.getName(),pclass);
                        }
                    }
                }
            }

            MessageBox.Show(fileIOClasses.Count.ToString() + " file io plugins found","Number of Plug-ins");

            if (fileIOClasses.Count > 0)
            {
                foreach(KeyValuePair<string,IFileIOPlugin> t in fileIOClasses)
                {
                    IFileIOPlugin p = t.Value;

                    MessageBox.Show(p.getName() + " reads in " + p.getExtension() + " files", "First Plugin");
                    break;
                }
            }
        }
    }
}