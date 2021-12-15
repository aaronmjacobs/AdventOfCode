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
}
