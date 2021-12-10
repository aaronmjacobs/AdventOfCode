namespace _10
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            List<string> incompleteLines = new();

            int totalCost = 0;
            foreach (string line in lines)
            {
                Stack<char> stack = new();
                int errorCost = 0;
                foreach (char c in line)
                {
                    if (c == '(' || c == '[' || c == '{' || c == '<')
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        char open = stack.Pop();
                        if (open == '(' && c != ')' || open == '[' && c != ']' || open == '{' && c != '}' || open == '<' && c != '>')
                        {
                            if (c == ')')
                            {
                                errorCost = 3;
                            }
                            else if (c == ']')
                            {
                                errorCost = 57;
                            }
                            else if (c == '}')
                            {
                                errorCost = 1197;
                            }
                            else if (c == '>')
                            {
                                errorCost = 25137;
                            }
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
                    if (c == '(' || c == '[' || c == '{' || c == '<')
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
                    char c = stack.Pop();
                    score *= 5;
                    score += c == '(' ? 1u : c == '[' ? 2u : c == '{' ? 3u : c == '<' ? 4u : 0u;
                }

                scores.Add(score);
            }

            scores.Sort();

            Console.WriteLine(scores[scores.Count / 2]);

            Console.ReadLine();
        }
    }
}
