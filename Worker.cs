namespace CherryBot;

public class Worker : BackgroundService
{
    private readonly ILogger<TgBot> _logger;

    public Worker(ILogger<TgBot> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("CherryBot running at: {Time}", DateTimeOffset.Now);
            }

            var bot = new TgBot(_logger);
            await bot.RunAsync();

            await Task.Delay(1000, stoppingToken);
        }
    }
}