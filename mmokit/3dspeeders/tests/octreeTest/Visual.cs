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

using Cameras;
using Grids;
using World;
using Math3D;
using Drawables.Materials;

namespace octreeTest
{
    public class Visual : GameWindow
    {
        Camera camera = new Camera();
        Grid grid = new Grid();
        BoundingFrustum frustum = new BoundingFrustum();

        OctreeWorld world = new OctreeWorld();

        Dictionary<OctreeLeaf, GLColor> treeColorMap = new Dictionary<OctreeLeaf, GLColor>();

        float groundSize = 100.0f;

        bool showOctreeObjects = false;
        bool cullToFrustum = false;

        int drawnObject = 0;

        float[] viewMat = new float[16];

        Vector3 cullBoxPos = new Vector3();

        TextPrinter printer = new TextPrinter(TextQuality.High);
        Font sans_serif = new Font(FontFamily.GenericSansSerif, 16.0f);

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

            camera.set(new Vector3(0, 0, 1), 0, 0);

            grid.gridSize = groundSize;
            grid.majorSpacing = 10.0f;
            grid.minorSpacing = 2.0f;
            grid.alpha = 0.5f;

            bool fixedMap = false;

            if (!fixedMap)
            {
                int boxes = 250;// new Random().Next() % 10 + 100;
                for (int i = 0; i < boxes; i++)
                    world.Add(new BoxObject(groundSize));
                world.BuildTree(new BoundingBox(new Vector3(-groundSize, -groundSize, -1), new Vector3(groundSize, groundSize, 50)));
            }
            else
            {
                float step = 25;
                Vector3 size = new Vector3(0.5f, 0.5f, 0.5f);
                for (float x = -groundSize + size.X; x < groundSize - size.X; x += step)
                {
                    for (float y = -groundSize + size.Y; y < groundSize - size.Y; y += step)
                        world.Add(new BoxObject(new Vector3(x, y, 0.5f), size, 0));
                }

                world.BuildTree(new BoundingBox(new Vector3(-groundSize, -groundSize, -1), new Vector3(groundSize, groundSize, size.Z * 5)));
            }
        }

        public override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Glu.Perspective(45.0, Width / (double)Height, 1.0, 1000.0);

            float[] projMat = new float[16];
            GL.GetFloat(GetPName.ProjectionMatrix, projMat);
            frustum.updateProjection(projMat);
        }

        protected bool doInput (UpdateFrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                return true;

            float turnSpeed = 50.0f;
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
                showOctreeObjects = true;
            if (Keyboard[Key.F2])
                showOctreeObjects = false;

            if (Keyboard[Key.F3])
                cullToFrustum = true;
            if (Keyboard[Key.F4])
                cullToFrustum = false;

            Vector3 forward = new Vector3(camera.Heading());
            Vector3 leftward = new Vector3(forward);
            leftward.X = -forward.Y;
            leftward.Y = forward.X;

            Vector2 movement = new Vector2();

            float speed = 50.0f;
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

        protected void drawGround()
        {
            GL.Color3(System.Drawing.Color.LightGreen);

            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(100, 100, -0.01f);
            GL.Vertex3(-100, 100, -0.01f);
            GL.Vertex3(-100, -100, -0.01f);
            GL.Vertex3(100, -100, -0.01f);
            GL.End();

            GL.Disable(EnableCap.Lighting);
            grid.Exectute();
            GL.Enable(EnableCap.Lighting);
        }

        void drawOverlay(RenderFrameEventArgs e)
        {
            GL.Disable(EnableCap.Lighting);
            printer.Begin();
            printer.Print(((int)(1 / e.Time)).ToString("F0"), sans_serif, Color.Wheat);
            printer.Print("Forward(" + camera.HeadingAngle().ToString() + ") " + camera.Heading().ToString(), sans_serif, Color.Wheat, new RectangleF(0, Height - 36, Width, Height), TextPrinterOptions.Default);
            printer.Print("Drawn Objects(" + drawnObject.ToString() + ")", sans_serif, Color.Wheat, new RectangleF(0, Height - 64, Width, Height - 36), TextPrinterOptions.Default);

            printer.End();
        }

        void drawBox (BoxObject box)
        {
            if (box != null)
            {
                GL.PushMatrix();

                GL.Translate(box.postion);
                GL.Rotate(box.rotation, 0, 0, 1);

                GL.Begin(BeginMode.Quads);

                // top
                GL.Normal3(0, 0, 1.0f);
                GL.Vertex3(box.size.X, box.size.Y, box.size.Z);
                GL.Vertex3(-box.size.X, box.size.Y, box.size.Z);
                GL.Vertex3(-box.size.X, -box.size.Y, box.size.Z);
                GL.Vertex3(box.size.X, -box.size.Y, box.size.Z);

                // bottom
                GL.Normal3(0, 0, 1.0f);
                GL.Vertex3(box.size.X, box.size.Y, -box.size.Z);
                GL.Vertex3(box.size.X, -box.size.Y, -box.size.Z);
                GL.Vertex3(-box.size.X, -box.size.Y, -box.size.Z);
                GL.Vertex3(-box.size.X, box.size.Y, -box.size.Z);

                //X+
                GL.Normal3(1.0f, 0, 0);
                GL.Vertex3(box.size.X, box.size.Y, box.size.Z);
                GL.Vertex3(box.size.X, -box.size.Y, box.size.Z);
                GL.Vertex3(box.size.X, -box.size.Y, -box.size.Z);
                GL.Vertex3(box.size.X, box.size.Y, -box.size.Z);

                //X-
                GL.Normal3(-1.0f, 0, 0);
                GL.Vertex3(-box.size.X, box.size.Y, box.size.Z);
                GL.Vertex3(-box.size.X, box.size.Y, -box.size.Z);
                GL.Vertex3(-box.size.X, -box.size.Y, -box.size.Z);
                GL.Vertex3(-box.size.X, -box.size.Y, box.size.Z);

                //Y+
                GL.Normal3(0, 1.0f, 0);
                GL.Vertex3(box.size.X, box.size.Y, box.size.Z);
                GL.Vertex3(box.size.X, box.size.Y, -box.size.Z);
                GL.Vertex3(-box.size.X, box.size.Y, -box.size.Z);
                GL.Vertex3(-box.size.X, box.size.Y, box.size.Z);

                //Y-
                GL.Normal3(0, -1.0f, 0);
                GL.Vertex3(box.size.X, -box.size.Y, box.size.Z);
                GL.Vertex3(-box.size.X, -box.size.Y, box.size.Z);
                GL.Vertex3(-box.size.X, -box.size.Y, -box.size.Z);
                GL.Vertex3(box.size.X, -box.size.Y, -box.size.Z);

                GL.End();

                GL.PopMatrix();
            }
        }

        void drawOctreeNodes (OctreeLeaf leaf)
        {
            if (!treeColorMap.ContainsKey(leaf))
                treeColorMap.Add(leaf, new GLColor(FloatRand.RandInRange(0, 1), FloatRand.RandInRange(0, 1), FloatRand.RandInRange(0, 1), 0.75f));

            treeColorMap[leaf].glColor();

            drawBox(new BoxObject(leaf.bounds));
            if (leaf.children != null)
            {
                foreach (OctreeLeaf l in leaf.children)
                    drawOctreeNodes(l);
            }
        }

        void drawWorld()
        {
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            if (cullToFrustum)
            {
                if (showOctreeObjects)
                {
                    GL.Color3(System.Drawing.Color.White);
                    drawnObject = world.objects.Count;
                    foreach (OctreeWorldObject o in world.objects)
                        drawBox(o as BoxObject);
                }

                List<OctreeObject> vis = new List<OctreeObject>();
                world.ObjectsInFrustum(vis, frustum);
                drawnObject = vis.Count;

                GL.Color3(System.Drawing.Color.Red);

                GL.Enable(EnableCap.PolygonOffsetFill);
                GL.PolygonOffset(-1.0f, 1.0f);
                foreach (OctreeObject o in vis)
                    drawBox(o as BoxObject);
                GL.Disable(EnableCap.PolygonOffsetFill);
            }
            else
            {
                GL.Color3(System.Drawing.Color.White);
                drawnObject = world.objects.Count;
                foreach (OctreeWorldObject o in world.objects)
                    drawBox(o as BoxObject);

                if (showOctreeObjects)
                {
                    List<OctreeObject> vis = new List<OctreeObject>();

                    Vector3 boundSize = new Vector3(5,5,5);

                    BoundingBox visBox = new BoundingBox(boundSize * -1.0f + cullBoxPos, boundSize + cullBoxPos);
                    world.ObjectsInBoundingBox(vis, visBox);
                    drawnObject = vis.Count;

                    GL.Color3(System.Drawing.Color.Red);

                    GL.Enable(EnableCap.PolygonOffsetFill);
                    GL.PolygonOffset(-1.0f, 1.0f);
                    foreach (OctreeObject o in vis)
                        drawBox(o as BoxObject);
                    GL.Disable(EnableCap.PolygonOffsetFill);

                    GL.Color4(0.0f, 0.25f, 0.5f, 0.5f);
                    drawBox(new BoxObject(visBox));

                    GL.Disable(EnableCap.DepthTest);
                    GL.Disable(EnableCap.CullFace);
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    drawOctreeNodes(world);
                    GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                    GL.Enable(EnableCap.DepthTest);
                    GL.Enable(EnableCap.CullFace);

                    
                    GL.Color4(1.0f,1.0f,1.0f,1.0f);
                }
            }

        }

        public override void OnRenderFrame(RenderFrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            camera.Execute();

            GL.GetFloat(GetPName.ModelviewMatrix, viewMat);
            frustum.update(viewMat);

            GL.Enable(EnableCap.Light0);
            GL.Lightv(LightName.Light0, LightParameter.Position, new Vector4(10, 15, 10, 1.0f));

            drawGround();

            drawWorld();
            Glu.Sphere(Glu.NewQuadric(), 1, 25, 25);

            drawOverlay(e);

            SwapBuffers();
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
            postion = box.CenterPoint;
            size = box.BoxSize;
            rotation = 0;
            bounds = new BoundingBox(box.Min, box.Max);
        }
    }
}
