using Microsoft.Extensions.Logging;

namespace Logger
{
    public class DoSomethingElse
    {

        private readonly ILogger<DoSomethingElse> _logger;

        public DoSomethingElse(ILogger<DoSomethingElse> logger)
        {
            _logger = logger;
        }

        public void Execute(int nbExecution)
        {

            using (_logger.BeginScope("DoSomethingElse"))
            {
                for (int i = 0; i < nbExecution; i++)
                {
                    using (_logger.BeginScope("loop : {i}", i))
                    {
                        _logger.LogInformation("Begin loop : {i}", i);

                        //Thread.Sleep(100);

                        _logger.LogInformation("End loop : {i}", i);
                    }
                }

                _logger.LogInformation("End DoSomethingElse");
            }

        }

        public void LogException()
        {
            using (_logger.BeginScope("throw an exception"))
            {
                try
                {
                    throw new Exception("throw an exception");
                }
                finally
                {
                    _logger.LogInformation("Exception is thrown");
                }

            }
        }

    }
}
