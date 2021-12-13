using Common;

namespace _13
{
    public class Program
    {
        public static void Fold(Grid<bool> grid, bool foldX, int where)
        {
            int newWidth = grid.Width;
            int newHeight = grid.Height;

            if (foldX)
            {
                newWidth = where;

                for (int y = 0; y < grid.Height; ++y)
                {
                    for (int x = where + 1; x < grid.Width; ++x)
                    {
                        int foldedX = where - (x - where);
                        grid.Set(foldedX, y, grid.Get(foldedX, y) || grid.Get(x, y));
                    }
                }
            }
            else
            {
                newHeight = where;

                for (int x = 0; x < grid.Width; ++x)
                {
                    for (int y = where + 1; y < grid.Height; ++y)
                    {
                        int foldedY = where - (y - where);
                        grid.Set(x, foldedY, grid.Get(x, foldedY) || grid.Get(x, y));
                    }
                }
            }

            grid.Resize(newWidth, newHeight);
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            Grid<bool> grid = new(false);
            bool parsingPoints = true;
            bool printedNumDots = false;
            foreach (string line in lines)
            {
                if (line.Length == 0)
                {
                    parsingPoints = false;
                }
                else
                {
                    if (parsingPoints)
                    {
                        string[] coords = line.Split(',');
                        grid.Set(int.Parse(coords[0]), int.Parse(coords[1]), true);
                    }
                    else
                    {
                        string[] halves = line.Split("fold along ")[1].Split('=');
                        bool foldX = halves[0] == "x";
                        int where = int.Parse(halves[1]);

                        Fold(grid, foldX, where);
                        if (!printedNumDots)
                        {
                            int numDots = grid.Reduce((result, value, point) => result = value ? result + 1 : result, 0);
                            Console.WriteLine(numDots);
                            printedNumDots = true;
                        }
                    }
                }
            }

            Console.WriteLine(grid.ToStringCustom('\n', ' ', (value) => value ? '#' : '.'));

            Console.ReadLine();
        }
    }
}
