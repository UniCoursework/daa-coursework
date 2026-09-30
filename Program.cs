using SearchBenchmark.Algorithms;
using SearchBenchmark.Core.Abstractions;
using SearchBenchmark.Pipeline;
using SearchBenchmark.Reporting;
using SearchBenchmark.Services.Data;
using SearchBenchmark.Services.Performance;
using SearchBenchmark.Services.Profiling;
using SearchBenchmark.Services.Verification;

namespace SearchBenchmark;

public static class Program
{
    public static void Main()
    {
        Console.Title = "Search Algorithm Benchmark Engine By Tina Khorsandi (.NET 8+)";

        const int DatasetSize = 10_000_000;
        const int VerificationQueries = 100_000;
        const int BenchmarkIterations = 2_000_000;

        IDataGenerator dataGenerator = new SortedDataGenerator();
        IVerificationEngine verificationEngine = new VerificationEngine();
        IPerformanceBenchmarkEngine benchmarkEngine = new PerformanceBenchmarkEngine();
        IMemoryProfiler memoryProfiler = new MemoryProfiler();
        IReportPresenter reportPresenter = new ConsoleReportPresenter();

        SearchBenchmarkPipeline pipeline = new(
            dataGenerator,
            verificationEngine,
            benchmarkEngine,
            memoryProfiler,
            reportPresenter
        );

        pipeline.Run(
            DatasetSize,
            VerificationQueries,
            BenchmarkIterations,
            dataset => new ISearchAlgorithm[]
            {
                new ClassicBinarySearch(dataset),
                new SimdSTreeSearch(dataset)
            }
        );
        Console.ReadKey();
    }
}