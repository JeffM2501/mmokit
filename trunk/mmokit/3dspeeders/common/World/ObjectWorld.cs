using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using Math3D;

namespace World
{
    public class WorldObject : OctreeObject
    {
        public CollisionBoundry Boundry = new CollisionBoundry();
        public string ObjectName = string.Empty;
        public object Tag;
        public List<string> Attributes = new List<string>();
    }

    public class ObjectWorld : Octree
    {
        List<WorldObject> objects = new List<WorldObject>();

        // static list just so we don't have to new one each frame
        [System.Xml.Serialization.XmlIgnoreAttribute]
        List<OctreeObject> visList = new List<OctreeObject>();

        public void Add(WorldObject item)
        {
            // make sure it has some bounds
            if (item.bounds == BoundingBox.Empty)
                item.bounds = item.Boundry.Bounds();

            objects.Add(item);
        }

        public void BuildTree(BoundingBox initalBounds)
        {
            foreach (WorldObject item in objects)
                ContainedObjects.Add(item as OctreeObject);

            ContainerBox = new BoundingBox(initalBounds.Min, initalBounds.Max);
            base.Distribute(0);
        }

        public void BuildTree()
        {
            foreach (WorldObject item in objects)
                ContainedObjects.Add(item as OctreeObject);

            Bounds();
            base.Distribute(0);
        }

        public void ObjectsInFrustum(List<WorldObject> visibleObjects, BoundingFrustum boundingFrustum, bool exact)
        {
            visList.Clear();
            base.ObjectsInFrustum(visList, boundingFrustum);

            foreach(OctreeObject o in visList)
            {
                if (exact)
                {
                    if (boundingFrustum.Intersects(o.bounds))
                        visibleObjects.Add(o as WorldObject);
                }
                else
                    visibleObjects.Add(o as WorldObject);
            }
        }

        public void ObjectsInBoundingBox(List<WorldObject> visibleObjects, BoundingBox boundingbox, bool exact)
        {
            visList.Clear();
            base.ObjectsInBoundingBox(visList, boundingbox);

            foreach (OctreeObject o in visList)
            {
                if (exact)
                {
                    if (boundingbox.Intersects(o.bounds))
                        visibleObjects.Add(o as WorldObject);
                }
                else
                    visibleObjects.Add(o as WorldObject);
            }
        }

        public void ObjectsInBoundingSphere(List<WorldObject> visibleObjects, BoundingSphere boundingSphere, bool exact)
        {
            visList.Clear();
            base.ObjectsInBoundingSphere(visList, boundingSphere);

            foreach (OctreeObject o in visList)
            {
                if (exact)
                {
                    if (boundingSphere.Intersects(o.bounds))
                        visibleObjects.Add(o as WorldObject);
                }
                else
                    visibleObjects.Add(o as WorldObject);
            }
        }

        // TODO, implement all the cylinder intersection stuff, so we don't have to do this from a box
        public void ObjectsInBoundingCylinder(List<WorldObject> visibleObjects, BoundingCylinderXY boundingCylinder, bool exact)
        {
            ObjectsInBoundingBox(visibleObjects, BoundingBox.CreateFromCylinderXY(boundingCylinder), exact);
        }
    }
}
