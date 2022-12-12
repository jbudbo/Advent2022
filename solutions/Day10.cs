using System.Diagnostics;
using System.Text;

namespace solutions;

internal readonly struct Day10 : IAdventDay<int>
{
    private static readonly IEnumerable<string> data = Data.For<Day10>();

    public int Part1()
    {
        var buffer = data.ToArray();

        var breakpoints = new int[] { 20, 60, 100, 140, 180, 220 };

        return PlayState(buffer)
            .Join(breakpoints, static t => t.Item1, static b => b, static (t, _) => t)
            .Sum(static t => t.Item1 * t.Item2);
    }

    public int Part2()
    {
        const int VERT = 40;

        foreach (var (pixel, spriteCenter) in PlayState(data))
        {
            int xIndex = (pixel % VERT) - 1;
            bool lit = xIndex >= spriteCenter - 1 && xIndex <= spriteCenter + 1;
            char p = lit ? '#' : ' ';

            Console.Write(p);

            if (xIndex is -1) 
                Console.WriteLine();
        }

        return -1;
    }

    internal IEnumerable<(int, int)> PlayState(IEnumerable<string> program)
    {
        using var en = program.GetEnumerator();

        int x = 1, instIndex = 0, tick = 0;

        Span<string> buff;

        while (en.MoveNext())
        {
            buff = en.Current.Split();

            switch (buff[0])
            {
                case "noop":
                    yield return (++tick, x);
                    instIndex++;
                    break;
                case "addx" when int.TryParse(buff[1], out int val):
                    yield return (++tick, x);
                    yield return (++tick, x);
                    x += val;
                    instIndex++;
                    break;
                default:
                    throw new UnreachableException();
            }
        }
    }
}