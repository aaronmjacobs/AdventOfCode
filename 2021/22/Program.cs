using Common;

namespace _22
{
    public struct Cube
    {
        public Point3D Min;
        public Point3D Max;
        public bool On;

        public Cube(int xMin, int yMin, int zMin, int xMax, int yMax, int zMax, bool on)
        {
            Min = new(xMin, yMin, zMin);
            Max = new(xMax, yMax, zMax);
            On = on;
        }

        public long Volume()
        {
            return (Max.X - Min.X + 1L) * (Max.Y - Min.Y + 1L) * (Max.Z - Min.Z + 1L);
        }

        public static Cube? Intersection(Cube first, Cube second, bool on)
        {
            if (first.Max.X >= second.Min.X && first.Min.X <= second.Max.X)
            {
                int xMin = Math.Max(first.Min.X, second.Min.X);
                int xMax = Math.Min(first.Max.X, second.Max.X);

                if (first.Max.Y >= second.Min.Y && first.Min.Y <= second.Max.Y)
                {
                    int yMin = Math.Max(first.Min.Y, second.Min.Y);
                    int yMax = Math.Min(first.Max.Y, second.Max.Y);

                    if (first.Max.Z >= second.Min.Z && first.Min.Z <= second.Max.Z)
                    {
                        int zMin = Math.Max(first.Min.Z, second.Min.Z);
                        int zMax = Math.Min(first.Max.Z, second.Max.Z);

                        return new(xMin, yMin, zMin, xMax, yMax, zMax, on);
                    }
                }
            }

            return null;
        }

        public override string ToString() => $"[{Min}, {Max}] {On}";
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            List<Cube> input = new();
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');
                string[] coordinates = parts[1].Split(',');
                string xRange = coordinates[0].Split('=')[1];
                string yRange = coordinates[1].Split('=')[1];
                string zRange = coordinates[2].Split('=')[1];
                string[] xValues = xRange.Split("..");
                string[] yValues = yRange.Split("..");
                string[] zValues = zRange.Split("..");

                bool on = parts[0] == "on";
                int xMin = int.Parse(xValues[0]);
                int xMax = int.Parse(xValues[1]);
                int yMin = int.Parse(yValues[0]);
                int yMax = int.Parse(yValues[1]);
                int zMin = int.Parse(zValues[0]);
                int zMax = int.Parse(zValues[1]);

                input.Add(new Cube(xMin, yMin, zMin, xMax, yMax, zMax, on));
            }

            List<Cube> cubes = new();
            foreach (Cube cube in input)
            {
                List<Cube> intersections = new();
                foreach (Cube existingCube in cubes)
                {
                    // If the existing cube is "on", then add the intersection as "off".
                    // If the new cube is "off", this will properly remove its volume.
                    // If the new cube is "on", this will remove the shared volume between it and the existing cube before adding the volume of the new cube.

                    // If the existing cube is "off", then add the intersection as "on".
                    // If the new cube is "off", this will add the shared volume between it and the existing cube, preventing the "off" volume from being double counted.
                    // If the new cube is "on", this will counteract the earlier "off" intersection that was added.

                    Cube? intersection = Cube.Intersection(cube, existingCube, !existingCube.On);
                    if (intersection != null)
                    {
                        intersections.Add(intersection.Value);
                    }
                }

                if (cube.On)
                {
                    cubes.Add(cube);
                }
                cubes.AddRange(intersections);
            }

            long numOn = cubes.Aggregate(0L, (totalVolume, cube) => totalVolume + cube.Volume() * (cube.On ? 1 : -1));
            Console.WriteLine(numOn);

            Console.ReadLine();
        }
    }
}
