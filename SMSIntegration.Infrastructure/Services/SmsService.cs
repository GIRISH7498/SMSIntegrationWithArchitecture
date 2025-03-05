using Microsoft.Extensions.Options;
using SMSIntegration.Domain.Entities;
using SMSIntegration.Domain.Interface;
using SMSIntegration.Infrastructure.Configurations;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SMSIntegration.Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsSettings _smsSettings;
        private readonly ISmsLogRepository _smsRepository;

        public SmsService(IOptions<SmsSettings> smsSettings, ISmsLogRepository smsRepository)
        {
            _smsSettings = smsSettings.Value;
            _smsRepository = smsRepository;
        }

        public async Task<bool> SendSmsNow(SmsLog smsLog)
        {
            try
            {
                TwilioClient.Init(_smsSettings.AccountSid, _smsSettings.AuthToken);

                var sms = await MessageResource.CreateAsync(
                    body: smsLog.Message,
                    from: new Twilio.Types.PhoneNumber(_smsSettings.FromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(smsLog.PhoneNumber)
                );

                if (!string.IsNullOrEmpty(sms.Sid))
                {
                    smsLog.IsSent = true;
                    smsLog.SentOn = DateTime.UtcNow;
                    smsLog.RetryCount = 0;
                    smsLog.ErrorMessage = null;
                }
                else
                {
                    smsLog.RetryCount++;
                    smsLog.ErrorMessage = "Failed to send SMS";
                }
            }
            catch (Exception ex)
            {
                smsLog.RetryCount++;
                smsLog.ErrorMessage = ex.Message;
            }
            finally
            {
                await _smsRepository.UpdateSmsLogAsync(smsLog);
                await _smsRepository.SaveChangesAsync();
            }

            return smsLog.IsSent;
        }
    }
}
