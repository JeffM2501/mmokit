using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ManaSource.Sprites;

namespace ManaSourceSpriteTool
{
    public class SpriteLayer
    {
        public Sprite Sprite;
        public FileInfo XMLFile;
        public bool Visable = true;
        public String Name = string.Empty;

        public SpriteLayer ( Sprite sprite, FileInfo file )
        {
            XMLFile = file;
            Sprite = sprite;

            foreach (KeyValuePair<string,ImageSet> imageSet in sprite.Imagesets)
            {
                SpriteImage img = SpriteImage.Get(imageSet.Value, file.FullName);
                imageSet.Value.Tag = img;
            }
        }
    }

    public class SpriteDoc
    {
        public List<SpriteLayer> Layers = new List<SpriteLayer>();
        public List<String> Actions = new List<String>();


    }
}
