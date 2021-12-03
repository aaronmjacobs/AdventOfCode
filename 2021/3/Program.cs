namespace _3
{
    public class Program
    {
        const int kNumBits = 12;

        public static int[] CountBits(List<string> lines)
        {
            int[] bitCounts = new int[kNumBits];
            foreach (string line in lines)
            {
                for (int i = 0; i < line.Length; ++i)
                {
                    if (i >= bitCounts.Length)
                    {
                        break;
                    }

                    if (line[i] == '1')
                    {
                        ++bitCounts[i];
                    }
                }
            }

            return bitCounts;
        }

        public static uint ComputeRating(List<string> lines, bool mostCommon)
        {
            while (lines.Count > 1)
            {
                for (int i = 0; i < kNumBits; ++i)
                {
                    int[] bitCounts = CountBits(lines);

                    bool moreOnes = bitCounts[i] * 2 >= lines.Count;
                    if (mostCommon)
                    {
                        lines.RemoveAll(line => (line[i] == '1') != moreOnes);
                    }
                    else
                    {
                        lines.RemoveAll(line => (line[i] == '1') == moreOnes);
                    }

                    if (lines.Count == 1)
                    {
                        break;
                    }
                }
            }

            return Convert.ToUInt32(lines[0], 2);
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            uint gammaRate = 0;
            int[] bitCounts = CountBits(lines);
            for (int i = 0; i < bitCounts.Length; ++i)
            {
                if (bitCounts[i] * 2 >= lines.Count)
                {
                    gammaRate |= 1u << ((bitCounts.Length - 1) - i);
                }
            }
            uint mask = (1u << kNumBits) - 1;
            uint epsilonRate = ~gammaRate & mask;

            uint oxygenRating = ComputeRating(new List<string>(lines), true);
            uint c02Rating = ComputeRating(new List<string>(lines), false);

            Console.WriteLine(gammaRate * epsilonRate);
            Console.WriteLine(oxygenRating * c02Rating);

            Console.ReadLine();
        }
    }
}