using BenchmarkDotNet.Attributes;
using NLog;

namespace Benchmark.Benchmarks;


[HtmlExporter]
[MinColumn, MaxColumn]
[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class BenchmarkCases
{

    private const string NB_EXECUTION = "50";
    private const string True = "true";
    private const string False = "false";
    private const string PATH = @"C:\Temps\NLogSerilogBenchmark";

    [GlobalCleanup]
    public void Cleanup()
    {
        //LogManager.Shutdown();
        //NLog.LogManager.Configuration = null;

        Directory.Delete(PATH, true);
    }

    [Benchmark(Baseline = true)]
    public void Nlog_DefaultExecution()
    {
        NlogProvider.Program.Main(new string[] { NB_EXECUTION, False, PATH });
    }

    [Benchmark()]
    public void Nlog_Async()
    {
        NlogProvider.Program.Main(new string[] { NB_EXECUTION, True, PATH });
    }

    [Benchmark()]
    public void Serilog_DefaultExecution()
    {
        SerilogProvider.Program.Main(new string[] { NB_EXECUTION, False, PATH });
    }

    [Benchmark()]
    public void Serilog_Async()
    {
        SerilogProvider.Program.Main(new string[] { NB_EXECUTION, True, PATH });
    }
}

