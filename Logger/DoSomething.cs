using Microsoft.Extensions.Logging;

namespace Logger
{
    public class DoSomething
    {
        private readonly DoSomethingElse _doSometingElse;
        private readonly ILogger<DoSomething> _logger;

        public DoSomething(DoSomethingElse doSometingElse, ILogger<DoSomething> logger)
        {
            _doSometingElse = doSometingElse;
            _logger = logger;
        }

        public void Execute(int nbExecution)
        {
            _logger.LogInformation("Begin Execute");

            try
            {
                _doSometingElse.Execute(nbExecution);
                _doSometingElse.LogException();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was thrown");
            }

            _logger.LogInformation("End Execute");

        }

    }
}
