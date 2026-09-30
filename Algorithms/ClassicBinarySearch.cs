using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SearchBenchmark.Core.Abstractions;

namespace SearchBenchmark.Algorithms;
public sealed class ClassicBinarySearch : ISearchAlgorithm
{
    private readonly int[] _array;

    public string Name => "Classic Binary Search";
    public string TheoreticalTimeComplexity => "O(log₂ N)";
    public string TheoreticalSpaceComplexity => "O(1) Auxiliary";

    public ClassicBinarySearch(int[] sortedArray)
    {
        _array = sortedArray ?? throw new ArgumentNullException(nameof(sortedArray));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int Search(int target)
    {
        ReadOnlySpan<int> span = _array;
        int left = 0;
        int right = span.Length - 1;

        while (left <= right)
        {
            int mid = left + ((right - left) >> 1);
            int val = Unsafe.Add(ref MemoryMarshal.GetReference(span), mid);

            if (val == target) return val;
            if (val < target) left = mid + 1;
            else right = mid - 1;
        }

        return -1;
    }

    public long GetAuxiliaryMemoryBytes() => 0;
    public void Dispose() { }
}