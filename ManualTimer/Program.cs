using System.Diagnostics;

const string NB_EXECUTION = "100000";
const string True = "true";
const string False = "false";
const string PATH = @"C:\Temps\NLogSerilogBenchmark";


Console.WriteLine($"Manual test with loop of {NB_EXECUTION}.");

Directory.Delete(PATH, true);


var timer = new Stopwatch();
timer.Start();
NlogProvider.Program.Main(new string[] { NB_EXECUTION, False, Path.Combine(PATH, "Nlog.log") });
timer.Stop();
Console.WriteLine($"Nlog default : {timer.ElapsedMilliseconds} ms");
timer.Reset();

timer.Start();
NlogProvider.Program.Main(new string[] { NB_EXECUTION, True, Path.Combine(PATH, "NlogAsync.log") });
timer.Stop();
Console.WriteLine($"Nlog Async : {timer.ElapsedMilliseconds} ms");
timer.Reset();

timer.Start();
NlogProvider_5_0_Beta.Program.Main(new string[] { NB_EXECUTION, False, Path.Combine(PATH, "NlogBeta.log") });
timer.Stop();
Console.WriteLine($"Nlog_5.0 default : {timer.ElapsedMilliseconds} ms");
timer.Reset();

timer.Start();
NlogProvider_5_0_Beta.Program.Main(new string[] { NB_EXECUTION, True, Path.Combine(PATH, "NlogBetaAsync.log") });
timer.Stop();
Console.WriteLine($"Nlog_5.0 Async : {timer.ElapsedMilliseconds} ms");
timer.Reset();

timer.Start();
SerilogProvider.Program.Main(new string[] { NB_EXECUTION, False, Path.Combine(PATH, "Serilog.log") });
timer.Stop();
Console.WriteLine($"Serilog default : {timer.ElapsedMilliseconds} ms");
timer.Reset();

timer.Start();
SerilogProvider.Program.Main(new string[] { NB_EXECUTION, True, Path.Combine(PATH, "SerilogAsync.log") });
timer.Stop();
Console.WriteLine($"Serilog Async : {timer.ElapsedMilliseconds} ms");
timer.Reset();