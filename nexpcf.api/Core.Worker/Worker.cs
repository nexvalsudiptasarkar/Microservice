using Core.Worker.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            AttendanceSummaryJob obj = new AttendanceSummaryJob(_logger, _configuration);

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                TimeSpan now = DateTime.Now.TimeOfDay;
                TimeSpan start = new TimeSpan(10, 0, 0);
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (start == now) {
                        obj.generateAttendanceSummaryAsync();
                    }
                    
                    await Task.Delay(86400, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Worker failed running at: {time} {error}", DateTimeOffset.Now, e);
            }
            _logger.LogInformation("Worker ending running at: {time}", DateTimeOffset.Now);
        }
    }
}
