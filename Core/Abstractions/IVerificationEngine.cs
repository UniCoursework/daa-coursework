using System;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Core.Abstractions;
public interface IVerificationEngine
{
    VerificationResult VerifyEquivalence(
        ISearchAlgorithm baseline, 
        ISearchAlgorithm candidate, 
        ReadOnlySpan<int> dataset, 
        int queryCount);
}
