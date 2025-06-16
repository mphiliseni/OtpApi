
//  Create a Mock SMS Sender

using System.Threading.Tasks;
namespace OtpApi.Service
{
    public interface ISmsSender
    {
            Task<bool> SendSmsAsync(string phoneNumber, string message);

    }
}