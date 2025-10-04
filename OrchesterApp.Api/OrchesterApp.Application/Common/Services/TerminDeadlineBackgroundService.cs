using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace TvJahnOrchesterApp.Application.Common.Services;

public class TerminDeadlineBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TerminDeadlineBackgroundService> _logger;

    public TerminDeadlineBackgroundService(ILogger<TerminDeadlineBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting scheduled deadline check");
                
                var scope = _serviceProvider.CreateScope();
                var deadlineCheckService = scope.ServiceProvider.GetRequiredService<ITerminDeadlineCheckService>();
                
                await deadlineCheckService.CheckTerminDeadlinesAsync(stoppingToken);
                
                _logger.LogInformation("Scheduled deadline check completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during scheduled deadline check");
            }

            await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
        }
    }
}

