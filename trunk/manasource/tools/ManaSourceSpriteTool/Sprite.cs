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
        Up,
        Down,
        Left,
        Right,
        Any,
        Unknown,
    }

     public class Utils
     {
         public static Direction ParseDirection(string text)
         {
             string name = text.ToLower();
             if (name == "up")
                 return Direction.Up;
             if (name == "down")
                 return Direction.Down;
             if (name == "left")
                 return Direction.Left;
             if (name == "right")
                 return Direction.Right;
             if (name == "default")
                 return Direction.Any;

             return Direction.Unknown;
         }

         public static string DirectionToString( Direction dir )
         {
             if (dir == Direction.Any || dir == Direction.Unknown)
                 return "default";

             return dir.ToString().ToLower();
         }
        
     }
    public class AnimationFrame
    {
        public Point Offset = Point.Empty;
        public int StartFrame = -1;
        public int EndFrame = -1;
        public int Delay = 0;

        public int Frame ( int index )
        {
            if (StartFrame == EndFrame || EndFrame < 0 || index > Length)
                return StartFrame;

            return StartFrame + index;
        }

        public override string  ToString()
        {
            if (Length > 1)
                return StartFrame.ToString() + "->" + EndFrame.ToString() + ":" + Delay.ToString();

            if (Delay == 0)
                return StartFrame.ToString();

            return StartFrame.ToString() + ":" + Delay.ToString();
        }

        public int Length
        {
            get { if (StartFrame == EndFrame || EndFrame < 0) return 1; else return EndFrame - StartFrame + 1; }
        }
    }

    public class Animation
    {
        public Direction Direction = Direction.Down;
        public List<AnimationFrame> Frames = new List<AnimationFrame>();

        public int Count
        {
            get { return Frames.Count; }
        }

        public AnimationFrame Get ( int sequence )
        {
            if (sequence < 0 || sequence >= Count)
                return null;

            return Frames[sequence];
        }

        public int Length
        {
            get
            {
                int count = 0;
                foreach (AnimationFrame frame in Frames)
                    count += frame.Length;

                return count;
            }
        }

        public int Time
        {
            get
            {
                int count = 0;
                foreach (AnimationFrame frame in Frames)
                    count += frame.Length * frame.Delay;
                return count;
            }
        }
    }

    public class Action
    {
        public string   Name;
        public String   Imageset;
        public Dictionary<Direction, Animation> Animations = new Dictionary<Direction, Animation>();

        public Animation GetAnimation ( Direction dir )
        {
            if (Animations.ContainsKey(dir))
                return Animations[dir];

            if (Animations.ContainsKey(Direction.Any))
                return Animations[Direction.Any];
            return null;
        }
    }

    public class Sprite
    {
        public string Name = string.Empty;
        public string DefaultAction = string.Empty;

        public Dictionary<string, ImageSet> Imagesets = new Dictionary<string, ImageSet>();
        public Dictionary<string, Action> Actions = new Dictionary<string, Action>();

        public Action GetAction (string name)
        {
            if (Actions.ContainsKey(name))
                return Actions[name];

            return null;
        }
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

            foreach (XmlNode n in node.ChildNodes)
            {
                XmlElement child = n as XmlElement;
                if (child == null)
                    continue;

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

            foreach (XmlNode n in node.ChildNodes)
            {
                XmlElement child = n as XmlElement;
                if (child == null)
                    continue;

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
                        anim.Direction = Utils.ParseDirection(attrib.Value);
                }
                catch (System.Exception ex)
                {
                    int i = 0;
                    i++;
                }
            }

            foreach (XmlNode n in node.ChildNodes)
            {
                XmlElement child = n as XmlElement;
                if (child == null)
                    continue;

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

    public class XMLWriter
    {
        public XMLWriter( FileInfo file, Sprite sprite )
        {
            XmlDocument doc = new XmlDocument();

            doc.CreateXmlDeclaration("1.0", "ASCII", string.Empty);
            XmlElement RootElement = doc.CreateElement("sprite");
            doc.AppendChild(doc.CreateWhitespace("\r\n"));
            doc.AppendChild(RootElement);
            RootElement.AppendChild(doc.CreateWhitespace("\r\n"));

            RootElement.SetAttribute("name", sprite.Name);
            RootElement.SetAttribute("action", sprite.DefaultAction);

            WriteImageSets(doc, RootElement, sprite);
            WriteActions(doc, RootElement, sprite);

            if (doc.ChildNodes.Count == 0)
                return;

            if (file.Exists)
                file.Delete();
            Stream outStream = file.OpenWrite();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outStream);
            doc.WriteTo(writer);
            writer.Close();
            outStream.Close();
        }

        protected void WriteImageSets ( XmlDocument doc, XmlElement root, Sprite sprite )
        {
            foreach (KeyValuePair<string,ImageSet> imageset in sprite.Imagesets)
            {
                XmlElement node = doc.CreateElement("imageset");

                node.SetAttribute("name", imageset.Value.Name);
                string source = imageset.Value.ImageFile;
                if (imageset.Value.DieString != string.Empty)
                    source += "|" + imageset.Value.DieString;

                node.SetAttribute("src", source);

                node.SetAttribute("width", imageset.Value.GridSize.Width.ToString());
                node.SetAttribute("height", imageset.Value.GridSize.Height.ToString());

                root.AppendChild(node);
                root.AppendChild(doc.CreateWhitespace("\r\n"));
            }
        }

        protected void WriteActions ( XmlDocument doc, XmlElement root, Sprite sprite )
        {
            foreach (KeyValuePair<string,Action> action in sprite.Actions)
            {
                XmlElement node = doc.CreateElement("action");
                node.SetAttribute("name", action.Value.Name);
                node.SetAttribute("imageset", action.Value.Imageset);
                node.AppendChild(doc.CreateWhitespace("\r\n"));

                foreach (KeyValuePair<Direction,Animation> anim in action.Value.Animations)
                    WriteAnimation(doc, node, anim.Value);
                root.AppendChild(node);
                root.AppendChild(doc.CreateWhitespace("\r\n"));
            }
        }

        protected void WriteAnimation ( XmlDocument doc, XmlElement root, Animation anim )
        {
            XmlElement node = doc.CreateElement("animation");
            node.SetAttribute("direction", Utils.DirectionToString(anim.Direction));
            node.AppendChild(doc.CreateWhitespace("\r\n"));

            foreach (AnimationFrame frame in anim.Frames)
                WriteAnimationFrame(doc, node, frame);
            root.AppendChild(node);
            root.AppendChild(doc.CreateWhitespace("\r\n"));
        }

        protected void WriteAnimationFrame(XmlDocument doc, XmlElement root, AnimationFrame frame)
        {
            XmlElement node;
            if (frame.Length > 1)
            {
                node = doc.CreateElement("sequence");
                node.SetAttribute("start", frame.StartFrame.ToString());
                node.SetAttribute("end", frame.EndFrame.ToString());
            }
            else
            {
                node = doc.CreateElement("frame");
                node.SetAttribute("index", frame.StartFrame.ToString());
            }
            if (frame.Delay > 0)
                node.SetAttribute("delay", frame.Delay.ToString());

            root.AppendChild(node);
            root.AppendChild(doc.CreateWhitespace("\r\n"));
        }
    }
}
