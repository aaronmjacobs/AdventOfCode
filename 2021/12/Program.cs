namespace _12
{
    public class Program
    {
        public static bool IsSmallCave(string node)
        {
            return node != "start" && node != "end" && node[0] >= 'a' && node[0] <= 'z';
        }

        public static int Traverse(Dictionary<string, List<string>> graph, string node, List<string> path, HashSet<string> visitedSmallCaves, bool doubleVisitedSmallCave)
        {
            path.Add(node);
            if (IsSmallCave(node))
            {
                if (visitedSmallCaves.Contains(node))
                {
                    doubleVisitedSmallCave = true;
                }
                visitedSmallCaves.Add(node);
            }

            if (node == "end")
            {
                //Console.WriteLine(String.Join(',', path));
                return 1;
            }

            int count = 0;
            foreach (string option in graph[node])
            {
                if (option != "start")
                {
                    if (!visitedSmallCaves.Contains(option) || !doubleVisitedSmallCave)
                    {
                        count += Traverse(graph, option, new List<string>(path), new HashSet<string>(visitedSmallCaves), doubleVisitedSmallCave);
                    }
                }
            }

            return count;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            Dictionary<string, List<string>> graph = new();
            foreach (string line in lines)
            {
                string[] halves = line.Split('-');
                if (!graph.ContainsKey(halves[0]))
                {
                    graph[halves[0]] = new List<string>();
                }
                if (!graph.ContainsKey(halves[1]))
                {
                    graph[halves[1]] = new List<string>();
                }

                graph[halves[0]].Add(halves[1]);
                graph[halves[1]].Add(halves[0]);
            }

            int numPaths = Traverse(graph, "start", new List<string>(), new HashSet<string>(), true);
            Console.WriteLine(numPaths);

            int numPathsWithDoubleVisit = Traverse(graph, "start", new List<string>(), new HashSet<string>(), false);
            Console.WriteLine(numPathsWithDoubleVisit);

            Console.ReadLine();
        }
    }
}
