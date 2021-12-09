using Common;

namespace _9
{
    public class Program
    {
        public static bool IsLowPoint(Grid<int> grid, int row, int col)
        {
            int lowest = int.MaxValue;
            int lowestRow = -1;
            int lowestCol = -1;

            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (grid.Has(r, c) && grid.Get(r, c) < lowest)
                    {
                        lowest = grid.Get(r, c);
                        lowestRow = r;
                        lowestCol = c;
                    }
                }
            }

            return lowestRow == row && lowestCol == col;
        }

        public static List<Point> GetAdjacent(Grid<int> grid, Point point)
        {
            List<Point> adjacent = new();

            for (int row = Math.Max(point.Y - 1, 0); row <= Math.Min(point.Y + 1, grid.NumRows - 1); ++row)
            {
                if (row != point.Y)
                {
                    adjacent.Add(new Point(point.X, row));
                }
            }

            for (int col = Math.Max(point.X - 1, 0); col <= Math.Min(point.X + 1, grid.NumCols - 1); ++col)
            {
                if (col != point.X)
                {
                    adjacent.Add(new Point(col, point.Y));
                }
            }

            return adjacent;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            Grid<int> grid = new(-1);
            {
                int row = 0;
                foreach (string line in lines)
                {
                    int col = 0;
                    foreach (char c in line)
                    {
                        grid.Set(row, col, c - '0');
                        ++col;
                    }
                    ++row;
                }
            }

            List<Point> lowPoints = new();
            {
                for (int row = 0; row < grid.NumRows; ++row)
                {
                    for (int col = 0; col < grid.NumCols; ++col)
                    {
                        if (IsLowPoint(grid, row, col))
                        {
                            lowPoints.Add(new Point(col, row));
                        }
                    }
                }
            }

            int totalRisk = 0;
            foreach (Point point in lowPoints)
            {
                totalRisk += 1 + grid.Get(point.Y, point.X);
            }

            List<HashSet<Point>> basins = new();
            foreach (Point lowPoint in lowPoints)
            {
                HashSet<Point> search = new();
                search.Add(lowPoint);

                int lastSize = 0;
                while (search.Count != lastSize)
                {
                    lastSize = search.Count;

                    HashSet<Point> searchTemp = new HashSet<Point>(search);
                    foreach (Point searchPoint in searchTemp)
                    {
                        List<Point> adjacent = GetAdjacent(grid, searchPoint);
                        foreach (Point adj in adjacent)
                        {
                            if (grid.Get(adj.Y, adj.X) != 9)
                            {
                                search.Add(adj);
                            }
                        }    
                    }
                }

                basins.Add(search);
            }

            basins.Sort((first, second) => first.Count < second.Count ? 1 : first.Count == second.Count ? 0 : -1);

            int multipliedSizes = basins[0].Count * basins[1].Count * basins[2].Count;
            
            Console.WriteLine(totalRisk);
            Console.WriteLine(multipliedSizes);
            Console.ReadLine();
        }
    }
}
