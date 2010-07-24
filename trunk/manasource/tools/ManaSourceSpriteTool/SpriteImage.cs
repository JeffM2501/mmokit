using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;

using ManaSource.Sprites;

namespace ManaSourceSpriteTool
{
    public class SpriteImage
    {
        protected static Dictionary<string, SpriteImage> ImageFiles = new Dictionary<string, SpriteImage>();

        public delegate void FileReloadHandler ( SpriteImage image );
        public delegate string UnknownFileHandler(string lostFile, string goodFile);

        public static UnknownFileHandler LocateFile = null;

        public static event FileReloadHandler Reload;

        public static bool Exists ( string path )
        {
            return ImageFiles.ContainsKey(path);
        }

        public static SpriteImage Add ( string path, string imagefile )
        {
            if (!Exists(path))
                ImageFiles.Add(path, new SpriteImage(new FileInfo(imagefile)));
            return ImageFiles[path];
        }

        public static SpriteImage Get ( ImageSet imageset, string filepath )
        {
            SpriteImage sprite = null;

            if (ImageFiles.ContainsKey(imageset.ImageFile))
            {
                sprite = ImageFiles[imageset.ImageFile];
                if (imageset.Tag == null)
                    imageset.Tag = sprite;
            }
            else
            {
                // find the file
                FileInfo imagepath = new FileInfo(GetRelativePath(filepath, imageset.ImageFile));
                if (!imagepath.Exists)
                {
                    if (LocateFile != null)
                        imagepath = new FileInfo(LocateFile(imageset.ImageFile, filepath));

                    if (!imagepath.Exists)
                        return null;
                }

                sprite = new SpriteImage(imagepath);
                ImageFiles.Add(imageset.ImageFile, sprite);
            }
            return sprite;
        }

        protected static string GetRelativePath ( string filepath, string relpath )
        {
            string[] fileParts = Path.GetDirectoryName(filepath).Split(Path.DirectorySeparatorChar.ToString().ToCharArray());
            
            string path = string.Empty;
            foreach (string part in fileParts)
            {
                if (path == string.Empty)
                    path = part + Path.DirectorySeparatorChar;
                else
                {
                    path = Path.Combine(path, part);
                    if (File.Exists(Path.Combine(path, relpath)))
                        return Path.Combine(path, relpath);
                }
            }            
            return string.Empty;
        }


        protected FileSystemWatcher Watcher = null;
        protected Bitmap ImageMap = null;
        protected FileInfo ImageFile;

        public Image Image
        {
            get { return ImageMap; }
        }

        public string FilePath
        {
            get { return ImageFile.FullName; }
        }

        public SpriteImage ( FileInfo file )
        {
            Reseat(file);
        }

        public void Reseat ( FileInfo file )
        {
            ImageFile = file;
            if (Watcher != null)
                Watcher.Dispose();

            Watcher = new FileSystemWatcher(Path.GetDirectoryName(file.FullName), "*." + Path.GetExtension(file.FullName));
            Watcher.Changed += new FileSystemEventHandler(Watcher_Changed);
            Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            Watcher.EnableRaisingEvents = true;

            if (ImageMap != null)
                ImageMap.Dispose();

            ImageMap = new Bitmap(file.FullName);
        }

        void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed || e.FullPath != ImageFile.FullName)
                return;

            if (ImageMap != null)
                ImageMap.Dispose();
            
            ImageMap = new Bitmap(ImageFile.FullName);

            if (Reload != null)
                Reload(this);
        }

        static bool UseTempImages = false;
        public void DrawFrame ( Graphics graphics, ImageSet image, AnimationFrame frame, int index )
        {
            if (UseTempImages)
            {
                Image img = GetFrameImage(image, frame, index);
                graphics.DrawImage(img, frame.Offset);
                img.Dispose();
            }
            else
                DrawRawImage(graphics, image, frame, index, true);
        }

        public void DrawRawImage ( Graphics graphics, ImageSet image, AnimationFrame frame, int index, bool useOffset )
        {
            Rectangle destRect;
            if (useOffset)
                destRect = new Rectangle(-image.GridSize.Width / 2 + frame.Offset.X, -image.GridSize.Height + frame.Offset.Y, image.GridSize.Width, image.GridSize.Height);
            else
                destRect = new Rectangle(Point.Empty, image.GridSize);

            int rows = ImageMap.Height/image.GridSize.Height;
            int cols = ImageMap.Width/image.GridSize.Width;

            int gridIndex = frame.Frame(index);

            int row = gridIndex/cols;
            int col = gridIndex - (row*cols);

            Rectangle srcRect = new Rectangle(col*image.GridSize.Width,row*image.GridSize.Width,image.GridSize.Width,image.GridSize.Height);
            graphics.DrawImage(ImageMap,destRect,srcRect,GraphicsUnit.Pixel);
        }

        public Image GetFrameImage ( ImageSet image, AnimationFrame frame, int index )
        {
            Bitmap bitmap = new Bitmap(image.GridSize.Width, image.GridSize.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            DrawRawImage(graphics, image, frame, index, false);
            graphics.Dispose();
            return bitmap;
        }
    }
}
