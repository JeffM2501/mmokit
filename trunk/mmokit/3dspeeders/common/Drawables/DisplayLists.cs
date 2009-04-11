using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;

namespace Drawables.DisplayLists
{
    class DisplayList
    {
        int listID = -1;

        bool generating = false;

        public bool Valid ()
        {
            return listID != -1;
        }

        public void Invalidate()
        {
            if (generating)
                End();

            if (listID != -1)
                GL.DeleteLists(listID,1);

            listID = -1;
        }

        public void Start ()
        {
            Start(false);
        }

        public void Start( bool execute )
        {
            Invalidate();
            listID = GL.GenLists(1);
            if (execute)
                GL.NewList(listID, ListMode.CompileAndExecute);
            else
                GL.NewList(listID, ListMode.Compile);

            generating = true;
        }

        public void End( )
        {
            if (generating)
                GL.EndList();

            generating = false;
        }

        public bool Call()
        {
            if (generating || !Valid())
                return false;

            GL.CallList(listID);
            return true;
        }
    }

    class DisplayListSystem
    {
        public static DisplayListSystem system = new DisplayListSystem();
    }
}
