using System.Text.RegularExpressions;

namespace solutions;

internal readonly partial struct Day4 : IAdventDay<int>
{
    private static readonly Regex parser = GetParserRegex();

    public int Part1() => Inputs.Day4
        .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(ToRangePair)
        .Where(static rp =>
        {
            var ((a,b),(c,d)) = rp;
            return (a <= c && b >= d)
                || (c <= a && d >= b);
        })
        .Count();

    public int Part2() => Inputs.Day4
        .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(ToRangePair)
        .Where(static rp =>
        {
            var ((a, b), (c, d)) = rp;
            return c <= b && a <= d;
        })
        .Count();

    private static ((int,int), (int,int)) ToRangePair(string entry)
    {
        ReadOnlySpan<string> rangeParts = parser.Split(entry);

        int a = int.Parse(rangeParts[1])
            , b = int.Parse(rangeParts[2])
            , c = int.Parse(rangeParts[3])
            , d = int.Parse(rangeParts[4]);

        return ((a,b),(c,d));
    }

    [GeneratedRegex(@"^(\d+)-(\d+),(\d+)-(\d+)$")]
    public static partial Regex GetParserRegex();
}
