using Common;

namespace _19
{
    public class Scanner
    {
        public List<Point3D> Beacons = new();
    }

    public struct Match
    {
        public Point3D offset;
        public int variantIndex;
    }

    public class Program
    {
        public static List<Point3D> ComputePointVariants(Point3D point)
        {
            List<Point3D> variants = new();

            variants.Add(new Point3D(point.X, point.Y, point.Z));
            variants.Add(new Point3D(point.X, point.Z, -point.Y));
            variants.Add(new Point3D(point.X, -point.Y, -point.Z));
            variants.Add(new Point3D(point.X, -point.Z, point.Y));

            variants.Add(new Point3D(-point.X, point.Z, point.Y));
            variants.Add(new Point3D(-point.X, point.Y, -point.Z));
            variants.Add(new Point3D(-point.X, -point.Z, -point.Y));
            variants.Add(new Point3D(-point.X, -point.Y, point.Z));

            variants.Add(new Point3D(point.Y, -point.X, point.Z));
            variants.Add(new Point3D(point.Y, -point.Z, -point.X));
            variants.Add(new Point3D(point.Y, point.X, -point.Z));
            variants.Add(new Point3D(point.Y, point.Z, point.X));

            variants.Add(new Point3D(-point.Y, -point.Z, point.X));
            variants.Add(new Point3D(-point.Y, -point.X, -point.Z));
            variants.Add(new Point3D(-point.Y, point.Z, -point.X));
            variants.Add(new Point3D(-point.Y, point.X, point.Z));

            variants.Add(new Point3D(point.Z, point.Y, -point.X));
            variants.Add(new Point3D(point.Z, -point.X, -point.Y));
            variants.Add(new Point3D(point.Z, -point.Y, point.X));
            variants.Add(new Point3D(point.Z, point.X, point.Y));

            variants.Add(new Point3D(-point.Z, point.X, -point.Y));
            variants.Add(new Point3D(-point.Z, -point.Y, -point.X));
            variants.Add(new Point3D(-point.Z, -point.X, point.Y));
            variants.Add(new Point3D(-point.Z, point.Y, point.X));

            return variants;
        }

        public static List<List<Point3D>> ComputeVariantLists(List<Point3D> list)
        {
            List<List<Point3D>> variantLists = new();
            for (int i = 0; i < 24; ++i)
            {
                variantLists.Add(new List<Point3D>());
            }

            foreach (Point3D point in list)
            {
                List<Point3D> pointVariants = ComputePointVariants(point);
                for (int i = 0; i < 24; ++i)
                {
                    variantLists[i].Add(pointVariants[i]);
                }
            }

            return variantLists;
        }

        public static Match? CheckIfScannersOverlap(List<Scanner> scanners, int i, int j)
        {
            Scanner first = scanners[i];
            Scanner second = scanners[j];

            HashSet<Point3D> firstPoints = new();
            foreach (Point3D firstPoint in first.Beacons)
            {
                firstPoints.Add(firstPoint);
            }

            List<List<Point3D>> secondVariants = ComputeVariantLists(second.Beacons);
            for (int variantIndex = 0; variantIndex < 24; ++variantIndex)
            {
                List<Point3D> secondVariant = secondVariants[variantIndex];

                for (int firstOriginIndex = 0; firstOriginIndex < first.Beacons.Count; ++firstOriginIndex)
                {
                    for (int secondOriginIndex = 0; secondOriginIndex < secondVariant.Count; ++secondOriginIndex)
                    {
                        Point3D offset = first.Beacons[firstOriginIndex] - secondVariant[secondOriginIndex];

                        List<Point3D> secondOffsetPoints = new();
                        foreach (Point3D secondPoint in secondVariant)
                        {
                            secondOffsetPoints.Add(secondPoint + offset);
                        }

                        int numMatching = 0;
                        foreach (Point3D secondOffsetPoint in secondOffsetPoints)
                        {
                            if (firstPoints.Contains(secondOffsetPoint))
                            {
                                ++numMatching;
                            }
                        }

                        if (numMatching >= 12)
                        {
                            Console.WriteLine($"Found match between scanner {i} and scanner {j}, variant {variantIndex}, origins {firstOriginIndex}, {secondOriginIndex}");

                            foreach (Point3D secondOffsetPoint in secondOffsetPoints)
                            {
                                if (firstPoints.Contains(secondOffsetPoint))
                                {
                                    Console.WriteLine($"{secondOffsetPoint}");
                                }
                            }

                            Match match;
                            match.offset = offset;
                            match.variantIndex = variantIndex;
                            return match;
                        }
                    }
                }
            }

            return null;
        }

        public static void MergeScanners(List<Scanner> scanners)
        {
            List<Point3D> offsets = new();
            offsets.Add(new Point3D(0, 0, 0));

            while (scanners.Count > 1)
            {
                for (int i = 1; i < scanners.Count; ++i)
                {
                    Match? match = CheckIfScannersOverlap(scanners, 0, i);
                    if (match != null)
                    {
                        List<List<Point3D>> variants = ComputeVariantLists(scanners[i].Beacons);
                        List<Point3D> variant = variants[((Match)match).variantIndex];
                        foreach (Point3D variantPoint in variant)
                        {
                            Point3D offsetVariantPoint = variantPoint + ((Match)match).offset;
                            if (!scanners[0].Beacons.Contains(offsetVariantPoint))
                            {
                                scanners[0].Beacons.Add(offsetVariantPoint);
                            }
                        }

                        offsets.Add(((Match)match).offset);

                        scanners.RemoveAt(i);
                        break;
                    }
                }
            }

            int maxDistance = int.MinValue;
            for (int i = 0; i < offsets.Count - 1; ++i)
            {
                for (int j = i + 1; j < offsets.Count; ++j)
                {
                    int distance = Point3D.ManhattanDistance(offsets[i], offsets[j]);
                    maxDistance = Math.Max(maxDistance, distance);
                }
            }

            Console.WriteLine($"Number of beacons: {scanners[0].Beacons.Count}");
            Console.WriteLine($"Largest manhattan distance: {maxDistance}");
        }

        public static List<Scanner> ParseScanners(List<string> lines)
        {
            List<Scanner> scanners = new();
            Scanner? currentScanner = null;
            foreach (string line in lines)
            {
                if (line.StartsWith("---"))
                {
                    if (currentScanner != null)
                    {
                        scanners.Add((Scanner)currentScanner);
                    }
                    currentScanner = new();
                }
                else if (line.Length > 0 && currentScanner != null)
                {
                    string[] values = line.Split(',');
                    ((Scanner)currentScanner).Beacons.Add(new Point3D(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2])));
                }
            }
            scanners.Add((Scanner)currentScanner);

            return scanners;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            List<Scanner> scanners = ParseScanners(lines);
            MergeScanners(scanners);

            Console.ReadLine();
        }
    }
}
