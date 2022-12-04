namespace solutions;

internal readonly struct Day2 : IAdventDay<int>
{
    private static readonly IEnumerable<string> data = Data.For<Day2>();

    private static readonly Dictionary<char, Dictionary<char, int>> scoreMap = new(3)
    {
        //  I throw Rock (1)
        ['X'] = new(3)
        {
            //  They throw Paper (loss) 1 + 0
            ['B'] = 1,
            //  They throw Rock (tie) 1 + 3
            ['A'] = 4,
            //  They throw Scissors (win) 1 + 6
            ['C'] = 7,
        },
        //  I throw Paper (2)
        ['Y'] = new(3)
        {
            //  They throw Scissors (loss) 2 + 0
            ['C'] = 2,
            //  They throw Paper (tie) 2 + 3
            ['B'] = 5,
            //  They throw Rock (win) 2 + 6
            ['A'] = 8,
        },
        //  I throw Scissors (3)
        ['Z'] = new(3)
        {
            //  They throw Rock (loss) 3 + 0
            ['A'] = 3,
            //  They throw Scissors (tie) 3 + 3
            ['C'] = 6,
            // They throw Paper (win) 3 + 6
            ['B'] = 9,
        }
    };

    private static readonly Dictionary<char, Dictionary<char, int>> playMap = new(3)
    {
        //  I need to lose (0)
        ['X'] = new(3)
        {
            //  against Paper, so throw rock (1)
            ['B'] = 1, // 0 + 1
            //  against Scissors, so throw paper (2)
            ['C'] = 2, // 0 + 3
            //  against Rock, so throw scissors (3)
            ['A'] = 3, // 0 + 2
        },
        //  I need to tie (3)
        ['Y'] = new(3)
        {
            //  against Rock, throw rock (1)
            ['A'] = 4, // 3 + 1
            //  against Paper, throw paper (2)
            ['B'] = 5, // 3 + 2
            //  against Scissors, throw scissors (3)
            ['C'] = 6, // 3 + 3
        },
        //  I need to win(6)
        ['Z'] = new(3)
        {
            //  against Scissors, throw Rock (1)
            ['C'] = 7, // 6 + 1
            //  against Rock, throw paper (2)
            ['A'] = 8, // 6 + 2
            // against Paper, throw Scissors (3)
            ['B'] = 9, // 6 + 3
        }
    };

    public readonly int Part1() => data
        .Select(static play => scoreMap[play[2]][play[0]])
        .Sum();

    public readonly int Part2() => data
        .Select(static play => playMap[play[2]][play[0]])
        .Sum();
}