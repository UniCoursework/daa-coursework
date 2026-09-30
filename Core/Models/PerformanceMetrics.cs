namespace SearchBenchmark.Core.Models;

public readonly record struct PerformanceMetrics(
    string AlgorithmName,
    double TotalTimeMs,
    double NanosecondsPerOp,
    double OperationsPerSecond
);