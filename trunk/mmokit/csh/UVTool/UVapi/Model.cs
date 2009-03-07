using System;
using System.Collections.Generic;
using System.Text;

namespace UVapi
{
    public class Vertex3D
    {
        public double x, y, z;
    }

    public class Vertex2D
    {
        public double u, v;
    }

    public class FaceVert
    {
        public int vert = -1;
        public int normal = -1;
        public int uv = -1;
    }

    public class Color
    {
        double r = 1, g = 1, b = 1, a = 1;
    }

    public class Face
    {
        public List<FaceVert> verts = new List<FaceVert>();
        public int normal = -1;

        public Color color = new Color();
    }

    public class Mesh
    {
        public List<Vertex3D> verts = new List<Vertex3D>();
        public List<Vertex3D> normals = new List<Vertex3D>();
        public List<Vertex2D> uvs = new List<Vertex2D>();
        public List<Face> faces = new List<Face>();

        public int addVert ( Vertex3D v )
        {
            if (!verts.Contains(v))
            {
                verts.Add(v);
                return verts.Count;
            }

            return verts.FindIndex(v);
        }

        public int addNormal ( Vertex3D v )
        {
            if (!normals.Contains(v))
            {
                normals.Add(v);
                return normals.Count;
            }

            return normals.FindIndex(v);
        }

        public int addUV ( Vertex2D v )
        {
            if (!uvs.Contains(v))
            {
                uvs.Add(v);
                return uvs.Count;
            }

            return uvs.FindIndex(v);
        }
    }

    public class Model
    {
        public List<Mesh> meshes = new List<Mesh>();
    }
}
