using System;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Core.Abstractions;
public interface IMemoryProfiler
{
    MemoryMetrics Profile(ISearchAlgorithm algorithm);
}
