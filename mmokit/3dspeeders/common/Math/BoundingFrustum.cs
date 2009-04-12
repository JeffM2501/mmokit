using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Math;
namespace Math3D
{

    public class BoundingFrustum
    {
         public const int CornerCount = 8;
         Matrix4 matrix;

         public Plane Near;
         public Plane Far;
         public Plane Left;
         public Plane Right;
         public Plane Top;
         public Plane Bottom;

         public BoundingFrustum(ref Matrix4 value)
         {
             matrix = value;
         }

         public void update (ref Matrix4 value)
         {
             matrix = value;
         }
    }
}
