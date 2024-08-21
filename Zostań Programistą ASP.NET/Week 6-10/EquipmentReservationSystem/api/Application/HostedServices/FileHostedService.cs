using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.HostedServices
{
    public class FileHostedService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly double? _timeCircle;
        private readonly IServiceProvider _serviceProvider;

        public FileHostedService(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _timeCircle = double.TryParse(config["File:CleanigTimer"], out double timeValue) ? timeValue : null;
        }

        private void WrapperCleaningFunction(object? state)
        {
            using (var scopt = _serviceProvider.CreateScope())
            {
                var fileService = scopt.ServiceProvider.GetService<IFileService>();
                fileService.SystemHostedServiceMode = true;
                fileService.ClearTemporaryFiles();
                fileService.SystemHostedServiceMode = false;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_timeCircle == null) return Task.CompletedTask;

            if (_timer == null) _timer = new Timer(WrapperCleaningFunction, null, TimeSpan.Zero, TimeSpan.FromDays(_timeCircle.Value));
            else _timer.Change(TimeSpan.Zero, TimeSpan.FromDays(_timeCircle.Value));            

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
