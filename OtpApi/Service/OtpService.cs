using System.Collections.Concurrent;
using System;

namespace OtpApi.Service;
public class OtpService : IOtpService
{
    private readonly ISmsSender _smsSender;
    private readonly ConcurrentDictionary<string, (string OtpCode, DateTime Expiry)> _otpStore = new();

    private const int OtpLength = 6;
    private const int ExpiryMinutes = 5;

    public OtpService(ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    public async Task<bool> GenerateAndSendOtpAsync(string phoneNumber)
    {
        var otp = GenerateOtp();
        var expiry = DateTime.UtcNow.AddMinutes(ExpiryMinutes);

        _otpStore[phoneNumber] = (otp, expiry);

        var message = $"Your OTP code is {otp}. It expires in {ExpiryMinutes} minutes.";
        return await _smsSender.SendSmsAsync(phoneNumber, message);
    }

    public Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode)
    {
        if (_otpStore.TryGetValue(phoneNumber, out var entry))
        {
            if (entry.OtpCode == otpCode && DateTime.UtcNow <= entry.Expiry)
            {
                _otpStore.TryRemove(phoneNumber, out _); // prevent reuse
                return Task.FromResult(true);
            }
        }
        return Task.FromResult(false);
    }

    private string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
