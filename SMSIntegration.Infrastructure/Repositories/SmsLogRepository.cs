using Microsoft.EntityFrameworkCore;
using SMSIntegration.Domain.Entities;
using SMSIntegration.Domain.Interface;
using SMSIntegration.Infrastructure.Persistence;

namespace SMSIntegration.Infrastructure.Repositories
{
    public class SmsLogRepository : ISmsLogRepository
    {
        private readonly ApplicationDbContext _context;

        public SmsLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddSmsLogAsync(SmsLog smsLog)
        {
            await _context.SmsLogs.AddAsync(smsLog);
        }

        public async Task<List<SmsLog>> GetPendingScheduledSmsAsync()
        {
            return await _context.SmsLogs
                .Where(s => s.IsScheduled && !s.IsSent && s.ScheduledTime <= DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<List<SmsLog>> GetFailedSmsAsync()
        {
            return await _context.SmsLogs
                .Where(s => !s.IsSent && s.RetryCount < 3)
                .ToListAsync();
        }

        public async Task UpdateSmsLogAsync(SmsLog smsLog)
        {
            _context.SmsLogs.Update(smsLog);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
