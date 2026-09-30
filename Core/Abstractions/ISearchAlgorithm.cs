using System;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Core.Abstractions;

public interface ISearchAlgorithm : IDisposable
{
    string Name { get; }
    string TheoreticalTimeComplexity { get; }
    string TheoreticalSpaceComplexity { get; }
    int Search(int target);
    long GetAuxiliaryMemoryBytes();
}

