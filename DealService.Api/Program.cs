using DealService.Api.Middleware;
using DealService.Application.Command;
using DealService.Application.Common.Interfaces;
using DealService.Infrastructure.Messaging;
using DealService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateDealCommand).Assembly));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Keycloak:ValidIssuer"],
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuer = true
        };

    });
builder.Services.AddSingleton<IIntegrationEventPublisher, KafkaIntegrationEventPublisher>();
var app = builder.Build();
app.UseExceptionHandler();
app.Use(async (context, next) =>
{
    context.Request.Headers.Append("Test-Header", "TestValue");
    context.Response.Headers.Append("Test-Response-Header", "TestResponseValue");
    await next();
   
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
