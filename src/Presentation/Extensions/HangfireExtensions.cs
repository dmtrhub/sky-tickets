using Application.Flights;
using Hangfire;

namespace Presentation.Extensions
{
    public static class HangfireExtensions
    {
        public static void UseHangfireJobs(this IApplicationBuilder app) => 
            RecurringJob.AddOrUpdate<FlightStatusUpdater>(
                "update-flight-statuses",
                updater => updater.UpdateFlightStatuses(),
                Cron.MinuteInterval(10)); // 10 minutes
    }
}
