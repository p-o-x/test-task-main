using task.Services;

namespace task;

public class Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    
    private readonly TimeSpan _scheduledTime = new TimeSpan(2, 0, 0);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Запущен сервис импорта");

        using (IServiceScope scope = serviceScopeFactory.CreateScope())
        {
            var importService = scope.ServiceProvider.GetRequiredService<ImportService>();
            await importService.ImportDataToDb(stoppingToken);
        }

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = GetMskNow();
                var nextRun = now.Date.Add(_scheduledTime);

                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }

                var delay = nextRun - now;

                logger.LogInformation($"Следующее выполнение в: {nextRun}. Через {delay}");
                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    using (IServiceScope scope = serviceScopeFactory.CreateScope())
                    {
                        var importService = scope.ServiceProvider.GetRequiredService<ImportService>();
                        await importService.ImportDataToDb(stoppingToken);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка импорта");
        }
    }

    private DateTime GetMskNow()
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
    }

}
