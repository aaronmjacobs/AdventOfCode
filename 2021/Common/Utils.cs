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
    }
}
