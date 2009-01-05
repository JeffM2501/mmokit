using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace patchserver
{
    public class Version
    {
        public Version()
        {
        }

        public int getVersion ( )
        {
            lock(lockToken)
            {
                return verison;
            }
        }

        public bool isDev ( )
        {
            lock(lockToken)
            {
                return dev;
            }
        }
        
        public bool itemExists ( string file )
        {
            lock(lockToken)
            {
                return fileKeys.ContainsKey(file);
            }
        }

        public List<string> getFiles( )
        {
            List<string> s = new List<string>();
            lock (lockToken)
            {
                foreach (KeyValuePair<string, string> i in fileKeys)
                    s.Add(i.Key);
            }
            return s;
        }

        public string getItemHash ( string file )
        {
            lock(lockToken)
            {
                if (!fileKeys.ContainsKey(file))
                    return string.Empty;
                return fileKeys[file];
            }
        }

        public DirectoryInfo getRoot()
        {
            lock (lockToken)
            {
                return new DirectoryInfo(dir.FullName);
            }
        }

        Object lockToken = new Object();

        public int verison = 0;
        public bool dev = false;
        public DirectoryInfo dir;
        public Dictionary<string, string> fileKeys = new Dictionary<string,string>();
    }

    public class Setup
    {
        public List<string> hosts = new List<string>();
        public string rootDir = "./";
    }

    public class VersionResources
    {
        public VersionResources ( )
        {

        }

        public bool versExists ( int vers )
        {
            lock(lockToken)
            {
                return versions.ContainsKey(vers);
            }
        }

        public List<int> getVersions ( bool dev )
        {
            List<int> l = new List<int>();
            lock(lockToken)
            {
                foreach(KeyValuePair<int,Version> v in versions)
                {
                    if (v.Value.dev == dev)
                        l.Add(v.Value.verison);
                }
            }

            return l;
        }

        public Version getVersion ( int vers )
        {
            lock(lockToken)
            {
                if (!versions.ContainsKey(vers))
                    return null;

                return versions[vers];
            }
        }

        Object lockToken = new Object();
        public Dictionary<int, Version> versions = new Dictionary<int,Version>();
    }

    public class Request
    {
        HttpListenerContext context;
        VersionResources versions;

        public Request ( HttpListenerContext c, VersionResources v )
        {
            context = c;
            versions = v; 
        }

        public void process (  )
        {
            int vers = 0;
            HttpListenerRequest request = context.Request;

            if (context.Request.QueryString.HasKeys())
            {
                for (int i = 0; i < request.QueryString.Count; i++)
                {
                    if(request.QueryString.GetKey(i) == "vers")
                    {
                        if (request.QueryString.GetValues(i).GetLength(0) > 0)
                         int.TryParse(request.QueryString.GetValues(i)[0],out vers);
                    }
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 200;
            context.Response.AppendHeader("request", context.Request.Url.AbsolutePath);

            if (context.Request.Url.AbsolutePath.Length < 2 || (context.Request.Url.AbsolutePath == "/index" && vers == 0))
            {
                string responce = "";

                List<int> l = versions.getVersions(false);
                foreach(int v in l)
                    responce += "vers " + v.ToString() + "\n";

                l = versions.getVersions(true);
                foreach (int v in l)
                    responce += "dev " + v.ToString() + "\n";

                sendText(responce);
            }
            else if (context.Request.Url.AbsolutePath == "/index") // they want a file listing for a version
            {
                Version v = versions.getVersion(vers);
                if(v != null)
                {
                    string responce = "";

                    List<string> files = v.getFiles();
                    foreach (string f in files)
                    {
                        responce += v.getItemHash(f);
                        responce += ":";
                        responce += f + "\n";
                    }
                    sendText(responce);
                }
                else
                    sendText("Error: Invalid Version");
            }
            else if (vers != 0)
            {
                Version v = versions.getVersion(vers);

                string filePath = context.Request.Url.AbsolutePath.Remove(0,1);
                DirectoryInfo root = v.getRoot();

                FileInfo file = new FileInfo(Path.Combine(root.FullName, filePath));
                if (!file.Exists)
                {
                    context.Response.StatusCode = 404;
                    sendText("Error: File Not Found");
                }
                else
                {
                    context.Response.ContentType = "application/octet-stream";
                    sendBinary(file);
                }
            }
            else
                sendText("Error: Invalid Version");

            context.Response.OutputStream.Close();
        }

        void sendText(string text)
        {
            context.Response.OutputStream.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(text), 0, text.Length);
        }

        public bool sendBinary(FileInfo file)
        {
            if (!file.Exists)
                return false;

            FileStream fs = file.OpenRead();
            int size = (int)fs.Length;

            if (size == 0)
                return false;

            byte[] data = new byte[size];
            fs.Read(data, 0, size);
            context.Response.OutputStream.Write(data, 0, size);

            fs.Close();
            return true;
        }
    }

    class Program
    {
        static bool runServer = true;
        static VersionResources versions = new VersionResources();
        static Setup setup = new Setup();

        static void Main(string[] args)
        {
            readSetup(args);
            fillVersions();

            HttpListener httpd = new HttpListener();
            foreach (string h in setup.hosts)
                httpd.Prefixes.Add(h);

            httpd.Start();

            AsyncCallback callback = new AsyncCallback(HTTPCallback);

            IAsyncResult result = httpd.BeginGetContext(callback, httpd);

            while (runServer)
            {
                Thread.Sleep(100);

                if (result.IsCompleted)
                    result = httpd.BeginGetContext(callback, httpd);
            }

            httpd.Close();
        }

        static void readSetup(string[] args)
        {
            if (args.Length > 0)
                setup.rootDir = args[0];

           // setup.hosts.Add("http://localhost/");
            if (args.Length > 1)
            {
                for ( int i = 1; i < args.Length; i++)
                {
                    if (args[i] != string.Empty)
                    {
                        Console.WriteLine("adding host " + args[i]);
                        setup.hosts.Add(args[i]);
                    }
                }
            }
        }

        static void fillVersions()
        {
            System.Console.Write("Starting Version Scan for " + setup.rootDir + "\n");

            DirectoryInfo rootDir = new DirectoryInfo(setup.rootDir);
            if (!rootDir.Exists)
                return;

            foreach (DirectoryInfo info in rootDir.GetDirectories())
                fillVersionInfo(info);
        }

        static void fillVersionInfo(DirectoryInfo info)
        {
            FileInfo infoFile = new FileInfo(Path.Combine(info.FullName,"vinfo.txt"));
            if (!infoFile.Exists)
                return;

            FileStream stream = infoFile.OpenRead();
            StreamReader reader = new StreamReader(stream);

            Version vers = new Version();

            string temp;
            temp = reader.ReadLine();
            int.TryParse(temp,out vers.verison);
            temp = reader.ReadLine();
            vers.dev = temp == "true";
            reader.Close();
            stream.Close();

            vers.dir = new DirectoryInfo(info.FullName);

            if (vers.dir.Exists && vers.verison > 0)
            {
                System.Console.Write("Found Version " + vers.verison.ToString() + " in " + vers.dir.Name + "\n");
                FileInfo hashFile = new FileInfo(Path.Combine(vers.dir.FullName,"finfo.txt"));
                if (hashFile.Exists)
                {
                    System.Console.Write("Found HashFile " + hashFile.FullName + "\n");

                    FileStream fs = hashFile.OpenRead();
                    reader = new StreamReader(fs);

                    string line = reader.ReadLine();
                    while (line != null && line != string.Empty)
                    {
                        string[] chunks = line.Split(new char[] { ':' }, 2);
                        if (chunks.Length > 1)
                            vers.fileKeys.Add(chunks[1], chunks[0]);

                        line = reader.ReadLine();
                    }
                    reader.Close();
                    fs.Close();
                }
                else
                {
                    scanDir(vers, info, info);
                    // write out the info file

                    FileStream fs = hashFile.OpenWrite();
                    StreamWriter writer = new StreamWriter(fs);
                    foreach(KeyValuePair<string,string> k in vers.fileKeys)
                        writer.WriteLine(k.Value + ":" + k.Key);

                    writer.Close();
                    fs.Close();
                }
                versions.versions.Add(vers.verison, vers);
            }
        }

        static void scanDir ( Version vers, DirectoryInfo info, DirectoryInfo root )
        {
            foreach (FileInfo file in info.GetFiles())
            {
                if (file.Name != "vinfo.txt" && file.Name != "finfo.txt")
                {
                    string path = file.FullName.Remove(0, root.FullName.Length+1);
                    vers.fileKeys.Add(path, computeHash(file));
                    System.Console.Write("Computed Hash for " + path + "\n");
                }
            }

            foreach (DirectoryInfo dir in info.GetDirectories())
                scanDir(vers, dir, root);
        }

        static string computeHash(FileInfo info)
        {
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            FileStream fs = info.OpenRead();
            Byte[] hashCode = md5Provider.ComputeHash(fs);
            fs.Close();
            return BitConverter.ToString(hashCode);
        }

        public static void HTTPCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            Request r = new Request(listener.EndGetContext(result), versions);
            new Thread(new ThreadStart(r.process)).Start();
        }
    }
}
