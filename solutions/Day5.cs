using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace solutions;

internal readonly struct Day5 : IAdventDay<string>
{
    private const int DIM = 9;

    private static readonly IEnumerable<string> data = Data.For<Day5>(opts: StringSplitOptions.None);

    string IAdventDay<string>.Part1()
    {
        Stack<char>[] state = Enumerable.Range(1, DIM).Select(static _ => new Stack<char>()).ToArray();

        ReadOnlySpan<string> buffer = data.ToArray();

        void LoadState(ref Span<char> stateBuffer)
        {
            for (int i = 0; i < DIM; i++)
            {
                if (stateBuffer[i] is ' ') continue;
                state[i].Push(stateBuffer[i]);
            }
        }
        BuildContainerState(buffer[..DIM], LoadState);

        foreach(var instruction in buffer[(DIM+1)..])
        {
            if (string.IsNullOrWhiteSpace(instruction)) continue;

            var (count, from, to) = ExtractInstructionSet(instruction);
            for (int i = 0; i < count; i++) state[to - 1].Push(state[from - 1].Pop());
        }

        StringBuilder answer = new(DIM);
        for(int i = 0; i < DIM; i++)
        {
            answer.Append(state[i].Pop());
        }

        return answer.ToString();
    }

    string IAdventDay<string>.Part2()
    {
        Stack<char>[] state = Enumerable.Range(1, DIM).Select(static _ => new Stack<char>()).ToArray();

        ReadOnlySpan<string> buffer = data.ToArray();

        void LoadState(ref Span<char> stateBuffer)
        {
            for (int i = 0; i < DIM; i++)
            {
                if (stateBuffer[i] is ' ') continue;
                state[i].Push(stateBuffer[i]);
            }
        }
        BuildContainerState(buffer[..DIM], LoadState);

        Span<char> moveBuffer = stackalloc char[DIM * DIM];
        foreach (var instruction in buffer[(DIM + 1)..])
        {
            if (string.IsNullOrWhiteSpace(instruction)) continue;

            var (count, from, to) = ExtractInstructionSet(instruction);
            
            for (int i = 0; i < count; i++) moveBuffer[i] = state[from - 1].Pop();
            for (int i = count; i > 0; i--) state[to - 1].Push(moveBuffer[i-1]);
        }

        StringBuilder answer = new(DIM);
        for (int i = 0; i < DIM; i++)
        {
            answer.Append(state[i].Pop());
        }

        return answer.ToString();
    }

    private static (int,int,int) ExtractInstructionSet(string line)
    {
        ReadOnlySpan<string> parts = line.Split();
        return (
            int.Parse(parts[1]),
            int.Parse(parts[3]),
            int.Parse(parts[5])
        );
    }
    private delegate void LoadStateDelegate(ref Span<char> state);
    private static void BuildContainerState(ReadOnlySpan<string> lines, LoadStateDelegate act)
    {
        int i = DIM - 1;
        Span<char> containerBuffer = stackalloc char[DIM];
        for (ref string head = ref MemoryMarshal.GetReference(lines), cur = ref Unsafe.Add(ref head, i);
            i >=0;
            cur = ref Unsafe.Add(ref head, --i))
        {
            ExtractContainers(ref containerBuffer, ref cur);
            act(ref containerBuffer);
        }
    }
    private static void ExtractContainers(ref Span<char> buffer, ref string level)
    {
        for (int i = 1, j = 0; i < level.Length; i += 4, j++) buffer[j] = level[i];
    }
}
