using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace solutions;

internal readonly struct Day1 : IAdventDay<int>
{
    private static readonly IEnumerable<string> data = Data.For<Day1>("\r\n\r\n");

    public readonly int Part1()
    {
        Span<int> calorieData = GetCalorieList();

        calorieData.Sort();

        return calorieData[^1];
    }

    public readonly int Part2()
    {
        Span<int> calorieData = GetCalorieList();

        calorieData.Sort();

        ref var sumSpace = ref MemoryMarshal.GetReference(calorieData[^3..]);

        int topThreeTotal = 0;
        for (int i = 0; i < 3; i++)
        {
            topThreeTotal += Unsafe.Add(ref sumSpace, i);
        }

        return topThreeTotal;
    }

    private static int[] GetCalorieList()
    {
        ReadOnlySpan<string> elveData = data.ToArray();
        Span<int> calorieBuffer = stackalloc int[elveData.Length];

        ref string elfSearchSpace = ref MemoryMarshal.GetReference(elveData);
        for (int i = 0, j = elveData.Length; i < j; i++)
        {
            ref string elf = ref Unsafe.Add(ref elfSearchSpace, i);

            ReadOnlySpan<string> data = elf.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            ref string dataSearchSpace = ref MemoryMarshal.GetReference(data);

            if (dataSearchSpace is null) continue;

            for (int x = 0, y = data.Length; x < y; x++)
            {
                ref string foodEntry = ref Unsafe.Add(ref dataSearchSpace, x);

                if (!int.TryParse(foodEntry, out int calorieValue))
                    continue;

                calorieBuffer[i] += calorieValue;
            }
        }

        return calorieBuffer.ToArray();
    }
}