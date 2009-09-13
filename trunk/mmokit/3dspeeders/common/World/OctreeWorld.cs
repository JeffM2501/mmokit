using System;
using System.Collections.Generic;
using System.Text;

using Math3D;
using OpenTK.Math;

namespace World
{
    public class OctreeWorldObject : OctreeObject
    {
    }

    public class OctreeWorld : Octree
    {
        public List<OctreeWorldObject> objects = new List<OctreeWorldObject>();

        public void Add(OctreeWorldObject item)
        {
            objects.Add(item);
        }

        public void BuildTree( BoundingBox initalBounds )
        {
            foreach (OctreeWorldObject item in objects)
                ContainedObjects.Add(item as OctreeObject);

            ContainerBox = new BoundingBox(initalBounds.Min,initalBounds.Max);
            base.Distribute(0);
        }

        public void BuildTree()
        {
            foreach (OctreeWorldObject item in objects)
                ContainedObjects.Add(item as OctreeObject);

            Bounds();
            base.Distribute(0);
        }

        public void ObjectsInFrustum(List<OctreeObject> objects, BoundingFrustum boundingFrustum, bool exact)
        {
            base.ObjectsInFrustum(objects, boundingFrustum);
            if (exact)
            {
                for (int i = objects.Count-1; i >= 0; i--)
                {
                    if (!boundingFrustum.Intersects(objects[i].bounds))
                        objects.RemoveAt(i);
                }
            }
        }

        public void ObjectsInBoundingBox(List<OctreeObject> objects, BoundingBox boundingbox, bool exact)
        {
            base.ObjectsInBoundingBox(objects, boundingbox);
            if (exact)
            {
                for (int i = objects.Count - 1; i >= 0; i--)
                {
                    if (!boundingbox.Intersects(objects[i].bounds))
                        objects.RemoveAt(i);
                }
            }
        }
    }
}
