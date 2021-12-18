namespace _18
{
    public class Pair
    {
        public Pair? Parent;

        public Pair? FirstPair;
        public Pair? SecondPair;

        public int? FirstNumber;
        public int? SecondNumber;

        public Pair(Pair first, Pair second)
        {
            FirstPair = first;
            SecondPair = second;

            FirstNumber = null;
            SecondNumber = null;
        }

        public Pair(int first, int second)
        {
            FirstPair = null;
            SecondPair = null;

            FirstNumber = first;
            SecondNumber = second;
        }

        public Pair(Pair first, int second)
        {
            FirstPair = first;
            SecondPair = null;

            FirstNumber = null;
            SecondNumber = second;
        }

        public Pair(int first, Pair second)
        {
            FirstPair = null;
            SecondPair = second;

            FirstNumber = first;
            SecondNumber = null;
        }

        public override string ToString()
        {
            string first = FirstPair != null ? FirstPair.ToString() : FirstNumber.ToString();
            string second = SecondPair != null ? SecondPair.ToString() : SecondNumber.ToString();
            return $"[{first},{second}]";
        }
    }

    public class Program
    {
        public static void UpdateParents(Pair pair)
        {
            if (pair.FirstPair != null)
            {
                pair.FirstPair.Parent = pair;
                UpdateParents(pair.FirstPair);
            }
            if (pair.SecondPair != null)
            {
                pair.SecondPair.Parent = pair;
                UpdateParents(pair.SecondPair);
            }
        }

        public static Pair Parse(string number)
        {
            int centerIndex = -1;
            int depth = 1;
            for (int i = 1; i < number.Length; ++i)
            {
                if (number[i] == '[')
                {
                    ++depth;
                }
                else if (number[i] == ']')
                {
                    --depth;
                }

                if (depth == 1)
                {
                    centerIndex = i + 1;
                    break;
                }
            }

            string firstPairString = number.Substring(1, centerIndex - 1);
            string secondPairString = number.Substring(centerIndex + 1, number.Length - (centerIndex + 2));

            Pair? firstPair = null;
            int? firstPairNumber = null;
            if (firstPairString[0] == '[')
            {
                firstPair = Parse(firstPairString);
            }
            else
            {
                firstPairNumber = int.Parse(firstPairString);
            }

            Pair? secondPair = null;
            int? secondPairNumber = null;
            if (secondPairString[0] == '[')
            {
                secondPair = Parse(secondPairString);
            }
            else
            {
                secondPairNumber = int.Parse(secondPairString);
            }

            if (firstPair != null)
            {
                if (secondPair != null)
                {
                    return new Pair(firstPair, secondPair);
                }
                else
                {
                    return new Pair(firstPair, (int)secondPairNumber);
                }
            }
            else
            {
                if (secondPair != null)
                {
                    return new Pair((int)firstPairNumber, secondPair);
                }
                else
                {
                    return new Pair((int)firstPairNumber, (int)secondPairNumber);
                }
            }
        }

        public static Pair Add(Pair first, Pair second)
        {
            Pair pair = new(first, second);
            first.Parent = pair;
            second.Parent = pair;
            return pair;
        }

        public static Pair Split(int number, Pair parent)
        {
            Pair split = new(number / 2, (number + 1) / 2);
            split.Parent = parent;
            return split;
        }

        public static Pair? FindVolatile(Pair pair, int depth)
        {
            if (depth == 4)
            {
                return pair;
            }

            if (pair.FirstPair != null)
            {
                Pair first = FindVolatile(pair.FirstPair, depth + 1);
                if (first != null)
                {
                    return first;
                }
            }
            if (pair.SecondPair != null)
            {
                Pair second = FindVolatile(pair.SecondPair, depth + 1);
                if (second != null)
                {
                    return second;
                }
            }

            return null;
        }

        public static Pair? DepthFirst(Pair pair, bool left)
        {
            if (left)
            {
                return pair.FirstPair != null ? DepthFirst(pair.FirstPair, left) : pair;
            }
            else
            {
                return pair.SecondPair != null ? DepthFirst(pair.SecondPair, left) : pair;
            }
        }

        public static Pair? First10OrGreater(Pair pair)
        {
            if ((pair.FirstNumber != null && pair.FirstNumber >= 10))
            {
                return pair;
            }

            if (pair.FirstPair != null)
            {
                Pair? firstResult = First10OrGreater(pair.FirstPair);
                if (firstResult != null)
                {
                    return firstResult;
                }
            }

            if ((pair.SecondNumber != null && pair.SecondNumber >= 10))
            {
                return pair;
            }

            if (pair.SecondPair != null)
            {
                Pair? secondResult = First10OrGreater(pair.SecondPair);
                if (secondResult != null)
                {
                    return secondResult;
                }
            }

            return null;
        }

        public static Pair? FindRegular(Pair start, bool left)
        {
            Pair last = start;
            Pair? itr = start.Parent;

            while (itr != null)
            {
                Pair? node = left ? itr.FirstPair : itr.SecondPair;

                if (node == null)
                {
                    return itr;
                }

                if (node != last)
                {
                    return DepthFirst(node, !left);
                }

                last = itr;
                itr = itr.Parent;
            }

            return null;
        }

        public static void Explode(Pair pair)
        {
            Pair? firstRegularLeft = FindRegular(pair, true);
            Pair? firstRegularRight = FindRegular(pair, false);

            if (firstRegularLeft != null)
            {
                if (firstRegularLeft.SecondNumber != null)
                {
                    firstRegularLeft.SecondNumber += pair.FirstNumber;
                }
                else
                {
                    firstRegularLeft.FirstNumber += pair.FirstNumber;
                }
            }
            if (firstRegularRight != null)
            {
                if (firstRegularRight.FirstNumber != null)
                {
                    firstRegularRight.FirstNumber += pair.SecondNumber;
                }
                else
                {
                    firstRegularRight.SecondNumber += pair.SecondNumber;
                }
            }

            Pair parent = pair.Parent;
            pair.Parent = null;
            if (parent.FirstPair == pair)
            {
                parent.FirstPair = null;
                parent.FirstNumber = 0;
            }
            if (parent.SecondPair == pair)
            {
                parent.SecondPair = null;
                parent.SecondNumber = 0;
            }
        }

        public static bool Reduce(Pair pair)
        {
            Pair? volatilePair = FindVolatile(pair, 0);
            if (volatilePair != null)
            {
                Explode(volatilePair);
                return true;
            }

            Pair? first10OrGreater = First10OrGreater(pair);
            if (first10OrGreater != null)
            {
                if (first10OrGreater.FirstNumber != null && first10OrGreater.FirstNumber >= 10)
                {
                    int value = (int)first10OrGreater.FirstNumber;
                    first10OrGreater.FirstNumber = null;
                    first10OrGreater.FirstPair = Split(value, first10OrGreater);
                }
                else if (first10OrGreater.SecondNumber != null && first10OrGreater.SecondNumber >= 10)
                {
                    int value = (int)first10OrGreater.SecondNumber;
                    first10OrGreater.SecondNumber = null;
                    first10OrGreater.SecondPair = Split(value, first10OrGreater);
                }
                return true;
            }

            return false;
        }

        public static int Magnitude(Pair pair)
        {
            int left = pair.FirstPair != null ? Magnitude(pair.FirstPair) : (int)pair.FirstNumber;
            int right = pair.SecondPair != null ? Magnitude(pair.SecondPair) : (int)pair.SecondNumber;

            return left * 3 + right * 2;
        }

        public static List<Pair> ParsePairs(List<string> lines)
        {
            List<Pair> list = new();
            foreach (string line in lines)
            {
                Pair pair = Parse(line);
                UpdateParents(pair);
                list.Add(pair);
            }
            return list;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            Pair result = Parse(lines[0]);
            UpdateParents(result);

            for (int i = 1; i < lines.Count; ++i)
            {
                Pair number = Parse(lines[i]);
                UpdateParents(number);

                result = Add(result, number);
                while (Reduce(result));
            }

            Console.WriteLine(result);
            Console.WriteLine(Magnitude(result));

            int largestMagnitude = int.MinValue;
            for (int i = 0; i < lines.Count - 1; ++i)
            {
                for (int j = i + 1; j < lines.Count; ++j)
                {
                    List<Pair> pairs1 = ParsePairs(lines);
                    List<Pair> pairs2 = ParsePairs(lines);

                    Pair first1 = pairs1[i];
                    Pair second1 = pairs1[j];

                    Pair result1 = Add(first1, second1);
                    while (Reduce(result1)) ;

                    Pair first2 = pairs2[i];
                    Pair second2 = pairs2[j];

                    Pair result2 = Add(second2, first2);
                    while (Reduce(result2)) ;

                    largestMagnitude = Math.Max(largestMagnitude, Magnitude(result1));
                    largestMagnitude = Math.Max(largestMagnitude, Magnitude(result2));
                }
            }
            Console.WriteLine(largestMagnitude);

            Console.ReadLine();
        }
    }
}
