using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;
using OpenTK.Input;
using OpenTK.Platform;

using Drawables.Cameras;
using Grids;
using World;
using Math3D;
using Drawables.Materials;

namespace octreeTest
{
    public class Visual : GameWindow
    {
        Camera camera = new Camera();
        
        DebugableVisibleFrustum clipingFrustum = null;
        BoundingBox clippingBox = new BoundingBox();

        DebugableOctreeWorld world = new DebugableOctreeWorld();

        Dictionary<OctreeLeaf, GLColor> treeColorMap = new Dictionary<OctreeLeaf, GLColor>();

        bool drawAll = false;
        bool drawTreeNodes = false;
        bool cullToFrustum = false;
        bool snapshotFrustum = true;
        bool clipToCamera = false;

        bool exactCulling = true;

        int drawnObjects = 0;

        float[] viewMat = new float[16];

        Vector3 cullBoxPos = new Vector3();

        TextPrinter printer = new TextPrinter(TextQuality.High);
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 16.0f);
        Font small_serif = new Font(FontFamily.GenericSansSerif, 8.0f);


        public Visual() : base(1024,550)
        {
            VSync = VSyncMode.Off;
        }

        public override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(System.Drawing.Color.Black);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.LightModel(LightModelParameter.LightModelColorControl,1);

            // setup light 0
            Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Lightv(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Lightv(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Lightv(LightName.Light0, LightParameter.Specular, lightInfo);

            camera.set(new Vector3(1, 1, 2), 0, 0);

            world.Setup(100.0f, 10.0f, 2.0f );

            int boxes = new Random().Next() % 10 + 100;
            for (int i = 0; i < boxes; i++)
                world.Add(new BoxObject(world.Size));
            world.FinalizeWorld();
          
            this.Mouse.Move += new MouseMoveEventHandler(Mouse_Move);
        }

        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            int i = e.XDelta;
        }

        public override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            camera.Resize(Width, Height);
        }

        protected bool doInput (UpdateFrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                return true;

            float turnSpeed = 40.0f;
            turnSpeed *= (float)e.Time;

            if (Keyboard[Key.Left])
                camera.turn(0, turnSpeed);
            if (Keyboard[Key.Right])
                camera.turn(0, -turnSpeed);
            if (Keyboard[Key.Up])
                camera.turn(-turnSpeed, 0);
            if (Keyboard[Key.Down])
                camera.turn(turnSpeed, 0);

            if (Keyboard[Key.F1])
                drawAll = true;
            if (Keyboard[Key.F2])
                drawAll = false;

            if (Keyboard[Key.F3])
            {
                if (clipToCamera)
                    snapshotFrustum = true;

                cullToFrustum = true;
                clipToCamera = false;
            }
            if (Keyboard[Key.F4])
            {
                cullToFrustum = false;
                clipToCamera = false;
            }
            if (Keyboard[Key.F8])
            {
                cullToFrustum = true;
                clipToCamera = true;
            }

            if (Keyboard[Key.F5])
                snapshotFrustum = true;

            if (Keyboard[Key.F6])
                drawTreeNodes = true;
            if (Keyboard[Key.F7])
                drawTreeNodes = false;

            if (Keyboard[Key.F9])
                exactCulling = true;
            if (Keyboard[Key.F10])
                exactCulling = false;

            Vector3 forward = new Vector3(camera.Heading());
            Vector3 leftward = new Vector3(forward);
            leftward.X = -forward.Y;
            leftward.Y = forward.X;

            Vector2 movement = new Vector2();

            float speed = 15.0f;
            speed *= (float)e.Time;

            if (Keyboard[Key.A])
                movement.X = 1;
            if (Keyboard[Key.D])
                movement.X = -1;
            if (Keyboard[Key.W])
                movement.Y = 1;
            if (Keyboard[Key.S])
                movement.Y = -1;

            if (Keyboard[Key.PageUp])
                camera.move(0, 0, speed);
            if (Keyboard[Key.PageDown])
                camera.move(0, 0, -speed);

            Vector3 incremnt = new Vector3();
            incremnt += forward * movement.Y * speed;
            incremnt += leftward * movement.X * speed;

            camera.move(incremnt);

            // cullbox

            if (Keyboard[Key.Keypad8])
                cullBoxPos.Y += speed;
            if (Keyboard[Key.Keypad2])
                cullBoxPos.Y -= speed;
            if (Keyboard[Key.Keypad4])
                cullBoxPos.X -= speed;
            if (Keyboard[Key.Keypad6])
                cullBoxPos.X += speed;
            if (Keyboard[Key.Keypad9])
                cullBoxPos.Z += speed;
            if (Keyboard[Key.Keypad3])
                cullBoxPos.Z -= speed;

            return false;
        }

        public override void OnUpdateFrame(UpdateFrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (doInput(e))
                Exit();
        }

        void drawOverlay(RenderFrameEventArgs e)
        {
            GL.Disable(EnableCap.Lighting);
            printer.Begin();

            GL.Color3(Color.DarkSlateGray);
            GL.Begin(BeginMode.Quads);
            GL.Vertex3(Width - 285, 70 + 70, -0.001f);
            GL.Vertex3(Width - 2, 70 + 70, -0.001f);
            GL.Vertex3(Width - 2, 10, -0.001f);
            GL.Vertex3(Width - 285, 10,-0.001f);


            GL.Vertex3(0, Height - 48, -0.001f);
            GL.Vertex3(Width, Height - 48, -0.001f);
            GL.Vertex3(Width, Height - 62, -0.001f);
            GL.Vertex3(0, Height - 62, -0.001f);

            GL.End();

            GL.Color3(Color.Wheat);

            printer.Print(((int)(1 / e.Time)).ToString("F0"), sans_serif, Color.Wheat);
            printer.Print("Drawn Objects(" + drawnObjects.ToString() + ")", sans_serif, Color.Wheat, new RectangleF(0, Height - 32, Width, Height), TextPrinterOptions.Default);
            string modes = string.Empty;
            if (drawAll)
                modes += "*";
            modes += "F1=DrawAll ";
            if (!drawAll)
                modes += "*";
            modes += "F2=DrawVis ";

            if (cullToFrustum && !clipToCamera)
                modes += "*";
            modes += "F3=CullFrustum ";
            if (!cullToFrustum && !clipToCamera)
                modes += "*";
            modes += "F4=CullBox ";

            modes += "F5=SnapFrustumToCam ";

            if (drawTreeNodes)
                modes += "*";
            modes += "F6=DrawTreeNodes ";
            if (!drawTreeNodes)
                modes += "*";
            modes += "F7=HideTreeNodes ";

            if (cullToFrustum && clipToCamera)
                modes += "*";
            modes += "F8=ClipToCamera ";

            if (exactCulling)
                modes += "*";
            modes += "F9=ExactCulling ";
            if (!exactCulling)
                modes += "*";
            modes += "F10=FastCulling ";

            printer.Print(modes, small_serif, Color.Wheat, new RectangleF(0, Height - 64, Width, Height - 32), TextPrinterOptions.Default);

            float offset = 10;
            printer.Print("View Matrix", small_serif, Color.Wheat, new RectangleF(Width - 220, offset + 10, Width, offset + 20), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ViewMatrix.Row0.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 20, Width, offset + 30), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ViewMatrix.Row1.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 30, Width, offset + 40), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ViewMatrix.Row2.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 40, Width, offset + 50), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ViewMatrix.Row3.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 50, Width, offset + 60), TextPrinterOptions.Default);

            offset = 70;
            printer.Print("Projection Matrix", small_serif, Color.Wheat, new RectangleF(Width - 220, offset + 10, Width, offset + 20), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ProjectionMatrix.Row0.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 20, Width, offset + 30), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ProjectionMatrix.Row1.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 30, Width, offset + 40), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ProjectionMatrix.Row2.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 40, Width, offset + 50), TextPrinterOptions.Default);
            printer.Print(clipingFrustum.ProjectionMatrix.Row3.ToString(), small_serif, Color.Wheat, new RectangleF(Width - 280, offset + 50, Width, offset + 60), TextPrinterOptions.Default);

            GL.LineWidth(1f);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(Width - 285, 10);
            GL.Vertex2(Width - 2, 10);
            GL.Vertex2(Width - 2, offset+70);
            GL.Vertex2(Width - 285, offset + 70);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(0, Height - 48);
            GL.Vertex2(Width, Height - 48);
            GL.Vertex2(Width, Height - 62);
            GL.Vertex2(0, Height - 62);
            GL.End();

            printer.End();
        }

        void drawOctreeNodes (OctreeLeaf leaf)
        {
            GL.LineWidth(1);

            if (!treeColorMap.ContainsKey(leaf))
                treeColorMap.Add(leaf, new GLColor(FloatRand.RandInRange(0, 0.5f), FloatRand.RandInRange(0, 1), FloatRand.RandInRange(0, 1), 0.75f));

            treeColorMap[leaf].glColor();

            if (cullToFrustum)
            {
                if (clipingFrustum.Intersects(leaf.bounds))
                {
                    GL.Color4(1f, 0, 0, 0.75f);
                    GL.LineWidth(3);
                }
            }
            else
            {
                if (clippingBox.Intersects(leaf.bounds))
                {
                    GL.Color4(1f, 0, 0, 0.75f);
                    GL.LineWidth(3);
                }
            }

            new BoxObject(leaf.bounds).draw();
            if (leaf.children != null)
            {
                foreach (OctreeLeaf l in leaf.children)
                    drawOctreeNodes(l);
            }
        }

        void drawWorld()
        {
            List<OctreeObject> vis = new List<OctreeObject>();

            if (cullToFrustum)
            {
                world.ObjectsInFrustum(vis, clipingFrustum, exactCulling);
                if (!clipToCamera)
                {
                    // draw the frustum
                    GL.Color4(0.0f, 0.35f, 0.6f, 0.6f);
                    clipingFrustum.drawFrustum();
                }
            }
            else
            {
                Vector3 boundSize = new Vector3(5, 5, 5);
                clippingBox = new BoundingBox(boundSize * -1.0f + cullBoxPos, boundSize + cullBoxPos);
                world.ObjectsInBoundingBox(vis, clippingBox, exactCulling);

                GL.DepthMask(false);
                GL.Color4(0.0f, 0.25f, 0.5f, 0.5f);
                new BoxObject(clippingBox).draw();
                GL.DepthMask(true);
            }

            drawnObjects = world.draw(vis, drawAll);

            if (drawTreeNodes)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.CullFace);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                drawOctreeNodes(world);
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.CullFace);
            }
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public override void OnRenderFrame(RenderFrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            camera.Execute();

            if (snapshotFrustum)
            {
                clipingFrustum = new DebugableVisibleFrustum(camera.SnapshotFrusum());
                snapshotFrustum = false;
            }
            else if (clipToCamera && cullToFrustum)
                clipingFrustum = new DebugableVisibleFrustum(camera.ViewFrustum);

            GL.Enable(EnableCap.Light0);
            GL.Lightv(LightName.Light0, LightParameter.Position, new Vector4(10, 15, 10, 1.0f));

            world.drawGround();

            drawWorld();

            GL.Clear(ClearBufferMask.DepthBufferBit);
            drawOverlay(e);

            SwapBuffers();
        }
    }

    public class DebugableOctreeWorld : OctreeWorld
    {
        Grid grid = new Grid();
        float groundSize = 100f;

        public void FinalizeWorld ()
        {
            BuildTree(new BoundingBox(new Vector3(-groundSize, -groundSize, -1), new Vector3(groundSize, groundSize, 50)));
        }

        public float Size
        {
            get { return groundSize; }
        }

        public void Setup ( float ground, float majorSpace, float minorSpace )
        {
            groundSize = ground;

            grid.gridSize = groundSize;
            grid.majorSpacing = majorSpace;
            grid.minorSpacing = minorSpace;
            grid.alpha = 0.5f;
        }

        public void drawGround()
        {
            GL.Color3(System.Drawing.Color.LightGreen);

            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(groundSize, groundSize, -0.01f);
            GL.Vertex3(-groundSize, groundSize, -0.01f);
            GL.Vertex3(-groundSize, -groundSize, -0.01f);
            GL.Vertex3(groundSize, -groundSize, -0.01f);
            GL.End();

            GL.Disable(EnableCap.Lighting);
            grid.Exectute();
            GL.Enable(EnableCap.Lighting);
        }

        public int draw ( List<OctreeObject> visList, bool drawNonVis )
        {
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            // draw what is vis
            GL.Color3(System.Drawing.Color.Red);

            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.PolygonOffset(-5.0f, 1.0f);

            foreach (OctreeObject o in visList)
            {
                BoxObject box = o as BoxObject;
                if (box != null)
                    box.draw();
            }

            GL.Disable(EnableCap.PolygonOffsetFill);

            if (drawNonVis)
            {
                GL.Color3(System.Drawing.Color.White);
                foreach( OctreeWorldObject o in objects )
                {
                    OctreeObject oc = o as OctreeObject;
                    if (!visList.Contains(oc))
                    {
                        BoxObject box = o as BoxObject;
                        if (box != null)
                            box.draw();
                    }
                }
            }
            return visList.Count;
        }
    }

    public class BoxObject : OctreeWorldObject
    {
        public Vector3 postion;
        public Vector3 size;
        public float rotation;

        public BoxObject( float posBounds ) : base()
        {
            Random rand = new Random();
            size = new Vector3(FloatRand.RandInRange(1,5), FloatRand.RandInRange(1,5), FloatRand.RandInRange(1,5));
            postion = new Vector3();
            postion.Z = FloatRand.RandInRange(size.Z, 5 + size.Z);
            float radius = size.X;
            if (size.Y > radius)
                radius = size.Y;
            postion.X = FloatRand.RandInRange(-posBounds + radius, posBounds - radius);
            postion.Y = FloatRand.RandInRange(-posBounds + radius, posBounds - radius);

            rotation = FloatRand.RandInRange(0, 360);

            if (size.Z > radius)
                radius = size.Z;
            bounds = BoundingBox.CreateFromSphere(new BoundingSphere(postion, radius));
        }

        public BoxObject(Vector3 pos, Vector3 s, float rot)
            : base()
        {
            postion = pos;
            size = s;
            rotation = rot;

            float radius = size.X;
            if (size.Y > radius)
                radius = size.Y;
            if (size.Z > radius)
                radius = size.Z;

            bounds = BoundingBox.CreateFromSphere(new BoundingSphere(postion, radius));
        }

        public BoxObject(BoundingBox box)
            : base()
        {
            size = box.Max - box.Min;
            size *= 0.5f;
            postion = box.Min + size;
            rotation = 0;
            bounds = new BoundingBox(box.Min, box.Max);
        }

        public void draw()
        {
            GL.PushMatrix();

            GL.Translate(postion);
            GL.Rotate(rotation, 0, 0, 1);

            GL.Begin(BeginMode.Quads);

            // top
            GL.Normal3(0, 0, 1.0f);
            GL.Vertex3(size.X, size.Y, size.Z);
            GL.Vertex3(-size.X, size.Y, size.Z);
            GL.Vertex3(-size.X, -size.Y, size.Z);
            GL.Vertex3(size.X, -size.Y, size.Z);

            // bottom
            GL.Normal3(0, 0, 1.0f);
            GL.Vertex3(size.X, size.Y, -size.Z);
            GL.Vertex3(size.X, -size.Y, -size.Z);
            GL.Vertex3(-size.X, -size.Y, -size.Z);
            GL.Vertex3(-size.X, size.Y, -size.Z);

            //X+
            GL.Normal3(1.0f, 0, 0);
            GL.Vertex3(size.X, size.Y, size.Z);
            GL.Vertex3(size.X, -size.Y, size.Z);
            GL.Vertex3(size.X, -size.Y, -size.Z);
            GL.Vertex3(size.X, size.Y, -size.Z);

            //X-
            GL.Normal3(-1.0f, 0, 0);
            GL.Vertex3(-size.X, size.Y, size.Z);
            GL.Vertex3(-size.X, size.Y, -size.Z);
            GL.Vertex3(-size.X, -size.Y, -size.Z);
            GL.Vertex3(-size.X, -size.Y, size.Z);

            //Y+
            GL.Normal3(0, 1.0f, 0);
            GL.Vertex3(size.X, size.Y, size.Z);
            GL.Vertex3(size.X, size.Y, -size.Z);
            GL.Vertex3(-size.X, size.Y, -size.Z);
            GL.Vertex3(-size.X, size.Y, size.Z);

            //Y-
            GL.Normal3(0, -1.0f, 0);
            GL.Vertex3(size.X, -size.Y, size.Z);
            GL.Vertex3(-size.X, -size.Y, size.Z);
            GL.Vertex3(-size.X, -size.Y, -size.Z);
            GL.Vertex3(size.X, -size.Y, -size.Z);

            GL.End();

            GL.PopMatrix();
        }
    }

    public class DebugableVisibleFrustum : VisibleFrustum
    {
        public DebugableVisibleFrustum ( VisibleFrustum val ) : base(val)
        {
        }

        void drawEyePoint()
        {
            // put a sphere at the eye point and draw the axes
            GL.Color4(0.25f, 0.25f, 1f, 0.5f);
            Glu.Sphere(Glu.NewQuadric(), 0.25f, 6, 6);

            GL.Disable(EnableCap.Lighting);

            GL.LineWidth(2.0f);
            GL.Begin(BeginMode.Lines);

            // the "up" vector
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(Up);
            GL.Vertex3(Up);
            GL.Vertex3(Up * 0.75f + RightVec * 0.35f);

            // the "right" vector
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(RightVec);
            GL.Vertex3(RightVec);
            GL.Vertex3(RightVec * 0.75f + Up * 0.25f);

            // the "forward" vector
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(ViewDir * 3f);
            GL.Vertex3(ViewDir * 3f);
            GL.Vertex3(ViewDir * 2.75f + Up * 0.125f);
            GL.Vertex3(ViewDir * 3f);
            GL.Vertex3(ViewDir * 2.75f + Up * -0.125f);

            GL.End();

            GL.Enable(EnableCap.Lighting);
        }

        void drawViewFrustum()
        {
            GL.Disable(EnableCap.Lighting);

            float endViewAlpha = 0.25f;
            float startViewAlpha = 0.75f;

            //GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            float planeAlphaFactor = 0.25f;
            GL.Begin(BeginMode.Triangles);

            // top
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[2] * farClip);

            // bottom
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[0] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[0] * farClip);

            // left
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[0] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[0] * farClip);

            // right
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Color4(1f, 1f, 1f, startViewAlpha * planeAlphaFactor);
            GL.Vertex3(0f, 0f, 0f);
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[2] * farClip);

            GL.End();

            GL.Begin(BeginMode.Quads);
            // far
            GL.Color4(1f, 1f, 1f, endViewAlpha * planeAlphaFactor);
            GL.Vertex3(edge[0] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[3] * farClip);
            GL.Vertex3(edge[2] * farClip);
            GL.Vertex3(edge[1] * farClip);
            GL.Vertex3(edge[0] * farClip);
            GL.End();

            GL.LineWidth(2.0f);
            // draw the edge vectors
            GL.Begin(BeginMode.Lines);
            foreach (Vector3 e in edge)
            {
                GL.Color4(1f, 1f, 1f, startViewAlpha);
                GL.Vertex3(0, 0, 0);
                GL.Color4(1f, 1f, 1f, endViewAlpha);
                GL.Vertex3(e * farClip);
            }
            GL.End();

            GL.LineWidth(3.0f);
            // compute the view plane points for normal drawing
            Vector3 leftPoint = (edge[0] + edge[3]) * 2;
            Vector3 rightPoint = (edge[1] + edge[2]) * 2;
            Vector3 topPoint = (edge[3] + edge[2]) * 2;
            Vector3 bottomPoint = (edge[0] + edge[1]) * 2;

            GL.Begin(BeginMode.Lines);

            GL.Color4(1f, 0f, 0f, 0.5f);
            GL.Vertex3(leftPoint);
            GL.Vertex3(leftPoint + Left.Normal);

            GL.Color4(0f, 1f, 0f, 0.5f);
            GL.Vertex3(rightPoint);
            GL.Vertex3(rightPoint + Right.Normal);

            GL.Color4(0f, 0f, 1f, 0.5f);
            GL.Vertex3(topPoint);
            GL.Vertex3(topPoint + Top.Normal);

            GL.Color4(1f, 0f, 1f, 0.5f);
            GL.Vertex3(bottomPoint);
            GL.Vertex3(bottomPoint + Bottom.Normal);

            GL.Color4(1f, 1f, 1f, endViewAlpha);
            GL.Vertex3(ViewDir * farClip);
            GL.Vertex3(ViewDir * farClip + Far.Normal);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            GL.Color4(0.5f, 0.5f, 0.5f, endViewAlpha);
            foreach (Vector3 e in edge)
                GL.Vertex3(e * farClip);
            GL.End();

            GL.PushMatrix();
            GL.Color4(1f, 1f, 1f, startViewAlpha);
            GL.Translate(ViewDir * nearClip);
            Glu.Sphere(Glu.NewQuadric(), 0.125f, 3, 2);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Color4(0.5f, 0.5f, 0.5f, endViewAlpha);
            GL.Translate(ViewDir * farClip);
            Glu.Sphere(Glu.NewQuadric(), 0.125f, 3, 2);
            GL.PopMatrix();

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            GL.Enable(EnableCap.Lighting);
        }

        public void drawFrustum()
        {
            GL.PushMatrix();
            //move to the eye point
            GL.Translate(EyePoint);

            drawEyePoint();

            drawViewFrustum();

            GL.PopMatrix();

            GL.Enable(EnableCap.Lighting);
        }
    }
}
