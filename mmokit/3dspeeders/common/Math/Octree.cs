using System;
using System.Collections.Generic;

using OpenTK.Math;

namespace Math3D
{
    public class OctreeObject
    {
        public BoundingBox bounds = new BoundingBox(new Vector3(), new Vector3());

    }

    public class OctreeLeaf
    {
        const int maxDepth = 35;

        int maxObjects = 2;
        public List<OctreeObject> containedObjects = new List<OctreeObject>();
        public List<OctreeLeaf> children = null;
        public BoundingBox bounds;

        public OctreeLeaf(BoundingBox containerBox)
        {
            bounds = containerBox;
        }

        public List<OctreeObject> ContainedObjects
        {
            get { return containedObjects; }
            set { containedObjects = value; }
        }

        public List<OctreeLeaf> ChildLeaves
        {
            get { return children; }
        }

        public BoundingBox ContainerBox
        {
            get { return bounds; }
            set { bounds = value; }
        }

        protected void Split()
        {
            if (children != null)
                return;

            Vector3 half = ContainerBox.Max - ContainerBox.Min;
            half *= 0.5f;
            Vector3 halfx = new Vector3(half.X, 0, 0);
            Vector3 halfy = new Vector3(0, half.Y, 0);
            Vector3 halfz = new Vector3(0, 0, half.Z);

            children = new List<OctreeLeaf>();

            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min, ContainerBox.Min + half)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfx, ContainerBox.Max - half + halfx)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfz, ContainerBox.Min + half + halfz)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfx + halfz, ContainerBox.Max - halfy)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy, ContainerBox.Max - halfx - halfz)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy + halfx, ContainerBox.Max - halfz)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + halfy + halfz, ContainerBox.Max - halfx)));
            ChildLeaves.Add(new OctreeLeaf(new BoundingBox(ContainerBox.Min + half, ContainerBox.Max)));
        }

        public void Distribute( int depth )
        {
            if (containedObjects.Count > maxObjects && depth <= maxDepth)
            {
                Split();
                for ( int i = containedObjects.Count -1; i >= 0; i--)// (OctreeObject item in containedObjects)
                {
                    OctreeObject item = containedObjects[i];
                    foreach (OctreeLeaf leaf in ChildLeaves)
                    {
                        if (leaf.ContainerBox.Contains(item.bounds) == ContainmentType.Contains)
                        {
                            leaf.ContainedObjects.Add(item);
                            containedObjects.Remove(item);
                            break;
                        }
                    }
                }

                depth++;
                foreach (OctreeLeaf leaf in ChildLeaves)
                    leaf.Distribute(depth);
                depth--;
            }
        }

        public virtual void ObjectsInFrustum(List<OctreeObject> objects, BoundingFrustum boundingFrustum)
        {
            foreach (OctreeObject item in containedObjects)
                objects.Add(item);

            if (ChildLeaves != null)
            {
                foreach (OctreeLeaf leaf in ChildLeaves)
                {
                    if (leaf.ContainerBox.Intersects(boundingFrustum))
                        leaf.ObjectsInFrustum(objects, boundingFrustum);
                }
            }
        }

        public virtual void ObjectsInBoundingBox(List<OctreeObject> objects, BoundingBox box)
        {
            foreach (OctreeObject item in containedObjects)
                objects.Add(item);

            if (ChildLeaves != null)
            {
                foreach (OctreeLeaf leaf in ChildLeaves)
                {
                    if (leaf.ContainerBox.Intersects(box))
                        leaf.ObjectsInBoundingBox(objects, box);
                }
            }
        }

        public virtual void ObjectsInBoundingSphere(List<OctreeObject> objects, BoundingSphere sphere)
        {
            foreach (OctreeObject item in containedObjects)
                objects.Add(item);

            if (ChildLeaves != null)
            {
                foreach (OctreeLeaf leaf in ChildLeaves)
                {
                    if (leaf.ContainerBox.Intersects(sphere))
                        leaf.ObjectsInBoundingSphere(objects, sphere);
                }
            }
        }
    }

    public class Octree : OctreeLeaf
    {
        public Octree()
            : base(new BoundingBox())
        {
        }

        public void Bounds()
        {
            foreach (OctreeObject item in ContainedObjects)
                ContainerBox = BoundingBox.CreateMerged(ContainerBox, item.bounds);
        }

        public virtual void Add(IEnumerable<OctreeObject> items)
        {
            foreach (OctreeObject item in items)
                ContainedObjects.Add(item);

            Bounds();
            base.Distribute(0);
        }

        public virtual void Add(OctreeObject item)
        {
            ContainedObjects.Add(item);

            Bounds();
            base.Distribute(0);
        }

        public override void ObjectsInFrustum(List<OctreeObject> objects, BoundingFrustum boundingFrustum)
        {
            base.ObjectsInFrustum(objects, boundingFrustum);
        }

        public override void ObjectsInBoundingBox(List<OctreeObject> objects, BoundingBox box)
        {
            base.ObjectsInBoundingBox(objects, box);
        }

        public override void ObjectsInBoundingSphere(List<OctreeObject> objects, BoundingSphere sphere)
        {
            base.ObjectsInBoundingSphere(objects, sphere);
        }
    }
}
