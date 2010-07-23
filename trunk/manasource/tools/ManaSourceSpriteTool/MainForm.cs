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
        protected ManaSource.Sprites.Direction CurrentDirection = ManaSource.Sprites.Direction.down;
        protected int CurrentSequence = 0;
        protected int CurrentFrame = 0;

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

            DrawOffset = new Point(MainView.Width / ImageZoom / 2, MainView.Height / ImageZoom - 4);

            LastDragLoc = new Point(DrawOffset.X, DrawOffset.Y);
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
                ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(layer.XMLFile.Name), j);
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

            if (layer.Sprite.Actions.ContainsKey(CurrentAction))
                return layer.Sprite.Actions[CurrentAction];
            return null;
        }

        protected ManaSource.Sprites.ImageSet GetCurrentImageset(SpriteLayer layer)
        {
            ManaSource.Sprites.Action action = GetCurrentAction(layer);
            if (action == null)
                return null;

            if (layer.Sprite.Imagesets.ContainsKey(action.Imageset))
                return layer.Sprite.Imagesets[action.Imageset];

            return null;
        }

        protected ManaSource.Sprites.Animation GetCurrentAnimation(SpriteLayer layer)
        {
            ManaSource.Sprites.Action action = GetCurrentAction(layer);
            if (action == null)
                return null;

            if (action.Animations.ContainsKey(CurrentDirection))
                return action.Animations[CurrentDirection];

            return null;
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
                return img.GetFrameImage(imageSet, anim.Frames[CurrentSequence], CurrentFrame);
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
                        img.DrawFrame(e.Graphics, imageSet, anim.Frames[CurrentSequence], CurrentFrame);
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
                    foreach (KeyValuePair<string, ManaSource.Sprites.Action> action in l.Sprite.Actions)
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
            LayerInfo.Enabled = enable;
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

            SequenceNumber.Text = CurrentSequence.ToString();
            IndexNumber.Text = CurrentFrame.ToString();

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
                PlayControllsPanel.Enabled = true;

                ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);
                XOffset.Enabled = true;
                YOffset.Enabled = true;
                XOffset.Value = anim.Frames[CurrentSequence].Offset.X;
                YOffset.Value = anim.Frames[CurrentSequence].Offset.Y;

                FrameDataGroup.Enabled = true;

                ManaSource.Sprites.AnimationFrame frame = anim.Frames[CurrentSequence];

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

                GridIndex.Text = frame.Frame(CurrentFrame).ToString();
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
            UpButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.up);
            DownButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.down);
            LeftButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.left);
            RightButton.BackColor = GetDirButtonColor(ManaSource.Sprites.Direction.right);
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.up;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();

            Redraw();
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.left;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();
            Redraw();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.down;
            SetDirChecks();
            UpdateFrameData();
            UpdateAnimations();
            Redraw();
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            CurrentDirection = ManaSource.Sprites.Direction.right;
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
            foreach (KeyValuePair<string, ManaSource.Sprites.ImageSet> imageset in layer.Sprite.Imagesets)
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

            if (anim == null || anim.Frames.Count <= CurrentSequence)
                return;

            anim.Frames[CurrentSequence].Offset = new Point((int)XOffset.Value, (int)YOffset.Value);
            Redraw();
        }

        private void HideLayer_Click(object sender, EventArgs e)
        {
        }

        private void SaveLayer_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            new ManaSource.Sprites.XMLWriter(new FileInfo("temp.xml"), layer.Sprite);
        }

        private void CheckCurrentFrame ()
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);
            if (CurrentSequence >= anim.Frames.Count)
            {
                CurrentSequence = 0;
                CurrentFrame = 0;
            }
            else if (CurrentFrame >= anim.Frames[CurrentSequence].Length)
                CurrentFrame = 0;
        }

        private void StepForwardButton_Click(object sender, EventArgs e)
        {
            SpriteLayer layer = SelectedLayer();
            if (layer == null)
                return;

            ManaSource.Sprites.Animation anim = GetCurrentAnimation(layer);
            ManaSource.Sprites.AnimationFrame frame = anim.Frames[CurrentSequence];

            if (anim.Frames.Count == 1 && frame.Length == 1)
                return;

            CurrentFrame++;
            if (CurrentFrame >= frame.Length)
            {
                NoSelect = true;
                SequenceList.Items[CurrentSequence].Selected = false;

                CurrentFrame = 0;
                CurrentSequence++;
                if (CurrentSequence >= anim.Frames.Count)
                    CurrentSequence = 0;

                SequenceList.Items[CurrentSequence].Selected = true;
                NoSelect = false;
            }

            UpdateFrameData();
            Redraw();
        }
    }
}
