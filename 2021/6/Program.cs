namespace _6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            List<int> fish = lines[0].Split(',').Select(int.Parse).ToList();

            long[] numFish = new long[9];
            foreach (int fishy in fish)
            {
                ++numFish[fishy];
            }

            for (int day = 0; day < 256; ++day)
            {
                long numNewFish = numFish[0];

                for (int i = 0; i < numFish.Length - 1; ++i)
                {
                    numFish[i] = numFish[i + 1];
                }

                numFish[6] += numNewFish;
                numFish[8] = numNewFish;
            }

            long totalFish = 0;
            foreach (long fishy in numFish)
            {
                totalFish += fishy;
            }

            Console.WriteLine(totalFish);
            Console.ReadLine();
        }
    }
}
