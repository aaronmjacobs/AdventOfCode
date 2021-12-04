using System.Text.RegularExpressions;

namespace _4
{
    public class Program
    {
        const int kBoardSize = 5;

        public static List<int[,]> ParseBoards(List<string> lines)
        {
            List<int[,]> boards = new();

            for (int group = 0; group < lines.Count; group += (kBoardSize + 1))
            {
                int[,] board = new int[kBoardSize, kBoardSize];

                for (int row = 0; row < kBoardSize; ++row)
                {
                    string[] numberStrings = Regex.Split(lines[group + 1 + row].Trim(), "\\s+");
                    if (numberStrings.Length == kBoardSize)
                    {
                        for (int col = 0; col < numberStrings.Length; ++col)
                        {
                            board[row, col] = int.Parse(numberStrings[col]);
                        }
                    }
                }

                boards.Add(board);
            }

            return boards;
        }

        public static void UpdateBoard(int[,] board, int drawnNumber)
        {
            for (int row = 0; row < kBoardSize; ++row)
            {
                for (int col = 0; col < kBoardSize; ++col)
                {
                    if (board[row, col] == drawnNumber)
                    {
                        board[row, col] = -1;
                    }
                }
            }
        }

        public static bool BoardWins(int[,] board)
        {
            for (int row = 0; row < kBoardSize; ++row)
            {
                bool colWin = true;
                for (int col = 0; col < kBoardSize; ++col)
                {
                    if (board[row, col] != -1)
                    {
                        colWin = false;
                        break;
                    }
                }

                if (colWin)
                {
                    return true;
                }
            }

            for (int col = 0; col < kBoardSize; ++col)
            {
                bool rowWin = true;
                for (int row = 0; row < kBoardSize; ++row)
                {
                    if (board[row, col] != -1)
                    {
                        rowWin = false;
                        break;
                    }
                }

                if (rowWin)
                {
                    return true;
                }
            }

            return false;
        }

        public static int ScoreBoard(int[,] board, int drawnNumber)
        {
            int sum = 0;
            for (int row = 0; row < kBoardSize; ++row)
            {
                for (int col = 0; col < kBoardSize; ++col)
                {
                    if (board[row, col] != -1)
                    {
                        sum += board[row, col];
                    }
                }
            }

            return sum * drawnNumber;
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            if (lines.Count == 0)
            {
                return;
            }

            string[] numberStrings = lines[0].Split(',');
            List<int> numbers = new();
            foreach (string numberString in numberStrings)
            {
                numbers.Add(int.Parse(numberString));
            }
            lines.RemoveAt(0);

            List<int[,]> boards = ParseBoards(lines);

            bool winFound = false;
            foreach (int number in numbers)
            {
                foreach (int[,] board in boards)
                {
                    UpdateBoard(board, number);

                    if (BoardWins(board) && (!winFound || boards.Count == 1))
                    {
                        int score = ScoreBoard(board, number);
                        Console.WriteLine(score);
                        winFound = true;
                    }
                }

                boards.RemoveAll(board => BoardWins(board));
            }

            Console.ReadLine();
        }
    }
}