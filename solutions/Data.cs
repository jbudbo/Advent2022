using System.Resources;

namespace solutions;

internal static class Data
{
    private static readonly ResourceManager resourceManager
        = new ("solutions.Inputs", typeof(Data).Assembly);

    internal static async IAsyncEnumerable<ReadOnlyMemory<char>> ForAsync<T>(string? token
        , StringSplitOptions opts = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
    {
        string file = $"Data/{typeof(T).Name}.dat";

        if (!File.Exists(file))
            yield break;

        await using var stream = File.OpenRead(file);
        using var rdr = new StreamReader(stream);

        int buffcount = 0;
        Memory<char> buffer = new char[1024];
        while (!rdr.EndOfStream)
        {
            buffcount = await rdr.ReadBlockAsync(buffer);
            if (buffcount is 0) yield break;

        }
    }

    internal static IEnumerable<string> For<T>(string? token = null
        , StringSplitOptions opts = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
    {
        string file = $"Data/{typeof(T).Name}.dat";

        if (!File.Exists(file))
            return Array.Empty<string>();

        string? data = File.ReadAllText(file);

        if (data is null)
            return Array.Empty<string>();

        token ??= Environment.NewLine;

        return data.Split(token, opts);
    }
}
