using System.Threading.Tasks;

namespace solutions;

internal interface IAdventDay<out T>
{
    internal T Part1();

    internal T Part2();
}

internal interface IAsyncAdventDay<T>
{
    internal ValueTask<T> Part1Async();

    internal ValueTask<T> Part2();
}