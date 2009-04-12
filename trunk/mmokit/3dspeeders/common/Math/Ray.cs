using System;
using System.Collections.Generic;

using OpenTK.Math;

namespace Math3D
{
    public class Ray
    {
        public Vector3 Direction;
        public Vector3 Position;

        public Ray (Vector3 position, Vector3 direction)
        {
            Direction = direction;
            Direction.Normalize();
            Position = position;
        }

        public static bool operator !=(Ray a, Ray b)
        {
            return a.Direction != b.Direction || a.Position != b.Position;
        }

        public static bool operator ==(Ray a, Ray b)
        {
            return a.Direction == b.Direction && a.Position == b.Position;
        }

        public bool Equals(Ray other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            Ray other = obj as Ray;
            if (other == null)
                return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            return Direction.GetHashCode() ^ Position.GetHashCode();
        }

        public override string ToString()
        {
            return Direction.ToString() + Position.ToString(); ;
        }
    }
}
