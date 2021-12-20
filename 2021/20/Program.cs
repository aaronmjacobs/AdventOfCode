using Common;

namespace _20
{
    public class Program
    {
        public static Grid<bool> Enhance(Grid<bool> image, List<bool> algorithm, bool defaultValue)
        {
            Grid<bool> enhancedImage = new(defaultValue);
            enhancedImage.Resize(image.Width + 2, image.Height + 2);

            image.ForEach((value, point) =>
            {
                int binaryValue = 0;
                int shift = 8;
                image.ForEachNeighbor(point, (neighborValue, neighborPoint) =>
                {
                    binaryValue |= (neighborValue ? 1 : 0) << shift;
                    --shift;
                }, true, true);

                enhancedImage.Set(point + new Point(1, 1), algorithm[binaryValue]);
            }, 1);

            return enhancedImage;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            List<bool> algorithm = new();
            foreach (char c in lines[0])
            {
                algorithm.Add(c == '#');
            }

            Grid<bool> image = new(false);
            for (int line = 2; line < lines.Count; ++line)
            {
                int y = line - 2;
                for (int x = 0; x < lines[line].Length; ++x)
                {
                    image.Set(x, y, lines[line][x] == '#');
                }
            }

            for (int i = 0; i < 50; ++i)
            {
                image = Enhance(image, algorithm, i % 2 == 0);

                if (i == 1 || i == 49)
                {
                    Console.WriteLine(image.Reduce<int>((result, value, point) => result += value ? 1 : 0));
                }
            }

            Console.ReadLine();
        }
    }
}
