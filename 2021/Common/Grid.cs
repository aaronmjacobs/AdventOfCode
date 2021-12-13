namespace Common
{
    public class Grid<T>
    {
        private List<List<T>> _Elements;
        private readonly T? _DefaultElement;

        public Grid(T? defaultElement = default)
        {
            _Elements = new();
            _DefaultElement = defaultElement;
        }

        public int Width
        {
            get { return _Elements.Count == 0 ? 0 : _Elements[0].Count; }
        }

        public int Height
        {
            get { return _Elements.Count; }
        }

        public bool Has(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool Has(Point point)
        {
            return Has(point.X, point.Y);
        }

        public T? Get(int x, int y)
        {
            if (Has(x, y))
            {
                return _Elements[y][x];
            }

            return _DefaultElement;
        }

        public T? Get(Point point)
        {
            return Get(point.X, point.Y);
        }

        public void Set(int x, int y, T element)
        {
            EnsureLargeEnough(x, y);
            _Elements[y][x] = element;
        }

        public void Set(Point point, T element)
        {
            Set(point.X, point.Y, element);
        }

        public void Resize(int newWidth, int newHeight)
        {
            EnsureLargeEnough(newWidth - 1, newHeight - 1);

            int currentHeight = Height;
            if (newHeight < currentHeight)
            {
                _Elements.RemoveRange(newHeight, currentHeight - newHeight);
            }

            int currentWidth = Width;
            if (newWidth < currentWidth)
            {
                for (int y = 0; y < Height; ++y)
                {
                    _Elements[y].RemoveRange(newWidth, currentWidth - newWidth);
                }
            }
        }

        public void ForEach(Action<T? /* element */, int /* x */, int /* y */> function)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    function(_Elements[y][x], x, y);
                }
            }
        }

        public void ForEach(Action<T? /* element */, Point /* point */> function)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    function(_Elements[y][x], new Point(x, y));
                }
            }
        }

        public void ForEachAdjacent(int x, int y, Action<T? /* element */, int /* x */, int /* y */> function)
        {
            for (int yAdj = y - 1; yAdj <= y + 1; ++yAdj)
            {
                for (int xAdj = x - 1; xAdj <= x + 1; ++xAdj)
                {
                    if (!(yAdj == y && xAdj == x) && Has(xAdj, yAdj))
                    {
                        function(_Elements[yAdj][xAdj], xAdj, yAdj);
                    }
                }
            }
        }

        public void ForEachAdjacent(Point point, Action<T? /* element */, Point /* point */> function)
        {
            for (int yAdj = point.Y - 1; yAdj <= point.Y + 1; ++yAdj)
            {
                for (int xAdj = point.X - 1; xAdj <= point.X + 1; ++xAdj)
                {
                    if (!(yAdj == point.Y && xAdj == point.X) && Has(xAdj, yAdj))
                    {
                        function(_Elements[yAdj][xAdj], new Point(xAdj, yAdj));
                    }
                }
            }
        }

        public Grid<T> Map(Func<T? /* element */, int /* x */, int /* y */, T /* result */> function)
        {
            Grid<T> grid = new();

            ForEach((element, x, y) => grid.Set(x, y, function(element, x, y)));

            return grid;
        }

        public Grid<T> Map(Func<T? /* element */, Point /* point */, T /* result */> function)
        {
            Grid<T> grid = new();

            ForEach((element, point) => grid.Set(point, function(element, point)));

            return grid;
        }

        public U? Reduce<U>(Func<U? /* previous */, T? /* element */, int /* x */, int /* y */, U? /* result */> function, U? initial = default)
        {
            U? result = initial;

            ForEach((element, x, y) => result = function(result, element, x, y));

            return result;
        }

        public U? Reduce<U>(Func<U? /* previous */, T? /* element */, Point /* point */, U? /* result */> function, U? initial = default)
        {
            U? result = initial;

            ForEach((element, point) => result = function(result, element, point));

            return result;
        }

        public string ToStringCustom<TResult>(char RowSeparator = '\n', char ColSeparator = ' ', Func<T, TResult>? ToStringFunction = null)
        {
            if (ToStringFunction == null)
            {
                return string.Join(RowSeparator, _Elements.Select(row => string.Join(ColSeparator, row)));
            }
            else
            {
                return string.Join(RowSeparator, _Elements.Select(row => string.Join(ColSeparator, row.Select(ToStringFunction))));
            }
        }

        public override string ToString()
        {
            return ToStringCustom<object>();
        }

        private void EnsureLargeEnough(int x, int y)
        {
            if (y >= Height)
            {
                int numNewRows = (y + 1) - Height;
                List<T>[] newRows = new List<T>[numNewRows];
                for (int i = 0; i < newRows.Length; i++)
                {
                    T[] newCols = new T[Width];
                    Array.Fill(newCols, _DefaultElement);
                    newRows[i] = new List<T>(newCols);
                }

                _Elements.AddRange(newRows);
            }

            if (x >= Width)
            {
                int numNewCols = (x + 1) - Width;
                foreach (List<T> row in _Elements)
                {
                    T[] newCols = new T[numNewCols];
                    Array.Fill(newCols, _DefaultElement);
                    row.AddRange(newCols);
                }
            }
        }
    }
}
