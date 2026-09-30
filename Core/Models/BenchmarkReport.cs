namespace SearchBenchmark.Core.Models;

public sealed record BenchmarkReport(
    int DatasetSize,
    VerificationResult Verification,
    IReadOnlyList<AlgorithmAuditReport> AlgorithmReports
);