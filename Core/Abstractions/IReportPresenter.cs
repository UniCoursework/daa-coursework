using System;
using SearchBenchmark.Core.Models;

namespace SearchBenchmark.Core.Abstractions;

public interface IReportPresenter
{
    void RenderReport(BenchmarkReport report);
}
