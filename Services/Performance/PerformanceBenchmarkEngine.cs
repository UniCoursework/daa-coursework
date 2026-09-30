using System.Diagnostics;
using SearchBenchmark.Core.Abstractions;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Services.Performance;

public sealed class PerformanceBenchmarkEngine : IPerformanceBenchmarkEngine
{
    public PerformanceMetrics MeasurePerformance(
        ISearchAlgorithm algorithm, 
        ReadOnlySpan<int> dataset, 
        int iterations)
    {
        int[] targets = GC.AllocateUninitializedArray<int>(iterations);
        Random rand = new(2026);
        for (int i = 0; i < iterations; i++)
        {
            targets[i] = dataset[rand.Next(0, dataset.Length)];
        }

        for (int i = 0; i < 20_000; i++)
        {
            algorithm.Search(targets[i % iterations]);
        }

        GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
        GC.WaitForPendingFinalizers();

        long startTimestamp = Stopwatch.GetTimestamp();

        for (int i = 0; i < iterations; i++)
        {
            algorithm.Search(targets[i]);
        }

        long endTimestamp = Stopwatch.GetTimestamp();

        TimeSpan elapsed = Stopwatch.GetElapsedTime(startTimestamp, endTimestamp);
        double totalMs = elapsed.TotalMilliseconds;
        double nsPerOp = (totalMs * 1_000_000.0) / iterations;
        double opsPerSec = iterations / elapsed.TotalSeconds;

        return new PerformanceMetrics(
            AlgorithmName: algorithm.Name,
            TotalTimeMs: totalMs,
            NanosecondsPerOp: nsPerOp,
            OperationsPerSecond: opsPerSec
        );
    }
}