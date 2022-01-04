using Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SerilogProvider
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(bool.Parse(args[1]), args[2]).Build();
            var runner = host.Services.GetRequiredService<DoSomething>();
            runner.Execute(int.Parse(args[0]));
        }

        public static IHostBuilder CreateHostBuilder(bool isAsync, string path)
        {
            var host = Host.CreateDefaultBuilder()
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddTransient<DoSomething>();
                  services.AddTransient<DoSomethingElse>();
              });

            if (isAsync)
                host.UseSerilogForBenchmarkAsync(Path.Combine(path, "SerilogAsync.log"));
            else
                host.UseSerilogForBenchmark(Path.Combine(path, "Serilog.log"));

            return host;
        }

        public static IHostBuilder UseSerilogForBenchmark(this IHostBuilder host, string fileName)
        {

            host.UseSerilog((context, logger) =>
            {
                logger.Enrich.FromLogContext();
                logger.WriteTo.File(fileName,
                    outputTemplate: GetOutputTemplate()
                    );
            });

            return host;
        }

        public static IHostBuilder UseSerilogForBenchmarkAsync(this IHostBuilder host, string fileName)
        {

            host.UseSerilog((context, logger) =>
            {
                logger.Enrich.FromLogContext();
                logger.WriteTo.Async(a => a.File(fileName,
                    outputTemplate: GetOutputTemplate()
                    ));
            });

            return host;
        }

        private static string GetOutputTemplate()
        {
            return "{Timestamp:HH:mm:ss} |{Level:u3} |{Scope} |{Message} {NewLine}{Exception}";
        }
    }
}