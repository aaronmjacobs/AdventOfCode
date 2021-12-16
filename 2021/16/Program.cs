namespace _15
{
    public class Program
    {
        public static string HexToBinary(string hex)
        {
            return String.Join(String.Empty, hex.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        public static int offset = 0;

        public static long Evaluate(string binary)
        {
            int version = Convert.ToInt32(binary.Substring(offset, 3), 2);
            offset += 3;

            int typeID = Convert.ToInt32(binary.Substring(offset, 3), 2);
            offset += 3;

            int subpacketsTotalLength = 0;
            int numSubpackets = 0;
            if (typeID != 4)
            {
                if (binary[offset++] == '0')
                {
                    subpacketsTotalLength = Convert.ToInt32(binary.Substring(offset, 15), 2);
                    offset += 15;
                }
                else
                {
                    numSubpackets = Convert.ToInt32(binary.Substring(offset, 11), 2);
                    offset += 11;
                }
            }
            int lastBit = offset + subpacketsTotalLength;

            long result = 0;
            switch (typeID)
            {
                case 0: // sum
                    if (subpacketsTotalLength > 0)
                    {
                        while (offset < lastBit)
                        {
                            result += Evaluate(binary);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSubpackets; ++i)
                        {
                            result += Evaluate(binary);
                        }
                    }
                    break;
                case 1: // product
                    result = 1;
                    if (subpacketsTotalLength > 0)
                    {
                        while (offset < lastBit)
                        {
                            result *= Evaluate(binary);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSubpackets; ++i)
                        {
                            result *= Evaluate(binary);
                        }
                    }
                    break;
                case 2: // minimum
                    result = long.MaxValue;
                    if (subpacketsTotalLength > 0)
                    {
                        while (offset < lastBit)
                        {
                            result = Math.Min(result, Evaluate(binary));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSubpackets; ++i)
                        {
                            result = Math.Min(result, Evaluate(binary));
                        }
                    }
                    break;
                case 3: // maximum
                    result = long.MinValue;
                    if (subpacketsTotalLength > 0)
                    {
                        while (offset < lastBit)
                        {
                            result = Math.Max(result, Evaluate(binary));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSubpackets; ++i)
                        {
                            result = Math.Max(result, Evaluate(binary));
                        }
                    }
                    break;
                case 4: // literal value
                    long literalValue = 0;
                    bool isLastSegment = false;
                    while (!isLastSegment)
                    {
                        literalValue <<= 4;
                        isLastSegment = binary[offset++] == '0';
                        long segmentValue = Convert.ToInt64(binary.Substring(offset, 4), 2);
                        offset += 4;

                        literalValue += segmentValue;
                    }
                    result = literalValue;
                    break;
                case 5: // greater than
                    long firstResult = 0;
                    long secondResult = 0;
                    bool firstEval = true;
                    if (subpacketsTotalLength > 0)
                    {
                        while (offset < lastBit)
                        {
                            if (firstEval)
                            {
                                firstResult = Evaluate(binary);
                            }
                            else
                            {
                                secondResult = Evaluate(binary);
                            }
                            firstEval = false;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSubpackets; ++i)
                        {
                            if (firstEval)
                            {
                                firstResult = Evaluate(binary);
                            }
                            else
                            {
                                secondResult = Evaluate(binary);
                            }
                            firstEval = false;
                        }
                    }
                    result = firstResult > secondResult ? 1 : 0;
                    break;
                case 6: // less than
                    long firstResult2 = 0;
                    long secondResult2 = 0;
                    bool firstEval2 = true;
                    if (subpacketsTotalLength > 0)
                    {
                        while (offset < lastBit)
                        {
                            if (firstEval2)
                            {
                                firstResult2 = Evaluate(binary);
                            }
                            else
                            {
                                secondResult2 = Evaluate(binary);
                            }
                            firstEval2 = false;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSubpackets; ++i)
                        {
                            if (firstEval2)
                            {
                                firstResult2 = Evaluate(binary);
                            }
                            else
                            {
                                secondResult2 = Evaluate(binary);
                            }
                            firstEval2 = false;
                        }
                    }
                    result = firstResult2 < secondResult2 ? 1 : 0;
                    break;
                case 7: // equal to
                    long firstResult3 = 0;
                    long secondResult3 = 0;
                    bool firstEval3 = true;
                    if (subpacketsTotalLength > 0)
                    {
                        while (offset < lastBit)
                        {
                            if (firstEval3)
                            {
                                firstResult3 = Evaluate(binary);
                            }
                            else
                            {
                                secondResult3 = Evaluate(binary);
                            }
                            firstEval3 = false;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSubpackets; ++i)
                        {
                            if (firstEval3)
                            {
                                firstResult3 = Evaluate(binary);
                            }
                            else
                            {
                                secondResult3 = Evaluate(binary);
                            }
                            firstEval3 = false;
                        }
                    }
                    result = firstResult3 == secondResult3 ? 1 : 0;
                    break;
                default:
                    break;
            }

            return result;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            string binary = HexToBinary(lines[0]);

            offset = 0;
            long result = Evaluate(binary);
            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
