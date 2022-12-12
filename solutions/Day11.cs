using System.Diagnostics;
using System.Text.RegularExpressions;

namespace solutions;

internal readonly partial struct Day11 : IAdventDay<long>
{
    private static readonly string data = Data.Full<Day11>();
    private static readonly Regex monkeyRegex = GetMonkeyRegex();

    public long Part1()
    {
        var monkeys = monkeyRegex
            .Matches(data)
            .Select(static m => new Monkey(m))
            .ToArray();

        for (int round = 1; round <= 20; round++)
        {
            foreach(var monkey in monkeys)
            {
                foreach(var (w,m) in monkey.PlayRound())
                {
                    monkeys[m].Receive(w);
                }
            }
        }

        long[] business = monkeys.Select(static m => m.InspectCount).OrderDescending().Take(2).ToArray();

        return business[0] * business[1];
    }

    public long Part2()
    {
        var monkeys = monkeyRegex
            .Matches(data)
            .Select(static m => new Monkey(m, 1))
            .ToArray();

        var reductionFactor = monkeys.Aggregate(1L, static (f, m) => f * m.Divisor);

        for (int round = 1; round <= 10_000; round++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (var (w, m) in monkey.PlayRound(reductionFactor))
                {
                    monkeys[m].Receive(w);
                }
            }
        }

        long[] business = monkeys.Select(static m => m.InspectCount).OrderDescending().Take(2).ToArray();

        return business[0] * business[1];
    }

    [GeneratedRegex("\\D+(\\d+)\\D+\\D+([\\d, ]+)\\D+new = old (\\*|\\+|\\-|\\/) (old|\\d+)\\D+(\\d+)\\D+(\\d+)\\D+(\\d+)")]
    private static partial Regex GetMonkeyRegex();

    [DebuggerDisplay("{index}, {InspectCount}, {items.Count}")]
    private sealed class Monkey
    {
        private readonly int index;

        internal int Divisor { get; init; }

        private readonly int? multiplier;

        private readonly char sign;

        private readonly int monkeyTrue;

        private readonly int monkeyFalse;

        private readonly int worryModifier;

        private readonly Queue<long> items;
        
        internal long InspectCount { get; private set; }

        public Monkey(Match m, int worryModifier = 3)
        {
            InspectCount = 0;

            this.worryModifier = worryModifier;

            index = int.Parse(m.Groups[1].Value);

            var startingItems = m.Groups[2].Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(long.Parse);

            items = new(startingItems);

            sign = m.Groups[3].Value[0];

            multiplier = int.TryParse(m.Groups[4].Value, out int val)
                ? val : null;

            Divisor = int.Parse(m.Groups[5].Value);

            monkeyTrue = int.Parse(m.Groups[6].Value);

            monkeyFalse = int.Parse(m.Groups[7].Value);
        }

        private long MultVal(in long i) => i * (multiplier ?? i);
        private long SubVal(in long i) => i - (multiplier ?? i);
        private long AddVal(in long i) => i + (multiplier ?? i);
        private long DivVal(in long i) => i / (multiplier ?? i);

        internal void Receive(in long worry) => items.Enqueue(worry);

        public IEnumerable<(long worry, int monkey)> PlayRound(long? factor = null)
        {
            long i = items.Count;
            while (items.Count > 0)
            {
                long worry = items.Dequeue();
                long adjusted = sign switch
                {
                    '*' => MultVal(in worry),
                    '-' => SubVal(in worry),
                    '+' => AddVal(in worry),
                    '/' => DivVal(in worry),
                    _ => throw new UnreachableException()
                } / worryModifier;
                if (factor is long f)
                    adjusted %= f;
                yield return (adjusted, adjusted % Divisor is 0 ?  monkeyTrue : monkeyFalse);
            }
            InspectCount += i;
        }
    }
}