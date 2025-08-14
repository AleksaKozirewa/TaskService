using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            _logger.LogInformation("Фоновый сервис проверки просроченных задач запущен.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                        _logger.LogInformation("Проверка наличия просроченных задач...");
                        await taskRepository.MarkOverdueTasksAsync();
                        _logger.LogInformation("Проверка просроченных задач завершена успешно.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при проверке просроченных задач.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Фоновый сервис проверки просроченных задач остановлен.");
        }
    }
}
