using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using UVapi.FileIO;
using UVapi;

namespace UVapi.FileIO.OBJReader
{
    [FileIOPlugin]
    public class OBJreader : IFileIOPlugin
    {

        public string getName()
        {
            return "OBJ_FILE_IO";
        }

        public string getExtension()
        {
            return "obj";
        }

        public string getDescription()
        {
            return "Wavefront OBJ Files";
        }

        public bool canRead()
        {
            return true;
        }

        public bool canWrite()
        {
            return false;
        }

        string[] splitOnDelim ( string data, string delim, int count )
        {
            return data.Split(delim.ToCharArray(), count);
        }

        string[] splitOnDelim(string data, string delim)
        {
            return data.Split(delim.ToCharArray());
        }     

        Vertex3D readV3D ( string data )
        {
            Vertex3D v = new Vertex3D();
            string[] n = splitOnDelim(data, " ", 3);
            if (n.Length > 2)
            {
                v.x = double.Parse(n[0]);
                v.y = double.Parse(n[1]);
                v.z = double.Parse(n[2]);
            }
            return v;
        }

        Vertex2D readV2D(string data)
        {
            Vertex2D v = new Vertex2D();
            string[] n = splitOnDelim(data, " ", 2);
            if (n.Length > 1)
            {
                v.u = double.Parse(n[0]);
                v.v = double.Parse(n[1]);
            }
            return v;
        }

        List<FaceVert> readFaces (string data)
        {
            List<FaceVert> faces = new List<FaceVert>();
            string[] nubs = splitOnDelim(data, " ");
            foreach(string n in nubs)
            {
                if (n.Contains("#"))
                    break;

                FaceVert f = new FaceVert();
                string[] i = splitOnDelim(n, "/",3);
                if (i.Length > 0 && i[0] != string.Empty)
                    f.vert = int.Parse(i[0]);
                if (i.Length > 1 && i[1] != string.Empty)
                    f.uv = int.Parse(i[1]);
                if (i.Length > 2 && i[2] != string.Empty)
                    f.normal = int.Parse(i[2]);

                faces.Add(f);
            }

            return faces;
        }

        public bool read(FileInfo file, Model model)
        {
            FileStream fs = file.OpenRead();
            StreamReader sr = new StreamReader(fs);

            // save off the verts, as they may span groups
            // we'll add em to each submesh as the faces get added
            // this way each submesh only has the verts for it's faces
            List<Vertex3D> verts = new List<Vertex3D>();
            List<Vertex3D> norms = new List<Vertex3D>();
            List<Vertex2D> uvs = new List<Vertex2D>();

            Mesh currentMesh = null;

            string currentObjectName = string.Empty;
            string currentGroupName = string.Empty;
            string currentMatName = string.Empty;
            string currentMapName = string.Empty;

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] nubs = splitOnDelim(line," ", 2);
                if (nubs.Length > 0)
                {
                    if (nubs.Length > 1)
                    {
                        string code = nubs[0];
                        if (code == "o")
                            currentObjectName = nubs[1];
                        else if (code == "usemtl")
                            currentMatName = nubs[1];
                        else if (code == "usemap")
                            currentMapName = nubs[1];
                        else if (code == "g")
                        {
                            currentMesh = null;
                            currentGroupName = nubs[1];
                        }
                        else if (code == "v")
                            verts.Add(readV3D(nubs[1]));
                        else if (code == "vn")
                            norms.Add(readV3D(nubs[1]));
                        else if (code == "vt")
                            uvs.Add(readV2D(nubs[1]));
                        else if (code == "f")
                        {
                            if (currentMesh == null)
                            {
                                currentMesh = new Mesh();
                                model.meshes.Add(currentMesh);
                            }
                            List<FaceVert> fv = readFaces(nubs[1]);

                            foreach(FaceVert f in fv)
                            {

                            }

                        }
                    }
                    else
                    {

                    }
                }
            }
            return false;
        }
    }
}
