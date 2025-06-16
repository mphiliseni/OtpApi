using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Options;
using OtpApi.Models;

namespace OtpApi.Service;
public class TwilioSmsSender : ISmsSender
{
    private readonly TwilioSettings _settings;

    public TwilioSmsSender(IOptions<TwilioSettings> options)
    {
        _settings = options.Value;
        TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            var result = await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_settings.FromNumber),
                body: message
            );

            return result.ErrorCode == null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Twilio error: {ex.Message}");
            return false;
        }
    }
}
