namespace Common
{
    public class Utils
    {
        public static Grid<int> ParseIntGrid(IEnumerable<string> lines)
        {
            Grid<int> grid = new();

            int y = 0;
            foreach (string line in lines)
            {
                int x = 0;
                foreach (char c in line)
                {
                    grid.Set(x, y, c - '0');
                    ++x;
                }
                ++y;
            }

            return grid;
        }

        public static string HexToBinary(string hex)
        {
            return string.Join(string.Empty, hex.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }
    }
}
