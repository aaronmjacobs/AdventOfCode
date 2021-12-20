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

        public void ForEach(Action<T? /* element */, int /* x */, int /* y */> function, int inflation = 0)
        {
            for (int y = -inflation; y < Height + inflation; ++y)
            {
                for (int x = -inflation; x < Width + inflation; ++x)
                {
                    function(Get(x, y), x, y);
                }
            }
        }

        public void ForEach(Action<T? /* element */, Point /* point */> function, int inflation = 0)
        {
            ForEach((element, x, y) => function(element, new Point(x, y)), inflation);
        }

        public void ForEachNeighbor(int x, int y, Action<T? /* element */, int /* x */, int /* y */> function, bool includeSelf = false, bool unbounded = false)
        {
            for (int nY = y - 1; nY <= y + 1; ++nY)
            {
                for (int nX = x - 1; nX <= x + 1; ++nX)
                {
                    if ((includeSelf || !(nY == y && nX == x)) && (unbounded || Has(nX, nY)))
                    {
                        function(Get(nX, nY), nX, nY);
                    }
                }
            }
        }

        public void ForEachNeighbor(Point point, Action<T? /* element */, Point /* point */> function, bool includeSelf = false, bool unbounded = false)
        {
            ForEachNeighbor(point.X, point.Y, (element, x, y) => function(element, new Point(x, y)), includeSelf, unbounded);
        }

        public void ForEachAdjacent(int x, int y, Action<T? /* element */, int /* x */, int /* y */> function)
        {
            if (Has(x - 1, y))
            {
                function(_Elements[y][x - 1], x - 1, y);
            }
            if (Has(x + 1, y))
            {
                function(_Elements[y][x + 1], x + 1, y);
            }
            if (Has(x, y - 1))
            {
                function(_Elements[y - 1][x], x, y - 1);
            }
            if (Has(x, y + 1))
            {
                function(_Elements[y + 1][x], x, y + 1);
            }
        }

        public void ForEachAdjacent(Point point, Action<T? /* element */, Point /* point */> function)
        {
            ForEachAdjacent(point.X, point.Y, (element, x, y) => function(element, new Point(x, y)));
        }

        public Grid<T> Map(Func<T? /* element */, int /* x */, int /* y */, T /* result */> function)
        {
            Grid<T> grid = new();

            ForEach((element, x, y) => grid.Set(x, y, function(element, x, y)));

            return grid;
        }

        public Grid<T> Map(Func<T? /* element */, Point /* point */, T /* result */> function)
        {
            return Map((element, x, y) => function(element, new Point(x, y)));
        }

        public U? Reduce<U>(Func<U? /* previous */, T? /* element */, int /* x */, int /* y */, U? /* result */> function, U? initial = default)
        {
            U? result = initial;

            ForEach((element, x, y) => result = function(result, element, x, y));

            return result;
        }

        public U? Reduce<U>(Func<U? /* previous */, T? /* element */, Point /* point */, U? /* result */> function, U? initial = default)
        {
            return Reduce<U>((previous, element, x, y) => function(previous, element, new Point(x, y)));
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
