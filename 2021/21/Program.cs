namespace _21
{
    public struct State
    {
        public int Player1Pos;
        public int Player2Pos;

        public int Player1Score;
        public int Player2Score;

        public bool Player1Turn;
    }

    public struct WinCounts
    {
        public ulong Player1 = 0;
        public ulong Player2 = 0;

        public static WinCounts operator +(WinCounts first, WinCounts second)
        {
            WinCounts result = new();
            result.Player1 = first.Player1 + second.Player1;
            result.Player2 = first.Player2 + second.Player2;
            return result;
        }

        public static WinCounts operator *(WinCounts first, ulong second)
        {
            WinCounts result = new();
            result.Player1 = first.Player1 * second;
            result.Player2 = first.Player2 * second;
            return result;
        }
    }

    public class Program
    {
        public static Dictionary<State, WinCounts> StateMap = new();
        public static ulong[] RollCounts = new ulong[7];

        public static void ComputeRollCounts()
        {
            for (ulong first = 1; first <= 3; ++first)
            {
                for (ulong second = 1; second <= 3; ++second)
                {
                    for (ulong third = 1; third <= 3; ++third)
                    {
                        ++RollCounts[first + second + third - 3];
                    }
                }
            }
        }

        public static void Step(State state, ref WinCounts winCounts)
        {
            if (StateMap.TryGetValue(state, out WinCounts memoizedWinCounts))
            {
                winCounts += memoizedWinCounts;
            }
            else
            {
                WinCounts localWinCounts = new();
                Roll(state, ref localWinCounts);

                StateMap[state] = localWinCounts;
                winCounts += localWinCounts;
            }
        }

        public static void Roll(State state, ref WinCounts winCounts)
        {
            for (int i = 0; i < RollCounts.Length; ++i)
            {
                WinCounts localWinCounts = new();
                Update(state, i + 3, ref localWinCounts);

                winCounts += localWinCounts * RollCounts[i];
            }
        }

        public static void Update(State state, int value, ref WinCounts winCounts)
        {
            if (state.Player1Turn)
            {
                state.Player1Pos = (state.Player1Pos + value - 1) % 10 + 1;
                state.Player1Score += state.Player1Pos;
                if (state.Player1Score >= 21)
                {
                    ++winCounts.Player1;
                    return;
                }
            }
            else
            {
                state.Player2Pos = (state.Player2Pos + value - 1) % 10 + 1;
                state.Player2Score += state.Player2Pos;
                if (state.Player2Score >= 21)
                {
                    ++winCounts.Player2;
                    return;
                }
            }

            state.Player1Turn = !state.Player1Turn;
            Step(state, ref winCounts);
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            int player1Pos = int.Parse(lines[0].Split(": ")[1]);
            int player2Pos = int.Parse(lines[1].Split(": ")[1]);

            long player1Score = 0;
            long player2Score = 0;
            int die = 1;
            int rollCount = 0;
            while (true)
            {
                player1Pos = (player1Pos + die + die + 1 + die + 2 - 1) % 10 + 1;
                die = (die + 3 - 1) % 100 + 1;
                rollCount += 3;
                player1Score += player1Pos;
                if (player1Score >= 1000)
                {
                    break;
                }

                player2Pos = (player2Pos + die + die + 1 + die + 2 - 1) % 10 + 1;
                die = (die + 3 - 1) % 100 + 1;
                rollCount += 3;
                player2Score += player2Pos;
                if (player2Score >= 1000)
                {
                    break;
                }
            }

            Console.WriteLine(Math.Min(player1Score, player2Score) * rollCount);

            ComputeRollCounts();

            State state;
            state.Player1Pos = int.Parse(lines[0].Split(": ")[1]);
            state.Player2Pos = int.Parse(lines[1].Split(": ")[1]);
            state.Player1Score = 0;
            state.Player2Score = 0;
            state.Player1Turn = true;

            WinCounts winCounts = new();
            Step(state, ref winCounts);

            Console.WriteLine(Math.Max(winCounts.Player1, winCounts.Player2));

            Console.ReadLine();
        }
    }
}
