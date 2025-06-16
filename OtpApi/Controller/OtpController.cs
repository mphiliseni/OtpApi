using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtpApi.Models;
using OtpApi.Service;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OtpController : ControllerBase
{
    private readonly IOtpService _otpService;

    public OtpController(IOtpService otpService)
    {
        _otpService = otpService;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestOtp([FromBody] OtpRequestDto dto)
    {
        var result = await _otpService.GenerateAndSendOtpAsync(dto.PhoneNumber);
        return result ? Ok("OTP sent.") : StatusCode(500, "Failed to send OTP.");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto dto)
    {
        var result = await _otpService.VerifyOtpAsync(dto.PhoneNumber, dto.OtpCode);
        return result ? Ok("OTP verified.") : BadRequest("Invalid or expired OTP.");
    }
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        // This endpoint can be used to check if the user is authenticated
        // and to return any relevant status information.
        return Ok("OTP service is running.");
    }
    [HttpGet("me")]
    public IActionResult Me()
    {
        // This endpoint can be used to get the current user's information.
        var phone = User.Identity?.Name;
        return Ok(new { PhoneNumber = phone });
    }
}
