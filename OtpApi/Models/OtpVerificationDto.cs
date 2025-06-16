using OtpApi.Models;    

namespace OtpApi.Models;
public class OtpVerificationDto
{
    public string? PhoneNumber { get; set; }
    public string? OtpCode { get; set; }
}