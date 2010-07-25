using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ManaSource.Sprites;

namespace ManaSourceSpriteTool
{
    public class BToolMenuPlugin
    {
        public virtual string MenuName() { return string.Empty; }
        public virtual void Execute(SpriteDoc document) { return; }
    }

    public class SpriteLayer
    {
        public Sprite LayerSprite;
        public FileInfo XMLFile;
        public bool Visable = true;
        public String Name = string.Empty;

        public int CurrentSequence = 0;
        public int CurrentIndex = 0;

        public SpriteLayer ( Sprite sprite, FileInfo file )
        {
            XMLFile = file;
            LayerSprite = sprite;

            Name = Path.GetFileNameWithoutExtension(XMLFile.Name);

            foreach (KeyValuePair<string,ImageSet> imageSet in sprite.Imagesets)
            {
                SpriteImage img = SpriteImage.Get(imageSet.Value, file.FullName);
                imageSet.Value.Tag = img;
            }
        }

        public SpriteLayer(Sprite sprite, string xmlPath)
        {
            XMLFile = new FileInfo(xmlPath);
            LayerSprite = sprite;

            Name = Path.GetFileNameWithoutExtension(xmlPath);
        }
    }

    public class SpriteDoc
    {
        public List<SpriteLayer> Layers = new List<SpriteLayer>();
       // public List<String> Actions = new List<String>();
    }
}
