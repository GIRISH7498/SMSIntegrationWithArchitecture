using MediatR;
using SMSIntegration.Domain.Entities;
using SMSIntegration.Domain.Interface;

namespace SMSIntegration.Application.Features.Sms.Commands
{
    public class SendSmsCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime? ScheduledTime { get; set; }

        public class SendSmsCommandHandler : IRequestHandler<SendSmsCommand, bool>
        {
            private readonly ISmsLogRepository _smsLogRepository;
            private readonly ISmsService _smsService;
            private readonly IBackgroundJobClient _jobs;

            public SendSmsCommandHandler(ISmsLogRepository smsLogRepository, ISmsService smsService, IBackgroundJobClient jobs)
            {
                _smsLogRepository = smsLogRepository;
                _smsService = smsService;
                 _jobs = jobs;
            }

            public async Task<bool> Handle(SendSmsCommand request, CancellationToken cancellationToken)
            {
                var smsLog = new SmsLog
                {
                    PhoneNumber = request.PhoneNumber,
                    Message = request.Message,
                    SentOn = DateTime.UtcNow,
                    IsScheduled = request.IsScheduled,
                    ScheduledTime = request.ScheduledTime,
                    IsSent = false,
                    RetryCount = 0,
                    ErrorMessage = null
                };

                await _smsLogRepository.AddSmsLogAsync(smsLog);
                await _smsLogRepository.SaveChangesAsync();

                if (request.IsScheduled && request.ScheduledTime.HasValue)
                {
                    var delay = request.ScheduledTime.Value - DateTime.UtcNow;
                    if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;
                
                    _jobs.Schedule<ISmsService>(
                      svc => svc.SendSmsNow(smsLog),
                      delay
                    );
                }
                else
                {
                    _jobs.Enqueue<ISmsService>(
                      svc => svc.SendSmsNow(smsLog)
                    );
                }

                return true;
            }
        }
    }
}
