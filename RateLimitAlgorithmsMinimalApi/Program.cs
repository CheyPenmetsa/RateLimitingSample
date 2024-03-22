using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Rate Limiting
builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter("Fixed", opt => {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 4;
        opt.QueueLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.AddSlidingWindowLimiter("Sliding", opt => {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(30);
        opt.SegmentsPerWindow = 3;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });

    options.AddTokenBucketLimiter("Token", opt => {
        opt.TokenLimit = 100;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        opt.TokensPerPeriod = 10; //Rate at which you want to fill
        opt.AutoReplenishment = true;
    });

    options.AddConcurrencyLimiter("Concurrency", opt => {
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
        opt.PermitLimit = 100;
    });

    options.RejectionStatusCode = 429;
});

var app = builder.Build();

app.UseSwagger();

app.UseRateLimiter();

app.MapGet("/fixedAlg", () => "Hello World!").RequireRateLimiting("Fixed");

app.MapGet("/slidingAlg", () => "Hello World!").RequireRateLimiting("Sliding");

app.MapGet("/tokenAlg", () => "Hello World!").RequireRateLimiting("Token");

app.MapGet("/concurrentAlg", () => "Hello World!").RequireRateLimiting("Concurrency");

app.UseSwaggerUI();

app.Run();
