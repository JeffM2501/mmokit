using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;
using System.IO;

using Drawables.Materials;

namespace Drawables.Models
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

    public class MeshGroup
    {
        public string name = string.Empty;
        public List<Face> faces = new List<Face>();
    }

    public class Mesh
    {
        public Material material = new Material();
        public List<Vector3> verts = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<MeshGroup> groups = new List<MeshGroup>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Dictionary<string, MeshGroup> groupMap = new Dictionary<string, MeshGroup>();

        private void checkGroupMap ()
        {
            if (groupMap.Count == groups.Count)
                return;

            groupMap.Clear();
            foreach (MeshGroup group in groups)
                groupMap.Add(group.name,group);
        }

        public void build ( )
        {
            foreach (MeshGroup group in groups)
                build(group);
        }

        public void buildDisplayNormals()
        {
            foreach (MeshGroup group in groups)
                buildDisplayNormals(group);
        }

        public void buildWireframe()
        {
            foreach (MeshGroup group in groups)
                buildWireframe(group);
        }

        public void build ( List<string> hiddenGroups )
        {
            checkGroupMap();

            foreach (KeyValuePair<string, MeshGroup> group in groupMap)
            {
                if (!hiddenGroups.Contains(group.Key))
                    build(group.Value);
            }
        }

        public void build ( MeshGroup group )
        {
            foreach (Face f in group.faces)
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

        public void buildDisplayNormals(MeshGroup group)
        {
            GL.Begin(BeginMode.Lines);
            foreach (Face f in group.faces)
            {
                foreach (FaceVert v in f.verts)
                {
                    if (v.vert >= 0 && v.vert < verts.Count)
                    {
                        if (v.normal >= 0 && v.normal < normals.Count)
                        {
                            Vector3 normalEP = verts[v.vert] + normals[v.normal];
                            Vector3 normalSP = verts[v.vert];

                            GL.Vertex3(normalSP);
                            GL.Vertex3(normalEP);
                        }
                    }
                }
            }
            GL.End();
        }

        public void buildWireframe(MeshGroup group)
        {
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            foreach (Face f in group.faces)
            {
                GL.Begin(BeginMode.Polygon);
                foreach (FaceVert v in f.verts)
                {
                    if (v.vert >= 0 && v.vert < verts.Count)
                        GL.Vertex3(verts[v.vert]);
                }
                GL.End();
            }
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
        }

        public void build ( string groupName )
        {
            checkGroupMap();
   
            build(groupMap[groupName]);
        }

        protected MeshGroup getGroup ( string name )
        {
            if (groupMap.ContainsKey(name))
                return groupMap[name];

            foreach( MeshGroup group in groups)
            {
                if (group.name == name)
                {
                    // it wasn't in the name map
                    groupMap[name] = group;
                    return group;
                }
            }

            MeshGroup g = new MeshGroup();
            g.name = name;
            groups.Add(g);
            groupMap.Add(name, g);
            return g;
        }

        public void addFace ( string groupName, Face face )
        {
            getGroup(groupName).faces.Add(face);
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

    public class Model 
    {
        public List<Mesh> meshes = new List<Mesh>();

        List<MaterialOverride> skins = new List<MaterialOverride>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Dictionary<Material, Mesh> meshMap = new Dictionary<Material, Mesh>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        Dictionary<Material, int> geoLists = new Dictionary<Material, int>();
        [System.Xml.Serialization.XmlIgnoreAttribute]
        int displayNormalsList = -1;
        [System.Xml.Serialization.XmlIgnoreAttribute]
        int displayWireframeList = -1;

        public void LinkToSystem(MaterialSystem system)
        {
            foreach (MaterialOverride m in skins)
                m.LinkToSystem(system);

            foreach (Mesh m in meshes)
                m.material = system.getMaterial(m.material);
        }

        public MaterialOverride findSkin ( string name )
        {
            foreach(MaterialOverride m in skins)
            {
                if (m.name == name)
                    return m;
            }
            return null;
        }

        public bool removeSkin ( string name )
        {
            MaterialOverride skin = findSkin(name);
            if (skin == null)
                return false;

            skins.Remove(skin);
            return true;
        }

        public MaterialOverride newSkin ( string name )
        {
            foreach (MaterialOverride m in skins)
            {
                if (m.name == name)
                    return m;
            }
            MaterialOverride skin = new MaterialOverride();
            skin.name = name;
            skins.Add(skin);
            return skin;
        }

        public void addMaterial (Material mat)
        {
            if (meshMap.ContainsKey(mat))
                return;

            foreach (Mesh m in meshes)
            {
                if (m.material == mat)
                {
                    meshMap.Add(m.material, m);
                    return;
                }
            }

            Mesh mesh = new Mesh();
            mesh.material = mat;
            meshes.Add(mesh);
            meshMap.Add(mesh.material, mesh);
            return;
        }

        public Mesh getMesh (Material mat)
        {
            if (meshMap.ContainsKey(mat))
                return meshMap[mat];

            foreach(Mesh m in meshes)
            {
                if (m.material == mat)
                {
                    meshMap.Add(m.material, m);
                    return m;
                }
            }

            Mesh mesh = new Mesh();
            mesh.material = mat;
            meshes.Add(mesh);
            meshMap.Add(mesh.material, mesh);
            return mesh;
        }

        public Mesh getMeshForMaterial(string name)
        {
            foreach (KeyValuePair<Material, Mesh> m in meshMap)
            {
                if (m.Key.name == name)
                    return m.Value;
            }

            foreach (Mesh m in meshes)
            {
                if (m.material.name == name)
                {
                    meshMap.Add(m.material, m);
                    return m;
                }
            }
            return null;
        }

        public void Invalidate ()
        {
            foreach (KeyValuePair<Material, int> m in geoLists)
                GL.DeleteLists(m.Value, 1);
            geoLists.Clear();

            if (displayNormalsList != -1)
                GL.DeleteLists(displayNormalsList, 1);
            if (displayWireframeList != -1)
                GL.DeleteLists(displayWireframeList, 1);

            foreach (Mesh m in meshes)
                m.material.Invalidate();
       }

        void Rebuild ()
        {
            if (meshes.Count == geoLists.Count)
                return;

            // make sure it's clear
            Invalidate();

            foreach (Mesh m in meshes)
            {
                int list = GL.GenLists(1);
                GL.NewList(list, ListMode.Compile);
                m.build();
                GL.EndList();
                geoLists.Add(m.material, list);
            }

            displayNormalsList = GL.GenLists(1);
            GL.NewList(displayNormalsList, ListMode.Compile);
            foreach (Mesh m in meshes)
                m.buildDisplayNormals();
            GL.EndList();

            displayWireframeList = GL.GenLists(1);
            GL.NewList(displayWireframeList, ListMode.Compile);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, 0.05f);
            foreach (Mesh m in meshes)
                m.buildWireframe();
            GL.Disable(EnableCap.PolygonOffsetLine);
            GL.EndList();
        }

        public void drawAll ( )
        {
            if (meshes.Count == 0)
                return;

            Rebuild();
            foreach (Mesh m in meshes)
            {
               m.material.Execute();
               GL.CallList(geoLists[m.material]);
            }
        }

        public void drawAll(MaterialOverride matOverride)
        {
            if (meshes.Count == 0)
                return;

            foreach (Mesh m in meshes)
            {
                int listID = matOverride.getGeoListID(m.material);
                if (listID < 0)
                {
                    listID = GL.GenLists(1);
                    GL.NewList(listID, ListMode.Compile);
                    m.build(matOverride.getHiddenGroups(m.material));
                    GL.EndList();
                    matOverride.setGeoListID(m.material, listID);
                }

                matOverride.Execute(m.material);
                GL.CallList(listID);
            }
        }

        protected void drawAllExtras ( bool normals, bool wireframe )
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            if (normals)
            {
                GL.Color4(Color.Red);
                GL.CallList(displayNormalsList);
            }

            if (wireframe)
            {
                GL.Color4(Color.White);
                GL.CallList(displayWireframeList);
            }
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
        }

        public void drawAll ( bool normals, bool wireframe )
        {
            if (meshes.Count == 0)
                return;
            drawAll();

            drawAllExtras(normals, wireframe);
        }

        public void drawAll(bool normals, bool wireframe, MaterialOverride matOverride)
        {
            if (meshes.Count == 0)
                return;
            drawAll(matOverride);

            drawAllExtras(normals, wireframe);
        }

        public void clear ()
        {
            Invalidate();
            meshes.Clear();
        }

        public bool valid ()
        {
            return meshes.Count > 0;
        }

        public void swapYZ()
        {
            Invalidate();

            foreach (Mesh mesh in meshes)
            {
                for (int i = 0; i < mesh.verts.Count; i++)
                    mesh.verts[i] = new Vector3(mesh.verts[i].X, -mesh.verts[i].Z, mesh.verts[i].Y);
                for (int i = 0; i < mesh.normals.Count; i++)
                    mesh.normals[i] = new Vector3(mesh.normals[i].X, -mesh.normals[i].Z, mesh.normals[i].Y);
            }
        }

        public void scale( float factor)
        {
            Invalidate();

            foreach (Mesh mesh in meshes)
            {
                for (int i = 0; i < mesh.verts.Count; i++)
                    mesh.verts[i] = new Vector3(mesh.verts[i].X * factor, mesh.verts[i].Y * factor, mesh.verts[i].Z * factor);
            }
        }
    }
}
