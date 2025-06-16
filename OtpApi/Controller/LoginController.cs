
using Microsoft.AspNetCore.Mvc;
using Twilio;
using OtpApi.Service;
using OtpApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace OtpApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IOtpService _otpService;
    private readonly JwtService _jwtService;

    public AuthController(IOtpService otpService, JwtService jwtService)
    {
        _otpService = otpService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            return BadRequest("Phone number is required.");
            
        var result = await _otpService.GenerateAndSendOtpAsync(dto.PhoneNumber);
        return result ? Ok("OTP sent.") : StatusCode(500, "Failed to send OTP.");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] OtpVerificationDto dto)
    {
        var isValid = await _otpService.VerifyOtpAsync(dto.PhoneNumber, dto.OtpCode);
        if (!isValid) return BadRequest("Invalid or expired OTP.");

        var token = _jwtService.GenerateToken(dto.PhoneNumber);
        return Ok(new { token });
    }


    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var phone = User.Identity?.Name;
        return Ok($"You are logged in as: {phone}");
    }
}

