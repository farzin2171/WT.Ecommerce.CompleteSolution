using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WT.Ecommerce.Infrastructure.StartupTasks
{
    public class InformationStartupTask : IStartupTask
    {
        private readonly ILogger<InformationStartupTask> _logger;


        public InformationStartupTask(ILogger<InformationStartupTask> logger)
        {
            _logger = logger;
        }

        public void Execute()
        {
            var assembly = Assembly.GetEntryAssembly();
            var version = assembly?.GetName().Version.ToString();
            _logger.LogInformation("WT.Ecommerce {Version} successfully started", version);

        }
    }
}
