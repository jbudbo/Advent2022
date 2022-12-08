namespace solutions;
internal readonly struct Day7 : IAdventDay<long>
{
    private static readonly IEnumerable<string> data = Data.For<Day7>();

    long IAdventDay<long>.Part1()
    {
        using var e = data.GetEnumerator();
        if (!e.MoveNext()) throw new InvalidDataException();

        var dir = new Directory(e);

        return dir.Find(static d => d.Size <= 100_000).Sum(static d => d.Size);
    }

    long IAdventDay<long>.Part2()
    {
        const long FULL_DISK = 70_000_000;
        const long REQUIRED_SIZE = 30_000_000;

        using var e = data.GetEnumerator();
        if (!e.MoveNext()) throw new InvalidDataException();

        var dir = new Directory(e);
        var spaceNeeded = REQUIRED_SIZE - (FULL_DISK - dir.Size);

        return dir
            .Find(d => d.Size >= spaceNeeded)
            .Min(static d => d.Size);
    }

    private readonly struct Directory
    {
        internal readonly string Name { get; } = string.Empty;
        internal readonly long Size { get => CalculateSizeInternal(); }

        internal readonly Dictionary<string, long> FileListing { get; } = new();
        internal readonly List<Directory> DirectoryListing { get; } = new();

        public Directory(IEnumerator<string> dirEnumerator)
        {
            //  We're expecting to start with a CD command
            if (dirEnumerator.Current[..4] != "$ cd")
                return;

            Name = dirEnumerator.Current[5..];

            PopulateChildren(dirEnumerator);
        }

        private readonly long CalculateSizeInternal() 
            => FileListing.Sum(static kvp => kvp.Value)
            + DirectoryListing.Sum(static d => d.Size);

        internal readonly IEnumerable<Directory> Find(Func<Directory, bool> predicate)
        {
            if (predicate(this)) yield return this;

            foreach (var dir in DirectoryListing)
            {
                foreach(var subDir in dir.Find(predicate))
                {
                    yield return subDir;
                }
            }
        }

        private void PopulateChildren(IEnumerator<string> dirEnumerator)
        {
            HashSet<string> directoriesToMake = new();
            while(dirEnumerator.MoveNext())
            {
                switch (dirEnumerator.Current) 
                {
                    //  Listings don't do much for us
                    case string ls when ls[..4] == "$ ls": continue;
                    case string cd when cd[..4] == "$ cd" && directoriesToMake.Contains(cd[5..]):
                        DirectoryListing.Add(new Directory(dirEnumerator));
                        break;
                    case string back when back == "$ cd ..": return;
                    case string dir when dir[..3] == "dir":
                        directoriesToMake.Add(dir[4..]);
                        break;
                    case string file:
                        var fileData = file.Split();
                        FileListing.Add(fileData[1], uint.Parse(fileData[0]));
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }
        }
    }
}