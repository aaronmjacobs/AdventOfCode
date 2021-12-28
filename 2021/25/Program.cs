using Common;

namespace _25
{
    public class Program
    {
        public static Grid<char> Parse(List<string> lines)
        {
            Grid<char> grid = new('.');
            grid.Resize(lines[0].Length, lines.Count);

            int y = 0;
            foreach (string line in lines)
            {
                int x = 0;
                foreach (char c in line)
                {
                    grid.Set(x, y, c);
                    ++x;
                }
                ++y;
            }

            return grid;
        }

        public static bool Step(Grid<char> grid)
        {
            bool anyMoved = false;

            Grid<char> gridCopy = grid.Clone();
            gridCopy.ForEach((element, point) =>
            {
                Point right = new((point.X + 1) % gridCopy.Width, point.Y);
                if (element == '>' && gridCopy.Get(right) == '.')
                {
                    grid.Set(right, '>');
                    grid.Set(point, '.');

                    anyMoved = true;
                }
            });

            gridCopy = grid.Clone();
            gridCopy.ForEach((element, point) =>
            {
                Point bottom = new(point.X, (point.Y + 1) % gridCopy.Height);
                if (element == 'v' && gridCopy.Get(bottom) == '.')
                {
                    grid.Set(bottom, 'v');
                    grid.Set(point, '.');

                    anyMoved = true;
                }
            });

            return anyMoved;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            Grid<char> grid = Parse(lines);

            int stepCount = 0;
            do
            {
                ++stepCount;
            } while (Step(grid));

            Console.WriteLine(stepCount);

            Console.ReadLine();
        }
    }
}
