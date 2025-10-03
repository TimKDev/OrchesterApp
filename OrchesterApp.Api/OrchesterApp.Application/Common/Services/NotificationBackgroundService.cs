using System.Threading.Channels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchesterApp.Domain.NotificationAggregate;
using OrchesterApp.Domain.NotificationAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces;

namespace TvJahnOrchesterApp.Application.Common.Services;

public class NotificationBackgroundService : BackgroundService, INotificationBackgroundService
{
    private readonly Channel<NotificationId> _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationBackgroundService> _logger;

    public NotificationBackgroundService(IServiceProvider serviceProvider,
        ILogger<NotificationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        var options = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };

        _queue = Channel.CreateBounded<NotificationId>(options);
    }

    public async Task EnqueueNotificationAsync(NotificationId notificationId)
    {
        await _queue.Writer.WriteAsync(notificationId);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var notificationId = await _queue.Reader.ReadAsync(stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                await sender.Send(new NotificationSender.Command([notificationId]), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing notification");
            }
        }
    }
}