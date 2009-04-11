using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK.Graphics;

namespace Drawables.Textures
{
    public class Texture
    {
        int listID = -1;
        int boundID = -1;

        public bool mipmap = true;

        FileInfo file = null;

        public Texture( FileInfo info )
        {
            file = info;
        }

        public bool Valid ()
        {
            return listID != -1;
        }

        public void Invalidate()
        {
            if (listID != -1)
                GL.DeleteLists(listID, 1);

            listID = -1;

            if(boundID != -1)
                GL.DeleteTexture(boundID);
            boundID = -1;
        }

        public void Execute()
        {
            // easy out, do this most of the time
            if (listID != -1)
            {
                if (boundID != -1)
                    GL.Enable(EnableCap.Texture2D);
                else
                    GL.Disable(EnableCap.Texture2D);
                GL.CallList(listID);
                return;
            }

            if (boundID == -1 || listID == -1)
                Invalidate(); // we know one is bad, so make sure all is free;

            if (file != null && file.Exists)
            {
                Bitmap bitmap = new Bitmap(file.FullName);

                GL.GenTextures(1, out boundID);
                GL.BindTexture(TextureTarget.Texture2D, boundID);

                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                if (mipmap)
                    Glu.Build2DMipmap(TextureTarget.Texture2D, (int)PixelInternalFormat.Rgba, data.Width, data.Height, OpenTK.Graphics.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                else
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }

            listID = GL.GenLists(1);
            GL.NewList(listID,ListMode.CompileAndExecute);
            if (boundID != -1)
                GL.BindTexture(TextureTarget.Texture2D, boundID);
            GL.EndList();
        }
    }

    public class TextureSystem
    {
        public static TextureSystem system = new TextureSystem();

        public DirectoryInfo rootDir;

        Dictionary<string, Texture> textures = new Dictionary<string,Texture>();

        public void Invalidate()
        {
            foreach(KeyValuePair<string,Texture> t in textures)
                t.Value.Invalidate();
        }

        public Texture getTexture(string path)
        {
            return getTexture(new FileInfo(path));
        }

        public Texture getTexture( FileInfo file )
        {
            if (textures.ContainsKey(file.FullName))
                return textures[file.FullName];

            Texture texture = null;
            if (!textureIsValid(file.FullName))
            {
                if (file.Exists)
                    texture = new Texture(file);
                else if (rootDir != null)
                {
                    FileInfo newFile = new FileInfo(Path.Combine(rootDir.FullName, file.FullName));
                    if (newFile.Exists)
                        texture = new Texture(newFile);
                }
            }
            if (texture == null)
                texture = new Texture(null);

            textures.Add(file.FullName, texture);

            return texture;
        }

        bool textureIsValid(string t)
        {
            if (t == string.Empty)
                return false;
            string extension = Path.GetExtension(t);
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".tiff")
                return false;

            return true;
        }

    }
}
