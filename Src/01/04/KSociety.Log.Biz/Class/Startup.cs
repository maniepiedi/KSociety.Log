﻿using Autofac;
using KSociety.Log.Biz.Interface;
using Microsoft.Extensions.Logging;

namespace KSociety.Log.Biz.Class
{
    public class Startup : IStartable
    {
        private readonly ILogger<Startup> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IBiz _biz;

        public Startup(
            ILoggerFactory loggerFactory,
            IBiz biz
        )
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Startup>();
            _biz = biz;
            _logger.LogInformation("C20052.Biz.Log startup! ");
        }

        public void Start()
        {
            _logger.LogTrace("C20052.Biz.Log staring...");
            _biz.LoadEventBus();

        }
    }
}
