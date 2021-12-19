using System.Diagnostics.CodeAnalysis;

namespace Common
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator+(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator*(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        public static double Distance(Point a, Point b)
        {
            int xDiff = a.X - b.X;
            int yDiff = a.Y - b.Y;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }

        public static int ManhattanDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Point p && p.X == X && p.Y == Y;
        }

        public static bool operator==(Point lhs, Point rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(Point lhs, Point rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    public struct Point3D
    {
        public int X;
        public int Y;
        public int Z;

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3D operator +(Point3D a, Point3D b)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Point3D operator *(Point3D a, Point3D b)
        {
            return new Point3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static double Distance(Point3D a, Point3D b)
        {
            int xDiff = a.X - b.X;
            int yDiff = a.Y - b.Y;
            int zDiff = a.Z - b.Z;
            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff + zDiff * zDiff);
        }

        public static int ManhattanDistance(Point3D a, Point3D b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Point3D p && p.X == X && p.Y == Y && p.Z == Z;
        }

        public static bool operator ==(Point3D lhs, Point3D rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Point3D lhs, Point3D rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}
