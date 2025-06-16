namespace OtpApi.Service;
public class MockSmsSender : ISmsSender
{
    public Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        Console.WriteLine($"[Mock SMS] To: {phoneNumber} | Message: {message}");
        return Task.FromResult(true);
    }
}
