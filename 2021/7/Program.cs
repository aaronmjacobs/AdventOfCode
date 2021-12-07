namespace _7
{
    public class Program
    {
        public static int FuelCost(List<int> positions, int index, bool complex)
        {
            int cost = 0;

            foreach (int position in positions)
            {
                int diff = Math.Abs(position - index);
                cost += complex ? (diff * (diff + 1)) / 2 : diff;
            }

            return cost;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            List<int> positions = lines[0].Split(',').Select(int.Parse).ToList();

            int minCostBasic = int.MaxValue;
            int minCostComplex = int.MaxValue;
            for (int i = 0; i < positions.Max(); i++)
            {
                int basicCost = FuelCost(positions, i, false);
                int complexCost = FuelCost(positions, i, true);

                minCostBasic = Math.Min(minCostBasic, basicCost);
                minCostComplex = Math.Min(minCostComplex, complexCost);
            }

            Console.WriteLine($"Baisc: {minCostBasic}, complex: {minCostComplex}");
            Console.ReadLine();
        }
    }
}
