namespace _14
{
    public class Program
    {
        public static Dictionary<string, ulong> UpdatePairs(Dictionary<string, ulong> pairs, Dictionary<string, string> rules)
        {
            Dictionary<string, ulong> updatedPairs = new();

            foreach (string pair in pairs.Keys)
            {
                if (rules.ContainsKey(pair))
                {
                    string newPair1 = pair[0] + "" + rules[pair][0];
                    string newPair2 = rules[pair][0] + "" + pair[1];

                    updatedPairs.TryAdd(newPair1, 0);
                    updatedPairs.TryAdd(newPair2, 0);

                    updatedPairs[newPair1] += pairs[pair];
                    updatedPairs[newPair2] += pairs[pair];
                }
                else
                {
                    updatedPairs.TryAdd(pair, 0);
                    updatedPairs[pair] += pairs[pair];
                }
            }

            return updatedPairs;
        }

        public static ulong ComputeQuantityDiff(Dictionary<string, ulong> pairs)
        {
            Dictionary<char, ulong> counts = new();
            foreach (string pair in pairs.Keys)
            {
                counts.TryAdd(pair[0], 0);
                counts.TryAdd(pair[1], 0);

                counts[pair[0]] += pairs[pair];
                counts[pair[1]] += pairs[pair];
            }

            ulong maxCount = counts.Values.Max();
            ulong minCount = counts.Values.Min();
            return (maxCount + 1) / 2 - (minCount + 1) / 2;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            string template = lines[0];
            Dictionary<string, ulong> pairs = new();
            for (int i = 0; i < template.Length - 1; ++i)
            {
                string pair = template[i] + "" + template[i + 1];
                pairs.TryAdd(pair, 0);
                ++pairs[pair];
            }

            Dictionary<string, string> rules = new();
            for (int i = 2; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] halves = line.Split(" -> ");
                rules.Add(halves[0], halves[1]);
            }

            for (int i = 0; i < 10; ++i)
            {
                pairs = UpdatePairs(pairs, rules);
            }
            Console.WriteLine(ComputeQuantityDiff(pairs));

            for (int i = 10; i < 40; ++i)
            {
                pairs = UpdatePairs(pairs, rules);
            }
            Console.WriteLine(ComputeQuantityDiff(pairs));

            Console.ReadLine();
        }
    }
}
