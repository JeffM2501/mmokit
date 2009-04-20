using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using Math3D;
using OpenTK.Math;

namespace World
{
    public class WorldObject : OctreeObject
    {
        public CollisionBoundry boundry = new CollisionBoundry();

        public string name = string.Empty;

        // generic attributes
        public List<string> attributes = new List<string>();

        // mesh
        public string objectName = string.Empty;
        public string skin = string.Empty;
        public bool scaleSkinToSize = false;

        // mesh position data
        public Vector3 postion = new Vector3();
        public Vector3 rotation = new Vector3();
        public Vector3 scale = new Vector3(1,1,1);

        // used for non mesh stuff, like spawns and lights
        public bool skipTree = false;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public object tag = null;
    }

    public class ObjectWorld : Octree
    {
        public List<WorldObject> objects = new List<WorldObject>();

        // general world info
        public Vector3 size = new Vector3(100, 100, 50);

        // basic rendering info (non gl specific)
        public string groundMaterialName = string.Empty;
        public string wallMaterialName = string.Empty;
        public float groundUVSize = 1.0f;
        public float wallHeight = 5.0f;
        public string backgroundName = string.Empty;

        // static list just so we don't have to new one each frame
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public List<OctreeObject> visList = new List<OctreeObject>();

        public void Flush ()
        {
            visList.Clear();
            containedObjects.Clear();
            if (children != null)
                children.Clear();
            children = null;
        }

        public void Add(WorldObject item)
        {
            if (item.skipTree)
                return;
            // make sure it has some bounds
            if (item.bounds == BoundingBox.Empty)
                item.bounds = item.boundry.Bounds();

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
