namespace _24
{
    public class Program
    {
        public static void Monad(int[] input)
        {
            int[] divisors = new int[14] {   1,   1,   1,  26,   1,  26,   1,   1,  26,   1,  26,  26,  26,  26 };
            int[] xValues = new int[14]  {  12,  10,  13, -11,  13,  -1,  10,  11,   0,  10,  -5, -16,  -7, -11 };
            int[] yValues = new int[14]  {   6,   6,   3,  11,   9,   3,  13,   6,  14,  10,  12,  10,  11,  15 };
            //                               0    1    2    3    4    5    6    7    8    9   10   11   12   13

            int z = 0;
            for (int i = 0; i < 14; ++i)
            {
                int w = input[i];
                int x = (z % 26) + xValues[i];

                if (divisors[i] == 26)
                {
                    Console.WriteLine(i + " Pop " + (z % 26));
                }
                z /= divisors[i];

                if (x != w)
                {
                    Console.WriteLine(i + " Push " + (w + yValues[i]));
                    z = (26 * z) + (w + yValues[i]);
                }
            }

            Console.WriteLine(z == 0 ? "Valid\n" : "Invalid\n");
        }

        public static void Main(string[] args)
        {
            int[] maxInput = new int[14]    {   9,   9,   9,   1,   1,   9,   9,   3,   9,   4,   9,   6,   8,   4 };
            int[] minInput = new int[14]    {   6,   2,   9,   1,   1,   9,   4,   1,   7,   1,   6,   1,   1,   1 };

            Monad(maxInput);
            Monad(minInput);

            Console.ReadLine();
        }
    }
}
