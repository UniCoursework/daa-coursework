using System;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Core.Abstractions;

public interface IPerformanceBenchmarkEngine
{
    PerformanceMetrics MeasurePerformance(
        ISearchAlgorithm algorithm, 
        ReadOnlySpan<int> dataset, 
        int iterations);
}
