using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;
using WT.Ecommerce.Infrastructure.Options;

namespace WT.Ecommerce.Infrastructure.Logging
{
    public class DefaultLoggerEnricher : ILogEventEnricher
    {
        private readonly ConcurrentDictionary<string, LogEventProperty> _cachedProperties = new ConcurrentDictionary<string, LogEventProperty>();
        private readonly DefaultLoggerEnricherOptions _options;
        private readonly object _lock = new object();

        private const string ApplicationPropertyName = "ApplicationName";
        private const string ApplicationVersionPropertyName = "ApplicationVersion";
        private const string ApplicationInformationalVersionPropertyName = "ApplicationInformationalVersion";

        public DefaultLoggerEnricher(DefaultLoggerEnricherOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Enrich the log event.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(ApplicationPropertyName, _options.Application, propertyFactory));
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(ApplicationVersionPropertyName, _options.ApplicationVersion, propertyFactory));
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(ApplicationInformationalVersionPropertyName, _options.ApplicationInformationalVersion, propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(string propertyName, string value, ILogEventPropertyFactory propertyFactory)
        {
            if (_cachedProperties.TryGetValue(propertyName, out LogEventProperty property))
            {
                return property;
            }

            lock (_lock)
            {
                if (_cachedProperties.TryGetValue(propertyName, out property))
                {
                    return property;
                }

                property = propertyFactory.CreateProperty(propertyName, value);
                _cachedProperties.TryAdd(propertyName, property);
            }

            return property;
        }
    }
}
