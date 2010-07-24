using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;

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
            SpriteImage.LocateFile = new SpriteImage.UnknownFileHandler(FindSpriteImage);
            SpriteImage.Reload += new SpriteImage.FileReloadHandler(SpriteImage_Reload);

            LayerList_SelectedIndexChanged(this, EventArgs.Empty);
            DirButtonBackColor = UpButton.BackColor;
            SetDirChecks();
            SetToolTips();
            recenterToolStripMenuItem_Click(this, EventArgs.Empty);
        }

        protected void SetToolTips ()
        {
            toolTip1.SetToolTip(MainView, "Main graphics view, right drag to move");
            toolTip1.SetToolTip(LayerUp, "Move selected layer up one level");
            toolTip1.SetToolTip(LayerDown, "Move selected layer down one level");
            toolTip1.SetToolTip(SaveLayer, "Save selected layer XML");
            toolTip1.SetToolTip(HideLayer, "Show/Hide selected layer form main view");
            toolTip1.SetToolTip(AddLayer, "New layer from XML");
            toolTip1.SetToolTip(RemoveLayer, "Remove selected layer");
            toolTip1.SetToolTip(StepForwardButton, "Step to next frame in animation");
            toolTip1.SetToolTip(XOffset, "Offset in X for current animation");
            toolTip1.SetToolTip(YOffset, "Offset in Y for current animation");

            toolTip1.SetToolTip(IndexNumber, "Step for current layer animation");
            toolTip1.SetToolTip(SequenceNumber, "Sequence number for current layer animation");
            toolTip1.SetToolTip(GridIndex, "Image grid index for current layer animation frame");

            toolTip1.SetToolTip(SequenceList, "Animation sequences/frames for current layer");
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

            foreach (string action in Doc.Actions)
            {
                int id = ActionList.Items.Add(action);
                if (action == CurrentAction)
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

            if (action == null || imageSet == null || anim == null)
                return null;

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

                        foreach(KeyValuePair<string,ManaSource.Sprites.Action> action in sprite.Actions)
                        {
                            if (!Doc.Actions.Contains(action.Value.Name))
                                Doc.Actions.Add(action.Value.Name);
                        }
                    }
                    BuildLayerList();
                    BuildActionList();
                    UpdateImageSet();
                    UpdateFrameData();
                    UpdateAnimations();
                    SelectLayer(Doc.Layers[Doc.Layers.Count - 1]);
                    CheckCurrentFrame();
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

                Doc.Actions.Clear();
                foreach (SpriteLayer l in Doc.Layers)
                {
                    foreach (KeyValuePair<string, ManaSource.Sprites.Action> action in l.LayerSprite.Actions)
                    {
                        if (!Doc.Actions.Contains(action.Value.Name))
                            Doc.Actions.Add(action.Value.Name);
                    }
                }
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
        //    LayerInfo.Enabled = enable;
            HideLayer.Enabled = enable;
            RemoveLayer.Enabled = enable;
            SaveLayer.Enabled = enable;

            CheckCurrentFrame();
            UpdateFrameData();
            UpdateImageSet();
            UpdateAnimations();

            Redraw();
        }

        protected void UpdateFrameData ()
        {
            NoSelect = true;
            SpriteLayer layer = SelectedLayer();

            SequenceNumber.Text = string.Empty;
            IndexNumber.Text = string.Empty;
            LayerLabel.Text = string.Empty;

            GridIndex.Text = "";

            if (layer == null)
            {
                XOffset.Value = 0;
                YOffset.Value = 0;
                FrameDataGroup.Enabled = false;
                XOffset.Enabled = false;
                YOffset.Enabled = false;
                PlayControllsPanel.Enabled = false;
            }
            else
            {
                LayerLabel.Text = layer.Name;

                SequenceNumber.Text = layer.CurrentSequence.ToString();
                IndexNumber.Text = layer.CurrentIndex.ToString();

                PlayControllsPanel.Enabled = true;

                ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);
                XOffset.Enabled = true;
                YOffset.Enabled = true;
                XOffset.Value = anim.Frames[layer.CurrentSequence].Offset.X;
                YOffset.Value = anim.Frames[layer.CurrentSequence].Offset.Y;

                FrameDataGroup.Enabled = false;

                ManaSource.Sprites.AnimationFrame frame = anim.Frames[layer.CurrentSequence];

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

                GridIndex.Text = frame.Frame(layer.CurrentIndex).ToString();
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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ImageZoom = 1;
            Redraw();
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageZoom = 2;
            Redraw();
        }

        private void xToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ImageZoom = 3;
            Redraw();
        }

        private void xToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ImageZoom = 4;
            Redraw();
        }

        private void xToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ImageZoom = 5;
            Redraw();
        }

        private void xToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ImageZoom = 6;
            Redraw();
        }

        private void xToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            ImageZoom = 7;
            Redraw();
        }

        private void xToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ImageZoom = 8;
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
            {
                CurrentStep = 0;
                foreach (SpriteLayer layer in Doc.Layers)
                {
                    layer.CurrentSequence = 0;
                    layer.CurrentIndex = 0;
                }
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
    }
}
