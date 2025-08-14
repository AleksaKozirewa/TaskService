using Microsoft.EntityFrameworkCore;
using TaskService.Core.Data;
using TaskService.Core.Interfaces;
using TaskService.Core.Repositories;
using TaskService.Core.Services;

namespace TaskService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddMemoryCache();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TasksService>();
            builder.Services.AddHostedService<OverdueTaskBackgroundService>();

            var app = builder.Build();

            if (builder.Configuration.GetValue<bool>("EF_RUN_MIGRATIONS_ON_STARTUP"))
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var dbContext = services.GetRequiredService<AppDbContext>();
                        await dbContext.Database.MigrateAsync();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "При миграции базы данных возникла ошибка.");
                    }
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}