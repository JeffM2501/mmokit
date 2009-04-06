using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;
using System.Drawing.Imaging;
using System.IO;

namespace modeler
{
    public class FaceVert
    {
        public int vert = -1;
        public int normal = -1;
        public int uv = -1;

        public FaceVert()
        {
        }

        public FaceVert( int v, int n, int u)
        {
            vert = v;
            normal = n;
            uv = u;
        }
    }

    public class Face
    {
        public List<FaceVert> verts = new List<FaceVert>();
        public int normal = -1;
    }

    public class Mesh
    {
        public List<Vector3> verts = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector2> uvs = new List<Vector2>();
        public Dictionary<string, List<Face>> faces = new Dictionary<string, List<Face>>();

        public void build ( )
        {
            foreach (KeyValuePair<string, List<Face>> group in faces)
                build(group.Key);
        }

        public void build ( string[] hiddenGroups )
        {
            foreach (KeyValuePair<string, List<Face>> group in faces)
            {
                if (!hiddenGroups.Contains(group.Key))
                    build(group.Key);
            }
        }

        public void build ( string groupName )
        {
            List<Face> group = faces[groupName];

            foreach(Face f in group)
            {
                GL.Begin(BeginMode.Polygon);
                foreach (FaceVert v in f.verts)
                {
                    if (v.vert >= 0 && v.vert < verts.Count)
                    {
                        if (v.uv >= 0 && v.uv < uvs.Count)
                            GL.TexCoord2(uvs[v.uv]);

                        if (v.normal >= 0 && v.normal < normals.Count)
                            GL.Normal3(normals[v.normal]);
                        GL.Vertex3(verts[v.vert]);
                    }
                }
                GL.End();
            }
        }

        public void addFace ( string group, Face face )
        {
            List<Face> faceGroup;
            if (!faces.ContainsKey(group))
            {
                faceGroup = new List<Face>();
                faces.Add(group, faceGroup);
            }
            else
                faceGroup = faces[group];

            faceGroup.Add(face);
        }

        public int addVert(Vector3 v)
        {
            if (!verts.Contains(v))
            {
                verts.Add(v);
                return verts.Count-1;
            }

            return verts.IndexOf(v);
        }

        public int addNormal(Vector3 v)
        {
            if (!normals.Contains(v))
            {
                normals.Add(v);
                return normals.Count-1;
            }

            return normals.IndexOf(v);
        }

        public int addUV(Vector2 v)
        {
            if (!uvs.Contains(v))
            {
                uvs.Add(v);
                return uvs.Count-1;
            }

            return uvs.IndexOf(v);
        }
    }

    public class Material
    {
        public string name = "Default";
        public Color baseColor = Color.White;
        public Color ambinent = Color.Black;
        public Color specular = Color.Transparent;
        public Color emmision = Color.Transparent;
        public float shine = 0;

        public string texture = string.Empty;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        int textureID = -1;
        [System.Xml.Serialization.XmlIgnoreAttribute]
        int listID = -1;

        bool textureIsValid (string t)
        {
            if (t == string.Empty)
                return false;
            string extension = Path.GetExtension(t);
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                return false;

            return new FileInfo(t).Exists;
        }

        int getTextureID ()
        {
            if (textureID != -1)
                return textureID;

            Bitmap bitmap = new Bitmap(texture);
            
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Glu.Build2DMipmap(TextureTarget.Texture2D,(int)PixelInternalFormat.Rgba,data.Width, data.Height,OpenTK.Graphics.PixelFormat.Bgra,PixelType.UnsignedByte,data.Scan0);

  //          GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
   //             OpenTK.Graphics.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            
            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return textureID;
        }

        public void Invalidate ()
        {
            if (listID != -1)
                GL.DeleteLists(listID,1);

            if (textureID != -1)
                GL.DeleteTexture(textureID);
            listID = -1;
            textureID = -1;
        }

        public void Generate ()
        {
            if (listID == -1)
            {
                listID = GL.GenLists(1);
                GL.NewList(listID,ListMode.Compile);
                if (textureIsValid(texture))
                {
                    GL.Enable(EnableCap.Texture2D);
                    GL.BindTexture(TextureTarget.Texture2D, getTextureID());
                }
                else
                    GL.Disable(EnableCap.Texture2D);

                GL.Color4(baseColor);
                GL.EndList();
            }
        }

        public void Execute ()
        {
            Generate();
            GL.CallList(listID);
        }
    }

    public class Model 
    {
        public Dictionary<Material, Mesh> meshes = new Dictionary<Material,Mesh>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        Dictionary<Material, int> geoLists = new Dictionary<Material, int>();

        public void Invalidate ()
        {
            foreach (KeyValuePair<Material, int> m in geoLists)
                GL.DeleteLists(m.Value, 1);

            geoLists.Clear();

            foreach (KeyValuePair<Material, Mesh> m in meshes)
                m.Key.Invalidate();
       }

        void Rebuild ()
        {
            if (meshes.Count == geoLists.Count)
                return;

            // make sure it's clear
            Invalidate();

            foreach (KeyValuePair<Material, Mesh> m in meshes)
            {
                int list = GL.GenLists(1);
                GL.NewList(list, ListMode.Compile);
                m.Value.build();
                GL.EndList();
                geoLists.Add(m.Key, list);
            }
        }

        public void drawAll ( )
        {
            if (meshes.Count == 0)
                return;

            Rebuild();
            foreach (KeyValuePair<Material, Mesh> m in meshes)
            {
               m.Key.Execute();
                GL.CallList(geoLists[m.Key]);
               // m.Value.build();
            }
        }

        public void clear ()
        {
            Invalidate();
            meshes.Clear();
        }

        public Mesh getMeshForMaterial( string name )
        {
            foreach(KeyValuePair<Material,Mesh> m in meshes)
            {
                if (m.Key.name == name)
                    return m.Value;
            }
            return null;
        }

        public void swapYZ()
        {
            Invalidate();

            foreach (KeyValuePair<Material, Mesh> m in meshes)
            {
                Mesh mesh = m.Value;

                for (int i = 0; i < mesh.verts.Count; i++ )
                    mesh.verts[i] = new Vector3(mesh.verts[i].X,-mesh.verts[i].Z,mesh.verts[i].Y);                
            }
        }
    }
}
