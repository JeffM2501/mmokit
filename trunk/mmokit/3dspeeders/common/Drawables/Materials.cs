using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using OpenTK.Graphics;
using System.Drawing.Imaging;

using Drawables.Textures;
using Drawables.DisplayLists;

namespace Drawables.Materials
{
    public class GLColor
    {
        public float r = 1.0f;
        public float g = 1.0f;
        public float b = 1.0f;
        public float a = 1.0f;

        public GLColor()
        { }

        public override int GetHashCode()
        {
            return r.GetHashCode() ^ g.GetHashCode() ^ b.GetHashCode() ^ a.GetHashCode();
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;

            GLColor p = obj as GLColor;
            if ((System.Object)p == null)
                return false;

            return r == p.r && g == p.g && b == p.b && a == p.a;
        }

        public bool Equals(GLColor obj)
        {
            if (obj == null)
                return false;

            return r == obj.r && g == obj.g && b == obj.b && a == obj.a;
        }

        public static bool operator == (GLColor c1, GLColor c2)
        {
            return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b && c1.a == c2.a;
        }

        public static bool operator != (GLColor c1, GLColor c2)
        {
            return c1.r != c2.r || c1.g != c2.g || c1.b != c2.b || c1.a != c2.a;
        }

        public GLColor(float red, float green, float blue, float alpha)
        {
            r = red;
            g = green;
            b = blue;
            a = alpha;
        }

        public void glColor()
        {
            GL.Color4(r, g, b, a);
        }

        public GLColor(Color color, float alpha)
        {
            r = color.R / 255.0f;
            g = color.G / 255.0f;
            b = color.B / 255.0f;
            a = alpha;
        }

        public GLColor(Color color)
        {
            r = color.R / 255.0f;
            g = color.G / 255.0f;
            b = color.B / 255.0f;
            a = color.A / 255.0f;
        }

        public GLColor(GLColor color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public Color ToColor()
        {
            return Color.FromArgb((byte)(a * 255), (byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        public static GLColor Transparent = new GLColor(1.0f, 1.0f, 1.0f, 0.0f);
        public static GLColor White = new GLColor(1.0f, 1.0f, 1.0f, 1.0f);
        public static GLColor Black = new GLColor(0.0f, 0.0f, 0.0f, 1.0f);
    }


    public class Material
    {
        public string name = "Default";
        public GLColor baseColor = GLColor.White;
        public GLColor ambinent = GLColor.Black;
        public GLColor specular = GLColor.Transparent;
        public GLColor emmision = GLColor.Transparent;
        public float shine = 0;

        public string textureName = string.Empty;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        Texture texture;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        DisplayList displayList = DisplayListSystem.system.newList();

        public Material()
        {}

        public Material(GLColor color)
        {
            baseColor = color;
        }

        public Material( Color color )
        {
            baseColor = new GLColor(color);
        }

        public Material( Material mat)
        {
            name = string.Copy(mat.name);
            baseColor = new GLColor(mat.baseColor);
            ambinent = new GLColor(mat.ambinent);
            specular = new GLColor(mat.specular);
            emmision = new GLColor(mat.emmision);
            shine = mat.shine;

            textureName = string.Copy(mat.textureName);
        }

        public void Invalidate()
        {
            if (texture != null)
                texture.Invalidate();
            displayList.Invalidate();
        }

        public void Generate()
        {
            Invalidate();

            if (texture != null)
                texture.Invalidate();

            if (textureName != string.Empty)
            {
                texture = TextureSystem.system.getTexture(textureName);
                //execute just so we are sure it has a list before we put it in another list
                texture.Execute();
            }
            else
                texture = null;

            displayList.Start();
            if (texture != null)
                texture.Execute();
            else
                GL.Disable(EnableCap.Texture2D);
            baseColor.glColor();
            displayList.End();
        }

        public void Execute()
        {
            if (texture == null || !texture.Valid() || !displayList.Valid())
                Generate();

            displayList.Call();
        }
    }

    public class MeshOverride
    {
        public string origonalMatName = string.Empty;
        public Material newMaterial = new Material();
        public List<string> hiddenGroups = new List<string>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public DisplayList displayList = DisplayListSystem.system.newList();

        public void Invalidate()
        {
            newMaterial.Invalidate();
            displayList.Invalidate();
        }

        public void LinkToSystem (MaterialSystem system)
        {
            newMaterial = system.getMaterial(newMaterial);
        }
    }

    public class MaterialOverride
    {
        public string name = string.Empty;
        public List<MeshOverride> materials = new List<MeshOverride>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        Dictionary<Material, MeshOverride> materialReplacements = new Dictionary<Material, MeshOverride>();

        public void LinkToSystem (MaterialSystem system)
        {
            foreach(MeshOverride m in materials)
                m.LinkToSystem(system);
        }

        public List<string> getHiddenGroups(Material mat)
        {
            if (materialReplacements.ContainsKey(mat))
                return materialReplacements[mat].hiddenGroups;

            MeshOverride m = findOverride(mat.name);
            if (m != null)
            {
                materialReplacements.Add(mat, m);
                return m.hiddenGroups;
            }

            return new List<string>();
        }

        public MeshOverride getOverride(Material mat)
        {
            if (materialReplacements.ContainsKey(mat))
                return materialReplacements[mat];
            else
            {
                foreach (MeshOverride m in materials)
                {
                    if (m.origonalMatName == mat.name)
                    {
                        materialReplacements[mat] = m;
                        return m;
                    }
                }

                MeshOverride mesh = new MeshOverride();
                mesh.origonalMatName = mat.name;
                materials.Add(mesh);
                materialReplacements[mat] = mesh;

                return mesh;
            }
        }

        public Material getMaterial(Material mat)
        {
            if (materialReplacements.ContainsKey(mat))
                return materialReplacements[mat].newMaterial;
            return mat;
        }

        public DisplayList getGeoList(Material mat)
        {
            if (materialReplacements.ContainsKey(mat))
                return materialReplacements[mat].displayList;
            return null;
        }

        public MeshOverride findOverride(string matName)
        {
            foreach (MeshOverride m in materials)
            {
                if (m.origonalMatName == matName)
                    return m;
            }

            return null;
        }

        public void hideGroup(string matName, string name)
        {
            MeshOverride ovd = findOverride(matName);
            if (ovd == null)
                return;
            if (!ovd.hiddenGroups.Contains(name))
                ovd.hiddenGroups.Add(name);
        }

        public void showGroup(string matName, string name)
        {
            MeshOverride ovd = findOverride(matName);
            if (ovd == null)
                return;

            if (ovd.hiddenGroups.Contains(name))
                ovd.hiddenGroups.Remove(name);
        }

        public void addMaterial(string matName, Material mat)
        {
            MeshOverride mesh = findOverride(matName);
            if (mesh == null)
                mesh = new MeshOverride();

            mesh.origonalMatName = matName;
            mat.Invalidate();
            if (mesh.newMaterial != null)
                mesh.newMaterial.Invalidate();

            mesh.newMaterial = new Material(mat);

            materials.Add(mesh);
        }

        public void removeMaterial(string name)
        {
            Invalidate();

            foreach (MeshOverride m in materials)
            {
                if (m.origonalMatName == name)
                {
                    materials.Remove(m);
                    break;
                }
            }

            foreach (KeyValuePair<Material, MeshOverride> m in materialReplacements)
            {
                if (m.Key.name == name)
                    materialReplacements.Remove(m.Key);
            }
        }

        public void Invalidate()
        {
            // kill the old materials
            foreach (KeyValuePair<Material, MeshOverride> m in materialReplacements)
                m.Key.Invalidate();
            materialReplacements.Clear();

            foreach (MeshOverride m in materials)
                m.Invalidate();
        }

        public void Execute(Material mat)
        {
            if (materialReplacements.ContainsKey(mat))
            {
                Material ovrd = materialReplacements[mat].newMaterial;
                if (ovrd != null)
                    ovrd.Execute();
                else
                    mat.Execute();
            }
            else
            {
                foreach (MeshOverride m in materials)
                {
                    if (m.origonalMatName == mat.name)
                    {
                        materialReplacements.Add(mat, m);
                        if (m != null)
                            m.newMaterial.Execute();
                        else
                            mat.Execute();
                        return;
                    }
                }
                mat.Execute();
            }
        }
    }

    public class MaterialSystem
    {
        public static MaterialSystem system = new MaterialSystem();

        List<Material> materials = new List<Material>();

        bool sameMat ( Material m1, Material m2 )
        {
            if (m1.name != m2.name)
                return false;

            if (m1.textureName != m2.name)
                return false;
            if (m1.baseColor != m2.baseColor)
                return false;
            if (m1.specular != m2.specular)
                return false;
            if (m1.ambinent != m2.ambinent)
                return false;
            if (m1.emmision != m2.emmision)
                return false;
             if (m1.shine != m2.shine)
                return false;
            return true;
        }

        public Material getMaterial ( Material mat )
        {
            foreach(Material m in materials)
            {
                if (sameMat(m, mat))
                    return m;
            }

            materials.Add(mat);
            return mat;
        }

        public Material getMaterial(string name)
        {
            foreach (Material m in materials)
            {
                if (m.name == name)
                    return m;
            }
            return null;
        }

        public Material newMaterial()
        {
            Material mat = new Material();
            materials.Add(mat);
            return mat;
        }

        public void Invalidate()
        {
            foreach(Material m in materials)
                m.Invalidate();
        }

        public void Flush()
        {
            foreach (Material m in materials)
                m.Invalidate();

            materials.Clear();
        }
    }

}
