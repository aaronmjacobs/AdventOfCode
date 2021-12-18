using Common;

namespace _17
{
    public struct State
    {
        public Point Position;
        public Point Velocity;

        public State(int xVelocity, int yVelocity)
        {
            Position = new(0, 0);
            Velocity = new(xVelocity, yVelocity);
        }

        public void Step()
        {
            Position += Velocity;

            Velocity.X = Math.Max(0, Math.Abs(Velocity.X) - 1) * Math.Sign(Velocity.X);
            --Velocity.Y;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            string[] halves = lines[0].Split(": ")[1].Split(", ");
            string[] xValues = halves[0][2..].Split("..");
            string[] yValues = halves[1][2..].Split("..");
            Point targetMin = new(int.Parse(xValues[0]), int.Parse(yValues[0]));
            Point targetMax = new(int.Parse(xValues[1]), int.Parse(yValues[1]));

            int xVelocityMin = (int)Math.Ceiling((-1.0 + Math.Sqrt(1.0 + 8.0 * targetMin.X)) / 2.0);
            int xVelocityMax = targetMax.X;

            int yVelocityMin = targetMin.Y;
            int yVelocityMax = -targetMin.Y + 1;

            int highestY = targetMin.Y * (targetMin.Y + 1) / 2;
            Console.WriteLine(highestY);

            int numValidVelocities = 0;
            for (int xVelocity = xVelocityMin; xVelocity <= xVelocityMax; ++xVelocity)
            {
                for (int yVelocity = yVelocityMin; yVelocity <= yVelocityMax; ++yVelocity)
                {
                    State state = new(xVelocity, yVelocity);
                    for (;;)
                    {
                        state.Step();

                        if (state.Position.X > targetMax.X || state.Position.Y < targetMin.Y)
                        {
                            break;
                        }

                        if (state.Position.X >= targetMin.X && state.Position.X <= targetMax.X && state.Position.Y >= targetMin.Y && state.Position.Y <= targetMax.Y)
                        {
                            ++numValidVelocities;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine(numValidVelocities);

            Console.ReadLine();
        }
    }
}
