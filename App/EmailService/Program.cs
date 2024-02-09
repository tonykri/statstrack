var builder = WebApplication.CreateBuilder(args);

builder.Services.UtilsConfig();

var app = builder.Build();

app.Run();
