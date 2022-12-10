using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace solutions;

internal readonly struct Day9 : IAdventDay<int>
{
    private static readonly IEnumerable<string> data = Data.For<Day9>();

    public int Part1()
    {
        State s = new ();

        var moves = data.Select(static line =>
        {
            return (line[0], int.Parse(line[2..]));
        });
        foreach(var move in moves)
        {
            s.MoveHead(move.Item1, move.Item2);
        }

        return s.UniqeTailPositions;
    }

    public int Part2()
    {

        return -1;
    }

    private class State
    {
        private readonly HashSet<(int, int)> tailMovements = new();
        
        public int TotalTailMovements { get; private set; }
        public int UniqeTailPositions => tailMovements.Count;

        private (int X, int Y) head = default;
        private (int X, int Y) tail = default;

        public State()
        {
            tailMovements.Add(tail);
        }

        internal void MoveHead(char direction, int steps)
        {
            switch (direction)
            {
                case 'R':
                    while (steps-- > 0)
                    {
                        int delta = ++head.X - tail.X;
                        if (delta is 2)
                        {
                            tail.Y = head.Y;
                            tail.X++;
                            tailMovements.Add(tail);
                            TotalTailMovements++;
                        }
                    }
                    break;
                case 'L':
                    while (steps-- > 0)
                    {
                        int delta = tail.X - --head.X;
                        if (delta is 2)
                        {
                            tail.Y = head.Y;
                            tail.X--;
                            tailMovements.Add(tail);
                            TotalTailMovements++;
                        }
                    }
                    break;
                case 'U':
                    while (steps-- > 0)
                    {
                        int delta = ++head.Y - tail.Y;
                        if (delta is 2)
                        {
                            tail.X = head.X;
                            tail.Y++;
                            tailMovements.Add(tail);
                            TotalTailMovements++;
                        }
                    }
                    break;
                case 'D':
                    while (steps-- > 0)
                    {
                        int delta = tail.Y - --head.Y;
                        if (delta is 2)
                        {
                            tail.X = head.X;
                            tail.Y--;
                            tailMovements.Add(tail);
                            TotalTailMovements++;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
