using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dSpeeders
{
    public class RenderStateArgs
    {
        public float x, y;
        public bool fullscreen = false;
    }

    public class Scene
    {
        public virtual void load(RenderStateArgs e)
        {

        }

        public virtual void unload(RenderStateArgs e)
        {

        }

        public virtual void draw ( RenderStateArgs e )
        {

        }
    }
}
