using System.Net;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotnetRateLimitAPI.HealthCheck
{
    public static class HealthCheck
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql("Host=pgsql04.datacoper.com.br;Port=5435;Username=crmdesenv;Password=bi*2017;Database=cmdb_tetri.neto_6.0.7PD")
                .AddCheck<RemoteHealthCheck>("Remote endpoints", failureStatus: HealthStatus.Unhealthy)
                .AddCheck<MemoryHealthCheck>("Memory", failureStatus: HealthStatus.Unhealthy, tags: ["Feedback Service"]);

            services.AddHealthChecksUI(options =>
            {
                options.SetHeaderText("Health Check Status");
                options.SetEvaluationTimeInSeconds(60);
                options.MaximumHistoryEntriesPerEndpoint(100);
                options.SetApiMaxActiveRequests(1);
                options.AddHealthCheckEndpoint("Status", "/check");

                options.UseApiEndpointHttpMessageHandler(sp =>
                {
                    return new HttpClientHandler
                    {
                        Proxy = new WebProxy("http://tetri.neto:Cac2203171643%23@proxy.datacoper.com.br:3128", true)
                    };
                });
            })
                .AddInMemoryStorage();
        }
    }
}
