using System.Data;
using Npgsql;

namespace Postgres.Notifications.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private NpgsqlConnection? _connection;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
        
        _connection = new NpgsqlConnection("server=localhost;port=5432;database=postgres;user id=postgres;password=Password123");
        await _connection.OpenAsync(cancellationToken);
        _connection.Notification += ConnectionOnNotification;
        _logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);

        await using var cmd = new NpgsqlCommand("LISTEN mychannel", _connection);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
        _logger.LogInformation("Listening for notifications");
    }

    private void ConnectionOnNotification(object sender, NpgsqlNotificationEventArgs e)
    {
        _logger.LogInformation("Notification Received: " + e.Payload);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running...");


        while (!stoppingToken.IsCancellationRequested)
        {
            if (_connection?.State != ConnectionState.Open)
            {
                _logger.LogInformation("Connection not open yet...");
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                continue;
            }

            await _connection.WaitAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _logger.LogInformation("Stopping worker...");
    }
}
