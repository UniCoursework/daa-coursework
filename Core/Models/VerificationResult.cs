namespace SearchBenchmark.Core.Models;

public readonly record struct VerificationResult(
    bool IsEquivalent,
    int TotalQueries,
    int SuccessfulMatches,
    int Mismatches,
    int? FirstMismatchTarget
);