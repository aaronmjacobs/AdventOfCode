using Common;

namespace _15
{
    public class Interpreter
    {
        private string _binary;
        private int _offset;

        public Interpreter(string binary)
        {
            _binary = binary;
            _offset = 0;
        }

        public long Evaluate()
        {
            int version = ParseInt(3);
            int typeID = ParseInt(3);

            int numSubpackets = 0;
            int lastSubpacketBit = 0;
            if (typeID != 4) // operator
            {
                if (ParseBool())
                {
                    numSubpackets = ParseInt(11);
                }
                else
                {
                    int subpacketsTotalLength = ParseInt(15);
                    lastSubpacketBit = _offset + subpacketsTotalLength;
                }
            }

            void forEachSubpacket(Action action)
            {
                for (int i = 0; i < numSubpackets; ++i)
                {
                    action();
                }

                while (_offset < lastSubpacketBit)
                {
                    action();
                }
            }

            long result = 0;
            switch (typeID)
            {
                case 0: // sum
                    forEachSubpacket(() => result += Evaluate());
                    break;
                case 1: // product
                    result = 1;
                    forEachSubpacket(() => result *= Evaluate());
                    break;
                case 2: // minimum
                    result = long.MaxValue;
                    forEachSubpacket(() => result = Math.Min(result, Evaluate()));
                    break;
                case 3: // maximum
                    result = long.MinValue;
                    forEachSubpacket(() => result = Math.Max(result, Evaluate()));
                    break;
                case 4: // literal value
                    bool isLastSegment = false;
                    while (!isLastSegment)
                    {
                        isLastSegment = !ParseBool();
                        result = (result << 4) + ParseInt(4);
                    }
                    break;
                case 5: // greater than
                    result = Evaluate() > Evaluate() ? 1 : 0;
                    break;
                case 6: // less than
                    result = Evaluate() < Evaluate() ? 1 : 0;
                    break;
                case 7: // equal to
                    result = Evaluate() == Evaluate() ? 1 : 0;
                    break;
                default:
                    break;
            }

            return result;
        }

        private bool ParseBool()
        {
            return _binary[_offset++] == '1';
        }

        private int ParseInt(int numBits)
        {
            int result = Convert.ToInt32(_binary.Substring(_offset, numBits), 2);
            _offset += numBits;
            return result;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            string binary = Utils.HexToBinary(lines[0]);

            Interpreter interpreter = new(binary);
            long result = interpreter.Evaluate();
            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
