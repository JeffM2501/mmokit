using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace patcher
{
    public delegate bool PatchStatusCallback ( int item, int count );

    public enum PatchReturn
    {
        ePatchOK,
        ePatchFail,
        ePatcherReboot,
        eServerFail
    };

    public class PatchFileRecord
    {
        public FileInfo localFile = null;
        public string remoteFile = string.Empty;
        public string remoteHash = string.Empty;
      
        public FileInfo tempFile = null;

        public PatchFileRecord ( DirectoryInfo dir, string file, string hash )
        {
            remoteFile = file;
            remoteHash = hash;

            localFile = new FileInfo(dir.FullName + file);
        }

        public bool update ( string URL )
        {
            if (localFile == null)
                return false;

            tempFile = null;

            // check if the local file exists
            if (localFile.Exists)
            {
                localFile.Delete();
            }

            WebRequest request = WebRequest.Create(URL + "?file=" + remoteFile);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.Accepted)
                return false;

            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
 
            reader.Close();
            dataStream.Close();
            response.Close();

            return true;
        }

        public bool copy ( )
        {
            if (!tempFile.Exists)
                return false;
        }

    }

    public class Patcher
    {
        DirectoryInfo rootDir = null;
        string URL = string.Empty;

        public Patcher(string dir, string server)
        {
            rootDir = new DirectoryInfo(dir);

            URL = server;
        }

        public PatchReturn patch ( string patchApp )
        {
            return patch(patchApp, null);
        }

        public PatchReturn patch ( string patchApp, PatchStatusCallback callback )
        {
            if (!rootDir.Exists)
                return PatchReturn.ePatchFail;

            WebRequest request = WebRequest.Create(URL);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.Accepted)
                return PatchReturn.eServerFail;

            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            List<PatchFileRecord> patchFiles = new List<PatchFileRecord>();
            string index = reader.ReadLine();
            while (index.Length > 0)
            {
                index = reader.ReadLine();

                string[] chunks = index.Split(":");
                if (chunks.Length == 2)
                    patchFiles.Add(new PatchFileRecord(rootDir, chunks[0], chunks[1]));
            }

            reader.Close();
            dataStream.Close();
            response.Close();

            if (patchFiles.Length == 0)
                return PatchReturn.eServerFail;

            List<PatchFileRecord> delayedWrites = new List<PatchFileRecord>();

            int i;
            foreach (PatchFileRecord f in patchFiles)
            {
                if (callback != null)
                    callback(i++, patchFiles.Count);

                if (f.update(URL))
                {
                    if (f.tempFile.Exists)
                        delayedWrites.Add(f);
                }
            }

            patchFiles.Clear();

            if (delayedWrites.Count > 0)
            {

                return PatchReturn.ePatcherReboot;
            }
            return PatchReturn.ePatchOK;
        }
    }
}
