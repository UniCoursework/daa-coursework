using SearchBenchmark.Core.Abstractions;

namespace SearchBenchmark.Services.Data;

public sealed class SortedDataGenerator : IDataGenerator
{
    public int[] GenerateSortedDataset(int size, int seed = 42)
    {
        int[] dataset = GC.AllocateUninitializedArray<int>(size);
        Random rand = new(seed);
        int current = 10;

        for (int i = 0; i < size; i++)
        {
            current += rand.Next(1, 5);
            dataset[i] = current;
        }

        return dataset;
    }
}