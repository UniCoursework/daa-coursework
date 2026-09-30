namespace SearchBenchmark.Core.Models;

public readonly record struct MemoryMetrics(
    string AlgorithmName,
    string TheoreticalSpaceComplexity,
    long AuxiliaryAllocatedBytes,
    int Gen0Collections,
    int Gen1Collections,
    int Gen2Collections
);