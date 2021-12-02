namespace _2
{
    public class Program
    {
        public static int PositionTimesDepth(List<string> lines)
        {
            int horizontalPosition = 0;
            int depth = 0;
            foreach (string line in lines)
            {
                string[] sections = line.Split(' ', 2);
                if (int.TryParse(sections[1], out int value))
                {
                    if (sections[0] == "forward")
                    {
                        horizontalPosition += value;
                    }
                    else if (sections[0] == "up")
                    {
                        depth -= value;
                    }
                    else if (sections[0] == "down")
                    {
                        depth += value;
                    }
                }
            }

            return horizontalPosition * depth;
        }

        public static int NowWithAim(List<string> lines)
        {
            int horizontalPosition = 0;
            int depth = 0;
            int aim = 0;
            foreach (string line in lines)
            {
                string[] sections = line.Split(' ', 2);
                if (int.TryParse(sections[1], out int value))
                {
                    if (sections[0] == "forward")
                    {
                        horizontalPosition += value;
                        depth += aim * value;
                    }
                    else if (sections[0] == "up")
                    {
                        aim -= value;
                    }
                    else if (sections[0] == "down")
                    {
                        aim += value;
                    }
                }
            }

            return horizontalPosition * depth;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            Console.WriteLine(PositionTimesDepth(lines));
            Console.WriteLine(NowWithAim(lines));
            Console.ReadLine();
        }
    }
}