using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Rate Limiting
builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter("Fixed", opt => {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 3;
    });
    options.RejectionStatusCode = 429;
});

var app = builder.Build();

app.UseSwagger();

app.UseRateLimiter();

app.MapGet("/", () => "Hello World!").RequireRateLimiting("Fixed");

app.UseSwaggerUI();

app.Run();
