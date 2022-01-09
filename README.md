# Nlog vs Serilog with BenchmarkDotNet
This project benchmark Nlog and Serilog in a simple batch project in .Net6.

My goal is to find the best option for my program and also to provide you with an example for your own performance testing
with Serilog/Nlog or others.

I use [BenchmarkDotNet](https://benchmarkdotnet.org/articles/overview.html).

<br>

## The tests
3 series of tests is used, sync and async. More later.
    - Serilog
    -Nlog
    -Nlog version 5.0 cr1

For a total of 6 tests.

| Method       | 
| :----------- | 
| Serilog_DefaultExecution | 
| Nlog_DefaultExecution    | 
| Nlog_Async               | 
| Serilog_Async            | 
| Nlog_5_0_Beta_DefaultExecution |
| Nlog_5_0_Beta_Async |

<br>

#### Benchmark configuration

The Benchmark.csproj is the project where the benchmark tests is configure and run.

In the class BenchmarkCases.cs you can find the configuration of all tests.
Each run all log file are deleted with the Cleanup() method and all test is run by the attribute [Benchmark()].


```csharp
 [GlobalCleanup]
    public void Cleanup()
    {
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
```

<br>

#### How to run the benchmark

You need to run the Benchmark.csproj in **Release mode** and to start, go to **debug/Start without debugging** or **Ctrl+F5**.
A Console will start de benchmark. it will take a few minutes.

![File](file.png)


<br>
<br>


## Projects

#### Logger
Logger Is the batch exemple. It ILogger to write information log with [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line)

```csharp
    using (_logger.BeginScope("DoSomethingElse"))
    {
        for (int i = 0; i < nbExecution; i++)
        {
            using (_logger.BeginScope("loop : {i}", i))
            {
                _logger.LogInformation("Begin loop : {i}", i);

                Thread.Sleep(100);

                _logger.LogInformation("End loop : {i}", i);
            }
        }

        _logger.LogInformation("End DoSomethingElse");
    }
```


#### NlogProvider
This project uses [NLog](https://nlog-project.org) provider to log into a file the log of Logger project.

```csharp
 .ConfigureLogging((context, logging) =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
        SetupLogger(Path.Combine(path, "Nlog.log"));
    })
    .UseNLog();
```


#### SerilogProvider
This project uses [Serilog](https://serilog.net/) provider to log into a file the log of Logger project.

```csharp
host.UseSerilog((context, logger) =>
{
    logger.Enrich.FromLogContext();
    logger.WriteTo.File(fileName,
        outputTemplate: GetOutputTemplate()
        );
});
```

<br>
<br>

## The result


#### My pc setup

| Infos                          |                                                                                            |
|:-------------------------------|:-------------------------------------------------------------------------------------------|
| Nom du système d’exploitation: | Microsoft Windows 11 Professionnel                                                         |
| Processeur:                    | Intel(R) Core(TM) i7-8700K CPU @ 3.70GHz, 3696 MHz, 6 cœur(s), 12 processeur(s) logique(s) |
| Mémoire physique totale:       | 32 701 Mo                                                                                  |
| Description de la carte:       | NVIDIA GeForce GTX 1080 Ti                                                                 |

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```


<br>

### Result with thread.sleep(100)

Result Folder : NlogSerilogDotnetBenchmark\Benchmark\bin\Release\net6.0\BenchmarkDotNet.Artifacts\results


|                         Method |    Mean |    Error |   StdDev |     Min |     Max | Ratio | Rank | Allocated |
|------------------------------- |--------:|---------:|---------:|--------:|--------:|------:|-----:|----------:|
|       Serilog_DefaultExecution | 5.437 s | 0.0081 s | 0.0076 s | 5.423 s | 5.448 s |  1.00 |    1 |    341 KB |
|            Nlog_5_0_Beta_Async | 5.438 s | 0.0078 s | 0.0073 s | 5.425 s | 5.448 s |  1.00 |    1 |    354 KB |
|                  Serilog_Async | 5.438 s | 0.0078 s | 0.0073 s | 5.428 s | 5.451 s |  1.00 |    1 |    344 KB |
|          Nlog_DefaultExecution | 5.439 s | 0.0081 s | 0.0068 s | 5.428 s | 5.450 s |  1.00 |    1 |    370 KB |
|                     Nlog_Async | 5.439 s | 0.0097 s | 0.0090 s | 5.426 s | 5.457 s |  1.00 |    1 |    351 KB |
| Nlog_5_0_Beta_DefaultExecution | 5.444 s | 0.0094 s | 0.0088 s | 5.434 s | 5.458 s |  1.00 |    1 |    345 KB |

<br>


### Result without thread.sleep(100)
Because the execution time was long, I reduced the loop to 10 instead of 50.
The results is very different !


|                         Method |       Mean |       Error |      StdDev |      Median |        Min |         Max | Ratio | RatioSD | Rank |   Gen 0 |   Gen 1 | Allocated |
|------------------------------- |-----------:|------------:|------------:|------------:|-----------:|------------:|------:|--------:|-----:|--------:|--------:|----------:|
|       Serilog_DefaultExecution |   563.5 μs |    45.52 μs |   126.90 μs |    495.6 μs |   483.0 μs |    970.2 μs |  0.36 |    0.10 |    1 | 25.3906 |  8.7891 |    154 KB |
|            Nlog_5_0_Beta_Async | 1,284.5 μs |    24.97 μs |    23.36 μs |  1,286.9 μs | 1,239.7 μs |  1,320.5 μs |  0.99 |    0.05 |    2 | 41.0156 | 35.1563 |    250 KB |
|          Nlog_DefaultExecution | 1,676.1 μs |   102.06 μs |   300.94 μs |  1,626.5 μs | 1,209.7 μs |  2,310.5 μs |  1.00 |    0.00 |    3 | 39.0625 |  9.7656 |    243 KB |
| Nlog_5_0_Beta_DefaultExecution | 1,687.3 μs |   108.87 μs |   321.01 μs |  1,649.7 μs | 1,195.6 μs |  2,375.9 μs |  1.01 |    0.05 |    3 | 39.0625 | 29.2969 |    244 KB |
|                     Nlog_Async | 1,711.7 μs |   106.24 μs |   313.25 μs |  1,690.5 μs | 1,261.3 μs |  2,486.2 μs |  1.02 |    0.04 |    3 | 41.0156 |  9.7656 |    250 KB |
|                  Serilog_Async | 9,761.5 μs | 1,262.12 μs | 3,721.40 μs | 10,280.2 μs | 3,661.7 μs | 17,371.4 μs |  5.62 |    1.33 |    4 | 25.3906 |  9.7656 |    157 KB |



<br>

### Manual test (Elapsed time)

With manual testing that speaks to me a bit more, don't you?

| Provider         | 100    | 500    | 1000   | 10 000 | 100 000 | 
| ---------------- | -----: | -----: | -----: | ------ | ------: | 
| Nlog default     | 471 ms | 717 ms | 648 ms | 625 ms | 1878 ms | 
| Nlog Async       | 18 ms  | 38 ms  | 43 ms  | 222 ms | 1012 ms | 
| Nlog_5.0 default | 8 ms   | 24 ms  | 36 ms  | 228 ms | 978 ms  | 
| Nlog_5.0 Async   | 4 ms   | 17 ms  | 21 ms  | 249 ms | 984 ms  | 
| Serilog default  | 145 ms | 96 ms  | 125 ms | 464 ms | 2126 ms | 
| Serilog Async    | 14 ms  | 22 ms  | 51 ms  | 160 ms | 991 ms  | 


<br>

## Conclusion

In manual tests, Nlog 5.0 and Serilog Async look great on each try. Nlog Async is not too bad to.

With benchmark the rank is different, Serilog with default setting seem the best but in elapsed time (manual test) he is not.

I hope this example will help you in your process of finding the one that is best for your project.

<br><br>

Thanks

jakdaniel

<br>
<br>