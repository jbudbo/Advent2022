namespace solutions;

internal readonly struct Day6 : IAdventDay<int>
{
    private static readonly string data = Data.For<Day6>().Single();

    int IAdventDay<int>.Part1() => Windowed(data, 4)
        .First(static set => !set.Item3).Item1 + 4;

    int IAdventDay<int>.Part2() => BackWindowProp(data, 14);

    private static int BackWindowProp(in string data, in int windowSize)
    {
        int l = data.Length, index = 0;
        while (index < l)
        {
            int state = 0, pos = windowSize - 1;
            while (pos >= 0)
            {
                int bitIndex = 1 << (data[index + pos--] % 32);
                if ((state & bitIndex) is not 0) break;
                state |= bitIndex;
            }
            if (pos is -1) return index + windowSize + 1;
            index += pos + 1;
        }
        return -1;
    }

    //int IAdventDay<int>.Part2() => Windowed(data, 14)
    //    .First(static set => !set.Item3).Item1 + 14;

    private static IEnumerable<(int, string, bool)> Windowed(string source, int size)
    {
        int l = source.Length, i = -1;
        while (++i < l)
        {
            if (i + size > l) yield break;

            string buffer = source.Substring(i, size);

            HashSet<char> uniqueMap = new(buffer);

            yield return
            (
                i, // Index
                buffer, // Elements
                uniqueMap.Count != size // Any matches
            );
        }
    }
}
