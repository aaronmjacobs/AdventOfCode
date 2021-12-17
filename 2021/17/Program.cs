using Common;

namespace _16
{
    public struct State
    {
        public long xPos;
        public long yPos;

        public long xVel;
        public long yVel;

        public State(long xVelocity, long yVelocity)
        {
            xPos = 0;
            yPos = 0;

            xVel = xVelocity;
            yVel = yVelocity;
        }

        public void Step()
        {
            xPos += xVel;
            yPos += yVel;

            xVel = Math.Max(0, Math.Abs(xVel) - 1) * Math.Sign(xVel);
            --yVel;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            string blarg = lines[0].Split(": ")[1];
            string[] halves = blarg.Split(", ");
            string[] xValues = halves[0].Substring(2).Split("..");
            string[] yValues = halves[1].Substring(2).Split("..");
            long xMin = long.Parse(xValues[0]);
            long xMax = long.Parse(xValues[1]);
            long yMin = long.Parse(yValues[0]);
            long yMax = long.Parse(yValues[1]);

            long xRangeStart = long.MaxValue;
            long xRangeEnd = long.MinValue;
            for (long i = 0; i < 1000; ++i)
            {
                long lastX = -1;
                State state = new(i, 0);
                while (state.xPos != lastX)
                {
                    if (state.xPos >= xMin)
                    {
                        xRangeStart = Math.Min(xRangeStart, i);
                    }
                    if (state.xPos <= xMax)
                    {
                        xRangeEnd = Math.Max(xRangeEnd, i);
                    }

                    lastX = state.xPos;
                    state.Step();
                }
            }

            long numValidVelocities = 0;
            long highestY = long.MinValue;
            for (long xVel = xRangeStart; xVel <= xRangeEnd; ++xVel)
            {
                for (long yVel = -1000; yVel < 1000; ++yVel)
                {
                    long highestYThisRun = long.MinValue;
                    State state = new(xVel, yVel);
                    for (; ; )
                    {
                        state.Step();

                        if (state.xPos > xMax || state.yPos < yMin)
                        {
                            //Console.WriteLine("Miss");
                            break;
                        }

                        highestYThisRun = Math.Max(highestYThisRun, state.yPos);

                        //Console.WriteLine($"({state.xPos}, {state.yPos})");

                        if (state.xPos >= xMin && state.xPos <= xMax && state.yPos >= yMin && state.yPos <= yMax)
                        {
                            //Console.WriteLine($"{xVel},{yVel}");
                            highestY = Math.Max(highestY, highestYThisRun);
                            ++numValidVelocities;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(highestY);
            Console.WriteLine(numValidVelocities);

            Console.ReadLine();
        }
    }
}
