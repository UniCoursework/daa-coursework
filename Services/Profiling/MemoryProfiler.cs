using SearchBenchmark.Core.Abstractions;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Services.Profiling;

public sealed class MemoryProfiler : IMemoryProfiler
{
    public MemoryMetrics Profile(ISearchAlgorithm algorithm)
    {
        int gen0Before = GC.CollectionCount(0);
        int gen1Before = GC.CollectionCount(1);
        int gen2Before = GC.CollectionCount(2);

        long auxBytes = algorithm.GetAuxiliaryMemoryBytes();

        return new MemoryMetrics(
            AlgorithmName: algorithm.Name,
            TheoreticalSpaceComplexity: algorithm.TheoreticalSpaceComplexity,
            AuxiliaryAllocatedBytes: auxBytes,
            Gen0Collections: GC.CollectionCount(0) - gen0Before,
            Gen1Collections: GC.CollectionCount(1) - gen1Before,
            Gen2Collections: GC.CollectionCount(2) - gen2Before
        );
    }
}