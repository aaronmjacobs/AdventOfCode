using Common;

namespace _9
{
    public class Program
    {
        public static Grid<int> BuildGrid(List<string> lines)
        {
            Grid<int> grid = new(-1);

            int y = 0;
            foreach (string line in lines)
            {
                int x = 0;
                foreach (char c in line)
                {
                    grid.Set(x, y, c - '0');
                    ++x;
                }
                ++y;
            }

            return grid;
        }

        public static List<Point> GetAdjacent(Grid<int> grid, Point point)
        {
            return (new Point[] { new Point(point.X - 1, point.Y), new Point(point.X + 1, point.Y), new Point(point.X, point.Y - 1), new Point(point.X, point.Y + 1) }).Where(adjacent => grid.Has(adjacent)).ToList();
        }

        public static bool IsLowPoint(Grid<int> grid, Point point)
        {
            return grid.Get(point) < GetAdjacent(grid, point).Select(p => grid.Get(p)).Min();
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            Grid<int> grid = BuildGrid(lines);

            List<Point> lowPoints = new();
            grid.ForEach((height, point) =>
            {
                if (IsLowPoint(grid, point))
                {
                    lowPoints.Add(point);
                }
            });

            int totalRisk = lowPoints.Select(lowPoint => grid.Get(lowPoint)).Aggregate(0, (result, height) => result += height + 1);
            Console.WriteLine(totalRisk);

            List<HashSet<Point>> basins = new();
            foreach (Point lowPoint in lowPoints)
            {
                HashSet<Point> basin = new();

                Queue<Point> queue = new();
                queue.Enqueue(lowPoint);
                while (queue.Count > 0)
                {
                    Point point = queue.Dequeue();
                    foreach (Point newAdjacent in GetAdjacent(grid, point).Where(adjacent => grid.Get(adjacent) != 9 && !basin.Contains(adjacent)))
                    {
                        basin.Add(newAdjacent);
                        queue.Enqueue(newAdjacent);
                    }
                }

                basins.Add(basin);
            }
            basins.Sort((first, second) => second.Count - first.Count);

            int multipliedSizes = basins[0].Count * basins[1].Count * basins[2].Count;
            Console.WriteLine(multipliedSizes);

            Console.ReadLine();
        }
    }
}
