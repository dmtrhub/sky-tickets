using Application.Abstractions.Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Flights;

public class FlightStatusUpdater(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var now = DateTime.UtcNow;

            var flightsToComplete = await context.Flights
                .Where(f => f.Status == FlightStatus.Active && f.ArrivalTime <= now)
                .ToListAsync(stoppingToken);

            foreach (var flight in flightsToComplete)
            {
                flight.Status = FlightStatus.Completed;
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}
