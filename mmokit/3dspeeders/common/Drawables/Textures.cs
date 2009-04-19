using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK.Graphics;
using Drawables.DisplayLists;

namespace Drawables.Textures
{
    public class Texture
    {
        DisplayList listID = new DisplayList();
        int boundID = -1;

        public bool mipmap = true;

        FileInfo file = null;

        public Texture( FileInfo info )
        {
            file = info;
        }

        public bool Valid ()
        {
            return listID.Valid();
        }

        public void Invalidate()
        {
            listID.Invalidate();

            if(boundID != -1)
                GL.DeleteTexture(boundID);
            boundID = -1;
        }

        public void Execute()
        {
            // easy out, do this most of the time
            if (listID.Valid())
            {
                if (boundID != -1)
                    GL.Enable(EnableCap.Texture2D);
                else
                    GL.Disable(EnableCap.Texture2D);
                listID.Call();
                return;
            }

            if (boundID == -1 || !listID.Valid())
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

            listID.Start(true);
            if (boundID != -1)
                GL.BindTexture(TextureTarget.Texture2D, boundID);
            listID.End();
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
            if (textures.ContainsKey(path))
                return textures[path];

            Texture texture = null;
            if (textureIsValid(path))
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                    texture = new Texture(file);
                else if (rootDir != null)
                {
                    file = new FileInfo(Path.Combine(rootDir.FullName, path));
                    if (file.Exists)
                        texture = new Texture(file);
                }
            }
            if (texture == null)
                texture = new Texture(null);

            textures.Add(path, texture);

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
