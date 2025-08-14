using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskService.Core.Interfaces;

namespace TaskService.Core.Services
{
    public class OverdueTaskBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<OverdueTaskBackgroundService> _logger;

        public OverdueTaskBackgroundService(
            IServiceProvider services,
            ILogger<OverdueTaskBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Overdue Task Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                        _logger.LogInformation("Checking for overdue tasks...");
                        await taskRepository.MarkOverdueTasksAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking overdue tasks.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Overdue Task Background Service is stopping.");
        }
    }
}
