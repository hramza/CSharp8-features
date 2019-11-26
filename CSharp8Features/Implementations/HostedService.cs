using CSharp8Features.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class HostedService : IExecutable
    {
        public async Task Execute()
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(hostConfiguration =>
                {
                    hostConfiguration.SetBasePath(Directory.GetCurrentDirectory());
                    hostConfiguration.AddJsonFile("hostsettings.json", optional: true);
                    hostConfiguration.AddEnvironmentVariables(prefix: "PREFIX_");
                })
                .ConfigureAppConfiguration((hostBuilderContext, appConfig) =>
                {
                    appConfig.SetBasePath(Directory.GetCurrentDirectory());
                    appConfig.AddJsonFile("appsettings.json", optional: true);
                    appConfig.AddJsonFile(
                        $"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    appConfig.AddEnvironmentVariables(prefix: "PREFIX_");
                })
                .ConfigureLogging((_, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<SampleBackEndService>();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }

    class SampleBackEndService : IHostedService, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ILogger<SampleBackEndService> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly int RetriesNumber;

        public SampleBackEndService(ILogger<SampleBackEndService> logger, IHostApplicationLifetime applicationLifetime, IConfiguration configuration) =>
            (_logger, _applicationLifetime, RetriesNumber) = (logger, applicationLifetime, int.Parse(configuration[nameof(RetriesNumber)]));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting the background service");
            var token = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                int i = 0;
                while (i < RetriesNumber && !token.IsCancellationRequested)
                {
                    await Task.Delay(15);
                    Console.WriteLine(i);

                    i++;
                }

                _applicationLifetime.StopApplication();
            }, token);
            _logger.LogInformation("Calculating");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ending the background service");
            _cancellationTokenSource.Cancel();

            return Task.CompletedTask;
        }

        ~SampleBackEndService() => Dispose();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource.Dispose();
            }
        }
    }
}
