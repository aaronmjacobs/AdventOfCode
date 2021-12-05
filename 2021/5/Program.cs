using Common;

namespace _5
{
    public struct LineSegment
    {
        public Point Start;
        public Point End;

        public LineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public override string ToString() => $"{Start} -> {End}";
    }

    public class Program
    {
        public static LineSegment ParseLineSegment(string line)
        {
            string[] points = line.Split(" -> ");
            string[] firstValues = points[0].Split(',');
            string[] secondValues = points[1].Split(',');

            return new LineSegment(new Point(int.Parse(firstValues[0]), int.Parse(firstValues[1])), new Point(int.Parse(secondValues[0]), int.Parse(secondValues[1])));
        }

        public static void UpdateGrid(Grid<int> grid, LineSegment segment)
        {
            int xStep = Math.Sign(segment.End.X - segment.Start.X);
            int yStep = Math.Sign(segment.End.Y - segment.Start.Y);
            int numSteps = Math.Max(Math.Abs(segment.End.X - segment.Start.X), Math.Abs(segment.End.Y - segment.Start.Y)) + 1;

            int x = segment.Start.X;
            int y = segment.Start.Y;
            for (int i = 0; i < numSteps; ++i)
            {
                grid.Set(y, x, grid.Get(y, x) + 1);

                x += xStep;
                y += yStep;
            }
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            List<LineSegment> segments = lines.Select(ParseLineSegment).ToList();
            //Console.WriteLine(string.Join('\n', segments) + '\n');

            Grid<int> grid = new(0);
            segments.ForEach(segment => UpdateGrid(grid, segment));
            //Console.WriteLine(grid);

            int numIntersections = grid.Reduce<int>((previous, element, row, col) => previous + (element > 1 ? 1 : 0));

            Console.WriteLine(numIntersections);
            Console.ReadLine();
        }
    }
}
