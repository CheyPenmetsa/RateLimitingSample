using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adding Rate Limiting
builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter("Fixed", opt => {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 3;
    });
    options.RejectionStatusCode = 429;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.Run();
