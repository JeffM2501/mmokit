using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;


namespace GLRenderer
{
    class Renderer
    {
        private Form theForm;

        private IntPtr hDC;                                              // Private GDI Device Context
        private IntPtr hRC;                                              // Permanent Rendering Context
        private bool active = true;                                      // Window Active Flag, Set To True By Default

        // events

        private void activated(object sender, EventArgs e)
        {
            active = true;                                                      // Program Is Active
        }

        private void deactivate(object sender, EventArgs e)
        {
            active = false;                                                     // Program Is No Longer Active
        }

        private void resize(object sender, EventArgs e)
        {
            ReSizeGLScene(theForm.Width, theForm.Height);                             // Resize The OpenGL Window
        }

        public bool contextActive ()
        {
            return active;
        }
        public Renderer( Form form )
        {
            theForm = form;

//             theForm.CreateParams.ClassStyle = theForm.CreateParams.ClassStyle | User.CS_HREDRAW | User.CS_VREDRAW | User.CS_OWNDC;      // Redraw On Size, And Own DC For Window.
//             theForm.SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
//             theForm.SetStyle(ControlStyles.DoubleBuffer, true);                    // Buffer Control
//             theForm.SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
//             theForm.SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
//             theForm.SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves
// 
            theForm.Activated += new EventHandler(activated);            // On Activate Event Call Form_Activated
            theForm.Deactivate += new EventHandler(deactivate);          // On Deactivate Event Call Form_Deactivate
            theForm.Resize += new EventHandler(resize);                  // On Resize Event Call Form_Resize
          //  theForm.Paint += new PaintEventHandler(paint);                  // On Paint

            // create the window
            CreateGLWindow(32);
        }

        public bool setFullScreen ( int width, int height, int bits )
        {
            Gdi.DEVMODE dmScreenSettings = new Gdi.DEVMODE();               // Device Mode
            // Size Of The Devmode Structure
            dmScreenSettings.dmSize = (short)Marshal.SizeOf(dmScreenSettings);
            dmScreenSettings.dmPelsWidth = width;                           // Selected Screen Width
            dmScreenSettings.dmPelsHeight = height;                         // Selected Screen Height
            dmScreenSettings.dmBitsPerPel = bits;                           // Selected Bits Per Pixel
            dmScreenSettings.dmFields = Gdi.DM_BITSPERPEL | Gdi.DM_PELSWIDTH | Gdi.DM_PELSHEIGHT;

            // Try To Set Selected Mode And Get Results.  NOTE: CDS_FULLSCREEN Gets Rid Of Start Bar.
            if (User.ChangeDisplaySettings(ref dmScreenSettings, User.CDS_FULLSCREEN) != User.DISP_CHANGE_SUCCESSFUL)
            {
                // If The Mode Fails, Offer Two Options.  Quit Or Use Windowed Mode.
                if (MessageBox.Show("The Requested Fullscreen Mode Is Not Supported By\nYour Video Card.  Use Windowed Mode Instead?", "NeHe GL",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    return true;                                    // Windowed Mode Selected.  Fullscreen = false
                }
                else
                {
                    // Pop up A Message Box Lessing User Know The Program Is Closing.
                    MessageBox.Show("Program Will Now Close.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;                                           // Return false
                }
            }

            return true;
        }

        public bool unsetFullScreen(  )
        {
            User.ChangeDisplaySettings(IntPtr.Zero, 0);                     // If So, Switch Back To The Desktop
            Cursor.Show();                                                  // Show Mouse Pointer
            return true;
        }

        private bool CreateGLWindow( int bits )
        {
            int pixelFormat;                                                    // Holds The Results After Searching For A Match

            GC.Collect();                                                       // Request A Collection
            // This Forces A Swap
            Kernel.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

            Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();    // pfd Tells Windows How We Want Things To Be
            pfd.nSize = (short)Marshal.SizeOf(pfd);                            // Size Of This Pixel Format Descriptor
            pfd.nVersion = 1;                                                   // Version Number
            pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW |                              // Format Must Support Window
                Gdi.PFD_SUPPORT_OPENGL |                                        // Format Must Support OpenGL
                Gdi.PFD_DOUBLEBUFFER;                                           // Format Must Support Double Buffering
            pfd.iPixelType = (byte)Gdi.PFD_TYPE_RGBA;                          // Request An RGBA Format
            pfd.cColorBits = (byte)bits;                                       // Select Our Color Depth
            pfd.cRedBits = 0;                                                   // Color Bits Ignored
            pfd.cRedShift = 0;
            pfd.cGreenBits = 0;
            pfd.cGreenShift = 0;
            pfd.cBlueBits = 0;
            pfd.cBlueShift = 0;
            pfd.cAlphaBits = 0;                                                 // No Alpha Buffer
            pfd.cAlphaShift = 0;                                                // Shift Bit Ignored
            pfd.cAccumBits = 0;                                                 // No Accumulation Buffer
            pfd.cAccumRedBits = 0;                                              // Accumulation Bits Ignored
            pfd.cAccumGreenBits = 0;
            pfd.cAccumBlueBits = 0;
            pfd.cAccumAlphaBits = 0;
            pfd.cDepthBits = 16;                                                // 16Bit Z-Buffer (Depth Buffer)
            pfd.cStencilBits = 0;                                               // No Stencil Buffer
            pfd.cAuxBuffers = 0;                                                // No Auxiliary Buffer
            pfd.iLayerType = (byte)Gdi.PFD_MAIN_PLANE;                         // Main Drawing Layer
            pfd.bReserved = 0;                                                  // Reserved
            pfd.dwLayerMask = 0;                                                // Layer Masks Ignored
            pfd.dwVisibleMask = 0;
            pfd.dwDamageMask = 0;

            hDC = User.GetDC(theForm.Handle);                                      // Attempt To Get A Device Context
            if (hDC == IntPtr.Zero)
            {                                            // Did We Get A Device Context?
                KillGLWindow();                                                 // Reset The Display
                MessageBox.Show("Can't Create A GL Device Context.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            pixelFormat = Gdi.ChoosePixelFormat(hDC, ref pfd);                  // Attempt To Find An Appropriate Pixel Format
            if (pixelFormat == 0)
            {                                              // Did Windows Find A Matching Pixel Format?
                KillGLWindow();                                                 // Reset The Display
                MessageBox.Show("Can't Find A Suitable PixelFormat.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Gdi.SetPixelFormat(hDC, pixelFormat, ref pfd))
            {                // Are We Able To Set The Pixel Format?
                KillGLWindow();                                                 // Reset The Display
                MessageBox.Show("Can't Set The PixelFormat.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            hRC = Wgl.wglCreateContext(hDC);                                    // Attempt To Get The Rendering Context
            if (hRC == IntPtr.Zero)
            {                                            // Are We Able To Get A Rendering Context?
                KillGLWindow();                                                 // Reset The Display
                MessageBox.Show("Can't Create A GL Rendering Context.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Wgl.wglMakeCurrent(hDC, hRC))
            {                                 // Try To Activate The Rendering Context
                KillGLWindow();                                                 // Reset The Display
                MessageBox.Show("Can't Activate The GL Rendering Context.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            theForm.Show();                                                        // Show The Window
            theForm.Focus();                                                       // Focus The Window

            ReSizeGLScene(theForm.Width, theForm.Height);                                       // Set Up Our Perspective GL Screen

            if (!InitGL())
            {                                                     // Initialize Our Newly Created GL Window
                KillGLWindow();                                                 // Reset The Display
                MessageBox.Show("Initialization Failed.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;                                                        // Success
        }

        private bool InitGL()
        {
            Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
            Gl.glClearColor(0, 0, 0, 1);                                     // Black Background
            Gl.glClearDepth(1);                                                 // Depth Buffer Setup
            Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
            Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         // Really Nice Perspective Calculations
            return true;
        }

        private static void ReSizeGLScene(int width, int height)
        {
            if (height == 0)
                height = 1;                                                     // By Making Height Equal To One

            Gl.glViewport(0, 0, width, height);                                 // Reset The Current Viewport
            Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
            Gl.glLoadIdentity();                                                // Reset The Projection Matrix

            Gl.glOrtho(0, width, 0, height, 0, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
            Gl.glLoadIdentity();                                                // Reset The Modelview Matrix
        }

        private void KillGLWindow()
        {
            if (hRC != IntPtr.Zero)
            {                                            // Do We Have A Rendering Context?
                if (!Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero))// Are We Able To Release The DC and RC Contexts?
                    MessageBox.Show("Release Of DC And RC Failed.", "SHUTDOWN ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (!Wgl.wglDeleteContext(hRC))// Are We Able To Delete The RC?
                    MessageBox.Show("Release Rendering Context Failed.", "SHUTDOWN ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                hRC = IntPtr.Zero;                                              // Set RC To Null
            }

            if (hDC != IntPtr.Zero)
            {                                            // Do We Have A Device Context?
                if (theForm != null && !theForm.IsDisposed)
                {                          // Do We Have A Window?
                    if (theForm.Handle != IntPtr.Zero)
                    {                            // Do We Have A Window Handle?
                        if (!User.ReleaseDC(theForm.Handle, hDC))
                        {                 // Are We Able To Release The DC?
                            MessageBox.Show("Release Device Context Failed.", "SHUTDOWN ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                hDC = IntPtr.Zero;                                              // Set DC To Null
            }            
        }

        public bool beginDraw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        // Clear Screen And Depth Buffer
            Gl.glLoadIdentity();                                                // Reset The Current Modelview Matrix
            return true;
        }

        public bool endDraw()
        {
            Gdi.SwapBuffers(hDC);                                   // Swap Buffers (Double Buffering)
            return true;
        }

        public void setClearColor(float r, float g, float b)
        {
            Gl.glClearColor(r, g, b, 1);                                     // Black Background
        }

        public float Width ( )
        {
            return theForm.Width;
        }

        public float Height ( )
        {
            return theForm.Height;
        }
    }
}
