using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom;
using Axiom.Core;
using Axiom.Graphics;
using Axiom.Configuration;

namespace _3dSpeeders
{
    public class GameVisual
    {
        ConnectionInfo conInfo;
        Root root;

        RenderWindow window;

        public GameVisual (ConnectionInfo info)
        {
            root = info.root;
            info.root = null;

            root.RenderSystem = info.renderSystem;
            info.renderSystem = null;

            conInfo = info;

            window = root.Initialize(true,"3dSpeeders");

        }
    }
}
