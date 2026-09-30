using System;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Core.Abstractions;
public interface IDataGenerator
{
    int[] GenerateSortedDataset(int size, int seed = 42);
}
