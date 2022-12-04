using System.Resources;

namespace solutions;

internal static class Data
{
    private static readonly ResourceManager resourceManager
        = new ("solutions.Inputs", typeof(Data).Assembly);

    internal static IEnumerable<string> For<T>(string? token = null)
    {
        string resource = typeof(T).Name;

        string? data = resourceManager.GetString(resource);

        if (data is null) 
            return Array.Empty<string>();

        token ??= Environment.NewLine;

        return data.Split(token, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
