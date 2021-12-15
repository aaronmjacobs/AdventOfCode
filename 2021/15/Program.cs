using Common;

namespace _15
{
    public class Program
    {
        public static int AStar(Grid<int> grid, Point start, Point goal)
        {
            Grid<int> costs = new(int.MaxValue);
            costs.Resize(grid.Width, grid.Height);
            costs.Set(start, 0);

            PriorityQueue<Point, int> queue = new();
            queue.Enqueue(start, 0);

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();
                if (current == goal)
                {
                    return costs.Get(goal);
                }

                int currentCost = costs.Get(current);
                grid.ForEachAdjacent(current, (value, adjacent) =>
                {
                    int newAdjacentCost = currentCost + value;
                    if (newAdjacentCost < costs.Get(adjacent))
                    {
                        costs.Set(adjacent, newAdjacentCost);

                        int adjacentEstimatedTotalCost = newAdjacentCost + Point.ManhattanDistance(adjacent, goal);
                        queue.Enqueue(adjacent, adjacentEstimatedTotalCost);
                    }
                });
            }

            return -1;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            Grid<int> grid = Utils.ParseIntGrid(lines);

            int gridTotalRisk = AStar(grid, new Point(0, 0), new Point(grid.Width - 1, grid.Height - 1));
            Console.WriteLine(gridTotalRisk);

            Grid<int> bigGrid = new();
            bigGrid.Resize(grid.Width * 5, grid.Height * 5);
            for (int x = 0; x < 5; ++x)
            {
                for (int y = 0; y < 5; ++y)
                {
                    int costIncrease = x + y;
                    grid.ForEach((value, point) =>
                    {
                        int newValue = (value + costIncrease - 1) % 9 + 1;
                        bigGrid.Set(new Point(point.X + x * grid.Width, point.Y + y * grid.Height), newValue);
                    });
                }
            }

            int bigGridTotalRisk = AStar(bigGrid, new Point(0, 0), new Point(bigGrid.Width - 1, bigGrid.Height - 1));
            Console.WriteLine(bigGridTotalRisk);

            Console.ReadLine();
        }
    }
}
