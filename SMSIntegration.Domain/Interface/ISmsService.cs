using SMSIntegration.Domain.Entities;

namespace SMSIntegration.Domain.Interface
{
    public interface ISmsService
    {
        Task<bool> SendSmsNow(SmsLog smsLog);
    }
}
