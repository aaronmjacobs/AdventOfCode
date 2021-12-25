using System.Diagnostics.CodeAnalysis;

namespace _23
{
    public enum AmphipodType
    {
        A,
        B,
        C,
        D
    }

    public struct Amphipod
    {
        public AmphipodType Type;
        public int MoveCount = 0;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Amphipod a
                && a.Type == Type
                && a.MoveCount == MoveCount;
        }

        public static bool operator ==(Amphipod lhs, Amphipod rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Amphipod lhs, Amphipod rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return 1 | ((int)Type << 1) | (MoveCount << 3);
        }

        public Amphipod(AmphipodType type)
        {
            Type = type;
        }

        public override String ToString() => Type.ToString();

        public static Amphipod Parse(char c)
        {
            switch (c)
            {
                case 'A':
                    return new Amphipod(AmphipodType.A);
                case 'B':
                    return new Amphipod(AmphipodType.B);
                case 'C':
                    return new Amphipod(AmphipodType.C);
                case 'D':
                    return new Amphipod(AmphipodType.D);
                default:
                    return new Amphipod(AmphipodType.A);
            }
        }
    }

    public struct Burrow
    {
        public const int HallwayLength = 11;

        private int RoomLength;
        private Amphipod?[] Hallway;
        private Amphipod?[,] SideRooms;

        private List<int> _occupiedIndices = new();
        private List<int> _emptyIndices = new();

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Burrow b && b.RoomLength == RoomLength)
            {
                for (int i = 0; i < Hallway.Length; ++i)
                {
                    if (b.Hallway[i] != Hallway[i])
                    {
                        return false;
                    }
                    if (b.Hallway[i].HasValue && b.Hallway[i].Value != Hallway[i].Value)
                    {
                        return false;
                    }
                }
                for (int i = 0; i < SideRooms.GetLength(0); ++i)
                {
                    for (int j = 0; j < SideRooms.GetLength(1); ++j)
                    {
                        if (b.SideRooms[i,j] != SideRooms[i,j])
                        {
                            return false;
                        }
                        if (b.SideRooms[i,j].HasValue && b.SideRooms[i,j].Value != SideRooms[i,j].Value)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public static bool operator ==(Burrow lhs, Burrow rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Burrow lhs, Burrow rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            int result = 0;

            // 5 bits per amphipod
            int shift = 0;

            for (int i = 0; i < Hallway.Length; ++i)
            {
                result |= Hallway[i].HasValue ? Hallway[i].GetHashCode() << shift : 0;

                shift += 5;
                if (shift >= 32)
                {
                    shift = 0;
                }
            }
            for (int i = 0; i < SideRooms.GetLength(0); ++i)
            {
                for (int j = 0; j < SideRooms.GetLength(1); ++j)
                {
                    result |= SideRooms[i,j].HasValue ? SideRooms[i,j].GetHashCode() << shift : 0;

                    shift += 5;
                    if (shift >= 32)
                    {
                        shift = 0;
                    }
                }
            }

            return result;
        }

        public Burrow(List<string> lines)
        {
            RoomLength = lines.Count - 3;
            Hallway = new Amphipod?[11];
            SideRooms = new Amphipod?[4, RoomLength];

            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < RoomLength; ++j)
                {
                    SideRooms[i, j] = Amphipod.Parse(lines[j + 2][3 + i * 2]);
                }
            }

            for (int i = 0; i < Hallway.Length + SideRooms.Length; ++i)
            {
                if (IsSideRoom(i))
                {
                    _occupiedIndices.Add(i);
                }
                else
                {
                    _emptyIndices.Add(i);
                }
            }
        }

        public Burrow Clone()
        {
            Burrow newBurrow = this;

            newBurrow.Hallway = new Amphipod?[11];
            Array.Copy(Hallway, newBurrow.Hallway, Hallway.Length);

            newBurrow.SideRooms = new Amphipod?[4, RoomLength];
            Array.Copy(SideRooms, newBurrow.SideRooms, SideRooms.Length);

            newBurrow._occupiedIndices = new(_occupiedIndices);
            newBurrow._emptyIndices = new(_emptyIndices);

            return newBurrow;
        }

        public void GetSideRoomIndices(int index, out int sideRoom, out int sideRoomIndex)
        {
            sideRoom = (index - HallwayLength) / RoomLength;
            sideRoomIndex = (index - HallwayLength) % RoomLength;
        }

        public Amphipod? Get(int index)
        {
            if (IsHallway(index))
            {
                return Hallway[index];
            }

            GetSideRoomIndices(index, out int sideRoom, out int sideRoomIndex);
            return SideRooms[sideRoom, sideRoomIndex];
        }

        public void Set(int index, Amphipod? amphipod)
        {
            if (amphipod == null)
            {
                _occupiedIndices.Remove(index);
                _emptyIndices.Add(index);
            }
            else
            {
                _occupiedIndices.Add(index);
                _emptyIndices.Remove(index);
            }

            if (IsHallway(index))
            {
                Hallway[index] = amphipod;
                return;
            }

            GetSideRoomIndices(index, out int sideRoom, out int sideRoomIndex);
            SideRooms[sideRoom, sideRoomIndex] = amphipod;
        }

        public bool IsHallway(int index)
        {
            return index < HallwayLength;
        }

        public bool IsSideRoom(int index)
        {
            return !IsHallway(index);
        }

        public bool IsSideRoomType(int index, Amphipod amphipod)
        {
            if (IsHallway(index))
            {
                return false;
            }

            int sideRoom = (index - HallwayLength) / RoomLength;
            switch (amphipod.Type)
            {
                case AmphipodType.A:
                    return sideRoom == 0;
                case AmphipodType.B:
                    return sideRoom == 1;
                case AmphipodType.C:
                    return sideRoom == 2;
                case AmphipodType.D:
                    return sideRoom == 3;
            }

            return false;
        }

        public void GetHallwayDistanceAndIndex(int index, out int hallwayDistance, out int hallwayIndex)
        {
            if (IsHallway(index))
            {
                hallwayDistance = 0;
                hallwayIndex = index;
            }
            else
            {
                GetSideRoomIndices(index, out int sideRoom, out int sideRoomIndex);

                hallwayDistance = sideRoomIndex + 1;
                hallwayIndex = 2 + sideRoom * 2;
            }
        }

        public List<int> GetMoveIndices(int sourceIndex, int destinationIndex)
        {
            List<int> moveIndices = new();

            GetHallwayDistanceAndIndex(sourceIndex, out int sourceHallwayDistance, out int sourceHallwayIndex);
            GetHallwayDistanceAndIndex(destinationIndex, out int destinationHallwayDistance, out int destinationHallwayIndex);

            int currentIndex = sourceIndex;
            while (sourceHallwayDistance > 0)
            {
                moveIndices.Add(currentIndex);
                --currentIndex;
                --sourceHallwayDistance;
            }

            int hallwayIndexDelta = sourceHallwayIndex < destinationHallwayIndex ? 1 : -1;
            for (currentIndex = sourceHallwayIndex; currentIndex != destinationHallwayIndex + hallwayIndexDelta; currentIndex += hallwayIndexDelta)
            {
                moveIndices.Add(currentIndex);
            }

            currentIndex = destinationIndex - (destinationHallwayDistance - 1);
            while (destinationHallwayDistance > 0)
            {
                moveIndices.Add(currentIndex);
                ++currentIndex;
                --destinationHallwayDistance;
            }

            return moveIndices;
        }

        public int? GetMoveCost(int sourceIndex, int destinationIndex)
        {
            Amphipod? amphipod = Get(sourceIndex);
            if (amphipod == null)
            {
                return null;
            }

            int distance = GetMoveIndices(sourceIndex, destinationIndex).Count - 1;

            int energyPerStep = 0;
            switch (amphipod.Value.Type)
            {
                case AmphipodType.A:
                    energyPerStep = 1;
                    break;
                case AmphipodType.B:
                    energyPerStep = 10;
                    break;
                case AmphipodType.C:
                    energyPerStep = 100;
                    break;
                case AmphipodType.D:
                    energyPerStep = 1000;
                    break;
            }

            return distance * energyPerStep;
        }

        public bool IsValidMove(int sourceIndex, int destinationIndex)
        {
            // Source must not be empty, destination must be empty
            Amphipod? source = Get(sourceIndex);
            if (source == null || Get(destinationIndex) != null)
            {
                return false;
            }

            // Amphipods will never stop on the space immediately outside any room.
            if (destinationIndex == 2 || destinationIndex == 4 || destinationIndex == 6 || destinationIndex == 8)
            {
                return false;
            }

            // Once an amphipod stops moving in the hallway, it will stay in that spot until it can move into a room.
            if (source.Value.MoveCount > 1 || (source.Value.MoveCount > 0 && IsHallway(destinationIndex)))
            {
                return false;
            }

            // Amphipods will never move from the hallway into a room unless that room is their destination room and that room contains no amphipods which do not also have that room as their own destination
            if (IsSideRoom(destinationIndex))
            {
                if (!IsSideRoomType(destinationIndex, source.Value))
                {
                    return false;
                }

                GetSideRoomIndices(destinationIndex, out int sideRoom, out int sideRoomIndex);
                for (int i = 0; i < RoomLength; ++i)
                {
                    Amphipod? existingAmphipod = SideRooms[sideRoom, i];
                    if (i > sideRoomIndex && existingAmphipod == null)
                    {
                        // Don't place amphipods above empty spaces in rooms
                        return false;
                    }

                    if (existingAmphipod != null && existingAmphipod.Value.Type != source.Value.Type)
                    {
                        return false;
                    }
                }
            }

            // Can't go through other amphipods
            List<int> moveIndices = GetMoveIndices(sourceIndex, destinationIndex);
            for (int i = 1; i < moveIndices.Count; ++i)
            {
                if (Get(moveIndices[i]) != null)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsSolved()
        {
            for (int i = 0; i < SideRooms.GetLength(0); ++i)
            {
                AmphipodType type = AmphipodType.A;
                switch (i)
                {
                    case 0:
                        type = AmphipodType.A;
                        break;
                    case 1:
                        type = AmphipodType.B;
                        break;
                    case 2:
                        type = AmphipodType.C;
                        break;
                    case 3:
                        type = AmphipodType.D;
                        break;
                }

                for (int j = 0; j < SideRooms.GetLength(1); ++j)
                {
                    Amphipod? amphipod = SideRooms[i, j];
                    if (!amphipod.HasValue || amphipod.Value.Type != type)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int? ComputeMinSolve(Dictionary<Burrow, int?> memoizedBurrows)
        {
            if (memoizedBurrows.TryGetValue(this, out int? memoizedValue))
            {
                return memoizedValue;
            }

            if (IsSolved())
            {
                return 0;
            }

            int? minSolve = null;

            foreach (int occupiedIndex in _occupiedIndices)
            {
                foreach (int emptyIndex in _emptyIndices)
                {
                    if (IsValidMove(occupiedIndex, emptyIndex))
                    {
                        int? moveCost = GetMoveCost(occupiedIndex, emptyIndex);

                        if (moveCost != null)
                        {
                            Burrow newBurrow = Clone();
                            Amphipod newAmphipod = newBurrow.Get(occupiedIndex).GetValueOrDefault();

                            ++newAmphipod.MoveCount;
                            newBurrow.Set(emptyIndex, newAmphipod);
                            newBurrow.Set(occupiedIndex, null);

                            int? subSolveCost = newBurrow.ComputeMinSolve(memoizedBurrows);
                            if (subSolveCost != null)
                            {
                                int solveCost = moveCost.Value + subSolveCost.Value;
                                if (minSolve == null || solveCost < minSolve.Value)
                                {
                                    minSolve = solveCost;
                                }
                            }
                        }
                    }
                }
            }

            memoizedBurrows[this] = minSolve;
            return minSolve;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = File.ReadLines(@"../../../input.txt").ToList();
            Burrow burrow = new(lines);

            Dictionary<Burrow, int?> memoizedBurrows = new();
            int? minSolve = burrow.ComputeMinSolve(memoizedBurrows);
            Console.WriteLine(minSolve);

            Console.ReadLine();
        }
    }
}
