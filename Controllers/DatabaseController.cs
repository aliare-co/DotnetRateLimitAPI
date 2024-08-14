using DotnetRateLimitAPI.Core;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DotnetRateLimitAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("Limiter")]
    public class DatabaseController : ControllerBase
    {
        public DatabaseController() { }

        [HttpGet("new/{idusuario}")]
        public async Task<IResult> New(long idusuario)
        {
            try
            {
                var databaseFile = Path.Combine("Databases", $"database-{idusuario}.db");
                if (!System.IO.File.Exists(databaseFile))
                    await FileUtil.CopyFileAsync(Path.Combine("Resources", "sample-database.db"), databaseFile);

                var result = new
                {
                    message = $"New database for id = {idusuario} available in {databaseFile}...",
                    machine = Environment.MachineName,
                    username = Environment.UserName,
                    ip = NetworkUtil.GetLocalIPAddress()
                };

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Results.BadRequest(ex);
            }
        }

        /*
        [HttpGet("sync/{idusuario}")]
        public async Task<IResult> Sync(long idusuario)
        {
            await Task.Delay(new Random().Next(5000, 12001));

            var result = new
            {
                message = $"Sync database for id = {idusuario}...",
                machine = Environment.MachineName,
                username = Environment.UserName
            };

            Console.WriteLine(result);
            return Results.Ok(result);
        }
        */
    }
}
