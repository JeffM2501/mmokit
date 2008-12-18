using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;

namespace patchserver
{
    public class Version
    {
        public int verison = 0;
        public bool isDev = false;
    }

    public class Setup
    {
        public List<string> hosts = new List<string>();
    }

    public class Request
    {
        Request ( HttpListenerContext context )
        {
            context.Response.OutputStream.Close();
        }
    }

    class Program
    {
        static bool runServer = true;

        static void Main(string[] args)
        {
            Setup setup = new Setup();

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

        public static void HTTPCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            new Thread(new ThreadStart(new Request(listener.EndGetContext(result)))).Start();
        }
    }
}
