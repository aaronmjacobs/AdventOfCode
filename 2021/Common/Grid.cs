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

        public int NumRows
        {
            get { return _Elements.Count; }
        }

        public int NumCols
        {
            get { return _Elements.Count == 0 ? 0 : _Elements[0].Count; }
        }

        public T? Get(int row, int col)
        {
            if (row < NumRows && col < NumCols)
            {
                return _Elements[row][col];
            }

            return _DefaultElement;
        }

        public void Set(int row, int col, T element)
        {
            EnsureLargeEnough(row, col);
            _Elements[row][col] = element;
        }

        public void ForEach(Action<T? /* element */, int /* row */, int /* col */> function)
        {
            for (int row = 0; row < NumRows; ++row)
            {
                for (int col = 0; col < NumCols; ++col)
                {
                    function(_Elements[row][col], row, col);
                }
            }
        }

        public Grid<T> Map(Func<T? /* element */, int /* row */, int /* col */, T /* result */> function)
        {
            Grid<T> grid = new();

            ForEach((element, row, col) => grid.Set(row, col, function(element, row, col)));

            return grid;
        }

        public U? Reduce<U>(Func<U? /* previous */, T? /* element */, int /* row */, int /* col */, U? /* result */> function, U? initial = default)
        {
            U? result = initial;

            ForEach((element, row, col) => result = function(result, element, row, col));

            return result;
        }

        public override string ToString()
        {
            return string.Join('\n', _Elements.Select(elementRow => "[ " + string.Join(' ', elementRow) + " ]"));
        }

        private void EnsureLargeEnough(int row, int col)
        {
            if (row >= NumRows)
            {
                int numNewRows = (row + 1) - NumRows;
                List<T>[] newRows = new List<T>[numNewRows];
                for (int i = 0; i < newRows.Length; i++)
                {
                    T[] newCols = new T[NumCols];
                    Array.Fill(newCols, _DefaultElement);
                    newRows[i] = new List<T>(newCols);
                }

                _Elements.AddRange(newRows);
            }

            if (col >= NumCols)
            {
                int numNewCols = (col + 1) - NumCols;
                foreach (List<T> elementRow in _Elements)
                {
                    T[] newCols = new T[numNewCols];
                    Array.Fill(newCols, _DefaultElement);
                    elementRow.AddRange(newCols);
                }
            }
        }
    }
}
