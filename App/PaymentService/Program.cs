using System.Text.Json.Serialization;
using Config.Stracture;
using PaymentService.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));

builder.Services.UtilsConfig();

// Configure Database using Entity Framework
//string? ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string? ConnectionString = builder.Configuration.GetConnectionString("DeploymentConnection");
builder.Services.AddDbContext<DataContext>(
    opt => opt.UseSqlServer(ConnectionString)
);

// Avoid object cycles
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configure JWT tokens
string? AppToken = builder.Configuration.GetSection("AppSettings:Token").Value;
builder.Services.AuthConfig(AppToken);

// Configure Stripe
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

var app = builder.Build();

app.UseEndpointDefinitions();

app.UseAuthentication();
app.UseAuthorization();

app.Run();