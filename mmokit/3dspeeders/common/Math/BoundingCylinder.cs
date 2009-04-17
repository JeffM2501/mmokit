#region License
/*
 * Origonal *

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 * 
*/
#endregion License

using System;
using System.Collections.Generic;
using System.Globalization;

using OpenTK.Math;

namespace Math3D
{
    [Serializable]
    public struct BoundingCylinderXY : IEquatable<BoundingCylinderXY>
    {
        #region Public Fields

        public Vector2 Center;
        public float MaxZ;
        public float MinZ;
        public float Radius;

        #endregion Public Fields

        #region Public Constructors

        public BoundingCylinderXY( Vector3 cp, float height, float radius )
        {
            Center.X = cp.X;
            Center.Y = cp.Y;

            Radius = radius;

            MinZ = cp.Z;
            MaxZ = cp.Z + height;
        }

        #endregion Public Constructors

        #region Public Methods

        public static BoundingCylinderXY Empty = new BoundingCylinderXY(new Vector3(0, 0, 0), 0, 0);

        public ContainmentType Contains(BoundingBox box)
        {
            // above or below
            if (box.Min.Z > MaxZ || box.Max.Z < MinZ)
                return ContainmentType.Disjoint;

            // for containment it MUST fit in Z
            if (MaxZ <= box.Max.Z && MinZ >= box.Min.Z)
            {
                if (!pointInXY(box.Max.X, box.Max.Y) || !pointInXY(box.Min.X, box.Max.Y) || !pointInXY(box.Min.X, box.Min.Y) || !pointInXY(box.Max.X, box.Min.Y))
                    return ContainmentType.Intersects;

                return ContainmentType.Contains;
            }

            if (!pointInXY(box.Max.X, box.Max.Y) || !pointInXY(box.Min.X, box.Max.Y) || !pointInXY(box.Min.X, box.Min.Y) || !pointInXY(box.Max.X, box.Min.Y))
                return ContainmentType.Disjoint;

            return ContainmentType.Intersects;
        }

        public bool Equals(BoundingCylinderXY other)
        {
            return this.Center == other.Center && this.Radius == other.Radius && this.MinZ == other.MinZ && this.MaxZ == other.MaxZ;
        }

        public override bool Equals(object obj)
        {
            if (obj is BoundingCylinderXY)
                return this.Equals((BoundingCylinderXY)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return this.Center.GetHashCode() + this.Radius.GetHashCode() + this.MaxZ.GetHashCode() + this.MinZ.GetHashCode();
        }

        public static bool operator ==(BoundingCylinderXY a, BoundingCylinderXY b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BoundingCylinderXY a, BoundingCylinderXY b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Center:{0} Radius:{1} MinZ:{2} MaxZ:{3}}}", this.Center.ToString(), this.Radius.ToString(),this.MinZ.ToString(),this.MaxZ.ToString());
        }

        #endregion Public Methods

        #region Private Methods

        bool pointInXY(float X, float Y)
        {
            float distSquare = (X - Center.X) * (X - Center.X) + (Y - Center.Y) * (Y - Center.Y);
            return distSquare <= Radius * Radius;
        }

        #endregion Private Methods
    }
}