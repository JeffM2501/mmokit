using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        protected SpriteDoc Doc = new SpriteDoc();

        protected string CurrentAction = "stand";
        protected ManaSource.Sprites.Direction CurrentDirection = ManaSource.Sprites.Direction.down;
        protected int CurrentSequence = 0;
        protected int CurrentFrame = 0;

        public MainForm()
        {
            InitializeComponent();
            LayerList.StateImageList = new ImageList();
            LayerList.StateImageList.ColorDepth = ColorDepth.Depth32Bit;
            LayerList.StateImageList.ImageSize = new System.Drawing.Size(32, 32);
        }

        public void Redraw ()
        {
            MainView.Invalidate();
        }

        protected void BuildLayerList ()
        {
            SpriteLayer selectedLayer = null;
            if (LayerList.SelectedItems.Count > 0)
                selectedLayer = LayerList.SelectedItems[0].Tag as SpriteLayer;

            LayerList.Items.Clear();
            LayerList.StateImageList.Images.Clear();

            foreach (SpriteLayer layer in Doc.Layers)
            {
                Image img = GetCurrrentFameAsImage(layer);

                if (img != null)
                    LayerList.StateImageList.Images.Add(img);

                ListViewItem item = new ListViewItem(Path.GetFileNameWithoutExtension(layer.XMLFile.Name), LayerList.StateImageList.Images.Count-1);
                if (layer == selectedLayer)
                    item.Selected = true;
                item.Tag = layer;
                LayerList.Items.Add(item);
            }
        }

        protected ManaSource.Sprites.Action GetCurrentAction(SpriteLayer layer)
        {
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

            e.Graphics.ScaleTransform(ImageZoom, ImageZoom);
            // draw the shit

            e.Graphics.TranslateTransform(10, 4);
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
        }

        private void AddLayer_Click(object sender, EventArgs e)
        {
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
                    BuildLayerList();
                    Redraw();
                }
            }
        }

        private void MainView_Resize(object sender, EventArgs e)
        {
            Redraw();
        }
    }
}
