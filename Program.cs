using FinanceAPI.Core.Interfaces;
using FinanceAPI.Infrastructure.Data;
using Microsoft.OpenApi; // Ensure this is present; if it errors, use Microsoft.OpenApi

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// .NET 10 Swagger Setup
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Finance Dashboard API", 
        Version = "v1",
        Description = "BFSI Investment Tracker"
    });
});

// Hexagonal Architecture DI
// builder.Services.AddScoped<IFundRepository, OracleFundRepository>(); // RIP Oracle
builder.Services.AddScoped<IFundRepository, PostgresFundRepository>();

// CORS for Vue
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finance API v1");
    });
}

app.UseCors("VueCorsPolicy");
app.MapControllers();
app.Run();