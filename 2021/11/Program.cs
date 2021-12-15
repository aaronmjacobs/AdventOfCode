using Common;

namespace _11
{
    public class Program
    {
        public static void Step(Grid<int> grid, int value, Point point)
        {
            grid.Set(point, value + 1);
            if (value == 9)
            {
                grid.ForEachNeighbor(point, (neighborValue, neighborPoint) =>
                {
                    Step(grid, neighborValue, neighborPoint);
                });
            }
        }

        public static void Main(string[] args)
        {
            Grid<int> grid = Utils.ParseIntGrid(File.ReadLines(@"../../../input.txt"));

            int flashes = 0;
            int firstSimultaneousFlash = -1;
            for (int i = 0; i < 100 || firstSimultaneousFlash == -1; ++i)
            {
                grid.ForEach((value, point) => Step(grid, value, point));

                int stepFlashes = 0;
                grid.ForEach((value, point) =>
                {
                    if (grid.Get(point) > 9)
                    {
                        ++stepFlashes;
                        grid.Set(point, 0);
                    }
                });

                if (i < 100)
                {
                    flashes += stepFlashes;
                }

                if (stepFlashes == grid.Width * grid.Height)
                {
                    firstSimultaneousFlash = i + 1;
                }
            }

            Console.WriteLine(flashes);
            Console.WriteLine(firstSimultaneousFlash);

            Console.ReadLine();
        }
    }
}
