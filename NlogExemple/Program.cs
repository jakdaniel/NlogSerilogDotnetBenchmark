using Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;


namespace NlogProvider
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(bool.Parse(args[1]), args[2]).Build();
            var runner = host.Services.GetRequiredService<DoSomething>();
            runner.Execute(int.Parse(args[0]));
        }

        public static IHostBuilder CreateHostBuilder(bool isAsync, string path) =>
           Host.CreateDefaultBuilder()
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddTransient<DoSomething>();
                 services.AddTransient<DoSomethingElse>();
             })
             .ConfigureLogging((context, logging) =>
             {
                 logging.ClearProviders();
                 logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                 if (isAsync)
                     SetupLoggerAsync(Path.Combine(path, "NlogAsync.log"));
                 else
                     SetupLogger(Path.Combine(path, "Nlog.log"));
             })
             .UseNLog();

        private static void SetupLogger(string fileName)
        {
            var fileTarget = new FileTarget(fileName)
            {
                FileName = fileName,
                Layout = GetLayout()
            };

            var config = new LoggingConfiguration();
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;
        }

        private static void SetupLoggerAsync(string fileName)
        {
            var fileTarget = new FileTarget(fileName)
            {
                FileName = fileName,
                Layout = GetLayout()
            };


            // Setting output file writing to Async Mode
            var wrapper = new NLog.Targets.Wrappers.AsyncTargetWrapper(fileTarget)
            {
                OverflowAction = NLog.Targets.Wrappers.AsyncTargetWrapperOverflowAction.Block,
                QueueLimit = 10000
            };

            // Adding "File" as one of the log targets
            var config = new LoggingConfiguration();
            config.AddTarget("file", wrapper);

            // Configuring Log from Config File          
            fileTarget.FileName = fileName;
            var rule2 = new LoggingRule("*", NLog.LogLevel.Info, fileTarget);
            config.LoggingRules.Add(rule2);

            // Saving Configurations
            LogManager.Configuration = config;
        }

        private static string GetLayout()
        {
            return "${longdate} |${level:uppercase=true} |${ndlctiming}ms |${mdlc} |${message}      ${onexception:${newline}${exception}";
        }

    }
}