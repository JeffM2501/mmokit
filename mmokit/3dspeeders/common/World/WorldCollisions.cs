using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using OpenTK.Math;
using Math3D;

namespace World
{
    public enum CollisionBoundryType
    {
        None,
        AxisBox,
        RotatedBox,
        Sphere,
        Cylinder,
    }

    public class CollisionBoundry
    {
        public CollisionBoundryType type = CollisionBoundryType.AxisBox;
        public Vector3 center = new Vector3();
        public Vector3 bounds = new Vector3();
        public Vector3 Orientation = new Vector3();

        // cache for prims
        [System.Xml.Serialization.XmlIgnoreAttribute]
        BoundingBox box = BoundingBox.Empty;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        BoundingSphere sphere = BoundingSphere.Empty;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        BoundingCylinderXY cylinder = BoundingCylinderXY.Empty;

        public BoundingBox Bounds ()
        {
            switch(type)
            {
                case CollisionBoundryType.Sphere:
                    return BoundingBox.CreateFromSphere(getSphere());

                case CollisionBoundryType.Cylinder:
                    return BoundingBox.CreateFromCylinderXY(getCylinder());
            }

            return getBox();
        }

        BoundingBox getBox ()
        {
            if (box == BoundingBox.Empty)
                box = new BoundingBox(center + -bounds, center + bounds);
            return box;
        }

        BoundingSphere getSphere()
        {
            if (sphere == BoundingSphere.Empty)
                sphere = new BoundingSphere(center,bounds.X);
            return sphere;
        }

        BoundingCylinderXY getCylinder()
        {
            if (cylinder == BoundingCylinderXY.Empty)
                cylinder = new BoundingCylinderXY(center, bounds.X, bounds.Z);

            return cylinder;
        }
    }
}
