using SearchBenchmark.Core.Abstractions;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Services.Verification;

public sealed class VerificationEngine : IVerificationEngine
{
    public VerificationResult VerifyEquivalence(
        ISearchAlgorithm baseline, 
        ISearchAlgorithm candidate, 
        ReadOnlySpan<int> dataset, 
        int queryCount)
    {
        Random rand = new(1337);
        int matches = 0;
        int mismatches = 0;
        int? firstMismatchTarget = null;

        for (int i = 0; i < queryCount; i++)
        {
            int target = (i % 2 == 0)
                ? dataset[rand.Next(0, dataset.Length)]
                : rand.Next(dataset[0] - 100, dataset[^1] + 100);

            int baselineResult = baseline.Search(target);
            int candidateResult = candidate.Search(target);

            if (baselineResult == candidateResult)
            {
                matches++;
            }
            else
            {
                mismatches++;
                firstMismatchTarget ??= target;
            }
        }

        return new VerificationResult(
            IsEquivalent: mismatches == 0,
            TotalQueries: queryCount,
            SuccessfulMatches: matches,
            Mismatches: mismatches,
            FirstMismatchTarget: firstMismatchTarget
        );
    }
}