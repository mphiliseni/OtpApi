using OtpApi.Service;
using Microsoft.OpenApi.Models;
using OtpApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add Swagger manually
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OTP API",
        Version = "v1",
        Description = "An API for generating and verifying one-time passwords (OTPs)."
    });
});

// Register services
builder.Services.AddSingleton<ISmsSender, MockSmsSender>();
builder.Services.AddSingleton<IOtpService, OtpService>();
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddSingleton<ISmsSender, TwilioSmsSender>();

builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret not configured")))
        };
    });

builder.Services.AddAuthorization();

// Add controllers (important!)
builder.Services.AddControllers();


var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "OTP API V1");
    });
}

// app.UseHttpsRedirection();
app.MapControllers(); // This maps your OtpController

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
