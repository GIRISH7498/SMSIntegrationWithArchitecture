using SMSIntegration.Domain.Entities;

namespace SMSIntegration.Domain.Interface
{
    public interface ISmsLogRepository
    {
        Task AddSmsLogAsync(SmsLog smsLog);
        Task<List<SmsLog>> GetPendingScheduledSmsAsync();
        Task<List<SmsLog>> GetFailedSmsAsync();
        Task UpdateSmsLogAsync(SmsLog smsLog);
        Task SaveChangesAsync();
    }
}