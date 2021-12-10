namespace _10
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            Dictionary<char, char> chunkPairs = new() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' } };
            Dictionary<char, int> errorCosts = new() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            Dictionary<char, ulong> scoreValues = new() { { '(', 1u }, { '[', 2u }, { '{', 3u }, { '<', 4u } };

            List<string> incompleteLines = new();

            int totalCost = 0;
            foreach (string line in lines)
            {
                int errorCost = 0;
                Stack<char> stack = new();
                foreach (char c in line)
                {
                    if (chunkPairs.ContainsKey(c))
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        char start = stack.Pop();
                        if (c != chunkPairs[start])
                        {
                            errorCost = errorCosts[c];
                            break;
                        }
                    }
                }

                totalCost += errorCost;

                if (errorCost == 0)
                {
                    incompleteLines.Add(line);
                }
            }

            Console.WriteLine(totalCost);

            List<ulong> scores = new();
            foreach (string line in incompleteLines)
            {
                Stack<char> stack = new();
                foreach (char c in line)
                {
                    if (chunkPairs.ContainsKey(c))
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        stack.Pop();
                    }
                }

                ulong score = 0;
                while (stack.Count > 0)
                {
                    score = score * 5 + scoreValues[stack.Pop()];
                }

                scores.Add(score);
            }
            scores.Sort();

            Console.WriteLine(scores[scores.Count / 2]);

            Console.ReadLine();
        }
    }
}
