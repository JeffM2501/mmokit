using System;
using System.Collections.Generic;

using Drawables.Materials;
using Drawables.Models;
using World;

namespace GraphicWorld
{
    public class GraphicWorld
    {
        public Dictionary<string, Material> materials;
        public Dictionary<string, Model> models;
        public ObjectWorld world;
    }
}
