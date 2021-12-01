namespace _1
{
    public class Program
    {
        public static int CountIncreases(List<int> depths)
        {
            int numIncreases = 0;

            int lastDepth = int.MaxValue;
            foreach (int depth in depths)
            {
                if (depth > lastDepth)
                {
                    ++numIncreases;
                }

                lastDepth = depth;
            }

            return numIncreases;
        }

        public static int CountWindowIncreases(List<int> depths)
        {
            if (depths.Count < 4)
            {
                return 0;
            }

            int numWindowIncreases = 0;

            int windowSum1 = 0;
            int windowSum2 = 0;
            for (int i = 0; i < depths.Count - 1; ++i)
            {
                if (i >= 3)
                {
                    windowSum1 -= depths[i - 3];
                    windowSum2 -= depths[i - 2];
                }

                windowSum1 += depths[i];
                windowSum2 += depths[i + 1];

                if (i >= 2 && windowSum2 > windowSum1)
                {
                    ++numWindowIncreases;
                }
            }

            return numWindowIncreases;
        }

        public static void Main(string[] args)
        {
            List<int> depths = new();
            foreach (string line in File.ReadLines(@"../../../input.txt"))
            {
                if (int.TryParse(line, out int depth))
                {
                    depths.Add(depth);
                }
            }

            int numIncreases = CountIncreases(depths);
            int numWindowIncreases = CountWindowIncreases(depths);
            Console.WriteLine($"Increases: {numIncreases}, window increases: {numWindowIncreases}");
            Console.ReadLine();
        }
    }
}