using OtpApi.Models;

namespace OtpApi.Models;

public class TwilioSettings
{
    public string? AccountSid { get; set; }
    public string? AuthToken { get; set; }
    public string? FromNumber { get; set; }
}