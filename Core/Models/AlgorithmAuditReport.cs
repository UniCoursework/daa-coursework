namespace SearchBenchmark.Core.Models;

public sealed record AlgorithmAuditReport(
    string AlgorithmName,
    PerformanceMetrics Performance,
    MemoryMetrics Memory
);