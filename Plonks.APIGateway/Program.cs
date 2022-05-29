using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});


var ocelotConfig = builder.Environment.IsDevelopment() ? "ocelot.Development.json" : "ocelot.json";
builder.Configuration.AddJsonFile(ocelotConfig);

var app = builder.Build();

app.UseCors("cors");

app.UseOcelot().Wait();

app.Run();
