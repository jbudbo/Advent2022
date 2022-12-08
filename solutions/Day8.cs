using System.Numerics;

namespace solutions;

internal readonly struct Day8 : IAdventDay<int>
{
    private static readonly IEnumerable<string> data = Data.For<Day8>();

    private sealed record Tree(int X, int Y, int Height);

    public int Part1()
    {
        HashSet<Tree> grove = new();
        
        int y = -1, x = 0;
        foreach(ReadOnlySpan<char> line in data)
        {
            y++;
            x = 0;
            foreach (var t in line)
            {
                grove.Add(new(x++, y, t - '0'));
            }
        }
        x--;

        HashSet<Tree> visibleTrees = new(grove.Where(tree => tree.X is 0 || tree.X == x || tree.Y is 0 || tree.Y == y));

        void ProcessTreeLine(int head, Tree[] line)
        {
            foreach (var tree in line)
            {
                if (tree.Height > head)
                {
                    head = tree.Height;
                    visibleTrees.Add(tree);
                }
            }
        }

        // Walk the rows
        while(--y > 0)
        {
            Tree[] line = grove.Where(t => t.Y == y).ToArray();
            ProcessTreeLine(line[0].Height, line[1..^1]);
            Array.Reverse(line);
            ProcessTreeLine(line[0].Height, line[1..^1]);
        }

        //  Walk the columns
        while(--x > 0)
        {
            Tree[] line = grove.Where(t => t.X == x).ToArray();
            ProcessTreeLine(line[0].Height, line[1..^1]);
            Array.Reverse(line);
            ProcessTreeLine(line[0].Height, line[1..^1]);
        }

        return visibleTrees.Count;
    }

    public static int CountVisibleTrees(int[] line, int baseLine)
    {
        int treesSeen = 0;
        foreach (var tree in line)
        {
            if (tree > baseLine)
            {
                baseLine = tree;
                treesSeen++;
            }
        }
        return treesSeen;
    }

    //public int Part1()
    //{
    //    int[][] grove = data
    //        .Select(row => row.Select(static c => c - '0').ToArray())
    //        .ToArray();

    //    //  We'll presume a square
    //    var dim = grove.Length - 1;

    //    int visibleTreeCount = dim * dim;

    //    int[] largestXabove = grove[0][1..^1]
    //        , largestYleft = grove.Select(static row => row[0]).ToArray()[1..^1];

    //    for (int y = 1; y < dim; y++)
    //    {
    //        for (int x = 1, h = grove[y][x]; x < dim; h = grove[y][++x])
    //        {
    //            //  If this tree has no height it's hopeless
    //            if (h is 0) continue;

    //            bool seenFromAbove = h > largestXabove[x - 1]
    //                , seenFromLeft = h > largestYleft[y - 1]
    //                //  Optimistically check if there is any reason to even peek
    //                , seenFromRight = h > grove[y][^1] && grove[y][(x+1)..^1].All(i => h > i)
    //                , seenFromBelow = h > grove[^1][x] && grove[(y+1)..^1].All(row => h > row[x]);

    //            if (seenFromAbove) largestXabove[x - 1] = h;
    //            if (seenFromLeft) largestYleft[y - 1] = h;

    //            if (seenFromAbove || seenFromLeft || seenFromRight || seenFromBelow)
    //                visibleTreeCount++;
    //        }
    //    }


    //    return visibleTreeCount;
    //}

    //public int Part1()
    //{
    //    // I really only want to walk through the grid once.
    //    //  So as I go I'll essentially move in "windows" in essence
    //    //  and any tree in the middle of that window that's a candidate
    //    //  will be buffered off so that we can verify it when we hit the next row

    //    string file = $"Data/Day8.dat";

    //    if (!File.Exists(file))
    //        return -1;

    //    using var dataStream = File.OpenRead(file);
    //    using var rdr = new StreamReader(dataStream);

    //    //  First we need to pad up the "Top" row so we can get started checking neighbors after
    //    ReadOnlySpan<char> rowBuffer = rdr.ReadLine();
    //    //  Presumre we're working with a square grid
    //    int width = rowBuffer.Length;

    //    //  We always see our perimiter trees so start with our perim
    //    int visibleTreeCount = (width - 1) * (width - 1);

    //    //  For checking above we only ever need to track the biggest tree we've seen thus far above us.
    //    //  If that one is ever bigger than or equal to the one we're on, the one we're on is not visible from above
    //    Span<char> largestBuffer = rowBuffer[1..^1].ToArray();

    //    ReadOnlySpan<char> rowAbove = largestBuffer;
    //    ref char rowAboveSpace = ref MemoryMarshal.GetReference(rowAbove);

    //    //  Now setup our focus row so we can essentially move our bottom
    //    rowBuffer = rdr.ReadLine();
    //    ReadOnlySpan<char> rowCenter = rowBuffer.ToArray();
    //    ref char rowCenterSpace = ref MemoryMarshal.GetReference(rowCenter);

    //    while (!rdr.EndOfStream)
    //    {
    //        rowBuffer = rdr.ReadLine();

    //        ref char rowSearchSpace = ref MemoryMarshal.GetReference(rowBuffer);
    //        //  Establish a baseline for tracking the tallest tree to our left as we go
    //        char largestLeft = rowSearchSpace;

    //        int i = 1;
    //        do
    //        {
    //            int l = i - 1, r = i + 1;
    //            ref char tree = ref Unsafe.Add(ref rowCenterSpace, i)
    //                , below = ref Unsafe.Add(ref rowSearchSpace, i)
    //                , right = ref Unsafe.Add(ref rowCenterSpace, r);

    //            //  If the tree is the shortest possible, there is no way it's visible
    //            if (tree is '0')
    //                continue;

    //            //  If our tree is bigger than any we've seen thus far on X,
    //            //      Increment our count and mark this as the tallest in the row thus far
    //            bool isLeftKing;
    //            if (isLeftKing = tree > largestLeft)
    //            {
    //                largestLeft = tree;
    //                visibleTreeCount++;
    //            }

    //            //  If our tree is bigger than any we've seen thus far on Y,
    //            //      Increment our count and mark this as the tallest in the column thus far
    //            bool isTopKing;
    //            if (isTopKing = tree > largestBuffer[l])
    //            {
    //                largestBuffer[l] = tree;

    //                //  If we haven't already counted this tree
    //                if (!isLeftKing) visibleTreeCount++;
    //            }

    //            //  Now we need to start reviewing to the right of us but only if our tree is larger than the one to our right
    //            //      otherwise it's not worth it
    //            while (tree > right && ++r <= width) right = ref Unsafe.Add(ref rowCenterSpace, r);
    //            if (r == width+1 && !isLeftKing && !isTopKing)
    //                visibleTreeCount++;

    //        } while (++i < width - 1);

    //        rowAbove = rowCenter;
    //        rowAboveSpace = ref MemoryMarshal.GetReference(rowAbove);
    //        rowCenter = rowBuffer;
    //        rowCenterSpace = ref MemoryMarshal.GetReference(rowCenter);
    //    }

    //    return visibleTreeCount;
    //}

    int IAdventDay<int>.Part2()
    {
        throw new NotImplementedException();
    }
}
