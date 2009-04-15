using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Math;

namespace Math3D
{
    public class VizableFrustum : BoundingFrustum
    {
        Matrix4 view = Matrix4.Identity;
        public Matrix4 ViewMatrix
        {
            get { return view; }
            set { view = ViewMatrix; BuildMatrix(); }
        }
        Matrix4 projection = Matrix4.Identity;
        public Matrix4 ProjectionMatrix
        {
            get { return projection; }
            set { projection = ProjectionMatrix; BuildMatrix(); }
        }

        public VizableFrustum() : base (Matrix4.Identity)
        {

        }

        #region Protected Methods

        protected void BuildMatrix()
        {
            matrix = Matrix4.Mult(view,projection);
            CreatePlanes();
            CreateCorners();
        }

        #endregion
   }
}
