namespace solutions;

internal readonly struct Day3 : IAdventDay<int>
{
    public readonly int Part1()=> Inputs.Day3
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(static rucksack =>
        {
            int midpoint = rucksack.Length / 2;
            var (x, y) = (rucksack[..midpoint], rucksack[midpoint..]);
            char intersection = x.Intersect(y).SingleOrDefault();
            return char.IsUpper(intersection)
                ? intersection - 38
                : intersection - 96;
        })
        .Sum();

    public readonly int Part2() => Inputs.Day3
        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Chunk(3)
        .Select(static group =>
        {
            IEnumerable<char> a = group[0].Intersect(group[1])
                , b = group[1].Intersect(group[2])
                , c = a.Intersect(b);

            char badge = c.SingleOrDefault();

            return char.IsUpper(badge)
                ? badge - 38
                : badge - 96;
        })
        .Sum();

}
