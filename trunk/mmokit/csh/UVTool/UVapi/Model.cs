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
        public int vert;
        public int normal;
        public int uv;
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
    }

    public class Model
    {
        public List<Mesh> meshes = new List<Mesh>();
    }
}
