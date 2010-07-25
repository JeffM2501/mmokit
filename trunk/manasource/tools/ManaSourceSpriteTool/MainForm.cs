using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;

using System.Reflection;

namespace ManaSourceSpriteTool
{
    public partial class MainForm : Form
    {
        protected int ImageZoom = 4;
        protected Color TenPixColor = Color.FromArgb(128, Color.White);
        protected Color PixColor = Color.FromArgb(128, Color.Gray);

        protected Color OriginColor = Color.FromArgb(196, Color.Red);

        protected Color DirButtonBackColor;

        protected SpriteDoc Doc = new SpriteDoc();

        protected string CurrentAction = "stand";
        protected ManaSource.Sprites.Direction CurrentDirection = ManaSource.Sprites.Direction.Down;
       
        protected int CurrentStep = 0;

        protected bool NoSelect = false;

        protected Point DrawOffset = new Point(64, 48);

        protected Point LastMousePos = Point.Empty;
        protected Point LastDragLoc =Point.Empty;

        public MainForm()
        {
            InitializeComponent();

            // load plugins
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"plugins"));
            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles("*.dll"))
                {
                    try
                    {
                        Assembly plugin = Assembly.LoadFile(file.FullName);
                        if (plugin != null)
                        {
                            foreach (Type t in plugin.GetTypes())
                            {
                                if (t.IsAbstract)
                                    continue;

                                if (t.IsSubclassOf(typeof(BToolMenuPlugin)))
                                {
                                    BToolMenuPlugin p = (BToolMenuPlugin)Activator.CreateInstance(t);
                                    if (p != null)
                                    {
                                        ToolStripMenuItem item = new ToolStripMenuItem(p.MenuName());
                                        item.Click +=new EventHandler(item_Click);
                                        item.Tag = p;
                                        ToolsRootMenu.DropDownItems.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception /*ex*/)
                    {
                    	
                    }
                }
            }

            SpriteImage.LocateFile = new SpriteImage.UnknownFileHandler(FindSpriteImage);
            SpriteImage.Reload += new SpriteImage.FileReloadHandler(SpriteImage_Reload);

            LayerList_SelectedIndexChanged(this, EventArgs.Empty);
            DirButtonBackColor = UpButton.BackColor;
            SetDirChecks();
            SetToolTips();
            recenterToolStripMenuItem_Click(this, EventArgs.Empty);
            SetZoomCheck();
        }

        void item_Click(object sender, EventArgs e)
        {
            if (sender as ToolStripMenuItem != null)
            {
                if ((sender as ToolStripMenuItem).Tag as BToolMenuPlugin != null)
                {
                    ((sender as ToolStripMenuItem).Tag as BToolMenuPlugin).Execute(Doc);
                }
            }
        }

        protected void SetToolTips ()
        {
            toolTip1.SetToolTip(MainView, "Main graphics view, right drag to move");

            toolTip1.SetToolTip(LayerUp, "Move selected layer up one level");
            toolTip1.SetToolTip(LayerDown, "Move selected layer down one level");
            toolTip1.SetToolTip(SaveLayer, "Save selected layer XML");
            toolTip1.SetToolTip(HideLayer, "Show/Hide selected layer form main view");
            toolTip1.SetToolTip(AddLayer, "Load layer from existing XML");
            toolTip1.SetToolTip(NewLayer, "New layer");
            toolTip1.SetToolTip(LayerInfo, "Edit layer");
            toolTip1.SetToolTip(RemoveLayer, "Remove selected layer");
            toolTip1.SetToolTip(HideLayer, "Show/Hide selected layer form main view");

            toolTip1.SetToolTip(XOffset, "Offset in X for current animation for current segment");
            toolTip1.SetToolTip(YOffset, "Offset in Y for current animation for current segment");

            toolTip1.SetToolTip(FrameDelay, "Delay for frame segment(ms)");
            toolTip1.SetToolTip(FrameStartIndex, "Start frame for an animation segment");
            toolTip1.SetToolTip(FrameEndIndex, "End frame for an animation segment");
            toolTip1.SetToolTip(AnimIsFrame, "Animation segment is a single frame");
            toolTip1.SetToolTip(AnimIsSeq, "Animation segment is a sequence of frames");

            toolTip1.SetToolTip(StepForwardButton, "Step to next frame in animation");
            toolTip1.SetToolTip(IndexNumber, "Step for current layer animation");
            toolTip1.SetToolTip(SequenceNumber, "Sequence number for current layer animation");
            toolTip1.SetToolTip(GridIndex, "Image grid index for current layer animation frame");

            toolTip1.SetToolTip(SequenceList, "Animation sequences/frames for current layer");
            toolTip1.SetToolTip(AddSequence, "Add a new animation segment for current layer/direction");
            toolTip1.SetToolTip(RemoveSequence, "Remove selected animation segment");

        }

        void SpriteImage_Reload(SpriteImage image)
        {
            Redraw();
        }

        protected string FindSpriteImage(string lostFile, string goodFile)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Image|*.PNG|All Files|*.*";
            ofd.FileName = Path.GetFileName(lostFile);
            ofd.InitialDirectory = Path.GetDirectoryName(goodFile);

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                return ofd.FileName;
            }
            return lostFile;
        }

        public void Redraw ()
        {
            MainView.Invalidate();
        }

        protected void SelectLayer ( SpriteLayer layer )
        {
            NoSelect = true;
            foreach (ListViewItem item in LayerList.Items)
                item.Selected = item.Tag as SpriteLayer == layer;

            NoSelect = false;
            LayerList_SelectedIndexChanged(this, EventArgs.Empty);
        }

        protected SpriteLayer SelectedLayer()
        {
            if (LayerList.SelectedItems.Count == 0)
                return null;

            return LayerList.SelectedItems[0].Tag as SpriteLayer;
        }

        protected void BuildLayerList ()
        {
            NoSelect = true;
            SpriteLayer selectedLayer = null;
            if (LayerList.SelectedItems.Count > 0)
                selectedLayer = LayerList.SelectedItems[0].Tag as SpriteLayer;

            LayerList.Items.Clear();
            LayerListImages.Images.Clear();

            SpriteLayer layer;

            for ( int i = Doc.Layers.Count-1; i >= 0; i-- )
            {
                layer = Doc.Layers[i];
                Image img = GetCurrrentFameAsImage(layer);
                LayerListImages.Images.Add(img);
            }

            int  j = 0;
            for ( int i = Doc.Layers.Count-1; i >= 0; i-- )
            {
                layer = Doc.Layers[i];
                ListViewItem item = new ListViewItem(layer.Name, j);
                if (layer == selectedLayer)
                    item.Selected = true;
                item.Tag = layer;
                LayerList.Items.Add(item);
                j++;
            }
            NoSelect = false;
            LayerList_SelectedIndexChanged(this, EventArgs.Empty);
        }

        protected void BuildActionList ()
        {
            NoSelect = true;

            ActionList.Items.Clear();

            int selID = 0;
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
            {
                NoSelect = false;
                return;
            }

            foreach (KeyValuePair<string,ManaSource.Sprites.Action> action in layer.LayerSprite.Actions)
            {
                int id = ActionList.Items.Add(action.Key);
                if (action.Key == CurrentAction)
                    selID = id;
            }

            ActionList.SelectedIndex = selID;

            NoSelect = false;
        }

        protected ManaSource.Sprites.Action GetCurrentAction(SpriteLayer layer)
        {
            if (layer == null)
                return null;
            return layer.LayerSprite.GetAction(CurrentAction);
        }

        protected ManaSource.Sprites.ImageSet GetCurrentImageset(SpriteLayer layer)
        {
            ManaSource.Sprites.Action action = GetCurrentAction(layer);
            if (action == null)
                return null;

            if (layer.LayerSprite.Imagesets.ContainsKey(action.Imageset))
                return layer.LayerSprite.Imagesets[action.Imageset];

            return null;
        }

        protected ManaSource.Sprites.Animation GetCurrentAnimation(SpriteLayer layer)
        {
            ManaSource.Sprites.Action action = GetCurrentAction(layer);
            if (action == null)
                return null;

            return action.GetAnimation(CurrentDirection);
        }

        protected Image GetCurrrentFameAsImage(SpriteLayer layer)
        {
            ManaSource.Sprites.Action action = GetCurrentAction(layer);
            ManaSource.Sprites.ImageSet imageSet = GetCurrentImageset(layer);
            ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);

            if (action == null)
            {
                foreach (KeyValuePair<string,ManaSource.Sprites.Action> a in layer.LayerSprite.Actions)
                {
                    action = a.Value;
                    break;
                }
            }

            if (imageSet == null)
            {
                foreach (KeyValuePair<string,ManaSource.Sprites.ImageSet> i in layer.LayerSprite.Imagesets)
                {
                    imageSet = i.Value;
                    break;
                }
            }

            if (anim == null)
            {
                anim = action.GetAnimation(ManaSource.Sprites.Direction.Up);
                if (anim == null)
                    return new Bitmap(32,32);
            }

            SpriteImage img = imageSet.Tag as SpriteImage;
            if (img != null)
                return img.GetFrameImage(imageSet, anim.Frames[layer.CurrentSequence], layer.CurrentIndex);
            return null;
        }

        private void MainView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.CornflowerBlue);
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.ScaleTransform(ImageZoom, ImageZoom);
            // draw the shit

            e.Graphics.TranslateTransform(DrawOffset.X, DrawOffset.Y);

            Pen OriginPen = new Pen(OriginColor);
            int originSize = 64;

            e.Graphics.DrawLine(OriginPen, 0, -originSize, 0, originSize);
            e.Graphics.DrawLine(OriginPen, originSize, 0, -originSize, 0);

            foreach (SpriteLayer layer in Doc.Layers)
            {
                if (layer.Visable)
                {
                    ManaSource.Sprites.Action action = GetCurrentAction(layer);
                    ManaSource.Sprites.ImageSet imageSet = GetCurrentImageset(layer);
                    ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);

                    if (action == null || imageSet == null || anim == null)
                        continue;

                    SpriteImage img = imageSet.Tag as SpriteImage;
                    if (img != null)
                        img.DrawFrame(e.Graphics, imageSet, anim.Frames[layer.CurrentSequence], layer.CurrentIndex);
                }
            }

            e.Graphics.ResetTransform();

            Pen TenPen = new Pen(TenPixColor);
            Pen PixPen = new Pen(PixColor);

            for ( int y = 0; y < MainView.Height; y++ )
            {
                int realPix = y / ImageZoom;
                if ( y == realPix * ImageZoom)
                {
                    if (realPix % 10 == 0)
                        e.Graphics.DrawLine(TenPen,0,y,MainView.Width,y);
                    else if ( ImageZoom > 2)
                        e.Graphics.DrawLine(PixPen,0,y,MainView.Width,y);
                }
            } 

            for ( int x = 0; x < MainView.Width; x++ )
            {
                int realPix = x / ImageZoom;
                if (x == realPix * ImageZoom)
                {
                    if (realPix % 10 == 0)
                        e.Graphics.DrawLine(TenPen, x, 0, x, MainView.Height);
                    else if (ImageZoom > 2)
                        e.Graphics.DrawLine(PixPen, x, 0, x, MainView.Height);
                }
            }

            TenPen.Dispose();
            PixPen.Dispose();
        }

        private void AddLayer_Click(object sender, EventArgs e)
        {
            NoSelect = true;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Document|*.xml|All Files|*.*";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                ManaSource.Sprites.XMLReader reader = new ManaSource.Sprites.XMLReader(new FileInfo(ofd.FileName));
                if (reader.Sprites.Count > 0)
                {
                    foreach (ManaSource.Sprites.Sprite sprite in reader.Sprites)
                    {
                        Doc.Layers.Add(new SpriteLayer(sprite,reader.File));

                    }
                    SelectLayer(Doc.Layers[Doc.Layers.Count - 1]);
                    BuildLayerList();
                    BuildActionList();
                    UpdateImageSet();
                    UpdateFrameData();
                    UpdateAnimations();
                    CheckCurrentFrame();
                    SelectLayer(Doc.Layers[Doc.Layers.Count - 1]);
                    Redraw();
                }
            }
            NoSelect = false;
        }

        private void RemoveLayer_Click(object sender, EventArgs e)
        {
            NoSelect = true;
            SpriteLayer layer = SelectedLayer();
            if (layer != null)
            {
                Doc.Layers.Remove(layer);
                BuildActionList();
                BuildLayerList();
                UpdateFrameData();
                UpdateAnimations();
                CheckCurrentFrame();
                Redraw();
            }
            NoSelect = false;
        }

        private void MainView_Resize(object sender, EventArgs e)
        {
            Redraw();
        }

        private void LayerUp_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = LayerList.SelectedItems[0].Tag as SpriteLayer;
            if (layer != null)
            {
                int index = Doc.Layers.IndexOf(layer);
                if (index != Doc.Layers.Count-1)
                {
                    Doc.Layers.Remove(layer);
                    Doc.Layers.Insert(index + 1, layer);
                    BuildLayerList();
                    SelectLayer(layer);
                    CheckCurrentFrame();
                    UpdateFrameData();
                    UpdateAnimations();
                    Redraw();
                }
            }
        }

        private void LayerDown_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = LayerList.SelectedItems[0].Tag as SpriteLayer;
            if (layer != null)
            {
                int index = Doc.Layers.IndexOf(layer);
                if (index != 0)
                {
                    Doc.Layers.Remove(layer);
                    Doc.Layers.Insert(index - 1, layer);
                    BuildLayerList();
                    SelectLayer(layer);
                    CheckCurrentFrame();
                    UpdateFrameData();
                    UpdateAnimations();
                    Redraw();
                }
            }
        }

        private void LayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoSelect)
                return;
            bool enable = LayerList.SelectedIndices.Count > 0;
            LayerUp.Enabled = enable;
            LayerDown.Enabled = enable;
            LayerInfo.Enabled = enable;
            HideLayer.Enabled = enable;
            RemoveLayer.Enabled = enable;
            SaveLayer.Enabled = enable;

            BuildActionList();
            CheckCurrentFrame();
            UpdateFrameData();
            UpdateImageSet();
            UpdateAnimations();

            Redraw();
        }

        protected void UpdateSequenceData()
        {
            SequenceNumber.Text = string.Empty;
            IndexNumber.Text = string.Empty;
            LayerLabel.Text = string.Empty;
            GridIndex.Text = "";

            SpriteLayer layer = SelectedLayer();
            if (layer != null)
            {
                LayerLabel.Text = layer.Name;

                SequenceNumber.Text = layer.CurrentSequence.ToString();
                IndexNumber.Text = layer.CurrentIndex.ToString();
                ManaSource.Sprites.AnimationFrame frame = GetCurrentFrame(layer);
                if (frame != null)
                    GridIndex.Text = frame.Frame(layer.CurrentIndex).ToString();
            }
        }

       protected void UpdateFrameData ()
        {
            NoSelect = true;
            SpriteLayer layer = SelectedLayer();

            UpdateSequenceData();

            XOffset.Value = 0;
            YOffset.Value = 0;
            FrameDataGroup.Enabled = false;
            XOffset.Enabled = false;
            YOffset.Enabled = false;
            PlayControllsPanel.Enabled = false;

            if (layer != null)
            {
                ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);
                if (anim != null)
                {
                    ManaSource.Sprites.AnimationFrame frame = anim.Frames[layer.CurrentSequence];
                    if (frame != null)
                    {
                        PlayControllsPanel.Enabled = true;
                        XOffset.Enabled = true;
                        YOffset.Enabled = true;
                        XOffset.Value = anim.Frames[layer.CurrentSequence].Offset.X;
                        YOffset.Value = anim.Frames[layer.CurrentSequence].Offset.Y;

                        FrameDataGroup.Enabled = true;

                        AnimIsFrame.Checked = frame.Length == 1;
                        AnimIsSeq.Checked = frame.Length > 1;
                        FrameStartIndex.Value = frame.StartFrame;
                        if (frame.Length == 1)
                            FrameEndIndex.Enabled = false;
                        else
                        {
                            FrameEndIndex.Enabled = true;
                            FrameEndIndex.Value = frame.EndFrame;
                        }

                        FrameDelay.Value = frame.Delay;
                    }
                }
            }
       
           NoSelect = false;
        }

        private void MainView_MouseDown(object sender, MouseEventArgs e)
        {
            LastMousePos = new Point(e.Location.X, e.Location.Y);
            LastDragLoc = new Point(e.Location.X, e.Location.Y);
        }

        private void MainView_MouseMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - LastMousePos.X;
            int dy = e.Y - LastMousePos.Y;

            if (e.Button == MouseButtons.Right && !NoSelect)
            {
                if ((e.X - LastDragLoc.X) / ImageZoom != 0)
                {
                    DrawOffset.X += (e.X - LastDragLoc.X) / ImageZoom;
                    LastDragLoc.X = e.X;
                }
                if ((e.Y - LastDragLoc.Y) / ImageZoom != 0)
                {
                    DrawOffset.Y += (e.Y - LastDragLoc.Y) / ImageZoom;
                    LastDragLoc.Y = e.Y;
                }
                Redraw();
            }

            LastMousePos = new Point(e.Location.X, e.Location.Y);
        }

        private void MainView_MouseUp(object sender, MouseEventArgs e)
        {
            LastMousePos = new Point(e.Location.X, e.Location.Y);
        }

        protected Color GetDirButtonColor ( ManaSource.Sprites.Direction dir )
        {
            if (dir == CurrentDirection)
                return Color.WhiteSmoke;
            return DirButtonBackColor;
        }

        private void SetDirChecks ()
        {
            UpButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.Up);
            DownButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.Down);
            LeftButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.Left);
            RightButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.Right);
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.Up;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();

            Redraw();
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.Left;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();
            Redraw();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.Down;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();
            Redraw();
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.Right;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();
            Redraw();
        }

        private void AnyButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.Any;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();
            Redraw();
        }

        private void ActionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoSelect)
                return;

            CurrentAction = ActionList.SelectedItem.ToString();
            UpdateFrameData();
            UpdateImageSet();
            UpdateAnimations();
            Redraw();
        }

        private void UpdateImageSet ()
        {
            ImageSetList.Items.Clear();
            SpriteLayer layer = SelectedLayer();
            ManaSource.Sprites.Action action = GetCurrentAction(layer);
            if (action == null)
                return;

            NoSelect = true;
            foreach (KeyValuePair<string, ManaSource.Sprites.ImageSet> imageset in layer.LayerSprite.Imagesets)
                ImageSetList.Items.Add(imageset.Value.Name);

            ImageSetList.SelectedItem = action.Imageset;
            NoSelect = false;
        }

        protected void UpdateAnimations ()
        {
            SequenceList.Items.Clear();
            ManaSource.Sprites.Animation anim = GetCurrentAnimation(SelectedLayer());
            if (anim == null)
                return;

            NoSelect = true;

            foreach (ManaSource.Sprites.AnimationFrame frame in anim.Frames)
            {
                int id = 0;
                if (frame.Length > 1)
                    id = 1;

                ListViewItem item = new ListViewItem(frame.ToString(), id);
                SequenceList.Items.Add(item).Tag = frame;
            }


            NoSelect = false;
        }

        private void ActionList_TextUpdate(object sender, EventArgs e)
        {

        }

        protected void SetZoomCheck ()
        {
            Zoom_1x.Checked = ImageZoom == 1;
            Zoom_2x.Checked = ImageZoom == 2;
            Zoom_3x.Checked = ImageZoom == 3;
            Zoom_4x.Checked = ImageZoom == 4;
            Zoom_5x.Checked = ImageZoom == 5;
            Zoom_6x.Checked = ImageZoom == 6;
            Zoom_7x.Checked = ImageZoom == 7;
            Zoom_8x.Checked = ImageZoom == 8;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ImageZoom = 1;
            SetZoomCheck();
            Redraw();
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageZoom = 2;
            SetZoomCheck();
            Redraw();
        }

        private void xToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ImageZoom = 3;
            SetZoomCheck();
            Redraw();
        }

        private void xToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ImageZoom = 4;
            SetZoomCheck();
            Redraw();
        }

        private void xToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ImageZoom = 5;
            SetZoomCheck();
            Redraw();
        }

        private void xToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ImageZoom = 6;
            SetZoomCheck();
            Redraw();
        }

        private void xToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            ImageZoom = 7;
            SetZoomCheck();
            Redraw();
        }

        private void xToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ImageZoom = 8;
            SetZoomCheck();
            Redraw();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void XOffset_ValueChanged(object sender, EventArgs e)
        {
            if (NoSelect)
                return;

            SpriteLayer layer = SelectedLayer();
            ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);

            if (anim == null || anim.Frames.Count <= layer.CurrentSequence)
                return;

            anim.Frames[layer.CurrentSequence].Offset = new Point((int)XOffset.Value, (int)YOffset.Value);
            Redraw();
        }

        private void HideLayer_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            layer.Visable = !layer.Visable;
            Redraw();
        }

        private void SaveLayer_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            new ManaSource.Sprites.XMLWriter(layer.XMLFile, layer.LayerSprite);
        }


        private void saveAllLayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (SpriteLayer layer in Doc.Layers)
                new ManaSource.Sprites.XMLWriter(layer.XMLFile, layer.LayerSprite);
        }

        private void CheckCurrentFrame ()
        {
            // find the longest frame time
            int time = 0;
            foreach (SpriteLayer layer in Doc.Layers)
            {
                ManaSource.Sprites.Action action = layer.LayerSprite.GetAction(CurrentAction);
                if (action != null)
                {
                    ManaSource.Sprites.Animation anim = action.GetAnimation(CurrentDirection);

                    if (anim != null)
                    {
                        if (anim.Length > time)
                            time = anim.Length;
                    }
                }
            }

            if (CurrentStep >= time)
                SetAllLayersToSetpZero();
        }

        protected void SetAllLayersToSetpZero ()
        {
            CurrentStep = 0;
            foreach (SpriteLayer layer in Doc.Layers)
            {
                layer.CurrentSequence = 0;
                layer.CurrentIndex = 0;
            }
        }

        protected void SetAllLayersToCurrentStep ()
        {
            foreach (SpriteLayer layer in Doc.Layers)
            {
                ManaSource.Sprites.Action action = layer.LayerSprite.GetAction(CurrentAction);
                if (action != null)
                {
                    layer.CurrentSequence = -1;
                    layer.CurrentIndex = -1;

                    int count = 0;
                    foreach(KeyValuePair<ManaSource.Sprites.Direction, ManaSource.Sprites.Animation> a in action.Animations)
                    {
                        if (count == CurrentStep)
                            break;


                        ManaSource.Sprites.Animation anim = a.Value;
                        foreach (ManaSource.Sprites.AnimationFrame frame in anim.Frames)
                        {
                            layer.CurrentSequence++;
                            if (count + frame.Length > CurrentStep)
                            {
                                int leftover = CurrentStep - count;
                                layer.CurrentIndex = leftover;
                                count = CurrentStep;
                                break;
                            }
                            else
                                count += frame.Length;
                        }
                    }
                }
                if (layer.CurrentSequence == -1)
                    layer.CurrentSequence = 0;
                if (layer.CurrentIndex == -1)
                    layer.CurrentIndex = 0;
            }
        }

        private void StepForwardButton_Click(object sender, EventArgs e)
        {
            CurrentStep++;
            CheckCurrentFrame();
            if (CurrentStep == 0)
            {
                UpdateFrameData();
                Redraw();
                return;
            }

            int oldseq = -1;
            if (SelectedLayer() != null)
                oldseq = SelectedLayer().CurrentSequence;

            foreach (SpriteLayer layer in Doc.Layers)
            {
                ManaSource.Sprites.Action action = layer.LayerSprite.GetAction(CurrentAction);
                if (action != null)
                {
                    ManaSource.Sprites.Animation anim = action.GetAnimation(CurrentDirection);
                    if (anim != null)
                    {
                        ManaSource.Sprites.AnimationFrame frame = anim.Get(layer.CurrentSequence);
                        if (frame == null)
                        {
                            layer.CurrentSequence = anim.Count-1;
                            layer.CurrentIndex = anim.Frames[anim.Count - 1].Length - 1;
                        }
                        else
                        {
                            layer.CurrentIndex++;
                            if (layer.CurrentIndex >= frame.Length)
                            {
                                layer.CurrentIndex = 0;
                                layer.CurrentSequence++;
                                if (layer.CurrentSequence >= anim.Count)
                                {
                                    layer.CurrentSequence = anim.Count - 1;
                                    layer.CurrentIndex = anim.Frames[anim.Count - 1].Length - 1;
                                }
                            }
                        }
                    }
                }
            }

            if (SelectedLayer() != null)
            {
                NoSelect = true;
                SequenceList.Items[oldseq].Selected = false;
                SequenceList.Items[SelectedLayer().CurrentSequence].Selected = true;
                NoSelect = false;
            }

            UpdateFrameData();
            Redraw();
        }

        private void recenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawOffset = new Point(MainView.Width / ImageZoom / 2, MainView.Height / ImageZoom - 4);
            LastDragLoc = new Point(DrawOffset.X, DrawOffset.Y);
            Redraw();
        }

        protected ManaSource.Sprites.AnimationFrame GetCurrentFrame ( SpriteLayer layer )
        {
            if (layer == null)
                return null;
           
            ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);

            if (anim == null)
                return null;

            return anim.Get(layer.CurrentSequence);
        }

        private void AnimIsFrame_CheckedChanged(object sender, EventArgs e)
        {
            if (NoSelect)
                return;

            ManaSource.Sprites.AnimationFrame frame = GetCurrentFrame(SelectedLayer());
            if (frame == null)
                return;

            if (AnimIsFrame.Checked != (frame.Length == 1))
            {
                if (AnimIsFrame.Checked)
                    frame.EndFrame = frame.StartFrame;
                else
                    frame.EndFrame = frame.StartFrame+1;

                FrameEndIndex.Enabled = !AnimIsFrame.Checked;
                NoSelect = true;
                FrameEndIndex.Value = frame.EndFrame;
                NoSelect = false;
            }
        }

        private void FrameStartIndex_ValueChanged(object sender, EventArgs e)
        {
            if (NoSelect)
                return;

            ManaSource.Sprites.AnimationFrame frame = GetCurrentFrame(SelectedLayer());
            if (frame == null)
                return;

            frame.StartFrame = (int)FrameStartIndex.Value;
            frame.Delay = (int)FrameDelay.Value;
            if (!AnimIsFrame.Checked)
                frame.EndFrame = (int)FrameEndIndex.Value;
            else
                frame.EndFrame = frame.StartFrame;

            SequenceList.Items[SelectedLayer().CurrentSequence].Text = frame.ToString();
            UpdateSequenceData();
            Redraw();
        }

        private void AddSequence_Click(object sender, EventArgs e)
        {
            ManaSource.Sprites.Animation anim = GetCurrentAnimation(SelectedLayer());
            if (anim == null)
                return;

            ManaSource.Sprites.AnimationFrame frame = new ManaSource.Sprites.AnimationFrame();
            if (anim.Frames.Count > 1)
            {
                frame.StartFrame = anim.Frames[anim.Frames.Count - 1].StartFrame + anim.Frames[anim.Frames.Count - 1].Length - 1;
                frame.Delay = anim.Frames[anim.Frames.Count - 1].Delay;
                CurrentStep = anim.Length;
            }
            else
            {
                frame.StartFrame = 0;
                CurrentStep = 0;
            }
            anim.Frames.Add(frame);

            UpdateAnimations();
            UpdateFrameData();
            SequenceList.Items[SequenceList.Items.Count - 1].Selected = true;
            Redraw();
        }

        private void RemoveSequence_Click(object sender, EventArgs e)
        {
            ManaSource.Sprites.Animation anim = GetCurrentAnimation(SelectedLayer());
            if (anim == null)
                return;

            if (SequenceList.SelectedIndices.Count > 0)
            {
                int index = SequenceList.SelectedIndices[0];

                if (index >= anim.Count)
                    return;

                anim.Frames.RemoveAt(index);
                SetAllLayersToSetpZero();

                UpdateAnimations();
                UpdateFrameData();
                Redraw();
            }
        }

        private void SequenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoSelect)
                return;

            ManaSource.Sprites.Animation anim = GetCurrentAnimation(SelectedLayer());
            if (anim == null)
                return;

            if (SequenceList.SelectedIndices.Count > 0)
            {
                // figure out how many steps are in front of the current selected segment
                int index = SequenceList.SelectedIndices[0];

                if (index == 0)
                    SetAllLayersToSetpZero();
                else
                {
                    CurrentStep = 0;
                    for (int i = 0; i < index; i++)
                        CurrentStep += anim.Frames[i].Length;
                    SetAllLayersToCurrentStep();
                }
                UpdateFrameData();
                Redraw();
            }
        }

        private void ImageSetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoSelect)
                return;

            ManaSource.Sprites.Action action = GetCurrentAction(SelectedLayer());
            if (action == null)
                return;
           
            action.Imageset = ImageSetList.SelectedItem.ToString();
            BuildLayerList(); // the imageset may have changed and we need to update it's image
            UpdateSequenceData();
            Redraw();
        }

        private void AddImageSet_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            NewImageSetForm form = new NewImageSetForm();
            form.XMLName = "New Imageset";

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                if (layer.LayerSprite.Imagesets.ContainsKey(form.XMLName))
                {
                    MessageBox.Show(this, "The current sprite layer already has an image set with the name " + form.XMLName);
                    return;
                }

                if (!File.Exists(form.ImageFilePath))
                    return;

                ManaSource.Sprites.ImageSet imgset = new ManaSource.Sprites.ImageSet();
                imgset.Name = form.XMLName;
                imgset.DieString = form.XMLRecolor;
                imgset.ImageFile = form.XMLSrc;
                imgset.GridSize = form.GridSize;

                imgset.Tag = SpriteImage.Add(form.XMLSrc, form.ImageFilePath);
                layer.LayerSprite.Imagesets.Add(form.XMLName,imgset);

                UpdateImageSet();
            }
        }

        private void EditImageSet_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            int index = ImageSetList.SelectedIndex;
            string name = ImageSetList.SelectedItem.ToString();

            ManaSource.Sprites.ImageSet imgset = layer.LayerSprite.Imagesets[name];
            SpriteImage si = imgset.Tag as SpriteImage;

            NewImageSetForm form = new NewImageSetForm();
            form.XMLName = imgset.Name;
            form.XMLRecolor = imgset.DieString;
            form.GridSize = imgset.GridSize;
            form.XMLSrc = imgset.ImageFile;
            if (si != null)
                form.ImageFilePath = (imgset.Tag as SpriteImage).FilePath;

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                if (form.XMLName != name && layer.LayerSprite.Imagesets.ContainsKey(form.XMLName))
                {
                    MessageBox.Show(this, "The current sprite layer already has an image set with the name " + form.XMLName);
                    return;
                }

                if (form.XMLName != name)
                {
                    // name changed
                    imgset.Name = form.XMLName;
                    foreach (KeyValuePair<string, ManaSource.Sprites.Action> action in layer.LayerSprite.Actions)
                    {
                        if (action.Value.Imageset == name)
                            action.Value.Imageset = imgset.Name;
                    }

                    ImageSetList.SelectedItem = imgset.Name;
                }

                imgset.DieString = form.XMLRecolor;
                imgset.ImageFile = form.XMLSrc;
                imgset.GridSize = form.GridSize;

                if (imgset.ImageFile == form.XMLSrc && si != null)
                    si.Reseat(new FileInfo(form.ImageFilePath));
                else if (imgset.ImageFile != form.XMLSrc || si == null)
                    imgset.Tag = SpriteImage.Add(form.XMLSrc, form.ImageFilePath);

                BuildLayerList(); // the imageset may have changed and we need to update it's image
                UpdateSequenceData();
                Redraw();
            }
        }

        private void RemoveImageSet_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            if (layer.LayerSprite.Imagesets.Count == 1)
                return;

            int index = ImageSetList.SelectedIndex;
            string name = ImageSetList.SelectedItem.ToString();


            NoSelect = true;
            ImageSetList.Items.Remove(index);
            string baseName = ImageSetList.Items[0].ToString();

            layer.LayerSprite.Imagesets.Remove(name);

            foreach ( KeyValuePair<string,ManaSource.Sprites.Action> action in layer.LayerSprite.Actions )
            {
                if (action.Value.Imageset == name)
                    action.Value.Imageset = baseName;
            }
            NoSelect = false;
            UpdateSequenceData();
            Redraw();
        }

        protected void AddActionDir ( ManaSource.Sprites.Action action, ManaSource.Sprites.Direction dir )
        {
            if (action.Animations.ContainsKey(dir))
                return;

            ManaSource.Sprites.Animation anim = new ManaSource.Sprites.Animation();
            anim.Direction = dir;
            ManaSource.Sprites.AnimationFrame frame = new ManaSource.Sprites.AnimationFrame();
            frame.StartFrame = 0;
            frame.EndFrame = 0;
            frame.Delay = 75;
            anim.Frames.Add(frame);
            action.Animations.Add(dir, anim);
        }

        private void EditAction_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            ManaSource.Sprites.Action action = GetCurrentAction(layer);

            NewActionForm form = new NewActionForm();

            form.ImageSets = layer.LayerSprite.Imagesets;

            form.SelectedActionName = action.Name;
            form.SelectdImageSet = action.Imageset;
            bool cardinals = !action.Animations.ContainsKey(ManaSource.Sprites.Direction.Any);
            form.CardinalDirections = cardinals;

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                if (action.Name != form.SelectedActionName || layer.LayerSprite.Actions.ContainsKey(form.SelectedActionName))
                {
                    MessageBox.Show(this, "The current sprite layer already has an action with the name " + form.SelectedActionName);
                    return;
                }

                if (action.Name != form.SelectedActionName)
                {
                    CurrentAction = form.SelectedActionName;
                    action.Name = form.SelectedActionName;
                    NoSelect = true;
                    ActionList.SelectedItem = CurrentAction;
                    NoSelect = false;
                }

                action.Imageset = form.SelectdImageSet;
                NoSelect = true;
                ImageSetList.SelectedItem = action.Imageset;
                NoSelect = false;
                UpdateSequenceData();

                if (form.CardinalDirections != cardinals)
                {
                    // need to add or remove some anim dirs
                    if (form.CardinalDirections)
                    {
                        AddActionDir(action, ManaSource.Sprites.Direction.Up);
                        AddActionDir(action, ManaSource.Sprites.Direction.Down);
                        AddActionDir(action, ManaSource.Sprites.Direction.Left);
                        AddActionDir(action, ManaSource.Sprites.Direction.Right);
                        if (action.Animations.ContainsKey(ManaSource.Sprites.Direction.Any))
                            action.Animations.Remove(ManaSource.Sprites.Direction.Any);
                    }
                    else
                        AddActionDir(action, ManaSource.Sprites.Direction.Any);
                }

                Redraw();
            }
        }

        private void AddAction_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            NewActionForm form = new NewActionForm();

            form.ImageSets = layer.LayerSprite.Imagesets;

            form.SelectedActionName = "New Action";
            form.SelectdImageSet = string.Empty;
            form.CardinalDirections = true;

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                if (layer.LayerSprite.Actions.ContainsKey(form.SelectedActionName))
                {
                    MessageBox.Show(this, "The current sprite layer already has an action with the name " + form.SelectedActionName);
                    return;
                }

                ManaSource.Sprites.Action action = new ManaSource.Sprites.Action();

                action.Name = form.SelectedActionName;
                action.Imageset = form.SelectdImageSet;

                if (form.CardinalDirections)
                {
                    AddActionDir(action, ManaSource.Sprites.Direction.Up);
                    AddActionDir(action, ManaSource.Sprites.Direction.Down);
                    AddActionDir(action, ManaSource.Sprites.Direction.Left);
                    AddActionDir(action, ManaSource.Sprites.Direction.Right);
                }
                else
                    AddActionDir(action, ManaSource.Sprites.Direction.Any);

                layer.LayerSprite.Actions.Add(action.Name, action);

                CurrentAction = form.SelectedActionName;
                BuildActionList();
                ActionList.SelectedItem = CurrentAction;
                Redraw();
            }
        }

        private void RemoveAction_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            if (layer.LayerSprite.Imagesets.Count == 1)
                return;

            int index = ActionList.SelectedIndex;
            string name = ActionList.SelectedItem.ToString();


            NoSelect = true;
            ActionList.Items.Remove(index);
            layer.LayerSprite.Actions.Remove(name);

            CurrentAction = ActionList.Items[0].ToString();
            NoSelect = false;
            UpdateImageSet();
            UpdateFrameData();
            UpdateSequenceData();
            Redraw();
        }

        private void NewLayer_Click(object sender, EventArgs e)
        {
            NewLayerForm form = new NewLayerForm();

            form.Name = "New Sprite";
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                ManaSource.Sprites.Sprite sprite = new ManaSource.Sprites.Sprite();
                
                sprite.Name = form.SpriteName;
                
                ManaSource.Sprites.ImageSet imgset = new ManaSource.Sprites.ImageSet();
                imgset.Name = form.ImageSetName;
                imgset.DieString = string.Empty;
                imgset.ImageFile = form.ImageSetPath;
                imgset.GridSize = form.ImageSetGridSize;

                imgset.Tag = SpriteImage.Add(form.ImageSetPath, form.ImageSetFile);
                sprite.Imagesets.Add(form.ImageSetName, imgset);

                sprite.DefaultAction = form.SpriteDefaultAction;

                ManaSource.Sprites.Action action = new ManaSource.Sprites.Action();
                action.Name = sprite.DefaultAction;
                action.Imageset = imgset.Name;

                CurrentStep = 0;
                CurrentDirection = ManaSource.Sprites.Direction.Down;

                CurrentAction = action.Name;
                AddActionDir(action, ManaSource.Sprites.Direction.Any);

                sprite.Actions.Add(action.Name, action);

                SpriteLayer layer = new SpriteLayer(sprite, form.XMLFileLocation);
                Doc.Layers.Add(layer);
                BuildLayerList();
                SelectLayer(layer);
                BuildActionList();
                UpdateImageSet();
                UpdateFrameData();
                UpdateAnimations();
                CheckCurrentFrame();
                Redraw();
            }
        }

        private void LayerInfo_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            NewLayerForm form = new NewLayerForm();
            form.SpriteOnly = true;

            form.Name = layer.LayerSprite.Name;
            form.XMLFileLocation = layer.XMLFile.FullName;
            form.SpriteDefaultAction = layer.LayerSprite.DefaultAction;

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                layer.XMLFile = new FileInfo(form.XMLFileLocation);
                layer.Name = form.SpriteName;
                layer.LayerSprite.Name = form.SpriteName;
                layer.LayerSprite.DefaultAction = form.SpriteDefaultAction;

                BuildLayerList();
                SelectLayer(layer);
                BuildActionList();
                UpdateImageSet();
                UpdateFrameData();
                UpdateAnimations();
                CheckCurrentFrame();
                SelectLayer(layer);
                Redraw();
            }
        }
    }
}
