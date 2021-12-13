using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using WT.Libraries.Logging.seq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using WT.Libraries.Logging.Enums;

namespace WT.Libraries.Logging
{
    public static class Extensions
    {

        /// <summary>
        /// Returns the options from the configuration
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);
            return model;
        }

        /// <summary>
        /// Determines the deployment mode from the configuration properties
        /// </summary>
        /// <param name="configuration">The set of key/value application configuration properties.</param>
        /// <returns></returns>
        public static DeploymentMode GetDeploymentMode(this IConfiguration configuration)
        {
            if (Enum.TryParse(typeof(DeploymentMode), configuration.GetValue<string>(ConfigurationKeysConstants.DeploymentMode), out var deploymentMode))
            {
                return (DeploymentMode)deploymentMode;
            }

            return DeploymentMode.OnPremises;
        }

        /// <summary>
        /// Configure logging with Serilog.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
	    public static IHostBuilder UseLogging(this IHostBuilder builder)
        {
            builder.UseSerilog((hostingContext, services, loggerConfiguration) =>
            {
                var productInformation = new ProductInformation();
                var options = hostingContext.Configuration.GetOptions<LoggerOptions>("logger");

                var defaultLoggerEnricherOptions = new DefaultLoggerEnricherOptions
                {
                    Application = productInformation.Name,
                    ApplicationVersion = productInformation.Version,
                    ApplicationInformationalVersion = productInformation.InformationalVersion,
                    Division = options.Division,
                    Environment = options.Environment
                };

                var minimumLevel = hostingContext.HostingEnvironment.IsDevelopment()
                    ? LogEventLevel.Debug
                    : LogEventLevel.Information;

                loggerConfiguration
                    .MinimumLevel.ControlledBy(new LoggingLevelSwitch(minimumLevel))
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);

                loggerConfiguration
                    .Enrich.WithMachineName()
                    .Enrich.WithExceptionDetails()
                    .Enrich.FromLogContext()
                    .Enrich.With(new DefaultLoggerEnricher(defaultLoggerEnricherOptions));

                // write to console if in development mode or running in container
                if (hostingContext.HostingEnvironment.IsDevelopment() ||
                    hostingContext.Configuration.GetDeploymentMode() == DeploymentMode.Containers)
                {
                    loggerConfiguration.WriteTo.Console();
                }
                AddSeq(loggerConfiguration, options.Seq);
            });
            return builder;
        }

      

        private static void AddSeq(LoggerConfiguration loggerConfiguration, SeqLoggerOptions loggerOptions)
        {
            if (!loggerOptions.Enabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(loggerOptions.Hostname))
            {
                StartupLogger.Logger.Error("Logging was configured with Seq enabled but one of the following values are missing: Hostname");
                return;
            }

            loggerConfiguration.WriteTo.Seq(loggerOptions.Hostname, apiKey: loggerOptions.ApiKey);
            StartupLogger.Logger.Information("Logs will be sent to Seq");
        }
    }
}
