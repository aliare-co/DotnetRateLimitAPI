using Polly;

namespace DotnetRateLimitAPI.Core
{
    public static class FileUtil
    {
        public static async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            var retryPolicy = Policy
                .Handle<IOException>()
                .WaitAndRetry(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(1000),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.Error.WriteLine($"Tentativa {retryCount} falhou. Esperando {timeSpan.TotalMilliseconds} ms antes da próxima tentativa.");
                    }
                );

            try
            {
                await retryPolicy.Execute(async () =>
                {
                    using FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
                    using FileStream destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);

                    await sourceStream.CopyToAsync(destinationStream);
                });
            }
            catch (Exception ex)
            {
                //Console.Error.WriteLine($"Falha após várias tentativas: {ex.Message}");
            }
        }
    }
}
