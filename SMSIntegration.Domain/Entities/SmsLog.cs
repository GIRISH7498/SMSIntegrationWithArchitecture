using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSIntegration.Domain.Entities
{
    public class SmsLog
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime SentOn { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public bool IsSent { get; set; }
        public int RetryCount { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
