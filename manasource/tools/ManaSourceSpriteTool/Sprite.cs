using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;
using System.IO;

namespace ManaSource.Sprites
{
    public class ImageSet
    {
        public string Name = string.Empty;
        public string ImageFile = string.Empty;
        public Size GridSize = Size.Empty;
        public string DieString = string.Empty;
        public object Tag = null;
    }

    public enum Direction
    {
        up,
        down,
        left,
        right,
        unknown,
    }

    public class AnimationFrame
    {
        public Point Offset = Point.Empty;
        public int StartFrame = -1;
        public int EndFrame = -1;
        public int Delay = -1;

        public int Frame ( int index )
        {
            if (StartFrame == EndFrame || EndFrame < 0 || index > Length)
                return StartFrame;

            return StartFrame + index;
        }

        public int Length
        {
            get { if (StartFrame == EndFrame || EndFrame < 0) return 1; else return EndFrame - StartFrame + 1; }
        }
    }

    public class Animation
    {
        public Direction Direction = Direction.down;
        public List<AnimationFrame> Frames = new List<AnimationFrame>();
    }

    public class Action
    {
        public string   Name;
        public String   Imageset;
        public Dictionary<Direction, Animation> Animations = new Dictionary<Direction, Animation>();
    }

    public class Sprite
    {
        public string Name = string.Empty;
        public string DefaultAction = string.Empty;

        public Dictionary<string, ImageSet> Imagesets = new Dictionary<string, ImageSet>();
        public Dictionary<string, Action> Actions = new Dictionary<string, Action>();
    }

    public class XMLReader
    {
        public List<Sprite> Sprites = new List<Sprite>();
        protected FileInfo thisFile;

        public FileInfo File
        {
            get { return thisFile; }
        }

        public XMLReader(FileInfo file)
        {
            thisFile = file;
            XmlDocument doc = new XmlDocument();
            doc.Load(file.FullName);

            XmlElement root = doc.DocumentElement;
            if (root.Name == "sprite")
                Sprites.Add(ProcessSprite(root));
            else
            {
                foreach (XmlElement child in root.ChildNodes)
                {
                    if (child.Name == "sprite")
                        Sprites.Add(ProcessSprite(child));
                }
            }
        }

        public Sprite ProcessSprite(XmlElement node)
        {
            Sprite sprite = new Sprite();
            foreach (XmlAttribute attrib in node.Attributes)
            {
                if (attrib.Name == "name")
                    sprite.Name = attrib.Value;
                if (attrib.Name == "action")
                    sprite.DefaultAction = attrib.Value;
            }

            foreach (XmlElement child in node)
            {
                if (child.Name == "imageset")
                {
                    ImageSet imageset = ProcessImageSet(child);
                    sprite.Imagesets.Add(imageset.Name, imageset);
                }
                if (child.Name == "action")
                {
                    Action action = ProcessAction(child);
                    sprite.Actions.Add(action.Name, action);
                }
                if (child.Name == "include")
                {
                    string file = child.GetAttribute("file");
                    if (file == string.Empty)
                        continue;

                    FileInfo includeFile = new FileInfo(Path.Combine(Path.GetDirectoryName(thisFile.FullName), file));
                    if (includeFile.Exists)
                    {
                        XMLReader reader = new XMLReader(includeFile);
                        if (reader.Sprites.Count > 0)
                        {
                            reader.Sprites[0].Imagesets = sprite.Imagesets;
                            sprite = reader.Sprites[0];
                        }
                    }
                }
            }
            return sprite;
        }

        public ImageSet ProcessImageSet(XmlElement node)
        {
            ImageSet imageset = new ImageSet();

            int x = 0;
            int y = 0;
            foreach (XmlAttribute attrib in node.Attributes)
            {
                if (attrib.Name == "name")
                    imageset.Name = attrib.Value;
                if (attrib.Name == "src")
                {
                    if (attrib.Value.Contains("|"))
                    {
                        string[] nugs = attrib.Value.Split("|".ToCharArray());
                        imageset.ImageFile = nugs[0];
                        if (nugs.Length > 1)
                            imageset.DieString = nugs[1];
                    }
                    else
                        imageset.ImageFile = attrib.Value;
                }
                if (attrib.Name == "width")
                    x = int.Parse(attrib.Value);
                if (attrib.Name == "height")
                    y = int.Parse(attrib.Value);
            }

            imageset.GridSize = new Size(x, y);

            return imageset;
        }

        public Action ProcessAction(XmlElement node)
        {
            Action action = new Action();
            foreach (XmlAttribute attrib in node.Attributes)
            {
                if (attrib.Name == "name")
                    action.Name = attrib.Value;
                if (attrib.Name == "imageset")
                    action.Imageset = attrib.Value;
            }

            foreach (XmlElement child in node.ChildNodes)
            {
                if (child.Name == "animation")
                {
                    Animation anim = ProcessAnimation(child);
                    action.Animations.Add(anim.Direction, anim);
                }
            }

            return action;
        }

        public Animation ProcessAnimation(XmlElement node)
        {
            Animation anim = new Animation();
            foreach (XmlAttribute attrib in node.Attributes)
            {
                try
                {
                    if (attrib.Name == "direction")
                        anim.Direction = (Direction)Enum.Parse(typeof(Direction), attrib.Value.ToLower());
                }
                catch (System.Exception ex)
                {
                    int i = 0;
                    i++;
                }
            }

            foreach (XmlElement child in node.ChildNodes)
            {
                if (child.Name == "sequence" || child.Name == "frame")
                {
                    AnimationFrame frame = new AnimationFrame();

                    foreach (XmlAttribute attrib in child.Attributes)
                    {
                        if (attrib.Name == "start" || attrib.Name == "index")
                            frame.StartFrame = int.Parse(attrib.Value);
                        else if (attrib.Name == "end")
                            frame.EndFrame = int.Parse(attrib.Value);
                        else if (attrib.Name == "delay")
                            frame.Delay = int.Parse(attrib.Value);
                        else if (attrib.Name == "offsetX")
                            frame.Offset = new Point(int.Parse(attrib.Value), frame.Offset.Y);
                        else if (attrib.Name == "offsetY")
                            frame.Offset = new Point(frame.Offset.X, int.Parse(attrib.Value));
                    }
                    if (child.Name == "frame")
                        frame.EndFrame = frame.StartFrame;

                    anim.Frames.Add(frame);
                }
            }
            return anim;
        }
    }
}
