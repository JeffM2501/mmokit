using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;

namespace modeler
{
    public class GLListable
    {
        protected int GLList = -1;

        public void Invalidate ()
        {
            if (GLList == -1)
                return;

            GL.DeleteLists(GLList, 1);
            GLList = -1;
        }

        protected void Rebuild ()
        {
            GLList = GL.GenLists(1);

            GL.NewList(GLList, ListMode.Compile);
            GenerateList();
            GL.EndList();
        }

        protected virtual void GenerateList()
        {

        }

        public virtual void Execute ()
        {
            if (GLList == -1)
                Rebuild();

            GL.CallList(GLList);
        }
    }
}
