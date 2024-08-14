using System.Globalization;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.RateLimiting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRateLimiter(options =>
        {
            options.AddSlidingWindowLimiter("Limiter", options =>
            {
                options.PermitLimit = 10;
                options.SegmentsPerWindow = 10;
                options.QueueLimit = 0;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.Window = TimeSpan.FromMilliseconds(500);
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = (context, token) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

                //context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");

                return new ValueTask();
            };
        });

        //builder.Services.ConfigureHealthChecks(builder.Configuration);

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseStaticFiles();

        /*
        app.UseHealthChecks("/check", new HealthCheckOptions
        {
            Predicate = p => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            AllowCachingResponses = true,
        });

        app.MapHealthChecksUI(options =>
        {
            options.UIPath = "/dashboard";
            options.AddCustomStylesheet("wwwroot/ui/resources/custom.css");
        });
        */

        app.UseRateLimiter();

        app.MapControllers(); //.RequireRateLimiting("Limiter");

        app.Run();
    }
}