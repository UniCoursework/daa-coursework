using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using SearchBenchmark.Core.Abstractions;

namespace SearchBenchmark.Algorithms;

public unsafe sealed class SimdSTreeSearch : ISearchAlgorithm
{
    private const int K = 8;
    private readonly int* _data;
    private readonly int _nblocks;
    private readonly long _memorySize;
    private bool _disposed;

    public string Name => "SIMD AVX2 S-Tree Search";
    public string TheoreticalTimeComplexity => "O(log₉ N)";
    public string TheoreticalSpaceComplexity => "O(N) Pre-processed";

    public SimdSTreeSearch(ReadOnlySpan<int> sortedArray)
    {
        if (!Avx2.IsSupported)
            throw new NotSupportedException("پردازنده سیستم از دستورات AVX2 پشتیبانی نمی‌کند.");

        int n = sortedArray.Length;
        _nblocks = (n + K - 1) / K;
        _memorySize = (long)_nblocks * K * sizeof(int);
        _data = (int*)NativeMemory.AlignedAlloc((nuint)_memorySize, 64);

        int t = 0;
        BuildSTree(sortedArray, ref t, 0);
    }

    private void BuildSTree(ReadOnlySpan<int> sorted, ref int t, int k)
    {
        if (k >= _nblocks) return;

        for (int i = 0; i < K; i++)
        {
            BuildSTree(sorted, ref t, k * (K + 1) + i + 1);
            _data[k * K + i] = (t < sorted.Length) ? sorted[t++] : int.MaxValue;
        }

        BuildSTree(sorted, ref t, k * (K + 1) + K + 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int Search(int target)
    {
        Vector256<int> vt = Vector256.Create(target);
        int k = 0;
        int ans = int.MaxValue;

        while (k < _nblocks)
        {
            int* blockPtr = _data + k * K;

            if (Sse.IsSupported)
            {
                Sse.Prefetch0(_data + (k * (K + 1) + 1) * K);
            }

            Vector256<int> vb = Avx2.LoadAlignedVector256(blockPtr);
            Vector256<int> cmp = Avx2.CompareGreaterThan(vt, vb);
            uint mask = (uint)Avx.MoveMask(cmp.AsSingle());
            int c = BitOperations.PopCount(mask);

            if (c < K)
            {
                ans = blockPtr[c];
            }

            k = k * (K + 1) + c + 1;
        }

        return (ans == target) ? ans : -1;
    }

    public long GetAuxiliaryMemoryBytes() => _memorySize;

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_data != null)
            {
                NativeMemory.AlignedFree(_data);
            }
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    ~SimdSTreeSearch() => Dispose();
}