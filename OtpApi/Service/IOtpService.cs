using System.Threading.Tasks;

namespace OtpApi.Service
{
    public interface IOtpService
    {
        Task<bool> GenerateAndSendOtpAsync(string phoneNumber); // Generates an OTP and sends it to the specified phone number
        Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode); // Verifies the provided OTP code for the specified phone number
    }
}