using System.Diagnostics.CodeAnalysis;

namespace _21
{
    public struct State
    {
        public int Player1Pos = 0;
        public int Player2Pos = 0;

        public int Player1Score = 0;
        public int Player2Score = 0;

        public bool Player1Turn = true;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is State s
                && s.Player1Pos == Player1Pos
                && s.Player2Pos == Player2Pos
                && s.Player1Score == Player1Score
                && s.Player2Score == Player2Score
                && s.Player1Turn == Player1Turn;
        }

        public override int GetHashCode()
        {
            // 4 bits for position (0-10)
            // 5 bits for score (0-21)
            // 1 bit for turn
            return Player1Pos << 0
                | Player2Pos << 4
                | Player1Score << 8
                | Player2Score << 13
                | (Player1Turn ? 1 : 0) << 18;
        }

        public static bool operator ==(State left, State right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(State left, State right)
        {
            return !(left == right);
        }
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
        public static ulong[] ComputeRollCounts()
        {
            ulong[] rollCounts = new ulong[7];

            for (ulong first = 1; first <= 3; ++first)
            {
                for (ulong second = 1; second <= 3; ++second)
                {
                    for (ulong third = 1; third <= 3; ++third)
                    {
                        ++rollCounts[first + second + third - 3];
                    }
                }
            }

            return rollCounts;
        }

        public static void Step(State state, Dictionary<State, WinCounts> stateMap, ulong[] rollCounts, ref WinCounts winCounts)
        {
            if (stateMap.TryGetValue(state, out WinCounts memoizedWinCounts))
            {
                winCounts += memoizedWinCounts;
            }
            else if (state.Player1Score >= 21)
            {
                ++winCounts.Player1;
            }
            else if (state.Player2Score >= 21)
            {
                ++winCounts.Player2;
            }
            else
            {
                WinCounts localWinCounts = new();
                for (int i = 0; i < rollCounts.Length; ++i)
                {
                    int value = i + 3;

                    State rollState = state;
                    if (rollState.Player1Turn)
                    {
                        rollState.Player1Pos = (rollState.Player1Pos + value - 1) % 10 + 1;
                        rollState.Player1Score += rollState.Player1Pos;
                    }
                    else
                    {
                        rollState.Player2Pos = (rollState.Player2Pos + value - 1) % 10 + 1;
                        rollState.Player2Score += rollState.Player2Pos;
                    }
                    rollState.Player1Turn = !rollState.Player1Turn;

                    WinCounts rollWinCounts = new();
                    Step(rollState, stateMap, rollCounts, ref rollWinCounts);

                    localWinCounts += rollWinCounts * rollCounts[i];
                }

                stateMap[state] = localWinCounts;
                winCounts += localWinCounts;
            }
        }

        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();

            State initialState = new();
            initialState.Player1Pos = int.Parse(lines[0].Split(": ")[1]);
            initialState.Player2Pos = int.Parse(lines[1].Split(": ")[1]);

            State part1State = initialState;
            int die = 1;
            int rollCount = 0;
            while (true)
            {
                part1State.Player1Pos = (part1State.Player1Pos + die * 3 + 2) % 10 + 1;
                die = (die + 2) % 100 + 1;
                rollCount += 3;
                part1State.Player1Score += part1State.Player1Pos;
                if (part1State.Player1Score >= 1000)
                {
                    break;
                }

                part1State.Player2Pos = (part1State.Player2Pos + die * 3 + 2) % 10 + 1;
                die = (die + 2) % 100 + 1;
                rollCount += 3;
                part1State.Player2Score += part1State.Player2Pos;
                if (part1State.Player2Score >= 1000)
                {
                    break;
                }
            }
            Console.WriteLine(Math.Min(part1State.Player1Score, part1State.Player2Score) * rollCount);

            State part2State = initialState;
            Dictionary<State, WinCounts> stateMap = new();
            ulong[] rollCounts = ComputeRollCounts();
            WinCounts winCounts = new();
            Step(part2State, stateMap, rollCounts, ref winCounts);
            Console.WriteLine(Math.Max(winCounts.Player1, winCounts.Player2));

            Console.ReadLine();
        }
    }
}
