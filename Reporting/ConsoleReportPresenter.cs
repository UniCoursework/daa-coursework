using SearchBenchmark.Core.Abstractions;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Reporting;

public sealed class ConsoleReportPresenter : IReportPresenter
{
    public void RenderReport(BenchmarkReport report)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================================================================");
        Console.WriteLine($"                    SEARCH ALGORITHMS BENCHMARK REPORT BY TINA KHORSANDI TALAB");
        Console.WriteLine($"                    Dataset Size: {report.DatasetSize:N0} Sorted Elements (32-bit int)");
        Console.WriteLine("==================================================================================================");
        Console.ResetColor();

        Console.WriteLine("\n[1] FUNCTIONAL EQUIVALENCE AUDIT");
        Console.Write("     Status: ");
        if (report.Verification.IsEquivalent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"PASSED (100% Match across {report.Verification.TotalQueries:N0} random queries)");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FAILED ({report.Verification.Mismatches} mismatches found!)");
        }
        Console.ResetColor();

        Console.WriteLine("\n[2] PERFORMANCE & SPACE COMPLEXITY COMPARISON MATRIX");
        Console.WriteLine("┌───────────────────────────┬───────────────────┬───────────────────┬───────────────────┬──────────────────────┐");
        Console.WriteLine("│ Algorithm                 │ Latency (ns/op)   │ Throughput (ops/s)│ Exec Time (ms)    │ Auxiliary Memory     │");
        Console.WriteLine("├───────────────────────────┼───────────────────┼───────────────────┼───────────────────┼──────────────────────┤");

        foreach (var audit in report.AlgorithmReports)
        {
            Console.WriteLine($"│ {audit.AlgorithmName,-25} │ {audit.Performance.NanosecondsPerOp,17:N2} │ {audit.Performance.OperationsPerSecond,17:N0} │ {audit.Performance.TotalTimeMs,17:N2} │ {FormatBytes(audit.Memory.AuxiliaryAllocatedBytes),20} │");
        }
        Console.WriteLine("└───────────────────────────┴───────────────────┴───────────────────┴───────────────────┴──────────────────────┘");

        if (report.AlgorithmReports.Count >= 2)
        {
            double baselineTime = report.AlgorithmReports[0].Performance.TotalTimeMs;
            double candidateTime = report.AlgorithmReports[1].Performance.TotalTimeMs;
            double speedup = baselineTime / candidateTime;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" SPEEDUP FACTOR: {report.AlgorithmReports[1].AlgorithmName} is {speedup:F2}x FASTER than {report.AlgorithmReports[0].AlgorithmName}.");
            Console.ResetColor();
        }

        Console.WriteLine("\n==================================================================================================\n");
    }

    private static string FormatBytes(long bytes)
    {
        if (bytes == 0) return "0 B (In-Place)";
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F2} KB";
        return $"{bytes / (1024.0 * 1024.0):F2} MB";
    }
}