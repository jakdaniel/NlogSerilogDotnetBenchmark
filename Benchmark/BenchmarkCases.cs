using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;


[HtmlExporter]
[MinColumn, MaxColumn]
[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class BenchmarkCases
{

    private const string NB_EXECUTION = "10";
    private const string True = "true";
    private const string False = "false";
    private const string PATH = @"C:\Temps\NLogSerilogBenchmark";

    [GlobalSetup]
    public void Cleanup()
    {
        Directory.Delete(PATH, true);
    }

    [Benchmark(Baseline = true)]
    public void Nlog_DefaultExecution()
    {
        NlogProvider.Program.Main(new string[] { NB_EXECUTION, False, Path.Combine(PATH, "Nlog.log") });
    }

    [Benchmark()]
    public void Nlog_Async()
    {
        NlogProvider.Program.Main(new string[] { NB_EXECUTION, True, Path.Combine(PATH, "NlogAsync.log") });
    }

    [Benchmark()]
    public void Nlog_5_0_Beta_DefaultExecution()
    {
        NlogProvider_5_0_Beta.Program.Main(new string[] { NB_EXECUTION, False, Path.Combine(PATH, "NlogBeta.log") });
    }

    [Benchmark()]
    public void Nlog_5_0_Beta_Async()
    {
        NlogProvider_5_0_Beta.Program.Main(new string[] { NB_EXECUTION, True, Path.Combine(PATH, "NlogBetaAsync.log") });
    }

    [Benchmark()]
    public void Serilog_DefaultExecution()
    {
        SerilogProvider.Program.Main(new string[] { NB_EXECUTION, False, Path.Combine(PATH, "Serilog.log") });
    }

    [Benchmark()]
    public void Serilog_Async()
    {
        SerilogProvider.Program.Main(new string[] { NB_EXECUTION, True, Path.Combine(PATH, "SerilogAsync.log") });
    }
}

