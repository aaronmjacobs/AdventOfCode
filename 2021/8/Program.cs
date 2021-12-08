namespace _8
{
    public class Program
    {
        private static string[] Answers = new string[10];
        private static List<string> Remaining = new();

        public static string AndPatterns(string first, string second)
        {
            return new string(first.Intersect(second).OrderBy(x => x).ToArray());
        }

        public static string OrPatterns(string first, string second)
        {
            return new string(first.Union(second).OrderBy(x => x).ToArray());
        }

        public static void Update(int index, Predicate<string> predicate)
        {
            Answers[index] = Remaining.Find(predicate) ?? "";
            Remaining.Remove(Answers[index]);
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            int numTimes = 0;
            int total = 0;
            foreach (string line in lines)
            {
                List<string> halves = line.Split('|').ToList();
                List<string> patterns = halves[0].Trim().Split(' ').Select(pattern => String.Concat(pattern.OrderBy(c => c))).ToList();
                List<string> outputValues = halves[1].Trim().Split(' ').Select(pattern => String.Concat(pattern.OrderBy(c => c))).ToList();

                foreach (string outputValue in outputValues)
                {
                    if (outputValue.Length == 2 || outputValue.Length == 4 || outputValue.Length == 3 || outputValue.Length == 7)
                    {
                        ++numTimes;
                    }
                }

                Remaining = new(patterns);

                Update(1, pattern => pattern.Length == 2);
                Update(4, pattern => pattern.Length == 4);
                Update(7, pattern => pattern.Length == 3);
                Update(8, pattern => pattern.Length == 7);
                Update(0, pattern => pattern.Length == 6 && AndPatterns(pattern, Answers[1]) == Answers[1] && OrPatterns(pattern, Answers[4]) == Answers[8]);
                Update(6, pattern => pattern.Length == 6 && AndPatterns(pattern, Answers[1]).Length == 1);
                Update(9, pattern => pattern.Length == 6);
                Update(5, pattern => OrPatterns(pattern, Answers[6]) == Answers[6]);
                Update(3, pattern => AndPatterns(pattern, Answers[1]) == Answers[1]);
                Update(2, pattern => true);

                int decodedNumber = 0;
                int power = 1;
                for (int i = outputValues.Count - 1; i >= 0; --i)
                {
                    int digit = Array.IndexOf(Answers, outputValues[i]);
                    decodedNumber += digit * power;
                    power *= 10;
                }

                total += decodedNumber;
            }

            Console.WriteLine(numTimes);
            Console.WriteLine(total);

            Console.ReadLine();
        }
    }
}
