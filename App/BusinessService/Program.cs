using System.Text.Json.Serialization;
using Config.Stracture;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));

builder.Services.UtilsConfig();

builder.Services.AddAntiforgery();

// Configure Database using Entity Framework
string? ConnectionString = builder.Configuration.GetConnectionString("LocalDbConnection");
builder.Services.AddDbContext<DataContext>(
    opt => opt.UseSqlServer(ConnectionString)
);

// Avoid object cycles
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configure JWT tokens
string? AppToken = builder.Configuration.GetSection("AppSettings:Token").Value;
builder.Services.AuthConfig(AppToken);

var app = builder.Build();

app.UseEndpointDefinitions();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.Run();