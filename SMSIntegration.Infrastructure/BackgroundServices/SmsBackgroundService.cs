using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMSIntegration.Domain.Interface;

namespace SMSIntegration.Infrastructure.BackgroundServices
{
    public class SmsBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SmsBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var smsRepository = scope.ServiceProvider.GetRequiredService<ISmsLogRepository>();
                    var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();

                    var failedMessages = await smsRepository.GetFailedSmsAsync();
                    foreach (var sms in failedMessages)
                    {
                        await smsService.SendSmsNow(sms);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}
