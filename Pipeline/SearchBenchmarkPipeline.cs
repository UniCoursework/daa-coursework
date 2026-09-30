using SearchBenchmark.Core.Abstractions;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Pipeline;

public sealed class SearchBenchmarkPipeline
{
    private readonly IDataGenerator _dataGenerator;
    private readonly IVerificationEngine _verificationEngine;
    private readonly IPerformanceBenchmarkEngine _benchmarkEngine;
    private readonly IMemoryProfiler _memoryProfiler;
    private readonly IReportPresenter _reportPresenter;

    public SearchBenchmarkPipeline(
        IDataGenerator dataGenerator,
        IVerificationEngine verificationEngine,
        IPerformanceBenchmarkEngine benchmarkEngine,
        IMemoryProfiler memoryProfiler,
        IReportPresenter reportPresenter)
    {
        _dataGenerator = dataGenerator;
        _verificationEngine = verificationEngine;
        _benchmarkEngine = benchmarkEngine;
        _memoryProfiler = memoryProfiler;
        _reportPresenter = reportPresenter;
    }

    public void Run(int datasetSize, int verificationQueries, int benchmarkIterations, Func<int[], ISearchAlgorithm[]> algorithmFactory)
    {
        Console.WriteLine("[Pipeline] Generating Dataset...");
        int[] dataset = _dataGenerator.GenerateSortedDataset(datasetSize);

        ISearchAlgorithm[] algorithms = algorithmFactory(dataset);
        List<AlgorithmAuditReport> auditReports = new();

        try
        {
            Console.WriteLine("[Pipeline] Verifying Functional Equivalence...");
            VerificationResult verificationResult = _verificationEngine.VerifyEquivalence(
                algorithms[0], algorithms[1], dataset, verificationQueries);

            foreach (var algo in algorithms)
            {
                Console.WriteLine($"[Pipeline] Benchmarking {algo.Name}...");
                MemoryMetrics memMetrics = _memoryProfiler.Profile(algo);
                PerformanceMetrics perfMetrics = _benchmarkEngine.MeasurePerformance(algo, dataset, benchmarkIterations);

                auditReports.Add(new AlgorithmAuditReport(algo.Name, perfMetrics, memMetrics));
            }

            BenchmarkReport report = new(datasetSize, verificationResult, auditReports);
            _reportPresenter.RenderReport(report);
        }
        finally
        {
            foreach (var algo in algorithms)
            {
                algo.Dispose();
            }
        }
    }
}