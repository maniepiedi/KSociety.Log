﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;

namespace KSociety.Log.Serilog.Sinks.SignalR.Sinks.SignalR
{
    public class SignalRSink : PeriodicBatchingSink
    {
        private readonly ITextFormatter _formatter;
        private readonly HubProxy _proxy;

        private ILoggerFactory _loggerFactory { get; }

        public SignalRSink(SignalRSinkConfiguration signalRSinkConfiguration, HubProxy proxy)
        :base(signalRSinkConfiguration.BatchPostingLimit, signalRSinkConfiguration.Period)
        {
            _formatter = signalRSinkConfiguration.TextFormatter;
            _proxy = proxy;

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning);
            });
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            foreach (var logEvent in events)
            {
                var sw = new StringWriter();
                _formatter.Format(logEvent, sw);

                await _proxy.Log(new Srv.Dto.LogEvent(sw.ToString(), logEvent.Timestamp.DateTime, 1, (int)logEvent.Level, "LoggerName")).ConfigureAwait(false);
            }
        }
    }
}
